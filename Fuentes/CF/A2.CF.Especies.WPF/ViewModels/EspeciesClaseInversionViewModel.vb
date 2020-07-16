Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

''' <summary>
''' ViewModel para la pantalla Especies Clase de Inversion perteneciente al proyecto de Cálculos Financieros.
''' </summary>
''' Creado por       : Jorge Peña (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : Febrero 21/2014
''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok ''' 
''' <remarks></remarks>

Public Class EspeciesClaseInversionViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As EspeciesClaseInversion
    Private mdcProxyActualizar As EspeciesCFDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As EspeciesClaseInversion
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
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Mayo 14/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Mayo 14/2014 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                ConsultarEncabezadoPorDefectoSync()

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
    ''' Lista de EspeciesClaseInversion que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of EspeciesClaseInversion)
    Public Property ListaEncabezado() As EntitySet(Of EspeciesClaseInversion)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of EspeciesClaseInversion))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de EspeciesClaseInversion para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de EspeciesClaseInversion que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As EspeciesClaseInversion
    Public Property EncabezadoSeleccionado() As EspeciesClaseInversion
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As EspeciesClaseInversion)
            _EncabezadoSeleccionado = value
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaEspeciesClaseInversion
    Public Property cb() As CamposBusquedaEspeciesClaseInversion
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaEspeciesClaseInversion)
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

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_6
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Mayo 14/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Mayo 14/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New EspeciesClaseInversion

        Try
            If mdcProxyActualizar.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of EspeciesClaseInversion)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intIdTipoEspecieClaseInversion = -1
                objNvoEncabezado.strDescripcion = String.Empty
                objNvoEncabezado.strCodigo = String.Empty
                objNvoEncabezado.strMetodoValoracion = String.Empty
                objNvoEncabezado.strFactorRiesgo = String.Empty

            End If
            objNvoEncabezado.intTipoTitulo = Nothing

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado
            HabilitarEncabezado = True

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
    ''' ID caso de prueba: Id_3
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Mayo 14/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Mayo 14/2014 - Resultado Ok 
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
    ''' ID caso de prueba: Id_4
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Mayo 14/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Mayo 14/2014 - Resultado Ok 
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
            If Not IsNothing(cb.strCodigo) Or Not IsNothing(cb.strDescripcion) Or _
                Not IsNothing(cb.strMetodoValoracion) Or Not IsNothing(cb.intTipoTitulo) Then 'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, cb.intIdTipoEspecieClaseInversion, cb.strDescripcion, cb.strCodigo, cb.strMetodoValoracion, CInt(cb.intTipoTitulo), cb.strFactorRiesgo)
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
    Public Overrides Sub ActualizarRegistro()

        Dim intNroOcurrencias As Integer
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            ErrorForma = String.Empty

            IsBusy = True

            If ValidarRegistro() Then

                ' Incializar los mensajes de validación
                _EncabezadoSeleccionado.strMsgValidacion = String.Empty

                ' Validar si el registro ya existe en la lista
                intNroOcurrencias = (From e In ListaEncabezado Where e.intIdTipoEspecieClaseInversion = _EncabezadoSeleccionado.intIdTipoEspecieClaseInversion Select e).Count

                If intNroOcurrencias = 0 Then
                    strAccion = ValoresUserState.Ingresar.ToString()
                    ListaEncabezado.Add(_EncabezadoSeleccionado)
                End If


                ' Enviar cambios al servidor
                Program.VerificarCambiosProxyServidor(mdcProxyActualizar)
                mdcProxyActualizar.SubmitChanges(AddressOf TerminoSubmitChanges, strAccion)
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_7
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Mayo 14/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Mayo 14/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If mdcProxyActualizar.IsLoading Then
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
                mdcProxyActualizar.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezado = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_9
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Mayo 14/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Mayo 14/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxyActualizar.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra la especie clase inversión seleccionada. ¿Confirma el borrado de esta especie clase inversión?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
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

                    If mdcProxyActualizar.EspeciesClaseInversions.Where(Function(i) i.intIdTipoEspecieClaseInversion = EncabezadoSeleccionado.intIdTipoEspecieClaseInversion).Count > 0 Then
                        mdcProxyActualizar.EspeciesClaseInversions.Remove(mdcProxyActualizar.EspeciesClaseInversions.Where(Function(i) i.intIdTipoEspecieClaseInversion = EncabezadoSeleccionado.intIdTipoEspecieClaseInversion).First)
                    End If

                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If

                    Program.VerificarCambiosProxyServidor(mdcProxyActualizar)
                    mdcProxyActualizar.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaEspeciesClaseInversion
            objCB.strCodigo = String.Empty
            objCB.strDescripcion = String.Empty
            objCB.strMetodoValoracion = String.Empty
            objCB.intTipoTitulo = 0
            objCB.strFactorRiesgo = String.Empty
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
    Private Function ObtenerRegistroAnterior() As EspeciesClaseInversion
        Dim objEncabezado As EspeciesClaseInversion = New EspeciesClaseInversion

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of EspeciesClaseInversion)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIdTipoEspecieClaseInversion = _EncabezadoSeleccionado.intIdTipoEspecieClaseInversion
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    ''' <history>
    ''' ID caso de prueba: Id_5
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Mayo 14/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Mayo 14/2014 - Resultado Ok 
    ''' </history>
    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then
                'Valida el código
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strCodigo) Then
                    strMsg = String.Format("{0}{1} + El Código es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la descripción
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strDescripcion) Then
                    strMsg = String.Format("{0}{1} + La Descripción es un campo requerido.", strMsg, vbCrLf)
                End If

                'If String.IsNullOrEmpty(_EncabezadoSeleccionado.strMetodoValoracion) Then
                '    strMsg = String.Format("{0}{1} + El Método de Valoración es un campo requerido.", strMsg, vbCrLf)
                'End If

            Else
                strMsg = String.Format("{0}{1} Debe de seleccionar un registro", strMsg, vbCrLf)
            End If

            If String.IsNullOrEmpty(_EncabezadoSeleccionado.strFactorRiesgo) Then
                strMsg = String.Format("{0}{1} + El Factor Riesgo es un campo requerido.", strMsg, vbCrLf)
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
    ''' <history>
    ''' ID caso de prueba: Id_8
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Mayo 14/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Mayo 14/2014 - Resultado Ok 
    ''' </history>
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
                mdcProxyActualizar.RejectChanges()

                If So.UserState.Equals(ValoresUserState.Borrar.ToString) Then
                    ' Cuando se elimina un registro, el método RejectChanges lo vuelve a adicionar a la lista pero al final no en la posición original
                    _ListaEncabezadoPaginada.MoveToLastPage()
                    _ListaEncabezadoPaginada.MoveCurrentToLast()
                    MyBase.CambioItem("ListaEncabezadoPaginada")
                End If

                So.MarkErrorAsHandled()
            Else
                HabilitarEncabezado = False

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
    Private Async Sub ConsultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of EspeciesClaseInversion)
        Dim dcProxy As EspeciesCFDomainContext

        Try
            dcProxy = inicializarProxyespecies()

            objRet = Await dcProxy.Load(dcProxy.ConsultarEspeciesClaseInversionPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjEncabezadoPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjEncabezadoPorDefecto = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de EspeciesClaseInversion
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pintIdEspeciesClaseInversion As Integer = -1,
                                               Optional ByVal pstrDescripcion As String = "",
                                               Optional ByVal pstrCodigo As String = "",
                                               Optional ByVal pstrMetodoValoracion As String = "",
                                               Optional ByVal pintTipoTitulo As Integer = -1,
                                               Optional ByVal pstrFactorRiesgo As String = ""
                                                                                          ) As Task(Of Boolean)


        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of EspeciesClaseInversion)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxyActualizar Is Nothing Then
                mdcProxyActualizar = inicializarProxyEspecies()
            End If

            mdcProxyActualizar.EspeciesClaseInversions.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.FiltrarEspeciesClaseInversionSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.ConsultarEspeciesClaseInversionSyncQuery(pintIdEspeciesClaseInversion:=pintIdEspeciesClaseInversion,
                                                                                                          pstrDescripcion:=pstrDescripcion,
                                                                                                          pstrCodigo:=pstrCodigo,
                                                                                                          pstrMetodoValoracion:=pstrMetodoValoracion,
                                                                                                          pstrUsuario:=Program.Usuario,
                                                                                                          pintTipoTitulo:=pintTipoTitulo,
                                                                                                          pstrFactorRiesgo:=pstrFactorRiesgo,
                                                                                                          pstrInfoConexion:=Program.HashConexion)).AsTask()

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
                    ListaEncabezado = mdcProxyActualizar.EspeciesClaseInversions

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de EspeciesClaseInversion ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
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
Public Class CamposBusquedaEspeciesClaseInversion
    Implements INotifyPropertyChanged

    Private _intIdTipoEspecieClaseInversion As Int16
    Public Property intIdTipoEspecieClaseInversion() As Int16
        Get
            Return _intIdTipoEspecieClaseInversion
        End Get
        Set(ByVal value As Int16)
            _intIdTipoEspecieClaseInversion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIdTipoEspecieClaseInversion"))
        End Set
    End Property


    Private _strCodigo As String
    Public Property strCodigo() As String
        Get
            Return _strCodigo
        End Get
        Set(ByVal value As String)
            _strCodigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCodigo"))
        End Set
    End Property

    Private _strDescripcion As String
    Public Property strDescripcion() As String
        Get
            Return _strDescripcion
        End Get
        Set(ByVal value As String)
            _strDescripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcion"))
        End Set
    End Property

    Private _strMetodoValoracion As String
    Public Property strMetodoValoracion() As String
        Get
            Return _strMetodoValoracion
        End Get
        Set(ByVal value As String)
            _strMetodoValoracion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strMetodoValoracion"))
        End Set
    End Property

    Private _intTipoTitulo As System.Nullable(Of Integer)
    Public Property intTipoTitulo() As System.Nullable(Of Integer)
        Get
            Return _intTipoTitulo
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intTipoTitulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intTipoTitulo"))
        End Set
    End Property

    Private _strFactorRiesgo As String
    Public Property strFactorRiesgo() As String
        Get
            Return _strFactorRiesgo
        End Get
        Set(ByVal value As String)
            _strFactorRiesgo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strFactorRiesgo"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class