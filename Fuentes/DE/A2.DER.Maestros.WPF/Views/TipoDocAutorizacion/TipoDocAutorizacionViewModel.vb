
Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.WEB

''' <summary>
''' Métodos creados para la comunicación con el OPENRIA (DER_MaestrosDomainServices.vb y dbDER_Maestros.edmx)
''' Pantalla Instrumentos (Maestros)
''' </summary>
''' <remarks>Jhon Alexis Echavarria (Alcuadrado S.A.) - 20 de Marzo 2018</remarks>
''' <history>
'''
'''</history>
Public Class TipoDocAutorizacionViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As tblTipoDocAutorizacion
    Private mdcProxy As DER_MaestrosDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As tblTipoDocAutorizacion
    Dim strTipoFiltroBusqueda As String = String.Empty

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
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID caso de prueba: Id_1
    ''' Creado por       : Jhon Alexis Echavarria (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Marzo 20/2018
    ''' Pruebas CB       :  Jhon Alexis Echavarria (Alcuadrado S.A.) -Marzo 20/2018 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefecto()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado("FILTRAR", String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de Registros que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As List(Of CPX_tblTipoDocAutorizacion)
    Public Property ListaEncabezado() As List(Of CPX_tblTipoDocAutorizacion)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblTipoDocAutorizacion))
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
    Private _EncabezadoSeleccionado As CPX_tblTipoDocAutorizacion
    Public Property EncabezadoSeleccionado() As CPX_tblTipoDocAutorizacion
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblTipoDocAutorizacion)
            _EncabezadoSeleccionado = value
            'Llamado de metodo para realizar la consulta del encabezado editable
            ConsultarEncabezadoEdicion()
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As tblTipoDocAutorizacion
    Public Property EncabezadoEdicionSeleccionado() As tblTipoDocAutorizacion
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As tblTipoDocAutorizacion)
            _EncabezadoEdicionSeleccionado = value
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                If _EncabezadoEdicionSeleccionado.intidTipoDocAutorizacion > 0 Then
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

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaTipoDocAutorizacion
    Public Property cb() As CamposBusquedaTipoDocAutorizacion
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaTipoDocAutorizacion)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _HabilitarEdicionDetalle As Boolean = False
    Public Property HabilitarEdicionDetalle() As Boolean
        Get
            Return _HabilitarEdicionDetalle
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
            Dim objNvoEncabezado As New tblTipoDocAutorizacion
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
            If cb.intidTipoProducto > 0 Or Not String.IsNullOrEmpty(cb.strCodigoTipoDocumento) Then
                Await ConsultarEncabezado("BUSQUEDA", String.Empty, 0, cb.intidTipoProducto, cb.strCodigoTipoDocumento)
                strTipoFiltroBusqueda = "BUSQUEDA"
            End If
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
    ''' 

    Public Overrides Async Sub ActualizarRegistro()
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            MyBase.ActualizarRegistro()

            ErrorForma = String.Empty
            IsBusy = True

            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.TipoDocAutorizacion_ActualizarAsync(_EncabezadoEdicionSeleccionado.intidTipoDocAutorizacion,
                                                                                                                                            _EncabezadoEdicionSeleccionado.intidTipoProducto,
                                                                                                                                            _EncabezadoEdicionSeleccionado.strCodigoTipoDocumento,
                                                                                                                                             False,
                                                                                                                                             Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
                    Dim objListaMensajes As New List(Of ProductoValidaciones)
                    Dim intIDRegistroActualizado As Integer = -1

                    For Each li In objListaResultado
                        objListaMensajes.Add(New ProductoValidaciones With {
                                             .Campo = li.strCampo,
                                             .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                             .Mensaje = li.strMensaje
                                             })
                    Next

                    If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                        intIDRegistroActualizado = objListaResultado.First.intIDRegistro

                        MyBase.FinalizoGuardadoRegistros()

                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        If strTipoFiltroBusqueda = "FILTRAR" Then
                            Await ConsultarEncabezado("FILTRAR", FiltroVM, intIDRegistroActualizado)
                        ElseIf strTipoFiltroBusqueda = "BUSQUEDA" And Not IsNothing(cb) Then
                            Await ConsultarEncabezado("BUSQUEDA", String.Empty, Nothing, cb.intidTipoProducto, cb.strCodigoTipoDocumento)
                        Else
                            Await ConsultarEncabezado("FILTRAR", String.Empty, intIDRegistroActualizado)
                        End If
                    Else
                        MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
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

                ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

                Editando = True
                MyBase.CambioItem("Editando")
                MyBase.EditarRegistro()

                IsBusy = False
            End If
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
            MyBase.CancelarEditarRegistro()

            EncabezadoEdicionSeleccionado = mobjEncabezadoAnterior
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        Try
            MyBase.BorrarRegistro()

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("GENERICO_ELIMINARREGISTRO"), Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim strAccion As String = ValoresUserState.Actualizar.ToString()

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.TipoDocAutorizacion_EliminarAsync(_EncabezadoSeleccionado.intID, Program.Usuario, Program.HashConexion)

                    If Not IsNothing(objRespuesta) Then
                        If Not IsNothing(objRespuesta.Value) Then
                            Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
                            Dim objListaMensajes As New List(Of ProductoValidaciones)

                            For Each li In objListaResultado
                                objListaMensajes.Add(New ProductoValidaciones With {
                                             .Campo = li.strCampo,
                                             .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                             .Mensaje = li.strMensaje
                                             })
                            Next
                            If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)

                                EncabezadoSeleccionado = Nothing

                                Await ConsultarEncabezado(True, String.Empty)
                            Else
                                MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
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
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedatipoDocAutorizacion
            objCB.intidTipoProducto = -1
            objCB.strCodigoTipoDocumento = ""
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
    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As tblTipoDocAutorizacion, ByRef pobjRegistroSalvar As tblTipoDocAutorizacion)
        Dim objEncabezado As tblTipoDocAutorizacion = New tblTipoDocAutorizacion

        Try
            If Not IsNothing(pobjRegistro) Then

                objEncabezado.intidTipoDocAutorizacion = pobjRegistro.intidTipoDocAutorizacion
                objEncabezado.intidTipoProducto = pobjRegistro.intidTipoProducto
                objEncabezado.strCodigoTipoDocumento = pobjRegistro.strCodigoTipoDocumento
                objEncabezado.dtmActualizacion = pobjRegistro.dtmActualizacion
                objEncabezado.strUsuario = pobjRegistro.strUsuario

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
                If e.PropertyName = "" Then

                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "_EncabezadoEdicionSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefecto()
        Try
            Dim objRespuesta As InvokeResult(Of tblTipoDocAutorizacion)

            objRespuesta = Await mdcProxy.TipoDocAutorizacion_DefectoAsync(Program.Usuario, Program.HashConexion)

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
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de Registro
    ''' </summary>
    ''' <param name="pstrOpcion">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    Private Async Function ConsultarEncabezado(ByVal pstrOpcion As String,
                                               Optional ByVal pstrFiltro As String = "",
                                               Optional ByVal pintIDRegistro As Integer = 0,
                                               Optional ByVal pintTipoProducto As Integer = -1,
                                               Optional ByVal pstrCodigoTipoDocumento As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblTipoDocAutorizacion)) = Nothing

            ErrorForma = String.Empty

            If pstrOpcion = "FILTRAR" Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRespuesta = Await mdcProxy.TipoDocAutorizacion_FiltrarAsync(pstrFiltro, Program.Usuario, Program.HashConexion)

            Else
                pstrCodigoTipoDocumento = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrCodigoTipoDocumento, String.Empty))

                objRespuesta = Await mdcProxy.TipoDocAutorizacion_ConsultarAsync(pstrCodigoTipoDocumento, pintTipoProducto, Program.Usuario, Program.HashConexion)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    ListaEncabezado = objRespuesta.Value

                    If pintIDRegistro > 0 Then
                        If ListaEncabezado.Where(Function(i) i.intID = pintIDRegistro).Count > 0 Then
                            EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.intID = pintIDRegistro).First
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
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Async Sub ConsultarEncabezadoEdicion()
        Try
            EncabezadoEdicionSeleccionado = Nothing

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.intID > 0 Then
                    Await ConsultarEncabezadoEdicion(_EncabezadoSeleccionado.intID)
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
    ''' <param name="pintID">Indica el ID de la entidad a consultar</param>
    Private Async Function ConsultarEncabezadoEdicion(ByVal pintID As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of tblTipoDocAutorizacion) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.TipoDocAutorizacion_IDAsync(pintID, Program.Usuario, Program.HashConexion)

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
Public Class CamposBusquedatipoDocAutorizacion
    Implements INotifyPropertyChanged

    Private _intidTipoProducto As Integer
    Public Property intidTipoProducto() As Integer
        Get
            Return _intidTipoProducto
        End Get
        Set(ByVal value As Integer)
            _intidTipoProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intidTipoProducto"))
        End Set
    End Property

    Private _strCodigoTipoDocumento As String
    Public Property strCodigoTipoDocumento() As String
        Get
            Return _strCodigoTipoDocumento
        End Get
        Set(ByVal value As String)
            _strCodigoTipoDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCodigoTipoDocumento"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class