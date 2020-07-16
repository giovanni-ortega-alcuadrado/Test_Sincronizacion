Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA.Web.MaestrosCFDomainContext
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Text.RegularExpressions
Imports System.Object
Imports System.Globalization.CultureInfo
Imports A2Utilidades.Mensajes
Imports System.Web
Imports System.Collections.ObjectModel
Imports A2.OYD.OYDServer.RIA.Web.CFMaestros
Imports OpenRiaServices.DomainServices.Client

Public Class CuentasFondosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As CuentasFondos
    Private mdcProxy As MaestrosCFDomainContext  ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mobjVM As CuentasFondosViewModel

    Dim logGeneroDatosCorrectamente As Boolean
    Dim NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO As String = "Código"

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As CuentasFondos
    Private mobjDetallePorDefecto As CuentasFondosDetalle
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

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefectoSync()

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
    ''' Lista de CuentasFondos que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of CuentasFondos)
    Public Property ListaEncabezado() As EntitySet(Of CuentasFondos)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of CuentasFondos))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de CuentasFondos para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
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
    ''' Elemento de la lista de CuentasFondos que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As CuentasFondos
    Public Property EncabezadoSeleccionado() As CuentasFondos
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CuentasFondos)

            Dim logIncializarDet As Boolean = False

            _EncabezadoSeleccionado = value

            If _EncabezadoSeleccionado Is Nothing Then
                logIncializarDet = True
            Else
                If Not IsNothing(_EncabezadoSeleccionado.strNroDocumento) Then
                    ConsultarDetalle(_EncabezadoSeleccionado.strNroDocumento, _EncabezadoSeleccionado.strTipoIdComitente, _EncabezadoSeleccionado.lngidCuentaDeceval, _EncabezadoSeleccionado.strDeposito)
                    EstablecerEstadoSeleccionarTodos() 'DARM 20180518 SE IMPLEMENTA MANEJO DE CHECK
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
        Try
            Select Case e.PropertyName
                Case "strNroDocumento", "strDeposito"
                    If Not IsNothing(EncabezadoSeleccionado.strNroDocumento) And Not IsNothing(EncabezadoSeleccionado.lngidCuentaDeceval) And Not IsNothing(EncabezadoSeleccionado.strDeposito) Then
                        Await ConsultarDetalle(EncabezadoSeleccionado.strNroDocumento, EncabezadoSeleccionado.strTipoIdComitente, EncabezadoSeleccionado.lngidCuentaDeceval, EncabezadoSeleccionado.strDeposito)
                        EstablecerEstadoSeleccionarTodos() 'DARM 20180518 SE IMPLEMENTA MANEJO DE CHECK
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método PropertyChanged.", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub _DetalleSeleccionado_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _DetalleSeleccionado.PropertyChanged
        'DARM 
        Try
            Select Case e.PropertyName
                Case "logAplica"
                    EstablecerEstadoSeleccionarTodos() 'DARM 20180518 SE IMPLEMENTA MANEJO DE CHECK
            End Select
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso detalle de codificacion de activos
    ''' </summary>
    Private _ListaDetalle As List(Of CuentasFondosDetalle)
    Public Property ListaDetalle() As List(Of CuentasFondosDetalle)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of CuentasFondosDetalle))
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
    Private WithEvents _DetalleSeleccionado As CuentasFondosDetalle
    Public Property DetalleSeleccionado() As CuentasFondosDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As CuentasFondosDetalle)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaCuentasFondos
    Public Property cb() As CamposBusquedaCuentasFondos
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaCuentasFondos)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _HabilitarCamposNuevoRegistro As Boolean = False
    Public Property HabilitarCamposNuevoRegistro() As Boolean
        Get
            Return _HabilitarCamposNuevoRegistro
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposNuevoRegistro = value
            MyBase.CambioItem("HabilitarCamposNuevoRegistro")
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

    Private _logSeleccionarTodos As Boolean
    Public Property logSeleccionarTodos() As Boolean
        Get
            Return _logSeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _logSeleccionarTodos = value
            SeleccionarTodos(_logSeleccionarTodos)
            MyBase.CambioItem("logSeleccionarTodos")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New CuentasFondos

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of CuentasFondos)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intIDCuentasDeceval = -1
                objNvoEncabezado.lngidCuentaDeceval = 0
                objNvoEncabezado.strDeposito = String.Empty
                objNvoEncabezado.strDescripcionDeposito = String.Empty
                objNvoEncabezado.strTipoIdComitente = String.Empty
                objNvoEncabezado.strDescripcionTipoIdentificacion = String.Empty
                objNvoEncabezado.strNroDocumento = String.Empty
                objNvoEncabezado.strNombre = String.Empty
                objNvoEncabezado.strPrefijo = String.Empty
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado

            HabilitarEncabezado = True
            HabilitarCamposNuevoRegistro = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
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
            If Not IsNothing(cb.strDeposito) Or Not IsNothing(cb.strPrefijo) Or Not IsNothing(cb.lngidCuentaDeceval) Or Not IsNothing(cb.strNroDocumento) Then 'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, cb.lngidCuentaDeceval, cb.strDeposito, cb.strCodigoOyD, String.Empty, cb.strNroDocumento, String.Empty)
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
    Public Overrides Async Sub ActualizarRegistro()
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            Dim xmlCompleto As String
            Dim xmlDetalle As String
            ErrorForma = String.Empty

            'If ValidarRegistro() Then

            'If ValidarDetalle() Then

            If Not IsNothing(ListaDetalle) Then
                xmlCompleto = "<CuentasFondosDetalle>"

                For Each objeto In (From c In ListaDetalle)

                    xmlDetalle = "<Detalle intIDCuentasDecevalDetalle=""" & objeto.intIDCuentasDecevalDetalle &
                                    """ lngIDComitente=""" & objeto.lngIDComitente & """ strTipoIdentificacion=""" & objeto.strTipoIdentificacion &
                                    """ strNombre=""" & objeto.strNombre & """ strTipoProducto=""" & objeto.strTipoProducto &
                                    """ logAplica=""" & objeto.logAplica & """></Detalle>"

                    xmlCompleto = xmlCompleto & xmlDetalle
                Next

                xmlCompleto = xmlCompleto & "</CuentasFondosDetalle>"
            End If

            IsBusy = True

                    Dim strMsg As String = ""
                    Dim objRet As InvokeOperation(Of String)

            objRet = Await mdcProxy.CuentasFondos_ActualizarSync(EncabezadoSeleccionado.intIDCuentasDeceval, EncabezadoSeleccionado.lngidCuentaDeceval,
                                                                        EncabezadoSeleccionado.strDeposito, EncabezadoSeleccionado.lngidComitenteLider, EncabezadoSeleccionado.strTipoIdComitente,
                                                                        EncabezadoSeleccionado.strNroDocumento, EncabezadoSeleccionado.strNombre, EncabezadoSeleccionado.strPrefijo, xmlCompleto,
                                                                        EncabezadoSeleccionado.strConector1, EncabezadoSeleccionado.strTipoIdBenef1, EncabezadoSeleccionado.lngNroDocBenef1,
                                                                        EncabezadoSeleccionado.strConector2, EncabezadoSeleccionado.strTipoIdBenef2, EncabezadoSeleccionado.lngNroDocBenef2,
                                                                        Program.Usuario, EncabezadoSeleccionado.strCuentaPrincipalDCV).AsTask()

            If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())

                If Not String.IsNullOrEmpty(strMsg) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                Editando = False
                HabilitarEncabezado = False
                HabilitarEdicionDetalle = False
                HabilitarCamposNuevoRegistro = False
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)
            End If

            'Else
            '    HabilitarEncabezado = True
            '    HabilitarEdicionDetalle = True
            'End If

            'Else
            '    HabilitarEncabezado = True
            'HabilitarEdicionDetalle = True
            'End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If mdcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            _EncabezadoSeleccionado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            HabilitarEncabezado = True
            HabilitarEdicionDetalle = True
            HabilitarCamposNuevoRegistro = False

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
                HabilitarCamposNuevoRegistro = False
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
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                'If mdcProxy.IsLoading Then
                '    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    Exit Sub
                'End If

                A2Utilidades.Mensajes.mostrarMensaje("Esta opción se encuentra deshabilitada para esta pantalla.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
                'A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra el registro seleccionado. ¿Confirma el borrado de este registro?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
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
    Private Async Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    If mdcProxy.CuentasFondos.Where(Function(i) i.intIDCuentasDeceval = EncabezadoSeleccionado.intIDCuentasDeceval).Count > 0 Then
                        mdcProxy.CuentasFondos.Remove(mdcProxy.CuentasFondos.Where(Function(i) i.intIDCuentasDeceval = EncabezadoSeleccionado.intIDCuentasDeceval).First)
                    End If

                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If

                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)

                    Await ConsultarEncabezado(True, String.Empty)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SeleccionarTodos(blnIsChequed As Boolean)
        Try
            If ListaDetalle IsNot Nothing Then
                For Each li In ListaDetalle
                    li.logAplica = blnIsChequed
                Next
                MyBase.CambioItem("ListaDetalle")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cobrar todas las utilidades.", Me.ToString(), "SeleccionarTodos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub EstablecerEstadoSeleccionarTodos()
        'DARM 20180518 SE IMPLEMENTA MANEJO DE CHECK
        Try
            Dim intSeleccionados As Integer = 0
            Dim intDesSeleccionados As Integer = 0
            If ListaDetalle IsNot Nothing Then
                For Each li In ListaDetalle
                    If li.logAplica Then
                        intSeleccionados += 1
                    Else
                        intDesSeleccionados += 1
                    End If
                Next
                If intSeleccionados > 0 And intDesSeleccionados = 0 Then
                    _logSeleccionarTodos = True
                Else
                    _logSeleccionarTodos = False
                End If
                MyBase.CambioItem("logSeleccionarTodos")
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaCuentasFondos
            objCB.strDeposito = String.Empty
            objCB.strPrefijo = String.Empty
            objCB.lngidCuentaDeceval = 0
            objCB.strNroDocumento = String.Empty
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
    Private Function ObtenerRegistroAnterior() As CuentasFondos
        Dim objEncabezado As CuentasFondos = New CuentasFondos

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of CuentasFondos)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDCuentasDeceval = _EncabezadoSeleccionado.intIDCuentasDeceval
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then

                'Valida el depósito
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strDeposito) Then
                    strMsg = String.Format("{0}{1} + El depósito es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el nro. de cuenta
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.lngidCuentaDeceval) Then
                    strMsg = String.Format("{0}{1} + El nro. de cuenta es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el nro. de documento
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strNroDocumento) Then
                    strMsg = String.Format("{0}{1} + El nro. de documento es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el conector 1
                If (String.IsNullOrEmpty(_EncabezadoSeleccionado.strConector1) Or _EncabezadoSeleccionado.strConector1 = "N") And Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strPrimerBeneficiario) Then
                    strMsg = String.Format("{0}{1} + El conector entre el comitente y el primer beneficiario es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el primer beneficiario
                If (_EncabezadoSeleccionado.strConector1 = "Y" Or _EncabezadoSeleccionado.strConector1 = "O") And String.IsNullOrEmpty(_EncabezadoSeleccionado.strPrimerBeneficiario) Then
                    strMsg = String.Format("{0}{1} + El primer beneficiario es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el conector 2
                If (String.IsNullOrEmpty(_EncabezadoSeleccionado.strConector2) Or _EncabezadoSeleccionado.strConector2 = "N") And Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strSegundoBeneficiario) Then
                    strMsg = String.Format("{0}{1} + El conector entre el primer beneficiario y segundo beneficiario es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el segundo beneficiario
                If (_EncabezadoSeleccionado.strConector2 = "Y" Or _EncabezadoSeleccionado.strConector2 = "O") And String.IsNullOrEmpty(_EncabezadoSeleccionado.strSegundoBeneficiario) Then
                    strMsg = String.Format("{0}{1} + El segundo beneficiario es un campo requerido.", strMsg, vbCrLf)
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
        Dim objRet As LoadOperation(Of CuentasFondos)
        Dim dcProxy As MaestrosCFDomainContext

        Try
            dcProxy = inicializarProxyMaestros()

            objRet = Await dcProxy.Load(dcProxy.CuentasFondos_ConsultarPorDefectoSyncQuery(Program.Usuario)).AsTask

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
    ''' Consultar de forma sincrónica los datos de CuentasFondos
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                           ByVal pstrFiltro As String,
                                           Optional ByVal plngidCuentaDeceval As Integer = 0,
                                           Optional ByVal pstrDeposito As String = "",
                                           Optional ByVal plngIDComitente As String = "",
                                           Optional ByVal pstrTipoIdComitente As String = "",
                                           Optional ByVal pstrNroDocumento As String = "",
                                           Optional ByVal pstrNombre As String = "",
                                           Optional ByVal pstrPrefijo As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CuentasFondos)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyMaestros()
            End If

            mdcProxy.CuentasFondos.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.CuentasFondos_FiltrarSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.CuentasFondos_ConsultarSyncQuery(plngidCuentaDeceval:=plngidCuentaDeceval,
                                                                                        pstrDeposito:=pstrDeposito,
                                                                                        plngIDComitente:=plngIDComitente,
                                                                                        pstrTipoIdComitente:=pstrTipoIdComitente,
                                                                                        pstrNroDocumento:=pstrNroDocumento,
                                                                                        pstrNombre:=pstrNombre,
                                                                                        pstrPrefijo:=pstrPrefijo,
                                                                                        pstrUsuario:=Program.Usuario)).AsTask()
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
                    ListaEncabezado = mdcProxy.CuentasFondos

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de CuentasFondos ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
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
    ''' <param name="pstrNroDocumento">Id del encabezado</param>
    ''' <remarks>NOTA: En este método no se puede utilizar la propiedad IsBusy porque hace que sean necesarios dos clic para seleccionar un detalle</remarks>
    Public Async Function ConsultarDetalle(ByVal pstrNroDocumento As String, ByVal pstrTipoIdComitente As String, ByVal plngidCuentaDeceval As Integer, ByVal pstrDeposito As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As MaestrosCFDomainContext = inicializarProxyMaestros()
        Dim objRet As LoadOperation(Of CuentasFondosDetalle)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.CuentasFondosDetalles Is Nothing Then
                mdcProxy.CuentasFondosDetalles.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.CuentasFondosDetalle_ConsultarSyncQuery(EncabezadoSeleccionado.intIDCuentasDeceval, pstrNroDocumento, pstrTipoIdComitente, plngidCuentaDeceval, pstrDeposito, Program.Usuario)).AsTask()

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
            IsBusy = False
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
    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            '------------------------------------------------------------------------------------------------------------------------------------------------
            '-- Valida que por lo menos exista un detalle para poder crear todo un registro
            '------------------------------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_ListaDetalle) Then
                If ListaDetalle.Where(Function(i) CType(i.logAplica, Boolean) = True).Count = 0 Then
                    strMsg = String.Format("{0}{1} + Debe existir por lo menos un detalle seleccionado.", strMsg, vbCrLf)
                End If
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
Public Class CamposBusquedaCuentasFondos
    Implements INotifyPropertyChanged

    Private _strDeposito As String
    Public Property strDeposito() As String
        Get
            Return _strDeposito
        End Get
        Set(ByVal value As String)
            _strDeposito = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDeposito"))
        End Set
    End Property

    Private _strPrefijo As String
    Public Property strPrefijo() As String
        Get
            Return _strPrefijo
        End Get
        Set(ByVal value As String)
            _strPrefijo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strPrefijo"))
        End Set
    End Property

    Private _lngidCuentaDeceval As System.Nullable(Of Integer)
    Public Property lngidCuentaDeceval() As System.Nullable(Of Integer)
        Get
            Return _lngidCuentaDeceval
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _lngidCuentaDeceval = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngidCuentaDeceval"))
        End Set
    End Property

    Private _strTipoIdComitente As String
    Public Property strTipoIdComitente() As String
        Get
            Return _strTipoIdComitente
        End Get
        Set(ByVal value As String)
            _strTipoIdComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoIdComitente"))
        End Set
    End Property

    Private _strNroDocumento As String
    Public Property strNroDocumento() As String
        Get
            Return _strNroDocumento
        End Get
        Set(ByVal value As String)
            _strNroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNroDocumento"))
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

    'I-20100107 - Ajuste de filtro por código OyD y campo strCuentaPrincipalDCV
    Private _strCodigoOyD As String
    Public Property strCodigoOyD() As String
        Get
            Return _strCodigoOyD
        End Get
        Set(ByVal value As String)
            _strCodigoOyD = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCodigoOyD"))
        End Set
    End Property

    'I-20100107 - Ajuste de filtro por código OyD y campo strCuentaPrincipalDCV
    Private _strNombreCodigoOyD As String
    Public Property strNombreCodigoOyD() As String
        Get
            Return _strNombreCodigoOyD
        End Get
        Set(ByVal value As String)
            _strNombreCodigoOyD = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombreCodigoOyD"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class