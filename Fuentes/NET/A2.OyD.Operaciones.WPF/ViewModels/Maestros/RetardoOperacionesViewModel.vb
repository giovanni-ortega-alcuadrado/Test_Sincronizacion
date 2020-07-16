Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDOperaciones
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Text.RegularExpressions
Imports System.Object
Imports System.Globalization.CultureInfo
Imports A2ComunesControl

Public Class RetardoOperacionesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As OperacionesDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mobjVM As RetardoOperacionesViewModel

    Dim NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO As String = "Código"
    Dim logEditandoRegistro As Boolean = False
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As RetardoOperaciones
    Dim logNuevoRegistro As Boolean = False
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Dim strTipoBusqueda As String = "SinFiltro"

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyOperaciones()
                End If

                strTipoBusqueda = "SinFiltro"
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(False, False)

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de RetardoOperaciones que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of RetardoOperaciones)
    Public Property ListaEncabezado() As EntitySet(Of RetardoOperaciones)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of RetardoOperaciones))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de RetardoOperaciones para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaEncabezadoPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezado) Then
                If IsNothing(_ListaEncabezadoPaginada) Then
                    Dim view = New PagedCollectionView(_ListaEncabezado)
                    _ListaEncabezadoPaginada = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _EncabezadoSeleccionado As RetardoOperaciones
    Public Property EncabezadoSeleccionado() As RetardoOperaciones
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As RetardoOperaciones)

            Dim logIncializarDet As Boolean = False

            _EncabezadoSeleccionado = value

            If _EncabezadoSeleccionado Is Nothing Then
                logIncializarDet = True
            Else
                If _EncabezadoSeleccionado.intIDRetardo > 0 Then
                    ConsultarDetalle(_EncabezadoSeleccionado.intIDRetardo)
                    If _EncabezadoSeleccionado.logProcesado = True Then
                        dblTotalSeleccionado = _EncabezadoSeleccionado.dblNetoTotal
                    Else
                        dblTotalSeleccionado = 0
                    End If
                Else
                    logIncializarDet = True
                End If
            End If

            If logIncializarDet Then
                ListaDetalle = Nothing
            End If

            MyBase.CambioItem("dblTotalSeleccionado")
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso las operaciones que pueden tener retardos
    ''' </summary>
    Private _ListaDetalle As List(Of RetardoOperacionesDetalle)
    Public Property ListaDetalle() As List(Of RetardoOperacionesDetalle)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of RetardoOperacionesDetalle))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaDetallePaginada")
        End Set
    End Property

    ''' <summary>
    ''' Pagina la lista detalles. Se presenta en el grid del detalle 
    ''' </summary>
    Public ReadOnly Property ListaDetallePaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalle) Then
                Dim view = New PagedCollectionView(_ListaDetalle)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As RetardoOperacionesDetalle
    Public Property DetalleSeleccionado() As RetardoOperacionesDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As RetardoOperacionesDetalle)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaRetardos
    Public Property cb() As CamposBusquedaRetardos
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaRetardos)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _HabilitarEncabezado As Boolean = False
    Public Property HabilitarEncabezado() As Boolean
        Get
            Return _HabilitarEncabezado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncabezado = value
            MyBase.CambioItem("HabilitarEncabezado")
        End Set
    End Property

    Private _HabilitarBotonesDetalle As Boolean = False
    Public Property HabilitarEdicionDetalle() As Boolean
        Get
            Return _HabilitarBotonesDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotonesDetalle = value
            MyBase.CambioItem("HabilitarEdicionDetalle")
        End Set
    End Property

    Private _dblTotalLiquidaciones As Double = 0
    Public Property dblTotalLiquidaciones As Double
        Get
            Return _dblTotalLiquidaciones
        End Get
        Set(value As Double)
            _dblTotalLiquidaciones = value
            MyBase.CambioItem("dblTotalLiquidaciones")
        End Set
    End Property

    Private _dblTotalSeleccionado As Double = 0
    Public Property dblTotalSeleccionado As Double
        Get
            Return _dblTotalSeleccionado
        End Get
        Set(value As Double)
            _dblTotalSeleccionado = value
            MyBase.CambioItem("dblTotalSeleccionado")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    Public Overrides Async Sub Filtrar()
        Try
            strTipoBusqueda = "FiltroVM"
            Await ConsultarEncabezado(True, False, FiltroVM)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la ejecución del filtro", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Overrides Async Sub QuitarFiltro()
        Try
            strTipoBusqueda = "SinFiltro"
            Await ConsultarEncabezado(False, False)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la ejecución del filtro", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.dtmFecha) Or cb.intCuenta <> 0 Or Not IsNothing(cb.strIDEspecie) Or Not IsNothing(cb.logProcesado) Then 'Validar que ingresó algo en los campos de búsqueda
                strTipoBusqueda = "FiltroAvanzado"
                Await ConsultarEncabezado(False, True, String.Empty, cb.dtmFecha, cb.intCuenta, cb.strIDEspecie, cb.logProcesado)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Overrides Async Sub CancelarBuscar()
        Try
            strTipoBusqueda = "SinFiltro"
            Await ConsultarEncabezado(False, False)
            Editando = False
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la busqueda avanzada", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    Public Overrides Async Sub ActualizarRegistro()
        Try
            IsBusy = True
            A2Utilidades.Mensajes.mostrarMensajePregunta("Se realizará el proceso de retardo sobre las operaciones.", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf TerminaPreguntaProcesoRetardo, True)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "GuardarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Async Sub TerminaPreguntaProcesoRetardo(ByVal sender As Object, ByVal e As EventArgs)
        Try
            IsBusy = True

            Dim strAccion As String = ValoresUserState.Actualizar.ToString()
            Dim xmlCompleto As String = String.Empty
            Dim xmlDetalle As String = String.Empty
            Dim strMsg As String = String.Empty

            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then

                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyOperaciones()
                End If

                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                ErrorForma = String.Empty

                For Each objeto In (From c In ListaDetalle)
                    If objeto.dblRetardo > 0 Then
                        xmlDetalle = "<Detalle intIDRetardoOperacionesDetalle=""" & objeto.intIDRetardoOperacionesDetalle &
                                     """ intIDLiquidaciones=""" & objeto.intIDLiquidaciones &
                                     """ dblRetardo=""" & objeto.dblRetardo &
                                     """></Detalle>"
                        xmlCompleto = xmlCompleto & xmlDetalle
                    End If
                Next

                xmlCompleto = "<RetardoOperaciones>" & xmlCompleto & "</RetardoOperaciones>"

                Dim objRet As InvokeOperation(Of String)

                objRet = Await mdcProxy.ActualizarRetardoOperacionesDetalleSync(pintIDRetardo:=EncabezadoSeleccionado.intIDRetardo,
                                                                            pxmlDetalleGrid:=xmlCompleto,
                                                                            pstrUsuario:=Program.Usuario,
                                                                            pstrInfoConexion:=Program.HashConexion).AsTask()

                If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then

                    strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())
                    strMsg = strMsg.Replace(" + |", String.Format("{0}   -> ", vbCrLf))
                    strMsg = strMsg.Replace("++", String.Format("{0}      -> ", vbCrLf))
                    strMsg = strMsg.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                    strMsg = strMsg.Replace("|", String.Format("{0}   -> ", vbCrLf))
                    strMsg = strMsg.Replace("--", String.Format("{0}", vbCrLf))

                    For Each objeto In (From c In ListaDetalle)
                        objeto.dblRetardo = 0
                    Next

                    dblTotalSeleccionado = 0
                    MyBase.CambioItem("dblTotalSeleccionado")

                    If Not String.IsNullOrEmpty(strMsg) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    Editando = False
                    HabilitarEncabezado = False
                    HabilitarEdicionDetalle = False

                    If strTipoBusqueda = "SinFiltro" Then
                        Await ConsultarEncabezado(False, False)
                    Else
                        If strTipoBusqueda = "FiltroVM" Then
                            Await ConsultarEncabezado(True, False, FiltroVM)
                        Else
                            If Not IsNothing(cb.dtmFecha) Or cb.intCuenta <> 0 Or Not IsNothing(cb.strIDEspecie) Or Not IsNothing(cb.logProcesado) Then 'Validar que ingresó algo en los campos de búsqueda
                                Await ConsultarEncabezado(False, True, String.Empty, cb.dtmFecha, cb.intCuenta, cb.strIDEspecie, cb.logProcesado)
                            End If
                        End If
                    End If

                    MyBase.CambiarALista()

                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar la pregunta proceso retardos", Me.ToString(), "TerminaPreguntaProcesoRetardo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If _EncabezadoSeleccionado.logProcesado = True Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("No se puede editar un retardo que ha sido procesado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If mdcProxy.IsLoading Then
                Editando = False
                MyBase.CambioItem("Editando")
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            Editando = True
            MyBase.CambioItem("Editando")
            logNuevoRegistro = False
            HabilitarEncabezado = False
            HabilitarEdicionDetalle = True
            mobjEncabezadoAnterior = ObtenerRegistroAnterior()
            IsBusy = False

        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxy.RejectChanges()
            End If

            Editando = False
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = mobjEncabezadoAnterior
            HabilitarEncabezado = False
            HabilitarEdicionDetalle = False
            logNuevoRegistro = False
            dblTotalSeleccionado = 0
            MyBase.CambioItem("dblTotalSeleccionado")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Public Sub CalcularRetardo()
        Try
            dblTotalSeleccionado = (From ld In ListaDetalle Where ld.dblRetardo > 0 Select ld.dblRetardo).Sum
            MyBase.CambioItem("dblTotalSeleccionado")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular el total seleccionado", Me.ToString(), "CalcularTotalRetardo", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaRetardos
            objCB.intCuenta = 0
            objCB.strIDEspecie = String.Empty
            objCB.logProcesado = Nothing
            cb = objCB
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar los datos de la forma de búsqueda", Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Retorna una copia del encabezado activo. Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
    ''' </summary>
    Private Function ObtenerRegistroAnterior() As RetardoOperaciones
        Dim objEncabezado As RetardoOperaciones = New RetardoOperaciones

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of RetardoOperaciones)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDRetardo = _EncabezadoSeleccionado.intIDRetardo
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de RetardoOperaciones
    ''' </summary>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltroSencillo As Boolean,
                                               ByVal plogFiltroAvanzado As Boolean,
                                               Optional ByVal pstrFiltro As String = "",
                                               Optional ByVal pdtmFecha As System.Nullable(Of System.DateTime) = Nothing,
                                               Optional ByVal pintCuenta As Integer = 0,
                                               Optional ByVal pstrIDEspecie As String = "",
                                               Optional ByVal plogProcesado As System.Nullable(Of System.Boolean) = Nothing) As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Try
            IsBusy = True
            Dim objRet As LoadOperation(Of RetardoOperaciones)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyOperaciones()
            End If

            ErrorForma = String.Empty
            mdcProxy.RetardoOperaciones.Clear()
            pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarRetardoOperacionesSyncQuery(plogFiltroSencillo:=plogFiltroSencillo,
                                                                                       plogFiltroAvanzado:=plogFiltroAvanzado,
                                                                                       pstrFiltro:=pstrFiltro,
                                                                                       pdtmFecha:=pdtmFecha,
                                                                                       pintCuenta:=pintCuenta,
                                                                                       pstrIDEspecie:=pstrIDEspecie,
                                                                                       plogProcesado:=plogProcesado,
                                                                                       pstrUsuario:=Program.Usuario,
                                                                                       pstrInfoConexion:=Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaEncabezado = mdcProxy.RetardoOperaciones
                    If objRet.Entities.Count = 0 And Not pstrFiltro.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos que concuerden con los criterios de búsqueda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                ListaEncabezado.Clear()
            End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de retardos", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Métodos sincrónicos del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <remarks>NOTA: En este método no se puede utilizar la propiedad IsBusy porque hace que sean necesarios dos clic para seleccionar un detalle</remarks>
    Public Async Function ConsultarDetalle(ByVal intIDRetardoOperaciones As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Try
            'IsBusy = True
            Dim objRet As LoadOperation(Of RetardoOperacionesDetalle)

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyOperaciones()
            End If

            If Not mdcProxy.RetardoOperacionesDetalles Is Nothing Then
                mdcProxy.RetardoOperacionesDetalles.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarRetardoOperacionesDetalleSyncQuery(intIDRetardoOperaciones, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle del retardo pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle del retardo.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalle = objRet.Entities.ToList
                    dblTotalLiquidaciones = (From ld In ListaDetalle Select ld.dblCantidad).Sum
                    MyBase.CambioItem("dblTotalLiquidaciones")
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle del retardo seleccionada.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function
#End Region

End Class

''' <summary>
''' REQUERIDO
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda. Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
Public Class CamposBusquedaRetardos
    Implements INotifyPropertyChanged

    Private _dtmFecha As System.Nullable(Of System.DateTime)
    Public Property dtmFecha() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFecha
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFecha = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFecha"))
        End Set
    End Property

    Private _intCuenta As System.Nullable(Of Integer) = True
    Public Property intCuenta() As System.Nullable(Of Integer)
        Get
            Return _intCuenta
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intCuenta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intCuenta"))
        End Set
    End Property

    Private _strIDEspecie As String
    Public Property strIDEspecie() As String
        Get
            Return _strIDEspecie
        End Get
        Set(ByVal value As String)
            _strIDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIDEspecie"))
        End Set
    End Property

    Private _logProcesado As System.Nullable(Of Boolean) = True
    Public Property logProcesado() As System.Nullable(Of Boolean)
        Get
            Return _logProcesado
        End Get
        Set(ByVal value As System.Nullable(Of Boolean))
            _logProcesado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logProcesado"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
