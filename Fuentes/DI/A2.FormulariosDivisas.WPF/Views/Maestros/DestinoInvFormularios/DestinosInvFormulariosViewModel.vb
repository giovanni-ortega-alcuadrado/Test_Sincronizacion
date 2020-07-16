
'-- Santiago Alexander Vergara Orrego
'-- Septiembre 18/ 2018
'-- SV20180918_DESTINOSINVERSION

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web


Public Class DestinosInvFormulariosViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As tblDestinosInvFormulariosDivisas
    Private mdcProxy As FormulariosDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Public mobjEncabezadoAnterior As tblDestinosInvFormulariosDivisas
    Dim strTipoFiltroBusqueda As String = String.Empty

    Public objViewPrincipal As DestinosInvFormulariosView


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


    Private _ListaEncabezado As List(Of CPX_tblDestinosInvFormulariosDivisas)
    Public Property ListaEncabezado() As List(Of CPX_tblDestinosInvFormulariosDivisas)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblDestinosInvFormulariosDivisas))
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
    Private _EncabezadoSeleccionado As CPX_tblDestinosInvFormulariosDivisas
    Public Property EncabezadoSeleccionado() As CPX_tblDestinosInvFormulariosDivisas
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblDestinosInvFormulariosDivisas)
            _EncabezadoSeleccionado = value
            'Llamado de metodo para realizar la consulta del encabezado editable
            ConsultarEncabezadoEdicion()
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As tblDestinosInvFormulariosDivisas
    Public Property EncabezadoEdicionSeleccionado() As tblDestinosInvFormulariosDivisas
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As tblDestinosInvFormulariosDivisas)
            _EncabezadoEdicionSeleccionado = value
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaDestinosInvFormulariosDivisas
    Public Property cb() As CamposBusquedaDestinosInvFormulariosDivisas
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaDestinosInvFormulariosDivisas)
            _cb = value
            MyBase.CambioItem("cb")
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
            Dim objNvoEncabezado As New tblDestinosInvFormulariosDivisas
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
            Await ConsultarEncabezado("BUSQUEDA", String.Empty, cb.intId,
                                                      cb.intIdFormulario,
                                                      cb.intIdDestinoInversion,
                                                      cb.strGrupoFormulario,
                                                      cb.intNroCampoFormulario,
                                                      cb.strNombreCampo,
                                                      cb.intIdNumeralCambiario)
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

            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.DestinosInvFormulariosDivisas_ActualizarAsync(
                                                                                _EncabezadoEdicionSeleccionado.intId,
                                                                                _EncabezadoEdicionSeleccionado.intIdFormulario,
                                                                                _EncabezadoEdicionSeleccionado.intIdDestinoInversion,
                                                                                _EncabezadoEdicionSeleccionado.strGrupoFormulario,
                                                                                _EncabezadoEdicionSeleccionado.intNroCampoFormulario,
                                                                                _EncabezadoEdicionSeleccionado.strNombreCampo,
                                                                                _EncabezadoEdicionSeleccionado.intIdNumeralCambiario,
                                                                                _EncabezadoEdicionSeleccionado.logEditable,
                                                                                _EncabezadoEdicionSeleccionado.strNombreCampoFormulario,
                                                                                _EncabezadoEdicionSeleccionado.logEsRequerido,
                                                                                _EncabezadoEdicionSeleccionado.logAplicatodosDestinosInversion,
                                                                                _EncabezadoEdicionSeleccionado.logAplicatodosFormularios,
                                                                                _EncabezadoEdicionSeleccionado.logPermitido,
                                                                                _EncabezadoEdicionSeleccionado.logAplicatodosNumerales,
                                                                                Date.Now,
                                                                                False,
                                                                                Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
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
                        intIDRegistroActualizado = objListaResultado.First.intIdRegistro
                        dtmRegistroActualizado = objListaResultado.First.dtmRegistro
                        MyBase.FinalizoGuardadoRegistros()

                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        If strTipoFiltroBusqueda = "FILTRAR" Then
                            Await ConsultarEncabezado("FILTRAR", FiltroVM, intIDRegistroActualizado)
                        ElseIf strTipoFiltroBusqueda = "BUSQUEDA" And Not IsNothing(cb) Then
                            Await ConsultarEncabezado("BUSQUEDA",
                                                      String.Empty,
                                                      cb.intId,
                                                      cb.intIdFormulario,
                                                      cb.intIdDestinoInversion,
                                                      cb.strGrupoFormulario,
                                                      cb.intNroCampoFormulario,
                                                      cb.strNombreCampo,
                                                      cb.intIdNumeralCambiario)
                        Else
                            Await ConsultarEncabezado("FILTRAR", String.Empty, intIDRegistroActualizado)
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
                    Editando = False
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

    ''' <summary>
    ''' Iniciar la edición del registro
    ''' </summary>
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
            Dim objRespuesta As InvokeResult(Of tblDestinosInvFormulariosDivisas)

            objRespuesta = Await mdcProxy.DestinosInvFormulariosDivisas_DefectoAsync(Program.Usuario)

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
                                             , Optional ByVal pintID As Integer? = Nothing _
                                            , Optional ByVal pintIdFormulario As Integer? = Nothing _
                                            , Optional ByVal pintIdDestinoInversion As Integer? = Nothing _
                                            , Optional ByVal pstrGrupoFormulario As String = Nothing _
                                            , Optional ByVal pintNroCampoFormulario As Integer? = Nothing _
                                            , Optional ByVal pstrNombreCampo As String = Nothing _
                                            , Optional ByVal pintIdNumeralCambiario As Integer? = Nothing) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblDestinosInvFormulariosDivisas)) = Nothing

            Dim objRespuestaEntidad As InvokeResult(Of tblDestinosInvFormulariosDivisas) = Nothing

            ErrorForma = String.Empty

            If pstrOpcion = "FILTRAR" Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRespuesta = Await mdcProxy.DestinosInvFormulariosDivisas_FiltrarAsync(pstrFiltro, Program.Usuario)
            ElseIf pstrOpcion = "ID" Then
                objRespuestaEntidad = Await mdcProxy.DestinosInvFormulariosDivisas_IDAsync(pintID, Program.Usuario)
            Else
                objRespuesta = Await mdcProxy.DestinosInvFormulariosDivisas_ConsultarAsync(pintID, pintIdFormulario, pintIdDestinoInversion, pstrGrupoFormulario, pintNroCampoFormulario, pstrNombreCampo, pintIdNumeralCambiario, Program.Usuario)
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

    ''' <summary>
    ''' Consultar los datos del encabezado a editar
    ''' </summary>
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
            Dim objRespuesta As InvokeResult(Of tblDestinosInvFormulariosDivisas) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.DestinosInvFormulariosDivisas_IDAsync(pintId, Program.Usuario)

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

    ''' <summary>
    ''' Preparar nueva busqueda de registros
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaDestinosInvFormulariosDivisas
            objCB.intId = Nothing
            objCB.intIdFormulario = Nothing
            objCB.intIdDestinoInversion = Nothing
            objCB.strGrupoFormulario = Nothing
            objCB.intNroCampoFormulario = Nothing
            objCB.strNombreCampo = Nothing
            objCB.intIdNumeralCambiario = Nothing
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
    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As tblDestinosInvFormulariosDivisas, ByRef pobjRegistroSalvar As tblDestinosInvFormulariosDivisas)
        Dim objEncabezado As tblDestinosInvFormulariosDivisas = New tblDestinosInvFormulariosDivisas

        Try
            If Not IsNothing(pobjRegistro) Then
                objEncabezado.intId = pobjRegistro.intId
                objEncabezado.intIdFormulario = pobjRegistro.intIdFormulario
                objEncabezado.intIdDestinoInversion = pobjRegistro.intIdDestinoInversion
                objEncabezado.strGrupoFormulario = pobjRegistro.strGrupoFormulario
                objEncabezado.intNroCampoFormulario = pobjRegistro.intNroCampoFormulario
                objEncabezado.strNombreCampo = pobjRegistro.strNombreCampo
                objEncabezado.intIdNumeralCambiario = pobjRegistro.intIdNumeralCambiario
                objEncabezado.logEditable = pobjRegistro.logEditable
                objEncabezado.strNombreCampoFormulario = pobjRegistro.strNombreCampoFormulario
                objEncabezado.logEsRequerido = pobjRegistro.logEsRequerido
                objEncabezado.logAplicatodosDestinosInversion = pobjRegistro.logAplicatodosDestinosInversion
                objEncabezado.logAplicatodosFormularios = pobjRegistro.logAplicatodosFormularios
                objEncabezado.logPermitido = pobjRegistro.logPermitido
                objEncabezado.logAplicatodosNumerales = pobjRegistro.logAplicatodosNumerales
                objEncabezado.strUsuario = pobjRegistro.strUsuario
                objEncabezado.dtmActualizacion = pobjRegistro.dtmActualizacion
                pobjRegistroSalvar = objEncabezado
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    '''' <summary>
    '''' Llamado a procedimiento para validar procesos especiales de la forma
    '''' </summary>
    '''' <param name="strProceso"></param>
    'Public Async Sub Validar_Procesos(ByVal strProceso As String)
    '    Try
    '        Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.DestinosInvFormulariosDivisas_Validar_ProcesosAsync(
    '                                                                  _EncabezadoEdicionSeleccionado.intId,
    '                                                                  strProceso,
    '                                                                  Nothing,
    '                                                                  Program.Usuario)

    '        Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
    '        Dim objListaMensajes As New List(Of ProductoValidaciones)

    '        For Each li In objListaResultado
    '            objListaMensajes.Add(New ProductoValidaciones With {
    '                                             .Campo = li.strCampo,
    '                                             .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
    '                                             .Mensaje = li.strMensaje
    '                                             })
    '        Next

    '        If (objListaResultado.All(Function(i) i.logInconsitencia = False)) Then
    '            If strProceso = "EDICION" Then
    '                IniciaEditar()
    '            End If

    '        Else
    '            Editando = False
    '            If (objListaResultado.All(Function(i) i.strTipoMensaje = "INCONSISTENCIA")) Then

    '                Dim strMensaje As String = ""
    '                For Each m In objListaMensajes
    '                    strMensaje += IIf(String.IsNullOrWhiteSpace(m.TituloCampo), "", m.TituloCampo + ":") + m.Mensaje + vbCrLf
    '                Next
    '                A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
    '                MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema, False)
    '                IsBusy = False
    '            End If
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al hacer la validación del proceso.",
    '                             Me.ToString(), "Validar_Procesos", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try

    'End Sub


    ''' <summary>
    ''' Notificación de modificación de las propeiedades del encabezado.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub _EncabezadoEdicionSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoEdicionSeleccionado.PropertyChanged
        Try
            If Editando Then

                Select Case e.PropertyName

                End Select

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "_EncabezadoEdicionSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

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
Public Class CamposBusquedaDestinosInvFormulariosDivisas
    Implements INotifyPropertyChanged

    Private _intId As Integer?
    Public Property intId() As Integer?
        Get
            Return _intId
        End Get
        Set(ByVal value As Integer?)
            _intId = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intId"))
        End Set
    End Property

    Private _intIdFormulario As Integer?
    Public Property intIdFormulario() As Integer?
        Get
            Return _intIdFormulario
        End Get
        Set(ByVal value As Integer?)
            _intIdFormulario = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIdFormulario"))
        End Set
    End Property

    Private _intIdDestinoInversion As Integer?
    Public Property intIdDestinoInversion() As Integer?
        Get
            Return _intIdDestinoInversion
        End Get
        Set(ByVal value As Integer?)
            _intIdDestinoInversion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIdDestinoInversion"))
        End Set
    End Property

    Private _strGrupoFormulario As String
    Public Property strGrupoFormulario() As String
        Get
            Return _strGrupoFormulario
        End Get
        Set(ByVal value As String)
            _strGrupoFormulario = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strGrupoFormulario"))
        End Set
    End Property

    Private _intNroCampoFormulario As Integer?
    Public Property intNroCampoFormulario() As Integer?
        Get
            Return _intNroCampoFormulario
        End Get
        Set(ByVal value As Integer?)
            _intNroCampoFormulario = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intNroCampoFormulario"))
        End Set
    End Property

    Private _strNombreCampo As String
    Public Property strNombreCampo() As String
        Get
            Return _strNombreCampo
        End Get
        Set(ByVal value As String)
            _strNombreCampo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombreCampo"))
        End Set
    End Property

    Private _intIdNumeralCambiario As Integer?
    Public Property intIdNumeralCambiario() As Integer?
        Get
            Return _intIdNumeralCambiario
        End Get
        Set(ByVal value As Integer?)
            _intIdNumeralCambiario = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIdNumeralCambiario"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
