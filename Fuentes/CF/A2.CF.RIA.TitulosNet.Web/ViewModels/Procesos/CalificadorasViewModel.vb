Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web

Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

''' <summary>
''' ViewModel para la pantalla Calificadoras perteneciente al proyecto de Cálculos Financieros.
''' </summary>
''' Creado por       : Jorge Peña (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : Febrero 21/2014
''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok ''' 
''' <remarks></remarks>


Public Class CalificadorasViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As OyDTitulosNet.CalificadorasTest
    Private mdcProxyActualizar As TitulosNetDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As OyDTitulosNet.CalificadorasTest
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
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 21/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Febrero 21/2014 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefectoSync()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await consultarEncabezado(True, String.Empty)

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
    ''' Lista de Calificadoras que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of OyDTitulosNet.CalificadorasTest)
    Public Property ListaEncabezado() As EntitySet(Of OyDTitulosNet.CalificadorasTest)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of OyDTitulosNet.CalificadorasTest))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Calificadoras para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de Calificadoras que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As OyDTitulosNet.CalificadorasTest
    Public Property EncabezadoSeleccionado() As OyDTitulosNet.CalificadorasTest
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As OyDTitulosNet.CalificadorasTest)
            _EncabezadoSeleccionado = value
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaCalificadoras
    Public Property cb() As CamposBusquedaCalificadoras
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaCalificadoras)
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

    Private _HabilitarCodCalificadoras As Boolean = False
    Public Property HabilitarCodCalificadoras() As Boolean
        Get
            Return _HabilitarCodCalificadoras
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCodCalificadoras = value
            MyBase.CambioItem("HabilitarCodCalificadoras")
        End Set
    End Property

    Private _VisibilidadIdentityCalificadoras As Visibility = Visibility.Collapsed
    Public Property VisibilidadIdentityCalificadoras() As Visibility
        Get
            Return _VisibilidadIdentityCalificadoras
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadIdentityCalificadoras = value
            MyBase.CambioItem("VisibilidadIdentityCalificadoras")
        End Set
    End Property
#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' 
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New OyDTitulosNet.CalificadorasTest

        Try
            If mdcProxyActualizar.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of OyDTitulosNet.CalificadorasTest)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intIdCalificadora = -1
                objNvoEncabezado.intCodCalificadora = 0
                objNvoEncabezado.strNomCalificadora = String.Empty
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = obtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado
            HabilitarEncabezado = True
            HabilitarCodCalificadoras = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    ''' 
    Public Overrides Async Sub Filtrar()
        Try
            Await consultarEncabezado(True, FiltroVM)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la ejecución del filtro", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    ''' 
    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    ''' 
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not cb.intCodCalificadora Is Nothing Or Not String.IsNullOrEmpty(cb.strNomCalificadora) Then 'Validar que ingresó algo en los campos de búsqueda
                Await consultarEncabezado(False, String.Empty, cb.intIdCalificadora, CType(cb.intCodCalificadora, Integer), cb.strNomCalificadora)
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
    ''' 
    Public Overrides Sub ActualizarRegistro()

        Dim intNroOcurrencias As Integer
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            ErrorForma = String.Empty

            IsBusy = True

            If validarRegistro() Then

                ' Incializar los mensajes de validación
                _EncabezadoSeleccionado.strMsgValidacion = String.Empty

                ' Validar si el registro ya existe en la lista
                intNroOcurrencias = (From e In ListaEncabezado Where e.intIdCalificadora = _EncabezadoSeleccionado.intIdCalificadora Select e).Count

                If intNroOcurrencias = 0 Then
                    strAccion = ValoresUserState.Ingresar.ToString()
                    ListaEncabezado.Add(_EncabezadoSeleccionado)
                End If


                ' Enviar cambios al servidor
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
    ''' 
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If mdcProxyActualizar.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            IsBusy = True

            _EncabezadoSeleccionado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = obtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            HabilitarEncabezado = True
            HabilitarCodCalificadoras = False

            IsBusy = False
        End If
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    ''' 
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxyActualizar.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezado = False
                HabilitarCodCalificadoras = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' 
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxyActualizar.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra la calificadora seleccionada. ¿Confirma el borrado de esta Calificadora?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf borrarRegistroConfirmado)
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
    ''' 
    Private Sub borrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = obtenerRegistroAnterior()

                    If mdcProxyActualizar.CalificadorasTests.Where(Function(i) i.intIdCalificadora = EncabezadoSeleccionado.intIdCalificadora).Count > 0 Then
                        mdcProxyActualizar.CalificadorasTests.Remove(mdcProxyActualizar.CalificadorasTests.Where(Function(i) i.intIdCalificadora = EncabezadoSeleccionado.intIdCalificadora).First)
                    End If

                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If

                    mdcProxyActualizar.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "borrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    ''' 
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaCalificadoras
            objCB.intCodCalificadora = 0
            objCB.strNomCalificadora = String.Empty
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
    ''' 
    Private Function obtenerRegistroAnterior() As OyDTitulosNet.CalificadorasTest
        Dim objEncabezado As OyDTitulosNet.CalificadorasTest = New OyDTitulosNet.CalificadorasTest

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of OyDTitulosNet.CalificadorasTest)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIdCalificadora = _EncabezadoSeleccionado.intIdCalificadora
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    Private Function validarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then
                'Valida el Comprobante
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.intCodCalificadora.ToString) Then
                    strMsg = String.Format("{0}{1} + El Código es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el Nombre
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strNomCalificadora) Then
                    strMsg = String.Format("{0}{1} + El Nombre es un campo requerido.", strMsg, vbCrLf)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "validarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Procedimiento que se ejecuta cuando finaliza la ejecución de una actualización a la base de datos
    ''' </summary>
    Public Overrides Async Sub TerminoSubmitChanges(ByVal So As System.OpenRiaServices.DomainServices.Client.SubmitOperation)
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
                HabilitarCodCalificadoras = False

                MyBase.TerminoSubmitChanges(So)
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await consultarEncabezado(True, String.Empty)
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
        Dim objRet As System.OpenRiaServices.DomainServices.Client.LoadOperation(Of OyDTitulosNet.CalificadorasTest)
        Dim dcProxy As TitulosNetDomainContext

        Try
            dcProxy = inicializarProxyTitulosNet()

            objRet = Await dcProxy.Load(dcProxy.ConsultarCalificadorasPorDefectoSyncQuery(Program.Usuario)).AsTask

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
    ''' Consultar de forma sincrónica los datos de Calificadoras
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function consultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pintIdCalificadora As Integer = -1,
                                               Optional ByVal pintCodCalificadora As Integer = -1,
                                               Optional ByVal pstrNombreCalificadora As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As System.OpenRiaServices.DomainServices.Client.LoadOperation(Of OyDTitulosNet.CalificadorasTest)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxyActualizar Is Nothing Then
                mdcProxyActualizar = inicializarProxyTitulosNet()
            End If

            mdcProxyActualizar.CalificadorasTests.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.FiltrarCalificadorasSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario)).AsTask()
            Else
                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.ConsultarCalificadorasSyncQuery(pintIdCalificadora:=pintIdCalificadora,
                                                                                                          pintCodCalificadora:=pintCodCalificadora,
                                                                                                          pstrNombreCalificadora:=pstrNombreCalificadora,
                                                                                                          pstrUsuario:=Program.Usuario)).AsTask()
            End If

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "consultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaEncabezado = mdcProxyActualizar.CalificadorasTests

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de Calificadoras ", Me.ToString(), "consultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
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
''' 
Public Class CamposBusquedaCalificadoras
    Implements INotifyPropertyChanged

    <Display(Name:="Nombre Calificadora")> _
    Public Property strNomCalificadora As String

    <Display(Name:="Id Calificadora")> _
    Public Property intIdCalificadora As Integer

    <Display(Name:="Id Código calificadora")> _
    Public Property intCodCalificadora As System.Nullable(Of Integer)


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class