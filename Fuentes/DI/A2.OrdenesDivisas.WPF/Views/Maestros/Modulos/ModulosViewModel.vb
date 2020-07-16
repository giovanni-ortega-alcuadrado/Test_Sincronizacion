' Desarrollo de órdenes y maestros de módulos genericos
' Santiago Alexander Vergara Orrego
' SV20180711_ORDENES

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web


Public Class ModulosViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As tblModulos
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Public mobjEncabezadoAnterior As tblModulos
    Dim strTipoFiltroBusqueda As String = String.Empty

    Public objViewPrincipal As ModulosView


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
                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico

                Await consultarEncabezadoPorDefecto()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado("FILTRAR", String.Empty)
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


    Private _ListaEncabezado As List(Of CPX_tblModulos)
    Public Property ListaEncabezado() As List(Of CPX_tblModulos)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblModulos))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value
            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Registros para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private _EncabezadoSeleccionado As CPX_tblModulos
    Public Property EncabezadoSeleccionado() As CPX_tblModulos
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblModulos)
            _EncabezadoSeleccionado = value
            'Llamado de metodo para realizar la consulta del encabezado editable
            ConsultarEncabezadoEdicion()
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As tblModulos
    Public Property EncabezadoEdicionSeleccionado() As tblModulos
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As tblModulos)
            _EncabezadoEdicionSeleccionado = value
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                If _EncabezadoEdicionSeleccionado.intId > 0 Then
                    HabilitarEdicionDetalle = True
                Else
                    HabilitarEdicionDetalle = False
                End If
            Else
                HabilitarEdicionDetalle = False
            End If
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    Private _ListaModulosEstados As List(Of CPX_tblModulosEstados)
    Public Property ListaModulosEstados() As List(Of CPX_tblModulosEstados)
        Get
            Return _ListaModulosEstados
        End Get
        Set(ByVal value As List(Of CPX_tblModulosEstados))
            _ListaModulosEstados = value
            MyBase.CambioItem("ListaModulosEstados ")
        End Set
    End Property


    Private _ListaModulosEstadosEliminar As List(Of CPX_tblModulosEstados)
    Public Property ListaModulosEstadosEliminar() As List(Of CPX_tblModulosEstados)
        Get
            Return _ListaModulosEstadosEliminar
        End Get
        Set(ByVal value As List(Of CPX_tblModulosEstados))
            _ListaModulosEstados = value
            MyBase.CambioItem("ListaModulosEstadosEliminar")
        End Set
    End Property


    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaModulos
    Public Property cb() As CamposBusquedaModulos
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaModulos)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _HabilitarEdicionDetalle As Boolean = False
    Public Property HabilitarEdicionDetalle() As Boolean
        Get
            Return _HabilitarEdicionDetalle And Editando
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicionDetalle = value
            MyBase.CambioItem("HabilitarEdicionDetalle")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Sub NuevoRegistro()
        Try
            Dim objNvoEncabezado As New tblModulos
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'Obtiene el registro por defecto
            ObtenerRegistroAnterior(mobjEncabezadoPorDefecto, objNvoEncabezado)
            'Salva el registro anterior
            ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

            Editando = True
            MyBase.CambioItem("Editando")
            MyBase.CambioItem("HabilitarEdicionDetalle")

            EncabezadoEdicionSeleccionado = objNvoEncabezado
            MyBase.NuevoRegistro()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Async Sub Filtrar()
        Try
            Await ConsultarEncabezado("FILTRAR", FiltroVM)
            strTipoFiltroBusqueda = "FILTRAR"
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub QuitarFiltro()
        MyBase.QuitarFiltro()
        strTipoFiltroBusqueda = String.Empty
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
    ''' <history>
    ''' </history>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            Await ConsultarEncabezado("BUSQUEDA", String.Empty, cb.intID, cb.strModulo, cb.strSubModulo)
            strTipoFiltroBusqueda = "BUSQUEDA"
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Async Sub ActualizarRegistro()
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try

            MyBase.ActualizarRegistro()

            ErrorForma = String.Empty
            IsBusy = True
            Dim strDetalleModulosEstados As String = String.Empty
            Dim strDetalleModulosEstadosEliminar As String = String.Empty

            If Not IsNothing(_ListaModulosEstados) Then
                strDetalleModulosEstados = "<ModulosEstados>  "
                For Each objDet In _ListaModulosEstados
                    Dim strXMLDetalle = <Estado intID=<%= objDet.intId %>
                                            intIdModulo=<%= objDet.intIdModulo %>
                                            strEstado=<%= objDet.strEstado %>
                                            logRestrictivo=<%= objDet.logRestrictivo %>
                                            dtmActualizacion=<%= objDet.dtmActualizacion %>
                                            strUsuario=<%= objDet.strUsuario %>
                                            >
                                        </Estado>

                    strDetalleModulosEstados = strDetalleModulosEstados & strXMLDetalle.ToString
                Next
                strDetalleModulosEstados = strDetalleModulosEstados & " </ModulosEstados>"
            End If

            If Not IsNothing(_ListaModulosEstadosEliminar) Then
                strDetalleModulosEstadosEliminar = "<ModulosEstados>  "
                For Each objDet In _ListaModulosEstadosEliminar
                    Dim strXMLDetalleEliminar = <Estado intID=<%= objDet.intId %>
                                                    >
                                                </Estado>

                    strDetalleModulosEstadosEliminar = strDetalleModulosEstadosEliminar & strXMLDetalleEliminar.ToString
                Next
                strDetalleModulosEstadosEliminar = strDetalleModulosEstadosEliminar & " </ModulosEstados>"
            End If
            Dim strXMLDetFormulario1 As String = System.Web.HttpUtility.UrlEncode(strDetalleModulosEstados)
            Dim strXMLDetEliminarFormulario1 As String = System.Web.HttpUtility.UrlEncode(strDetalleModulosEstadosEliminar)

            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesOrdenes)) = Await mdcProxy.Modulos_ActualizarAsync(_EncabezadoEdicionSeleccionado.intId,
                                                                                _EncabezadoEdicionSeleccionado.strModulo,
                                                                                _EncabezadoEdicionSeleccionado.strSubModulo,
                                                                                _EncabezadoEdicionSeleccionado.strDescripcion,
                                                                                 _EncabezadoEdicionSeleccionado.strNombreConsecutivo,
                                                                                Date.Now,
                                                                                strDetalleModulosEstados,
                                                                                strDetalleModulosEstadosEliminar,
                                                                                Program.Usuario,
                                                                                False)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    Dim objListaResultado As List(Of CPX_tblValidacionesOrdenes) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesOrdenes))
                    Dim objListaMensajes As New List(Of ProductoValidaciones)
                    Dim intIDRegistroActualizado As Integer = -1
                    Dim dtmRegistroActualizado As Date = Now

                    For Each li In objListaResultado
                        objListaMensajes.Add(New ProductoValidaciones With {
                                             .Campo = li.strCampo,
                                             .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                             .Mensaje = li.strMensaje
                                             })
                    Next

                    If (objListaResultado.All(Function(i) i.logInconsitencia = False)) Then
                        A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                        intIDRegistroActualizado = objListaResultado.First.intIDRegistro
                        dtmRegistroActualizado = objListaResultado.First.dtmRegistro
                        MyBase.FinalizoGuardadoRegistros()

                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        If strTipoFiltroBusqueda = "FILTRAR" Then
                            Await ConsultarEncabezado("FILTRAR", FiltroVM, intIDRegistroActualizado)
                        ElseIf strTipoFiltroBusqueda = "BUSQUEDA" And Not IsNothing(cb) Then
                            Await ConsultarEncabezado("BUSQUEDA", String.Empty, cb.intID, cb.strModulo, cb.strSubModulo)
                        Else
                            Await ConsultarEncabezado("FILTRAR", String.Empty, intIDRegistroActualizado, dtmRegistroActualizado)
                        End If
                    Else
                        Dim strMensaje As String = ""
                        For Each m In objListaMensajes
                            strMensaje += IIf(String.IsNullOrWhiteSpace(m.TituloCampo), "", m.TituloCampo + ":") + m.Mensaje + vbCrLf
                        Next
                        A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema, False)
                        IsBusy = False
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then

                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                IsBusy = True

                IniciaEditar()

                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PreguntarModificacion()
        If _EncabezadoEdicionSeleccionado IsNot Nothing Then
            'If Editando And _EncabezadoEdicionSeleccionado.intTipoOperacion = TipoOperacion.Modificacion Then
            '    A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("DIVISAS_FORMULARIOYAENVIADO"), DiccionarioMensajesPantalla("DIVISAS_TITULOFORMULARIOYAENVIADO"), "1", AddressOf ModificarFormulario)
            'End If
        End If
    End Sub


    Public Sub IniciaEditar()
        ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)
        Editando = True
        MyBase.CambioItem("Editando")
        MyBase.CambioItem("HabilitarEdicionDetalle")
        MyBase.EditarRegistro()
    End Sub



    Public Sub TerminoPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IniciaEditar()
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty
            mdcProxy.RejectChanges()
            Editando = False
            MyBase.CambioItem("Editando")
            MyBase.CambioItem("HabilitarEdicionDetalle")
            MyBase.CancelarEditarRegistro()

            EncabezadoEdicionSeleccionado = mobjEncabezadoAnterior
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Function consultarEncabezadoPorDefecto() As Task
        Try
            Dim objRespuesta As InvokeResult(Of tblModulos)

            objRespuesta = Await mdcProxy.Modulos_DefectoAsync(Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    mobjEncabezadoPorDefecto = objRespuesta.Value
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarEncabezadoPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de Registro
    ''' </summary>
    Private Async Function ConsultarEncabezado(ByVal pstrOpcion As String _
                                             , Optional ByVal pstrFiltro As String = "" _
                                             , Optional ByVal pintID As Integer = 0 _
                                            , Optional ByVal pstrModulo As String = Nothing _
                                            , Optional ByVal pstrSubModulo As String = Nothing _
                                            , Optional ByVal pstrUsuario As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblModulos)) = Nothing

            Dim objRespuestaEntidad As InvokeResult(Of tblModulos) = Nothing

            ErrorForma = String.Empty

            If pstrOpcion = "FILTRAR" Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRespuesta = Await mdcProxy.Modulos_FiltrarAsync(pstrFiltro, Program.Usuario)
            ElseIf pstrOpcion = "ID" Then
                objRespuestaEntidad = Await mdcProxy.Modulos_IDAsync(pintID, Program.Usuario)
            Else
                objRespuesta = Await mdcProxy.Modulos_ConsultarAsync(pintID, pstrModulo, pstrSubModulo, Program.Usuario)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    ListaEncabezado = objRespuesta.Value

                    If pintID > 0 Then
                        If ListaEncabezado.Where(Function(i) i.intId = pintID).Count > 0 Then
                            EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.intId = pintID).First
                        End If
                    End If

                    If ListaEncabezado.Count > 0 Then
                        If pstrOpcion = "BUSQUEDA" Then
                            MyBase.ConfirmarBuscar()
                        End If
                    Else
                        If (pstrOpcion = "FILTRAR" And Not String.IsNullOrEmpty(pstrFiltro)) Or (pstrOpcion = "BUSQUEDA") Then
                            ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                            A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_BUSQUEDANOEXITOSA"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Async Sub ConsultarEncabezadoEdicion()
        Try
            EncabezadoEdicionSeleccionado = Nothing

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.intId > 0 Then
                    Await ConsultarEncabezadoEdicion(_EncabezadoSeleccionado.intId)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos del Registro a editar
    ''' </summary>
    ''' <param name="pintId">Indica el ID de la entidad a consultar</param>
    Private Async Function ConsultarEncabezadoEdicion(ByVal pintId As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of tblModulos) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.Modulos_IDAsync(pintId, Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    EncabezadoEdicionSeleccionado = objRespuesta.Value
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezadoEdicion", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"


    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaModulos
            objCB.intId = Nothing
            objCB.strModulo = Nothing
            objCB.strSubModulo = Nothing
            cb = objCB

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Retorna una copia del encabezado activo. 
    ''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
    ''' </summary>
    ''' <history>
    ''' </history>
    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As tblModulos, ByRef pobjRegistroSalvar As tblModulos)
        Dim objEncabezado As tblModulos = New tblModulos

        Try
            If Not IsNothing(pobjRegistro) Then
                objEncabezado.intId = pobjRegistro.intId
                objEncabezado.strModulo = pobjRegistro.strModulo
                objEncabezado.strSubModulo = pobjRegistro.strSubModulo
                objEncabezado.strDescripcion = pobjRegistro.strDescripcion
                objEncabezado.strNombreConsecutivo = pobjRegistro.strNombreConsecutivo
                objEncabezado.strUsuario = pobjRegistro.strUsuario
                objEncabezado.dtmActualizacion = pobjRegistro.dtmActualizacion
                pobjRegistroSalvar = objEncabezado
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Modificación de las propeiedades del encabezado.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub _EncabezadoEdicionSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoEdicionSeleccionado.PropertyChanged
        Try
            If Editando Then
                'If e.PropertyName = "strProducto" Then
                '    If Not IsNothing(EncabezadoEdicionSeleccionado.) Then
                '        CargarControlDetalle()
                '    End If
                'End If
                '    _EncabezadoEdicionSeleccionado.dblValorUSD1 = _EncabezadoEdicionSeleccionado.dblValormoneda1 * _EncabezadoEdicionSeleccionado.dblTipoCambioMoneda1
                'ElseIf e.PropertyName = "dblValormoneda2" Then
                '    _EncabezadoEdicionSeleccionado.dblValorUSD2 = _EncabezadoEdicionSeleccionado.dblValormoneda2 * _EncabezadoEdicionSeleccionado.dblTipoCambioMoneda1
                'ElseIf e.PropertyName = "dblValormoneda2" Then
                '    _EncabezadoEdicionSeleccionado.dblValorUSD2 = _EncabezadoEdicionSeleccionado.dblValormoneda2 * _EncabezadoEdicionSeleccionado.dblTipoCambioMoneda1
                'ElseIf e.PropertyName = "intTipoOperacion" Then

                'End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "_EncabezadoEdicionSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region



#Region "Comandos"
    'Private WithEvents _ImprimirCmd As RelayCommand
    'Public ReadOnly Property ImprimirCmd() As RelayCommand
    '    Get
    '        If _ImprimirCmd Is Nothing Then
    '            _ImprimirCmd = New RelayCommand(AddressOf Imprimir)
    '        End If
    '        Return _ImprimirCmd
    '    End Get

    'End Property
#End Region

End Class




''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
''' <history>
''' 
''' </history>
Public Class CamposBusquedaModulos
    Implements INotifyPropertyChanged

    Private _intId As Integer
    Public Property intId() As Integer
        Get
            Return _intId
        End Get
        Set(ByVal value As Integer)
            _intId = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intId"))
        End Set
    End Property

    Private _strModulo As String
    Public Property strModulo() As String
        Get
            Return _strModulo
        End Get
        Set(ByVal value As String)
            _strModulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strModulo"))
        End Set
    End Property

    Private _strSubModulo As String
    Public Property strSubModulo() As String
        Get
            Return _strSubModulo
        End Get
        Set(ByVal value As String)
            _strSubModulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strSubModulo"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
