Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Text.RegularExpressions
Imports System.Object
Imports System.Globalization.CultureInfo
Imports A2ComunesControl

Public Class MovimientosParticipacionesFondosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Constantes"
    Public Enum Detalles
        cmMovimientosParticipacionesFondos
    End Enum
#End Region

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As MovimientosParticipacionesFondos
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mobjVM As MovimientosParticipacionesFondosViewModel

    Dim cwMovimientosParticipacionesFondosView As cwMovimientosParticipacionesFondosView
    Dim logExisteApertura As Boolean = False
    Dim intIDExisteApertura As Integer
    Dim logExisteCancelacion As Boolean = False
    Dim intIDExisteCancelacion As Integer
    Dim NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO As String = "Código"
    Dim logEditandoRegistro As Boolean = False
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As MovimientosParticipacionesFondos
    Private mobjDetallePorDefecto As MovimientosParticipacionesFondosDetalle
    Dim logNuevoRegistro As Boolean = False

    'YAPP20160119 Variable para controlar las veces que se pasa a ingresar un registro para evaluar las veces que se llama el procedimiento de ingresar
    'y asi actualizar la propiedad lodEsNuevoRegistro correctamente
    Dim ContPases As Integer = 0

    Private WithEvents mobjBuscadorLst As BuscadorGenericoLista = Nothing
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
    ''' <history>
    ''' ID caso de prueba: CP0001
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Julio 31/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Julio 31/2015 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefectoSync()
                ConsultarDetallePorDefectoSync()

                If String.IsNullOrEmpty(NOMBRE_ETIQUETA_COMITENTE) Then
                    NOMBRE_ETIQUETA_COMITENTE = NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO
                End If

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)

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
    ''' Lista de MovimientosParticipacionesFondos que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of MovimientosParticipacionesFondos)
    Public Property ListaEncabezado() As EntitySet(Of MovimientosParticipacionesFondos)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of MovimientosParticipacionesFondos))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de MovimientosParticipacionesFondos para navegar sobre el grid con paginación
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

    ''' <summary>
    ''' Elemento de la lista de MovimientosParticipacionesFondos que se encuentra seleccionado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0002
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Julio 31/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Julio 31/2015 - Resultado Ok 
    ''' </history>
    Private WithEvents _EncabezadoSeleccionado As MovimientosParticipacionesFondos
    Public Property EncabezadoSeleccionado() As MovimientosParticipacionesFondos
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As MovimientosParticipacionesFondos)

            Dim logIncializarDet As Boolean = False

            _EncabezadoSeleccionado = value

            If _EncabezadoSeleccionado Is Nothing Then
                logIncializarDet = True
            Else
                If _EncabezadoSeleccionado.intIDMovimientosParticipacionesFondos > 0 Then
                    ConsultarDetalle(_EncabezadoSeleccionado.intCuenta, _EncabezadoSeleccionado.lngIDPortafolio, _EncabezadoSeleccionado.strFondo)
                Else
                    logIncializarDet = True
                End If
            End If

            If logIncializarDet Then
                ListaDetalle = Nothing
            End If

            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private Async Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        If e.PropertyName = "lngIDPortafolio" Then
            If Not IsNothing(EncabezadoSeleccionado) Then
                HabilitarEdicionDetalle = False

                If String.IsNullOrEmpty(EncabezadoSeleccionado.lngIDPortafolio) Then
                    EncabezadoSeleccionado.strPortafolio = String.Empty
                Else
                    HabilitarEdicionDetalle = True
                    Await ConsultarDatosPortafolio()
                End If

                If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strPortafolio) Then
                    HabilitarEdicionDetalle = True
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso detalle de codificacion de activos
    ''' </summary>
    Private _ListaDetalle As List(Of MovimientosParticipacionesFondosDetalle)
    Public Property ListaDetalle() As List(Of MovimientosParticipacionesFondosDetalle)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of MovimientosParticipacionesFondosDetalle))
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
    Private WithEvents _DetalleSeleccionado As MovimientosParticipacionesFondosDetalle
    Public Property DetalleSeleccionado() As MovimientosParticipacionesFondosDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As MovimientosParticipacionesFondosDetalle)
            _DetalleSeleccionado = value
            If Not IsNothing(DetalleSeleccionado) And HabilitarEdicionDetalle Then
                If DetalleSeleccionado.logAplicado Then
                    HabilitarBorrarDetalle = False
                Else
                    HabilitarBorrarDetalle = True
                End If
            End If
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaMovimientosParticipacionesFondos
    Public Property cb() As CamposBusquedaMovimientosParticipacionesFondos
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaMovimientosParticipacionesFondos)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _habilitarReceptor As Boolean = False
    Public Property HabilitarReceptor() As Boolean
        Get
            Return _habilitarReceptor
        End Get
        Set(ByVal value As Boolean)
            _habilitarReceptor = value
            MyBase.CambioItem("HabilitarReceptor")
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

    Private _HabilitarBorrarDetalle As Boolean = False
    Public Property HabilitarBorrarDetalle() As Boolean
        Get
            Return _HabilitarBorrarDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBorrarDetalle = value
            MyBase.CambioItem("HabilitarBorrarDetalle")
        End Set
    End Property

    Private _BorrarCliente As Boolean = False
    Public Property BorrarCliente() As Boolean
        Get
            Return _BorrarCliente
        End Get
        Set(ByVal value As Boolean)
            _BorrarCliente = value
            MyBase.CambioItem("BorrarCliente")
        End Set
    End Property

    Private _NOMBRE_ETIQUETA_COMITENTE As String = Program.STR_NOMBRE_ETIQUETA_COMITENTE
    Public Property NOMBRE_ETIQUETA_COMITENTE() As String
        Get
            Return _NOMBRE_ETIQUETA_COMITENTE
        End Get
        Set(ByVal value As String)
            _NOMBRE_ETIQUETA_COMITENTE = value
            MyBase.CambioItem("NOMBRE_ETIQUETA_COMITENTE")
        End Set
    End Property

    'YAPP10112015
    'Propiedad para mostrar el listado de errores de la revision
    Private _ListaResultadoValidacion As List(Of CFCalculosFinancieros.Respuesta_Validaciones)
    Public Property ListaResultadoValidacion() As List(Of CFCalculosFinancieros.Respuesta_Validaciones)
        Get
            Return _ListaResultadoValidacion
        End Get
        Set(ByVal value As List(Of CFCalculosFinancieros.Respuesta_Validaciones))
            _ListaResultadoValidacion = value
            MyBase.CambioItem("ListaResultadoValidacion")
        End Set
    End Property
#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0005
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Julio 31/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Julio 31/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New MovimientosParticipacionesFondos

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            logNuevoRegistro = True

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of MovimientosParticipacionesFondos)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intIDMovimientosParticipacionesFondos = -1
                objNvoEncabezado.logEsNuevoRegistro = True
                objNvoEncabezado.intCuenta = String.Empty
                objNvoEncabezado.strPortafolio = String.Empty
                objNvoEncabezado.strFondo = String.Empty
                objNvoEncabezado.dblEntradas = 0
                objNvoEncabezado.dblSalidas = 0
                objNvoEncabezado.dblSaldo = 0
                objNvoEncabezado.dblSaldoUnidades = 0
                objNvoEncabezado.dblUnidadesFinales = 0
                objNvoEncabezado.strMoneda = String.Empty
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado

            HabilitarEncabezado = True
            HabilitarReceptor = True
            HabilitarEdicionDetalle = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0003
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Julio 31/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Julio 31/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Async Sub Filtrar()
        Try
            Await ConsultarEncabezado(True, FiltroVM)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la ejecución del filtro", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0004
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Julio 31/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Julio 31/2015 - Resultado Ok 
    ''' </history>
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
            If Not IsNothing(cb.intCuenta) Or Not IsNothing(cb.lngIDPortafolio) Or Not IsNothing(cb.strFondo) Then 'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, cb.intCuenta, cb.lngIDPortafolio, cb.strFondo)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    ''' <history>
    ''' Modificado por:        Jhonatan Arley Acevedo Martínez
    ''' Fecha:             Septiembre 22/2015
    ''' Descripción:       Se añaden los campos dblValorMonedaOrigen y intIDBanco al xml
    ''' Modificado por:        Yessid Andrés Paniagua Pabon
    ''' Fecha:             Noviembre 10/2015
    ''' Descripción:       Se crea visualización del motor de errores
    ''' Id del Cambio:     YAPP20151110
    ''' </history>
    Public Overrides Sub ActualizarRegistro()
        Try
            LimpiarVariablesConfirmadas()
            GuardarRegistro()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' ""
    ''' ""
    ''' </summary>
    ''' <history>
    ''' Modificado por:    Yessid Andrés Paniagua Pabón
    ''' Fecha:             Febrero 29/2016
    ''' Descripción:       Se coloca control para evitar error al presionar varias veces el boton de guardar
    ''' ID del Cambio:     YAPP20160229
    '''</history>   
    Public Sub GuardarRegistro()
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            IsBusy = True

            'YAPP20160229- Inicio
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            'YAPP20160229 - Fin


            Dim xmlCompleto As String
            Dim xmlDetalle As String
            IsBusy = True

            ErrorForma = String.Empty

            If ValidarRegistro() Then

                If ValidarDetalle() Then

                    logExisteApertura = False
                    logExisteCancelacion = False

                    For Each Lista In ListaDetalle
                        If Lista.strTipo = "AP" Or Lista.strDescripcionTipo.ToLower = "apertura" Then
                            logExisteApertura = True
                        End If
                        If Lista.strTipo = "CA" Or Lista.strDescripcionTipo.ToLower = "cancelación" Then
                            logExisteCancelacion = True
                            intIDExisteCancelacion = Lista.intIDMovimientosParticipacionesFondosDetalle
                        End If
                    Next

                    If Not logExisteApertura Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe de existir por lo menos una apertura en el detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If

                    xmlCompleto = "<MovimientosParticipacionesFondos>"

                    Dim strConfirmaciones As String = String.Empty
                    Dim strConfirmacionesUsuario As String = String.Empty
                    Dim strJustificaciones As String = String.Empty
                    Dim strJustificacionesUsuario As String = String.Empty
                    Dim strAprobaciones As String = String.Empty
                    Dim strAprobacionesUsuario As String = String.Empty



                    'YAPP20160119 controlar las veces que se pasa a ingresar un registro para evaluar las veces que se llama el procedimiento de ingresar
                    'y asi actualizar la propiedad lodEsNuevoRegistro correctamente

                    ContPases = ContPases + 1
                    If ContPases > 1 Then
                        If EncabezadoSeleccionado.logEsNuevoRegistro = True Then
                            EncabezadoSeleccionado.logEsNuevoRegistro = False
                        End If
                        ContPases = 0
                    End If

                    'Fin YAPP20160119 Controla las veces que se envia a ingresar el registro

                    For Each objeto In (From c In ListaDetalle)
                        strConfirmaciones = String.Empty
                        strConfirmacionesUsuario = String.Empty
                        strJustificaciones = String.Empty
                        strJustificacionesUsuario = String.Empty
                        strAprobaciones = String.Empty
                        strAprobacionesUsuario = String.Empty

                        '  If Not IsNothing(ListaValidacionesDetalle) Then
                        If Not IsNothing(ObjValidacionesDetalle) Then
                            Dim objValidacion = ObjValidacionesDetalle 'ListaValidacionesDetalle.Where(Function(i) i.IDDetalle = objeto.intIDMovimientosParticipacionesFondosDetalle).First
                            If Not IsNothing(objValidacion.Confirmaciones) Then strConfirmaciones = objValidacion.Confirmaciones
                            If Not IsNothing(objValidacion.ConfirmacionesUsuario) Then strConfirmacionesUsuario = objValidacion.ConfirmacionesUsuario
                            If Not IsNothing(objValidacion.Justificaciones) Then strJustificaciones = objValidacion.Justificaciones
                            If Not IsNothing(objValidacion.JustificacionesUsuario) Then strJustificacionesUsuario = objValidacion.JustificacionesUsuario
                            If Not IsNothing(objValidacion.Aprobaciones) Then strAprobaciones = objValidacion.Aprobaciones
                            If Not IsNothing(objValidacion.AprobacionesUsuario) Then strAprobacionesUsuario = objValidacion.AprobacionesUsuario
                        End If
                        ' End If

                        xmlDetalle = "<Detalle intIDMovimientosParticipacionesFondosDetalle=""" & objeto.intIDMovimientosParticipacionesFondosDetalle &
                                    """ strDescripcionTipo=""" & objeto.strDescripcionTipo & """ dtmMovimiento=""" & Format(objeto.dtmMovimiento, "yyyy/MM/dd") &
                                    """ dblValor=""" & objeto.dblValor & """ dblUnidades=""" & objeto.dblUnidades & """ dblVlrUnidad=""" & objeto.dblVlrUnidad &
                                    """ logAplicado=""" & objeto.logAplicado & """ dblValorMonedaOrigen=""" & objeto.dblValorMonedaOrigen &
                                    """ intIDBanco=""" & objeto.intIDBanco & """ dblTasaConvMoneda=""" & objeto.dblTasaConvMoneda & """ dtmFechaRegistro=""" & Format(objeto.dtmFechaRegistro, "yyyy/MM/dd") &
                                    """ intIdEncabezado=""" & objeto.intIdEncabezado &
                                    """></Detalle>"

                        xmlCompleto = xmlCompleto & xmlDetalle


                    Next

                    xmlCompleto = xmlCompleto & "</MovimientosParticipacionesFondos>"

                    IsBusy = True

                    Dim strMsg As String = ""
                    'YAPP20151110
                    'Dim objRet As InvokeOperation(Of List(Of CFCalculosFinancieros.Respuesta_Validaciones))
                    xmlCompleto = System.Web.HttpUtility.UrlEncode(xmlCompleto)


                    If IsNothing(ObjValidacionesDetalle) Then
                        InicializarObjValidaciones()
                    End If


                    mdcProxy.Load(mdcProxy.ActualizarMovimientosParticipacionesFondosSyncQuery(EncabezadoSeleccionado.intIDMovimientosParticipacionesFondos,
                                                                                            EncabezadoSeleccionado.logEsNuevoRegistro, EncabezadoSeleccionado.intCuenta,
                                                                                            EncabezadoSeleccionado.lngIDPortafolio, EncabezadoSeleccionado.strFondo,
                                                                                            EncabezadoSeleccionado.dblEntradas, EncabezadoSeleccionado.dblSalidas,
                                                                                            EncabezadoSeleccionado.dblSaldo,
                                                                                            EncabezadoSeleccionado.dblSaldoUnidades, EncabezadoSeleccionado.dblUnidadesFinales,
                                                                                            xmlCompleto, Program.Usuario, EncabezadoSeleccionado.strMoneda,
                                                                                            ObjValidacionesDetalle.Confirmaciones, ObjValidacionesDetalle.ConfirmacionesUsuario, ObjValidacionesDetalle.Justificaciones, ObjValidacionesDetalle.JustificacionesUsuario, ObjValidacionesDetalle.Aprobaciones, ObjValidacionesDetalle.AprobacionesUsuario, EncabezadoSeleccionado.strIDReceptor, Program.HashConexion), LoadBehavior.RefreshCurrent, AddressOf TerminoIngresar, String.Empty)
                    ' YAPP20151110
                    'If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                    '    strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())

                    '    If Not String.IsNullOrEmpty(strMsg) Then
                    '        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    '    End If
                    'Else
                    '    Editando = False
                    '    HabilitarEncabezado = False
                    '    HabilitarEdicionDetalle = False
                    '    HabilitarBorrarDetalle = False
                    '    logExisteApertura = False
                    '    logExisteCancelacion = False
                    '    logNuevoRegistro = False
                    '    ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                    '    Await ConsultarEncabezado(True, String.Empty)
                    'End If
                End If
            Else
                HabilitarEncabezado = True
                HabilitarReceptor = True
                HabilitarEdicionDetalle = True
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "GuardarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Se ejecuta al terminar de ejecutar la actualización para mostrar las reglas que sean detectadas por el motor
    ''' </summary>
    ''' <history>
    ''' Creado por:        Yessid Andrés Paniagua Pabón (Alcuadrado S.A.)
    ''' Fecha:             Noviembre 10/2015
    ''' Pruebas CB:        Yessid Andrés Paniagua Pabón (Alcuadrado S.A.) - Noviembre 10/2015 - Resultado Ok 
    ''' </history>
    Public Async Sub TerminoIngresar(ByVal lo As LoadOperation(Of CFCalculosFinancieros.Respuesta_Validaciones))
        Try

            If lo.HasError = False Then

                ListaResultadoValidacion = lo.Entities.ToList

                If ListaResultadoValidacion.Count > 0 Then

                    Dim logExitoso As Boolean = False
                    Dim logContinuaMostrandoMensaje As Boolean = False
                    Dim logRequiereConfirmacion As Boolean = False
                    Dim logRequiereJustificacion As Boolean = False
                    Dim logRequiereAprobacion As Boolean = False
                    Dim logConsultaListaJustificacion As Boolean = False
                    Dim logError As Boolean = False
                    Dim strMensajeExitoso As String = "El movimiento fondo se actualizó correctamente."
                    Dim strMensajeError As String = "El movimiento fondo no se pudo actualizar."
                    Dim logEsHtml As Boolean = False
                    Dim strMensajeDetallesHtml As String = String.Empty
                    Dim strMensajeRetornoHtml As String = String.Empty

                    For Each li In ListaResultadoValidacion
                        If li.Exitoso Then
                            logExitoso = True
                            logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereJustificacion = False
                            logRequiereConfirmacion = False
                            logRequiereAprobacion = False
                            strMensajeExitoso = String.Format("{0}{1} {2}", strMensajeExitoso, vbCrLf, li.Mensaje)
                        ElseIf li.RequiereConfirmacion Then
                            logExitoso = False
                            '  logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereConfirmacion = True
                        ElseIf li.RequiereJustificacion Then
                            logExitoso = False
                            ' logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereJustificacion = True
                        ElseIf li.RequiereAprobacion Then
                            logExitoso = False
                            ' logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereAprobacion = True
                        ElseIf li.DetieneIngreso Then
                            logError = True
                            logExitoso = False
                            logRequiereJustificacion = False
                            logRequiereConfirmacion = False
                            logContinuaMostrandoMensaje = False
                            logRequiereAprobacion = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)
                            strMensajeRetornoHtml = String.Format("{0}{1}", strMensajeRetornoHtml, li.DetalleRegla)
                        Else
                            logError = True
                            logExitoso = False
                            logRequiereJustificacion = False
                            logRequiereConfirmacion = False
                            logContinuaMostrandoMensaje = False
                            logRequiereAprobacion = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)
                            strMensajeRetornoHtml = String.Format("{0}{1}", strMensajeRetornoHtml, li.DetalleRegla)
                        End If

                    Next

                    If logExitoso And
                        logContinuaMostrandoMensaje = False And
                        logRequiereConfirmacion = False And
                        logRequiereJustificacion = False And
                        logRequiereAprobacion = False And
                        logError = False Then

                        strMensajeExitoso = strMensajeExitoso.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("--", String.Format("{0}", vbCrLf))

                        'Valida sí es una orden cruzada.
                        Editando = False
                        HabilitarEncabezado = False
                        HabilitarEdicionDetalle = False
                        HabilitarBorrarDetalle = False
                        HabilitarReceptor = False
                        logExisteApertura = False
                        logExisteCancelacion = False
                        logNuevoRegistro = False
                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        'Await ConsultarEncabezado(True, EncabezadoSeleccionado.intCuenta)
                        Dim Encabezado As MovimientosParticipacionesFondos = EncabezadoSeleccionado
                        Dim Detalle As List(Of MovimientosParticipacionesFondosDetalle) = ListaDetalle
                        logEditandoRegistro = True
                        If String.Empty <> FiltroVM Then
                            Await ConsultarEncabezado(True, FiltroVM)
                        Else

                            Await ConsultarEncabezado(True, String.Empty)

                            EncabezadoSeleccionado = Encabezado
                            ListaDetalle = Detalle
                        End If

                    ElseIf logError Then
                        If Not String.IsNullOrEmpty(strMensajeRetornoHtml) Then
                            logEsHtml = True
                            strMensajeDetallesHtml = String.Format("<HTML><HEAD></HEAD><BODY>{0}</BODY></HTML>", strMensajeRetornoHtml)
                        Else
                            logEsHtml = False
                            strMensajeDetallesHtml = String.Empty
                        End If

                        strMensajeError = strMensajeError.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("--", String.Format("{0}", vbCrLf))

                        A2Utilidades.Mensajes.mostrarMensaje(strMensajeError, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia, String.Empty, "Reglas incumplidas en los detalles de las ordenes", logEsHtml, strMensajeDetallesHtml)
                        IsBusy = False
                    Else
                        ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.TODOS)
                    End If
                Else 'EXITOSO
                    Editando = False
                    HabilitarEncabezado = False
                    HabilitarEdicionDetalle = False
                    HabilitarBorrarDetalle = False
                    HabilitarReceptor = False
                    logExisteApertura = False
                    logExisteCancelacion = False
                    logNuevoRegistro = False
                    ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                    'Await ConsultarEncabezado(True, EncabezadoSeleccionado.intCuenta)
                    Dim Encabezado As MovimientosParticipacionesFondos = EncabezadoSeleccionado
                    Dim Detalle As List(Of MovimientosParticipacionesFondosDetalle) = ListaDetalle
                    logEditandoRegistro = True
                    If String.Empty <> FiltroVM Then
                        Await ConsultarEncabezado(True, FiltroVM)
                    Else

                        Await ConsultarEncabezado(True, String.Empty)

                        EncabezadoSeleccionado = Encabezado
                        ListaDetalle = Detalle
                    End If
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0006
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Julio 31/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Julio 31/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If mdcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            logNuevoRegistro = False

            IsBusy = True

            _EncabezadoSeleccionado.strUsuario = Program.Usuario
            _EncabezadoSeleccionado.logEsNuevoRegistro = False

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            HabilitarEncabezado = False

            HabilitarEdicionDetalle = True
            'Se Habilita el receptor para que permita ser modificado en el encabezado
            HabilitarReceptor = True

            If Not IsNothing(DetalleSeleccionado) Then
                If DetalleSeleccionado.logAplicado Then
                    HabilitarBorrarDetalle = False
                Else
                    HabilitarBorrarDetalle = True
                End If
            End If

            IsBusy = False
        End If
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxy.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezado = False
                HabilitarEdicionDetalle = False
                HabilitarBorrarDetalle = False
                logNuevoRegistro = False
                HabilitarReceptor = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        MyBase.CancelarBuscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0008
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Julio 31/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Julio 31/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra el movimiento participacción fondo seleccionado. ¿Confirma el borrado de este movimiento?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcutá cuando el usuario confirma la eliminación del encabezado activo.
    ''' Ejecuta el proceso de eliminación en la base de datos
    ''' </summary>
    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True


                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    If mdcProxy.MovimientosParticipacionesFondos.Where(Function(i) i.intIDMovimientosParticipacionesFondos = EncabezadoSeleccionado.intIDMovimientosParticipacionesFondos).Count > 0 Then
                        mdcProxy.MovimientosParticipacionesFondos.Remove(mdcProxy.MovimientosParticipacionesFondos.Where(Function(i) i.intIDMovimientosParticipacionesFondos = EncabezadoSeleccionado.intIDMovimientosParticipacionesFondos).First)
                    End If

                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                        EncabezadoSeleccionado.logEsNuevoRegistro = False
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If

                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)

                    'Await ConsultarEncabezado(True, String.Empty)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta el nombre y la fecha de portafolio de un cliente.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function ConsultarDatosPortafolio() As Task
        Try
            Dim objRet As LoadOperation(Of DatosPortafolios)
            Dim dcProxy As CalculosFinancierosDomainContext

            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarDatosPortafolioSyncQuery(_EncabezadoSeleccionado.lngIDPortafolio, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        EncabezadoSeleccionado.strPortafolio = objRet.Entities.First.strNombre
                        EncabezadoSeleccionado.dtmFechaCierrePortafolio = objRet.Entities.First.dtmFechaCierrePortafolio
                    Else
                        EncabezadoSeleccionado.strPortafolio = Nothing
                    End If
                End If
            Else
                EncabezadoSeleccionado.strPortafolio = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosPortafolio. ", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' JERF20190520 Se agrega logica para mostrar el buscardor de receptores 
    ''' </summary>
    Public Sub BuscarReceptor()
        If Not IsNothing(EncabezadoSeleccionado) Then
            mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("Receptores", "receptores", "receptores", BuscadorGenericoViewModel.EstadosItem.A, EncabezadoSeleccionado.strIDReceptor, "", "")
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
            mobjBuscadorLst.ShowDialog()
        End If
    End Sub

    Public Sub LimpiarReceptor()
        If Not IsNothing(EncabezadoSeleccionado) Then
            EncabezadoSeleccionado.strIDReceptor = Nothing
            EncabezadoSeleccionado.strNombreReceptor = Nothing
        End If
    End Sub


#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaMovimientosParticipacionesFondos
            objCB.intCuenta = String.Empty
            objCB.lngIDPortafolio = String.Empty
            objCB.strFondo = String.Empty
            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar los datos de la forma de búsqueda", Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Retorna una copia del encabezado activo. 
    ''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
    ''' </summary>
    Private Function ObtenerRegistroAnterior() As MovimientosParticipacionesFondos
        Dim objEncabezado As MovimientosParticipacionesFondos = New MovimientosParticipacionesFondos

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then

                Program.CopiarObjeto(Of MovimientosParticipacionesFondos)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDMovimientosParticipacionesFondos = _EncabezadoSeleccionado.intIDMovimientosParticipacionesFondos
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    ''' <history>
    ''' ID caso de prueba: CP0007
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Julio 31/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Julio 31/2015 - Resultado Ok 
    ''' </history>
    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then

                'Valida la cuenta
                If (_EncabezadoSeleccionado.intCuenta) = String.Empty Then
                    strMsg = String.Format("{0}{1} + La cuenta es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el código
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.lngIDPortafolio) And String.IsNullOrEmpty(_EncabezadoSeleccionado.strPortafolio) Then
                    strMsg = String.Format("{0}{1} + El " + NOMBRE_ETIQUETA_COMITENTE.ToLower + " es un campo requerido.", strMsg, vbCrLf)
                End If

                If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.lngIDPortafolio) And String.IsNullOrEmpty(_EncabezadoSeleccionado.strPortafolio) Then
                    strMsg = String.Format("{0}{1} + El " + NOMBRE_ETIQUETA_COMITENTE.ToLower + " no existe o no es válido.", strMsg, vbCrLf)
                End If

                'Valida el fondo
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strFondo) Then
                    strMsg = String.Format("{0}{1} + EL fondo es un campo requerido.", strMsg, vbCrLf)
                End If

                If logNuevoRegistro Then
                    'Valida la moneda
                    If String.IsNullOrEmpty(_EncabezadoSeleccionado.strMoneda) Then
                        strMsg = String.Format("{0}{1} + La moneda es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If

            Else
                strMsg = String.Format("{0}{1} Debe de seleccionar un registro", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function


    ''' <summary>
    ''' JERF20190520 Metodo donde se asigna el receptor seleccionado en el buscador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mobjBuscadorLst_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscadorLst.Closed
        If Not mobjBuscadorLst.ItemSeleccionado Is Nothing Then
            EncabezadoSeleccionado.strIDReceptor = mobjBuscadorLst.ItemSeleccionado.IdItem
            EncabezadoSeleccionado.strNombreReceptor = mobjBuscadorLst.ItemSeleccionado.Nombre
        End If
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean
        Return True
    End Function
#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando finaliza la ejecución de una actualización a la base de datos
    ''' </summary>
    Public Overrides Async Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Dim strMsg As String = String.Empty

        Try
            IsBusy = False
            If So.HasError Then
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If Not So.Error Is Nothing Then
                        strMsg = So.Error.Message
                    End If
                End If

                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.obtenerMensajeValidacion(strMsg, So.UserState.ToString, True), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                ' Marcar los cambios como rechazados
                mdcProxy.RejectChanges()

                If So.UserState.Equals(ValoresUserState.Borrar.ToString) Then
                    ' Cuando se elimina un registro, el método RejectChanges lo vuelve a adicionar a la lista pero al final no en la posición original
                    _ListaEncabezadoPaginada.MoveToLastPage()
                    _ListaEncabezadoPaginada.MoveCurrentToLast()
                    MyBase.CambioItem("ListaEncabezadoPaginada")
                End If

                So.MarkErrorAsHandled()
            Else
                HabilitarEncabezado = False
                HabilitarEdicionDetalle = False
                logExisteApertura = False
                logExisteCancelacion = False
                HabilitarReceptor = False

                MyBase.TerminoSubmitChanges(So)
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del resultado de la actualización de los datos", Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
            If Not So Is Nothing Then
                If So.HasError Then
                    So.MarkErrorAsHandled()
                End If
            End If
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of MovimientosParticipacionesFondos)
        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarMovimientosParticipacionesFondosPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjEncabezadoPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjEncabezadoPorDefecto = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de MovimientosParticipacionesFondos
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pintCuenta As String = "",
                                               Optional ByVal pstrPortafolio As String = "",
                                               Optional ByVal pstrFondo As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of MovimientosParticipacionesFondos)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.MovimientosParticipacionesFondos.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarMovimientosParticipacionesFondosSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarMovimientosParticipacionesFondosSyncQuery(pintCuenta:=pintCuenta,
                                                                                                         pstrPortafolio:=pstrPortafolio,
                                                                                                         pstrFondo:=pstrFondo,
                                                                                                         pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            End If

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaEncabezado = mdcProxy.MovimientosParticipacionesFondos

                    If Not IsNothing(EncabezadoSeleccionado) Then
                        Await ConsultarDatosPortafolio()
                    End If

                    If objRet.Entities.Count > 0 Then
                        If Not plogFiltrar Then
                            MyBase.ConfirmarBuscar()
                        End If
                    Else
                        If Not plogFiltrar Or (plogFiltrar And Not pstrFiltro.Equals(String.Empty)) Then
                            ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                            A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos que concuerden con los criterios de búsqueda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            Else
                ListaEncabezado.Clear()
            End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de MovimientosParticipacionesFondos ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos públicos del detalle - REQUERIDO (si hay detalle)"
#Region "Nuevo Codigo desarrollo reglas"

    Dim cantidadTotalConfirmacion As Integer = 0
    Dim cantidadTotalJustificacion As Integer = 0
    Dim CantidadTotalAprobaciones As Integer = 0


    Private Sub InicializarObjValidaciones()
        Dim AuxObj As ObjetosValidacion = New ObjetosValidacion()
        AuxObj.Aprobaciones = String.Empty
        AuxObj.AprobacionesUsuario = String.Empty
        AuxObj.Confirmaciones = String.Empty
        AuxObj.ConfirmacionesUsuario = String.Empty
        AuxObj.Justificaciones = String.Empty
        AuxObj.JustificacionesUsuario = String.Empty

        ObjValidacionesDetalle = AuxObj

    End Sub
    Private _ObjValidacionesDetalle As ObjetosValidacion
    Public Property ObjValidacionesDetalle() As ObjetosValidacion
        Get
            Return _ObjValidacionesDetalle
        End Get
        Set(ByVal value As ObjetosValidacion)
            _ObjValidacionesDetalle = value
        End Set
    End Property


    Private _CantidadJustificaciones As Integer = 0
    Public Property CantidadJustificaciones() As Integer
        Get
            Return _CantidadJustificaciones
        End Get
        Set(ByVal value As Integer)
            _CantidadJustificaciones = value
            If CantidadJustificaciones > 0 Then
                If CantidadConfirmaciones = cantidadTotalConfirmacion And CantidadJustificaciones = cantidadTotalJustificacion And CantidadAprobaciones = CantidadTotalAprobaciones Then
                    GuardarRegistro()
                End If
            End If
        End Set
    End Property

    Private _CantidadAprobaciones As Integer = 0
    Public Property CantidadAprobaciones() As Integer
        Get
            Return _CantidadAprobaciones
        End Get
        Set(ByVal value As Integer)
            _CantidadAprobaciones = value
            If CantidadAprobaciones > 0 Then
                If CantidadConfirmaciones = cantidadTotalConfirmacion And CantidadJustificaciones = cantidadTotalJustificacion And CantidadAprobaciones = CantidadTotalAprobaciones Then
                    GuardarRegistro()
                End If
            End If
        End Set
    End Property

    Private _CantidadConfirmaciones As Integer = 0
    Public Property CantidadConfirmaciones() As Integer
        Get
            Return _CantidadConfirmaciones
        End Get
        Set(ByVal value As Integer)
            _CantidadConfirmaciones = value
            If CantidadConfirmaciones > 0 Then
                If CantidadConfirmaciones = cantidadTotalConfirmacion And CantidadJustificaciones = cantidadTotalJustificacion And CantidadAprobaciones = CantidadTotalAprobaciones Then
                    GuardarRegistro()
                End If
            End If
        End Set
    End Property

    Private Enum TIPOMENSAJEUSUARIO
        CONFIRMACION
        JUSTIFICACION
        APROBACION
        TODOS
    End Enum
    Private Function ValidarEstadoDetalle(ByVal strEstado As String) As Boolean
        Dim Retorno As Boolean
        Retorno = True

        Select Case strEstado
            Case Is = "P"
                Retorno = False
            Case Is = "X"
                Retorno = False
        End Select
        Return Retorno
    End Function


    Private Sub ValidarMensajesMostrarUsuario(ByVal pobjTipoMensaje As TIPOMENSAJEUSUARIO, Optional ByVal pobjResultaUsuario As A2Utilidades.wppMensajePregunta = Nothing)
        Try
            IsBusy = True

            If Not IsNothing(_ListaResultadoValidacion) Then
                Dim logEsHtml As Boolean = False
                Dim strMensajeDetallesHtml As String = String.Empty
                Dim strMensajeRetornoHtml As String = String.Empty
                Dim ListaJustificacion As New List(Of A2Utilidades.CausasJustificacionMensajePregunta)
                Dim strMensajeEnviar As String = String.Empty
                Dim strConfirmacion As String = String.Empty
                Dim ConfirmacionesEnviarMensaje As String = String.Empty
                Dim strRegla As String = String.Empty
                Dim strNombreRegla As String = String.Empty
                Dim strTipoRespuesta As String = String.Empty
                Dim logPermiteSinItemLista As Boolean = True
                Dim logPermiteSinObservacion As Boolean = True


                If pobjTipoMensaje <> TIPOMENSAJEUSUARIO.TODOS Then

                    If pobjResultaUsuario.DialogResult Then
                        If pobjTipoMensaje = TIPOMENSAJEUSUARIO.CONFIRMACION Then
                            CantidadConfirmaciones += 1
                        ElseIf pobjTipoMensaje = TIPOMENSAJEUSUARIO.JUSTIFICACION Then
                            ' ListaValidacionesDetalle.Where(Function(i) i.IDDetalle = IDDetalleActual).First
                            If String.IsNullOrEmpty(ObjValidacionesDetalle.Justificaciones) Then
                                ObjValidacionesDetalle.Justificaciones = pobjResultaUsuario.CodConfirmacion
                                ObjValidacionesDetalle.JustificacionesUsuario = String.Format("'{0}'**'{1}'**'{2}'**'{3}'", pobjResultaUsuario.NombreRegla, pobjResultaUsuario.CodRegla, pobjResultaUsuario.MensajeRegla, String.Format("{0}++{1}", pobjResultaUsuario.Observaciones, pobjResultaUsuario.TextoConfirmacion.Replace("|", "++")))
                            Else
                                ObjValidacionesDetalle.Justificaciones = String.Format("{0}|{1}", ObjValidacionesDetalle.Justificaciones, pobjResultaUsuario.CodConfirmacion)
                                ObjValidacionesDetalle.JustificacionesUsuario = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'", ObjValidacionesDetalle.JustificacionesUsuario, pobjResultaUsuario.NombreRegla, pobjResultaUsuario.CodRegla, pobjResultaUsuario.MensajeRegla, String.Format("{0}++{1}", pobjResultaUsuario.Observaciones, pobjResultaUsuario.TextoConfirmacion.Replace("|", "++")))
                            End If

                            CantidadJustificaciones += 1
                        ElseIf pobjTipoMensaje = TIPOMENSAJEUSUARIO.APROBACION Then
                            'ListaValidacionesDetalle.Where(Function(i) i.IDDetalle = IDDetalleActual).First
                            If String.IsNullOrEmpty(ObjValidacionesDetalle.Aprobaciones) Then
                                ObjValidacionesDetalle.Aprobaciones = pobjResultaUsuario.CodConfirmacion
                                ObjValidacionesDetalle.AprobacionesUsuario = String.Format("'{0}'**'{1}'**'{2}'**'{3}'", pobjResultaUsuario.NombreRegla, pobjResultaUsuario.CodRegla, pobjResultaUsuario.MensajeRegla, String.Format("{0}++{1}", pobjResultaUsuario.Observaciones, pobjResultaUsuario.TextoConfirmacion.Replace("|", "++")))
                            Else
                                ObjValidacionesDetalle.Aprobaciones = String.Format("{0}|{1}", ObjValidacionesDetalle.Aprobaciones, pobjResultaUsuario.CodConfirmacion)
                                ObjValidacionesDetalle.AprobacionesUsuario = String.Format("{0}|'{1}'**'{2}'**'{3}'**'{4}'", ObjValidacionesDetalle.AprobacionesUsuario, pobjResultaUsuario.NombreRegla, pobjResultaUsuario.CodRegla, pobjResultaUsuario.MensajeRegla, String.Format("{0}++{1}", pobjResultaUsuario.Observaciones, pobjResultaUsuario.TextoConfirmacion.Replace("|", "++")))
                            End If

                            CantidadAprobaciones += 1
                        End If
                    Else
                        IsBusy = False
                        LimpiarVariablesConfirmadas()

                        Exit Sub
                    End If
                Else
                    If ListaResultadoValidacion.Where(Function(i) i.RequiereConfirmacion = True).Count > 0 Then
                        cantidadTotalConfirmacion = 1
                    End If
                    cantidadTotalJustificacion = ListaResultadoValidacion.Where(Function(i) i.RequiereJustificacion = True).Count
                    CantidadTotalAprobaciones = ListaResultadoValidacion.Where(Function(i) i.RequiereAprobacion = True).Count

                    'ListaValidacionesDetalle = objListaValidacion
                End If

                If CantidadConfirmaciones < cantidadTotalConfirmacion Then
                    strTipoRespuesta = "PREGUNTARCONFIRMACION"
                    Dim MensajeConfirmacion As String = String.Empty


                    If ListaResultadoValidacion.Where(Function(i) i.RequiereConfirmacion = True).Count > 0 Then
                        cantidadTotalConfirmacion = 1
                    End If

                    For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereConfirmacion = True).ToList
                        ' ListaValidacionesDetalle.Where(Function(i) i.IDDetalle = CInt(li.IDOrdenIdentity)).First

                        If String.IsNullOrEmpty(ObjValidacionesDetalle.Confirmaciones) Then
                            ObjValidacionesDetalle.Confirmaciones = String.Format("'{0}'", li.Confirmacion)
                            ObjValidacionesDetalle.ConfirmacionesUsuario = String.Format("'{0}'**'{1}'**'{2}'", li.NombreRegla, li.Regla, li.Mensaje)
                            MensajeConfirmacion = String.Format(" -> {0}", li.Mensaje)
                        Else
                            ObjValidacionesDetalle.Confirmaciones = String.Format("{0}|'{1}'", ObjValidacionesDetalle.Confirmaciones, li.Confirmacion)
                            ObjValidacionesDetalle.ConfirmacionesUsuario = String.Format("{0}|'{1}'**'{2}'**'{3}'", ObjValidacionesDetalle.ConfirmacionesUsuario, li.NombreRegla, li.Regla, li.Mensaje)
                            MensajeConfirmacion = String.Format("{0}{1} -> {2}", MensajeConfirmacion, vbCrLf, li.Mensaje)
                        End If

                        If String.IsNullOrEmpty(ConfirmacionesEnviarMensaje) Then
                            ConfirmacionesEnviarMensaje = String.Format("'{0}'", li.Confirmacion)
                        Else
                            ConfirmacionesEnviarMensaje = String.Format("{0}|'{1}'", ConfirmacionesEnviarMensaje, li.Confirmacion)
                        End If

                        strMensajeRetornoHtml = String.Format("{0}{1}", strMensajeRetornoHtml, li.DetalleRegla)
                    Next

                    If Not String.IsNullOrEmpty(strMensajeRetornoHtml) Then
                        logEsHtml = True
                        strMensajeDetallesHtml = String.Format("<HTML><HEAD></HEAD><BODY>{0}</BODY></HTML>", strMensajeRetornoHtml)
                    Else
                        logEsHtml = False
                        strMensajeDetallesHtml = String.Empty
                    End If

                    MensajeConfirmacion = Replace(MensajeConfirmacion, "-", vbCrLf)
                    MensajeConfirmacion = Replace(MensajeConfirmacion, "--", vbCrLf)

                    strMensajeEnviar = MensajeConfirmacion
                    strConfirmacion = ConfirmacionesEnviarMensaje
                    strRegla = String.Empty
                    strNombreRegla = String.Empty

                ElseIf CantidadJustificaciones < cantidadTotalJustificacion Then

                    strTipoRespuesta = "PREGUNTARJUSTIFICACION"

                    For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereJustificacion = True).ToList
                        'ListaValidacionesDetalle.Where(Function(i) i.IDDetalle = CInt(li.IDOrdenIdentity)).First
                        If Not ObjValidacionesDetalle.Justificaciones.Contains(li.Confirmacion) Then
                            ListaJustificacion.Clear()

                            If Not String.IsNullOrEmpty(li.CausasJustificacion) Then
                                For Each item In li.CausasJustificacion.Split(CChar("|"))
                                    ListaJustificacion.Add(New A2Utilidades.CausasJustificacionMensajePregunta With {.ID = item,
                                                                                                                     .Descripcion = item})
                                Next
                            End If

                            If Not String.IsNullOrEmpty(li.DetalleRegla) Then
                                logEsHtml = True
                                strMensajeDetallesHtml = String.Format("<HTML><HEAD></HEAD><BODY>{0}</BODY></HTML>", li.DetalleRegla)
                            Else
                                logEsHtml = False
                                strMensajeDetallesHtml = String.Empty
                            End If

                            strMensajeEnviar = li.Mensaje
                            strConfirmacion = li.Confirmacion
                            strRegla = li.Regla
                            strNombreRegla = li.NombreRegla
                            Exit For
                        End If
                    Next
                ElseIf CantidadAprobaciones < CantidadTotalAprobaciones Then
                    strTipoRespuesta = "PREGUNTARAPROBACION"


                    For Each li In ListaResultadoValidacion.Where(Function(i) i.RequiereAprobacion = True).ToList
                        'ListaValidacionesDetalle.Where(Function(i) i.IDDetalle = CInt(li.IDOrdenIdentity)).First
                        If Not ObjValidacionesDetalle.Aprobaciones.Contains(li.Confirmacion) Then
                            ListaJustificacion.Clear()

                            If Not String.IsNullOrEmpty(li.CausasJustificacion) Then
                                For Each item In li.CausasJustificacion.Split(CChar("|"))
                                    ListaJustificacion.Add(New A2Utilidades.CausasJustificacionMensajePregunta With {.ID = item,
                                                                                                                     .Descripcion = item})
                                Next
                            End If

                            If Not String.IsNullOrEmpty(li.DetalleRegla) Then
                                logEsHtml = True
                                strMensajeDetallesHtml = String.Format("<HTML><HEAD></HEAD><BODY>{0}</BODY></HTML>", li.DetalleRegla)
                            Else
                                logEsHtml = False
                                strMensajeDetallesHtml = String.Empty
                            End If

                            strMensajeEnviar = li.Mensaje
                            strConfirmacion = li.Confirmacion
                            strRegla = li.Regla
                            strNombreRegla = li.NombreRegla

                            Exit For
                        End If
                    Next
                End If

                If Not String.IsNullOrEmpty(strMensajeEnviar) Then
                    If strTipoRespuesta <> "PREGUNTARCONFIRMACION" Then
                        If (ListaJustificacion.Count > 0) Then
                            logPermiteSinItemLista = False
                            logPermiteSinObservacion = True
                        Else
                            logPermiteSinItemLista = True
                            logPermiteSinObservacion = False
                        End If
                    Else
                        logPermiteSinItemLista = True
                        logPermiteSinObservacion = True
                    End If

                    A2Utilidades.Mensajes.mostrarMensajePregunta(strMensajeEnviar,
                                       Program.TituloSistema,
                                       strTipoRespuesta,
                                       AddressOf TerminoMensajePregunta,
                                       True,
                                       "¿Desea continuar?",
                                       CBool(IIf(ListaJustificacion.Count > 0, True, False)),
                                       CBool(IIf(ListaJustificacion.Count > 0, True, False)),
                                       logPermiteSinItemLista,
                                       logPermiteSinObservacion,
                                       strConfirmacion,
                                       strRegla,
                                       strNombreRegla,
                                       CType(IIf(ListaJustificacion.Count > 0, ListaJustificacion, Nothing), Global.System.Collections.Generic.List(Of Global.A2Utilidades.CausasJustificacionMensajePregunta)),
                                       "Reglas incumplidas en los detalles de las ordenes",
                                       logEsHtml,
                                       strMensajeDetallesHtml)
                End If

            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al momento de validar el mensaje para mostrar al usuario.", Me.ToString(), "ValidarMensajesMostrarUsuario", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    Select Case objResultado.CodigoLlamado.ToUpper
                        Case "PREGUNTARCONFIRMACION"
                            ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.CONFIRMACION, objResultado)
                        Case "PREGUNTARAPROBACION"
                            ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.APROBACION, objResultado)
                        Case "PREGUNTARJUSTIFICACION"
                            ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.JUSTIFICACION, objResultado)
                    End Select
                End If
            Else
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta del mensaje pregunta.", Me.ToString(), "TerminoMensajePregunta", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Limpia las variables de tipo string Confirmacion, Justificacion, Aprobacion y los contadores acumulados de cada uno.
    ''' Desarrollado por Juan David Correa
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LimpiarVariablesConfirmadas()
        Try
            CantidadAprobaciones = 0
            CantidadConfirmaciones = 0
            CantidadJustificaciones = 0

            CantidadTotalAprobaciones = 0
            cantidadTotalConfirmacion = 0
            cantidadTotalJustificacion = 0

            InicializarObjValidaciones()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los valores de las confirmaciones.", Me.ToString(), "LimpiarVariablesConfirmadas", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
#End Region


    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar del detalle 
    ''' </summary>
    Public Sub ActualizarDetalle()
        Try

            logExisteApertura = False
            logExisteCancelacion = False


            If Not IsNothing(DetalleSeleccionado) Then
                If ValidarEstadoDetalle(DetalleSeleccionado.strEstado) = True Then
                    If Not IsNothing(ListaDetalle) Then
                        For Each Lista In ListaDetalle
                            If Lista.strTipo = "AP" Or Lista.strDescripcionTipo.ToLower = "apertura" Then
                                logExisteApertura = True
                                intIDExisteApertura = Lista.intIDMovimientosParticipacionesFondosDetalle
                            End If
                            If Lista.strTipo = "CA" Or Lista.strDescripcionTipo.ToLower = "cancelación" Then
                                logExisteCancelacion = True
                                intIDExisteCancelacion = Lista.intIDMovimientosParticipacionesFondosDetalle
                            End If
                        Next

                        cwMovimientosParticipacionesFondosView = New cwMovimientosParticipacionesFondosView(Me, DetalleSeleccionado, mobjDetallePorDefecto, HabilitarEdicionDetalle, logExisteApertura, intIDExisteApertura, logExisteCancelacion, intIDExisteCancelacion, EncabezadoSeleccionado)
                        Program.Modal_OwnerMainWindowsPrincipal(cwMovimientosParticipacionesFondosView)
                        cwMovimientosParticipacionesFondosView.ShowDialog()

                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No es posible editar este detalle", "Movimientos participaciones fondos", wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la operación interbancaria", Me.ToString(), "ActualizarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle 
    ''' Solamente se ingresa un nuevo elemento en la lista del detalle para que el usuario ingrese el nuevo detalle
    ''' </summary>
    Public Sub IngresarDetalle()
        Try

            logExisteApertura = False
            logExisteCancelacion = False

            If Not IsNothing(ListaDetalle) Then

                For Each Lista In ListaDetalle
                    If Lista.strTipo = "AP" Or Lista.strDescripcionTipo.ToLower = "apertura" Then
                        logExisteApertura = True
                        intIDExisteApertura = Lista.intIDMovimientosParticipacionesFondosDetalle
                    End If
                    If Lista.strTipo = "CA" Or Lista.strDescripcionTipo.ToLower = "cancelación" Then
                        logExisteCancelacion = True
                        intIDExisteCancelacion = Lista.intIDMovimientosParticipacionesFondosDetalle
                    End If
                Next
            End If

            cwMovimientosParticipacionesFondosView = New cwMovimientosParticipacionesFondosView(Me, Nothing, mobjDetallePorDefecto, HabilitarEdicionDetalle, logExisteApertura, intIDExisteApertura, logExisteCancelacion, intIDExisteCancelacion, EncabezadoSeleccionado)
            Program.Modal_OwnerMainWindowsPrincipal(cwMovimientosParticipacionesFondosView)
            cwMovimientosParticipacionesFondosView.ShowDialog()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la operación interbancaria", Me.ToString(), "IngresarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle 
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    Public Sub BorrarDetalle()
        Try
            If ValidarEstadoDetalle(DetalleSeleccionado.strEstado) = True Then
                If Not IsNothing(DetalleSeleccionado) Then
                    If DetalleSeleccionado.logAplicado Then

                    End If
                    If _ListaDetalle.Where(Function(i) i.intIDMovimientosParticipacionesFondosDetalle = _DetalleSeleccionado.intIDMovimientosParticipacionesFondosDetalle).Count > 0 Then
                        _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) i.intIDMovimientosParticipacionesFondosDetalle = _DetalleSeleccionado.intIDMovimientosParticipacionesFondosDetalle).First)
                    End If

                    Dim objNuevaListaDetalle As New List(Of MovimientosParticipacionesFondosDetalle)

                    For Each li In _ListaDetalle
                        objNuevaListaDetalle.Add(li)
                    Next

                    If objNuevaListaDetalle.Where(Function(i) i.intIDMovimientosParticipacionesFondosDetalle = _DetalleSeleccionado.intIDMovimientosParticipacionesFondosDetalle).Count > 0 Then
                        objNuevaListaDetalle.Remove(objNuevaListaDetalle.Where(Function(i) i.intIDMovimientosParticipacionesFondosDetalle = _DetalleSeleccionado.intIDMovimientosParticipacionesFondosDetalle).First)
                    End If

                    ListaDetalle = objNuevaListaDetalle

                    If Not IsNothing(_ListaDetalle) Then
                        If _ListaDetalle.Count > 0 Then
                            DetalleSeleccionado = _ListaDetalle.First
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No es posible borrar este detalle", "Movimientos participaciones fondos", wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la operación interbancaria", Me.ToString(), "BorrarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del detalle - REQUERIDO (si hay detalle)"
    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle
    ''' </summary>
    Private Async Sub ConsultarDetallePorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of MovimientosParticipacionesFondosDetalle)

            objRet = Await dcProxy.Load(dcProxy.ConsultarMovimientosParticipacionesFondoDetallePorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Me.ToString(), "ConsultarDetallePorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjDetallePorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjDetallePorDefecto = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Me.ToString(), "ConsultarDetallePorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <remarks>NOTA: En este método no se puede utilizar la propiedad IsBusy porque hace que sean necesarios dos clic para seleccionar un detalle</remarks>
    Public Async Function ConsultarDetalle(ByVal pintCuenta As String, ByVal plngIDPortafolio As String, ByVal pstrFondo As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of MovimientosParticipacionesFondosDetalle)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.MovimientosParticipacionesFondosDetalles Is Nothing Then
                mdcProxy.MovimientosParticipacionesFondosDetalles.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarMovimientosParticipacionesFondoDetalleSyncQuery(pintCuenta, plngIDPortafolio, pstrFondo, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de la operación interbancaria pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la operación interbancaria.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalle = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de la operación interbancaria seleccionada.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function
#End Region

#Region "Métodos privados del detalle - REQUERIDO (si hay detalle)"
    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ValidarRegistro
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0007
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Julio 31/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Julio 31/2015 - Resultado Ok 
    ''' </history>
    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            '------------------------------------------------------------------------------------------------------------------------------------------------
            '-- Valida que por lo menos exista un detalle para poder crear todo un registro
            '------------------------------------------------------------------------------------------------------------------------------------------------
            If IsNothing(_ListaDetalle) Then
                strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
            ElseIf _ListaDetalle.Count = 0 Then
                strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
            End If

            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle.", Me.ToString(), "validarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

#End Region

End Class

''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
Public Class CamposBusquedaMovimientosParticipacionesFondos
    Implements INotifyPropertyChanged

    Private _intCuenta As String
    Public Property intCuenta() As String
        Get
            Return _intCuenta
        End Get
        Set(ByVal value As String)
            _intCuenta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intCuenta"))
        End Set
    End Property

    Private _lngIDPortafolio As String
    Public Property lngIDPortafolio() As String
        Get
            Return _lngIDPortafolio
        End Get
        Set(ByVal value As String)
            _lngIDPortafolio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDPortafolio"))
        End Set
    End Property

    Private _strPortafolio As String
    Public Property strPortafolio() As String
        Get
            Return _strPortafolio
        End Get
        Set(ByVal value As String)
            _strPortafolio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strPortafolio"))
        End Set
    End Property

    Private Async Sub _lngIDPortafolio_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
        If e.PropertyName = "lngIDPortafolio" Then
            Await ConsultarDatosPortafolioBusqueda()
        End If
    End Sub

    Private _strFondo As String
    Public Property strFondo() As String
        Get
            Return _strFondo
        End Get
        Set(ByVal value As String)
            _strFondo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strFondo"))
        End Set
    End Property

    Public Async Function ConsultarDatosPortafolioBusqueda() As Task
        Try
            Dim mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

            Dim objRet As LoadOperation(Of DatosPortafolios)

            mdcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarDatosPortafolioSyncQuery(lngIDPortafolio, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        strPortafolio = objRet.Entities.First.strNombre
                    Else
                        strPortafolio = Nothing
                    End If
                End If
            Else
                strPortafolio = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosPortafolio. ", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

Public Class ObjetosValidacion

    Private _Confirmaciones As String
    Public Property Confirmaciones() As String
        Get
            Return _Confirmaciones
        End Get
        Set(ByVal value As String)
            _Confirmaciones = value
        End Set
    End Property

    Private _ConfirmacionesUsuario As String
    Public Property ConfirmacionesUsuario() As String
        Get
            Return _ConfirmacionesUsuario
        End Get
        Set(ByVal value As String)
            _ConfirmacionesUsuario = value
        End Set
    End Property

    Private _Justificaciones As String
    Public Property Justificaciones() As String
        Get
            Return _Justificaciones
        End Get
        Set(ByVal value As String)
            _Justificaciones = value
        End Set
    End Property

    Private _JustificacionesUsuario As String
    Public Property JustificacionesUsuario() As String
        Get
            Return _JustificacionesUsuario
        End Get
        Set(ByVal value As String)
            _JustificacionesUsuario = value
        End Set
    End Property

    Private _Aprobaciones As String
    Public Property Aprobaciones() As String
        Get
            Return _Aprobaciones
        End Get
        Set(ByVal value As String)
            _Aprobaciones = value
        End Set
    End Property

    Private _AprobacionesUsuario As String
    Public Property AprobacionesUsuario() As String
        Get
            Return _AprobacionesUsuario
        End Get
        Set(ByVal value As String)
            _AprobacionesUsuario = value
        End Set
    End Property


End Class