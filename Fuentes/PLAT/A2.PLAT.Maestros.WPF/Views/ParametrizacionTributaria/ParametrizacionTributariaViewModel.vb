
Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web

''' <summary>
''' Métodos creados para la comunicación con el OPENRIA (PLAT_MaestrosDomainServices.vb y dbPLAT_Maestros.edmx)
''' Pantalla Instrumentos (Maestros)
''' </summary>
''' <remarks>Natalia Andrea Otalvar (Alcuadrado S.A.) - Enero 24/2019</remarks>
''' <history>
'''
'''</history>
Public Class ParametrizacionTributariaViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As tblParametrizacionTributaria
    Private mdcProxy As PLAT_MaestrosDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As tblParametrizacionTributaria
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
    ''' Creado por       : Natalia Andrea Otalvaro(Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Enero 24/2019
    ''' Pruebas CB       :  Natalia Andrea Otalvaro (Alcuadrado S.A.) -Enero 24/2019 - Resultado Ok 
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
    Private _ListaEncabezado As List(Of CPX_tblParametrizacionTributaria)
    Public Property ListaEncabezado() As List(Of CPX_tblParametrizacionTributaria)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblParametrizacionTributaria))
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
    Private _EncabezadoSeleccionado As CPX_tblParametrizacionTributaria
    Public Property EncabezadoSeleccionado() As CPX_tblParametrizacionTributaria
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblParametrizacionTributaria)
            _EncabezadoSeleccionado = value
            'Llamado de metodo para realizar la consulta del encabezado editable
            ConsultarEncabezadoEdicion()
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As tblParametrizacionTributaria
    Public Property EncabezadoEdicionSeleccionado() As tblParametrizacionTributaria
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As tblParametrizacionTributaria)
            _EncabezadoEdicionSeleccionado = value
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                If _EncabezadoEdicionSeleccionado.intID > 0 Then
                    HabilitarEdicionDetalle = True

                    strDescripcionPais = String.Format("{0} - {1}", _EncabezadoEdicionSeleccionado.intIDPais, _EncabezadoSeleccionado.strNombrePais)
                    strDescripcionCiudad = String.Format("{0} - {1}", _EncabezadoEdicionSeleccionado.intIDPais, _EncabezadoSeleccionado.strNombreCiudad)
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
    Private _cb As CamposBusquedaParametrizacionTributaria
    Public Property cb() As CamposBusquedaParametrizacionTributaria
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaParametrizacionTributaria)
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

    Private _strDescripcionPais As String
    Public Property strDescripcionPais() As String
        Get
            Return _strDescripcionPais
        End Get
        Set(ByVal value As String)
            _strDescripcionPais = value
            MyBase.CambioItem("strDescripcionPais")
        End Set
    End Property

    Private _strDescripcionCiudad As String
    Public Property strDescripcionCiudad() As String
        Get
            Return _strDescripcionCiudad
        End Get
        Set(ByVal value As String)
            _strDescripcionCiudad = value
            MyBase.CambioItem("strDescripcionCiudad")
        End Set
    End Property

    Private _ListaFuncionalidad As List(Of ProductoCombos)
    Public Property ListaFuncionalidad() As List(Of ProductoCombos)
        Get
            Return _ListaFuncionalidad
        End Get
        Set(ByVal value As List(Of ProductoCombos))
            _ListaFuncionalidad = value
            MyBase.CambioItem("ListaFuncionalidad")
        End Set
    End Property

    Private _MostrarBotonDuplicar As Visibility = Visibility.Visible
    Public Property MostrarBotonDuplicar() As Visibility
        Get
            Return _MostrarBotonDuplicar
        End Get
        Set(ByVal value As Visibility)
            _MostrarBotonDuplicar = value
            MyBase.CambioItem("MostrarBotonDuplicar")
        End Set
    End Property

    Private _HabilitarEdicion As Boolean = False
    Public Property HabilitarEdicion() As Boolean
        Get
            Return _HabilitarEdicion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicion = value
            MyBase.CambioItem("HabilitarEdicion")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    Public Sub DuplicarRegistro()
        Try
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                Dim objNvoEncabezado As New tblParametrizacionTributaria
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                'Obtiene el registro por defecto
                ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, objNvoEncabezado)
                'Salva el registro anterior
                ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

                objNvoEncabezado.intID = 0

                Editando = True
                HabilitarEdicion = False
                MostrarBotonDuplicar = Visibility.Collapsed
                MyBase.CambioItem("Editando")

                EncabezadoEdicionSeleccionado = objNvoEncabezado
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "DuplicarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Sub NuevoRegistro()
        Try
            Dim objNvoEncabezado As New tblParametrizacionTributaria
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'Obtiene el registro por defecto
            ObtenerRegistroAnterior(mobjEncabezadoPorDefecto, objNvoEncabezado)
            'Salva el registro anterior
            ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

            Editando = True
            HabilitarEdicion = False
            MostrarBotonDuplicar = Visibility.Collapsed
            ListaFuncionalidad = Nothing
            MyBase.CambioItem("Editando")

            EncabezadoEdicionSeleccionado = objNvoEncabezado
            strDescripcionPais = String.Empty
            strDescripcionCiudad = String.Empty

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
            If cb.intID > 0 Or Not String.IsNullOrEmpty(cb.strNombre) Or Not String.IsNullOrEmpty(cb.strModulo) Or Not String.IsNullOrEmpty(cb.strFuncionalidad) Or cb.intIDPais > 0 Or cb.intIDCiudad > 0 Or Not String.IsNullOrEmpty(cb.strEstado) Then
                Await ConsultarEncabezado("BUSQUEDA", String.Empty, Nothing, cb.intID, cb.strNombre, cb.strModulo, cb.strFuncionalidad, cb.intIDPais, cb.intIDCiudad, cb.strEstado)
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

            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.ParametrizacionTributaria_ValidarAsync(_EncabezadoEdicionSeleccionado.intID,
                                                                                                                                  _EncabezadoEdicionSeleccionado.strNombre,
                                                                                                                                  _EncabezadoEdicionSeleccionado.dblTasaImpositiva,
                                                                                                                                  _EncabezadoEdicionSeleccionado.strModulo,
                                                                                                                                  _EncabezadoEdicionSeleccionado.strFuncionalidad,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIDPais,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIDCiudad,
                                                                                                                                  _EncabezadoEdicionSeleccionado.logActivo,
                                                                                                                                  Program.Usuario,
                                                                                                                                  Program.HashConexion)

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

                        HabilitarEdicion = False
                        MostrarBotonDuplicar = Visibility.Visible
                        MyBase.FinalizoGuardadoRegistros()

                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        If strTipoFiltroBusqueda = "FILTRAR" Then
                            Await ConsultarEncabezado("FILTRAR", FiltroVM, intIDRegistroActualizado)
                        ElseIf strTipoFiltroBusqueda = "BUSQUEDA" And Not IsNothing(cb) Then
                            Await ConsultarEncabezado("BUSQUEDA", String.Empty, cb.intID, Nothing, cb.intID, cb.strModulo, cb.strFuncionalidad, cb.intIDPais, cb.intIDCiudad, cb.strEstado)
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
                HabilitarEdicion = True
                MostrarBotonDuplicar = Visibility.Collapsed
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
            HabilitarEdicion = False
            MostrarBotonDuplicar = Visibility.Visible
            MyBase.CambioItem("Editando")
            MyBase.CancelarEditarRegistro()

            If Not IsNothing(mobjEncabezadoAnterior) Then
                RecargarComboFuncionalidad(mobjEncabezadoAnterior.strModulo)
            End If
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

            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If _EncabezadoEdicionSeleccionado.logActivo Then
                    A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("GENERICO_ANULARREGISTRO"), Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_NOPERMITE_ANULARREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                End If
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

                    Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.ParametrizacionTributaria_EliminarIDAsync(_EncabezadoSeleccionado.intID, Program.Usuario, Program.HashConexion)

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

                                Await ConsultarEncabezado("FILTRAR", String.Empty, EncabezadoSeleccionado.intID)
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
            Dim objListaFuncionalidad As New List(Of ProductoCombos)
            If Not IsNothing(MyBase.DiccionarioCombosPantalla) Then
                If MyBase.DiccionarioCombosPantalla.ContainsKey("PARAM_FUNCIONALIDAD") Then
                    objListaFuncionalidad = MyBase.DiccionarioCombosPantalla("PARAM_FUNCIONALIDAD").ToList
                End If
            End If

            Dim objCB As New CamposBusquedaParametrizacionTributaria(objListaFuncionalidad)
            objCB.intID = Nothing
            objCB.strNombre = String.Empty
            objCB.strModulo = String.Empty
            objCB.strFuncionalidad = String.Empty
            objCB.intIDPais = Nothing
            objCB.strDescripcionPais = String.Empty
            objCB.intIDCiudad = Nothing
            objCB.strDescripcionCiudad = String.Empty
            objCB.strEstado = "NINGUNO"

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
    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As tblParametrizacionTributaria, ByRef pobjRegistroSalvar As tblParametrizacionTributaria)
        Dim objEncabezado As tblParametrizacionTributaria = New tblParametrizacionTributaria

        Try
            If Not IsNothing(pobjRegistro) Then

                objEncabezado.intID = pobjRegistro.intID
                objEncabezado.strNombre = pobjRegistro.strNombre
                objEncabezado.dblTasaImpositiva = pobjRegistro.dblTasaImpositiva
                objEncabezado.strModulo = pobjRegistro.strModulo
                objEncabezado.strFuncionalidad = pobjRegistro.strFuncionalidad
                objEncabezado.intIDPais = pobjRegistro.intIDPais
                objEncabezado.intIDCiudad = pobjRegistro.intIDCiudad
                objEncabezado.logActivo = pobjRegistro.logActivo
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
                If e.PropertyName = "strModulo" Then
                    _EncabezadoEdicionSeleccionado.strFuncionalidad = String.Empty
                    RecargarComboFuncionalidad(_EncabezadoEdicionSeleccionado.strModulo)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "_EncabezadoEdicionSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub RecargarComboFuncionalidad(ByVal pstrModulo As String)
        Try
            Dim objListaFuncionalidad As New List(Of ProductoCombos)
            If Not IsNothing(MyBase.DiccionarioCombosPantalla) Then
                If MyBase.DiccionarioCombosPantalla.ContainsKey("PARAM_FUNCIONALIDAD") Then
                    For Each li In MyBase.DiccionarioCombosPantalla("PARAM_FUNCIONALIDAD")
                        If li.Dependencia1 = pstrModulo Then
                            objListaFuncionalidad.Add(li)
                        End If
                    Next
                End If
            End If

            ListaFuncionalidad = objListaFuncionalidad
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "RecargarComboFuncionalidad", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefecto()
        Try
            Dim objRespuesta As InvokeResult(Of tblParametrizacionTributaria)

            objRespuesta = Await mdcProxy.ParametrizacionTributaria_ConsultarDefectoAsync(Program.Usuario, Program.HashConexion)

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
                                               Optional ByVal pintID As Nullable(Of Integer) = 0,
                                               Optional ByVal pstrNombre As String = "",
                                               Optional ByVal pstrModulo As String = "",
                                               Optional ByVal pstrFuncionalidad As String = "",
                                               Optional ByVal pintIDPais As Nullable(Of Integer) = -1,
                                               Optional ByVal pintIDCiudad As Nullable(Of Integer) = -1,
                                               Optional ByVal pstrEstado As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblParametrizacionTributaria)) = Nothing

            ErrorForma = String.Empty

            If pstrOpcion = "FILTRAR" Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRespuesta = Await mdcProxy.ParametrizacionTributaria_FiltrarAsync(pstrFiltro, Program.Usuario, Program.HashConexion)

            Else
                Dim logActivo As Nullable(Of Boolean) = Nothing

                If pstrEstado = "ACTIVO" Then
                    logActivo = True
                ElseIf pstrEstado = "INACTIVO" Then
                    logActivo = False
                End If

                pstrNombre = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrNombre, String.Empty))

                objRespuesta = Await mdcProxy.ParametrizacionTributaria_ConsultarAsync(pintID, pstrNombre, pstrModulo, pstrFuncionalidad, pintIDPais, pintIDCiudad, logActivo, Program.Usuario, Program.HashConexion)
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
            Dim objRespuesta As InvokeResult(Of tblParametrizacionTributaria) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.ParametrizacionTributaria_ConsultarIDAsync(pintID, Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    RecargarComboFuncionalidad(objRespuesta.Value.strModulo)
                    EncabezadoEdicionSeleccionado = Nothing
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
Public Class CamposBusquedaParametrizacionTributaria
    Implements INotifyPropertyChanged

    Dim objListaFuncionalidadCompleta As List(Of ProductoCombos)

    Public Sub New(ByVal pobjListaFuncionalidadCompleta As List(Of ProductoCombos))
        objListaFuncionalidadCompleta = pobjListaFuncionalidadCompleta
    End Sub

    Private _intID As Nullable(Of Integer)
    Public Property intID() As Nullable(Of Integer)
        Get
            Return _intID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intID"))
        End Set
    End Property

    Private _strNombre As String
    Public Property strNombre() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombre"))
        End Set
    End Property

    Private _strModulo As String
    Public Property strModulo() As String
        Get
            Return _strModulo
        End Get
        Set(ByVal value As String)
            _strModulo = value
            strFuncionalidad = String.Empty

            Dim objListaFuncionalidad As New List(Of ProductoCombos)
            If Not IsNothing(objListaFuncionalidadCompleta) Then
                For Each li In objListaFuncionalidadCompleta
                    If li.Dependencia1 = _strModulo Then
                        objListaFuncionalidad.Add(li)
                    End If
                Next
            End If

            ListaFuncionalidad = objListaFuncionalidad
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strModulo"))
        End Set
    End Property

    Private _strFuncionalidad As String
    Public Property strFuncionalidad() As String
        Get
            Return _strFuncionalidad
        End Get
        Set(ByVal value As String)
            _strFuncionalidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strFuncionalidad"))
        End Set
    End Property

    Private _intIDPais As Nullable(Of Integer)
    Public Property intIDPais() As Nullable(Of Integer)
        Get
            Return _intIDPais
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intIDPais = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDPais"))
        End Set
    End Property

    Private _strDescripcionPais As String
    Public Property strDescripcionPais() As String
        Get
            Return _strDescripcionPais
        End Get
        Set(ByVal value As String)
            _strDescripcionPais = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionPais"))
        End Set
    End Property

    Private _intIDCiudad As Nullable(Of Integer)
    Public Property intIDCiudad() As Nullable(Of Integer)
        Get
            Return _intIDCiudad
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intIDCiudad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDCiudad"))
        End Set
    End Property

    Private _strDescripcionCiudad As String
    Public Property strDescripcionCiudad() As String
        Get
            Return _strDescripcionCiudad
        End Get
        Set(ByVal value As String)
            _strDescripcionCiudad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionCiudad"))
        End Set
    End Property

    Private _strEstado As String
    Public Property strEstado() As String
        Get
            Return _strEstado
        End Get
        Set(ByVal value As String)
            _strEstado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strEstado"))
        End Set
    End Property

    Private _ListaFuncionalidad As List(Of ProductoCombos)
    Public Property ListaFuncionalidad() As List(Of ProductoCombos)
        Get
            Return _ListaFuncionalidad
        End Get
        Set(ByVal value As List(Of ProductoCombos))
            _ListaFuncionalidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaFuncionalidad"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class