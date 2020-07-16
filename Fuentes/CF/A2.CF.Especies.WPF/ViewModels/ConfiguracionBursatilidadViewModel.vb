Imports Telerik.Windows.Controls
Imports A2ControlMenu
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

'Imports System.Linq
'Imports System.Collections.ObjectModel
'Imports System.Web
'
'

''' <summary>
''' ViewModel para la pantalla de configuración bursatilidad perteneciente al proyecto de Especies.
''' </summary>
''' <history>
''' Creado por       : Yessid Andres Paniagua Pabon  - Alcuadrado S.A.
''' Descripción      : Creación.
''' Fecha            : Octubre 16/2015
''' Pruebas CB       : Yessid Andres Paniagua Pabon - Octubre 16/2015 - Resultado Ok 
''' </history>

Public Class ConfiguracionBursatilidadViewModel
    Inherits A2ViewModel

#Region "Variables - REQUERIDO"

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As ConfiguracionBursatilidad
    Private mdcProxy As EspeciesCFDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As ConfiguracionBursatilidad

#End Region


#Region "Inicialización - REQUERIDO"

    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Yessid Paniagua (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Octubre 16/2015
    ''' Pruebas CB       : Yessid Paniagua (Alcuadrado S.A.) - Octubre 16/2015 - Resultado Ok 
    ''' </history>
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
    ''' Lista de Indicadores que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of ConfiguracionBursatilidad)
    Public Property ListaEncabezado() As EntitySet(Of ConfiguracionBursatilidad)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of ConfiguracionBursatilidad))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de Indicadores para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de Indicadores que se encuentra seleccionado para editar y nuevo
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As ConfiguracionBursatilidad
    Public Property EncabezadoSeleccionado() As ConfiguracionBursatilidad
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As ConfiguracionBursatilidad)
            _EncabezadoSeleccionado = value
            
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property


    ''' <summary>
    ''' Elemento de la lista de Indicadores que se encuentra seleccionado para la busqueda avanzada
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionadoBusqueda As ConfiguracionBursatilidad
    Public Property EncabezadoSeleccionadoBusqueda() As ConfiguracionBursatilidad
        Get
            Return _EncabezadoSeleccionadoBusqueda
        End Get
        Set(ByVal value As ConfiguracionBursatilidad)
            _EncabezadoSeleccionadoBusqueda = value
            
            MyBase.CambioItem("EncabezadoSeleccionadoBusqueda")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private WithEvents _cb As CamposBusquedaConfiguracionBursatilidad
    Public Property cb() As CamposBusquedaConfiguracionBursatilidad
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaConfiguracionBursatilidad)
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
    ''' 
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New ConfiguracionBursatilidad

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of ConfiguracionBursatilidad)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.strInfoSesion = String.Empty
                objNvoEncabezado.intIdConfiguracionBursatilidad = -1
                objNvoEncabezado.strBursatilidad = String.Empty
                objNvoEncabezado.logEntidadVigilada = False
                objNvoEncabezado.strClaseInversion = String.Empty
                objNvoEncabezado.strClaseContable = String.Empty
            End If

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
    ''' 
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
            Await ConsultarEncabezado(False, String.Empty, cb.strBursatilidad, cb.logEntidadVigilada, cb.strClaseInversion, cb.strClaseContable)
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

            If ValidarRegistro() Then
                ' Incializar los mensajes de validación
                _EncabezadoSeleccionado.strMsgValidacion = String.Empty
                ' Validar si el registro ya existe en la lista
                intNroOcurrencias = (From e In ListaEncabezado Where e.intIdConfiguracionBursatilidad = _EncabezadoSeleccionado.intIdConfiguracionBursatilidad Select e).Count

                If intNroOcurrencias = 0 Then
                    strAccion = ValoresUserState.Ingresar.ToString()
                    ListaEncabezado.Add(_EncabezadoSeleccionado)
                End If


                ' Enviar cambios al servidor
                _EncabezadoSeleccionado.strUsuario = Program.Usuario
                Program.VerificarCambiosProxyServidor(mdcProxy)
                mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, strAccion)
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
                mdcProxy.RejectChanges()

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
    ''' 
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra la configuración bursatilidad seleccionada. ¿Confirma el borrado de esta configuración?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
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
    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    If mdcProxy.ConfiguracionBursatilidads.Where(Function(i) i.intIdConfiguracionBursatilidad = EncabezadoSeleccionado.intIdConfiguracionBursatilidad).Count > 0 Then
                        mdcProxy.ConfiguracionBursatilidads.Remove(mdcProxy.ConfiguracionBursatilidads.Where(Function(i) i.intIdConfiguracionBursatilidad = EncabezadoSeleccionado.intIdConfiguracionBursatilidad).First)
                    End If


                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If

                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)
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
    ''' 
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaConfiguracionBursatilidad
            objCB.strBursatilidad = String.Empty
            objCB.logEntidadVigilada = False
            objCB.strClaseContable = String.Empty
            objCB.strClaseInversion = String.Empty

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
    Private Function ObtenerRegistroAnterior() As ConfiguracionBursatilidad
        Dim objEncabezado As ConfiguracionBursatilidad = New ConfiguracionBursatilidad

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of ConfiguracionBursatilidad)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIdConfiguracionBursatilidad = _EncabezadoSeleccionado.intIdConfiguracionBursatilidad
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
                'Valida la bursatilidad
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strBursatilidad) Then
                    strMsg = String.Format("{0}{1} + La bursatilidad es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la entidad vigilada
                If IsNothing(_EncabezadoSeleccionado.logEntidadVigilada) Then
                    strMsg = String.Format("{0}{1} + La entidad vigilada es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la clase de inversion
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strClaseInversion) Then
                    strMsg = String.Format("{0}{1} + La clase de inversión es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la clase de contable
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strClaseContable) Then
                    strMsg = String.Format("{0}{1} + La clase de contable es un campo requerido.", strMsg, vbCrLf)
                End If

            Else
                strMsg = String.Format("{0}{1} Debe de seleccionar un registro", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
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
                End If
                If strMsg.Equals(String.Empty) Then
                    If So.Error.ToString.Contains("El registro a insertar ya existe") Then
                        Dim intPosIni As Integer = So.Error.ToString.IndexOf("El registro a insertar ya existe")
                        Dim intPosFin As Integer = So.Error.ToString.IndexOf("|")
                        strMsg = So.Error.ToString.Substring(intPosIni, intPosFin - intPosIni)
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        strMsg = String.Empty
                    ElseIf So.Error.ToString.Contains("El registro a modificar ya existe") Then
                        Dim intPosIni As Integer = So.Error.ToString.IndexOf("El registro a modificar ya existe")
                        Dim intPosFin As Integer = So.Error.ToString.IndexOf("|")
                        strMsg = So.Error.ToString.Substring(intPosIni, intPosFin - intPosIni)
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        strMsg = String.Empty
                    Else
                        If Not So.Error Is Nothing Then
                            strMsg = So.Error.Message
                        End If
                    End If

                    If Not strMsg.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje(Program.obtenerMensajeValidacion(strMsg, So.UserState.ToString, True), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
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
        Dim objRet As LoadOperation(Of ConfiguracionBursatilidad)
        Dim dcProxy As EspeciesCFDomainContext

        Try
            dcProxy = inicializarProxyEspecies()

            objRet = Await dcProxy.Load(dcProxy.ConsultarConsultarConfiguracionBursatilidadPorDefectoQuery(Program.Usuario, Program.HashConexion)).AsTask


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
    ''' Consultar de forma sincrónica los datos de la configuracion bursatilidad
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pstrBursatilidad As String = "",
                                               Optional ByVal plogEntidadVigilada As Boolean? = Nothing,
                                               Optional ByVal pstrClaseInversion As String = "",
                                                Optional ByVal pstrClaseContable As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of ConfiguracionBursatilidad)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyEspecies()
            End If

            mdcProxy.ConfiguracionBursatilidads.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarConfiguracionBursatilidadSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarConfiguracionBursatilidadSyncQuery(pstrBursatilidad:=pstrBursatilidad,
                                                                                                        plogEntidadVigilada:=plogEntidadVigilada,
                                                                                                        pstrClaseInversion:=pstrClaseInversion,
                                                                                                        pstrClaseContable:=pstrClaseContable,
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
                    ListaEncabezado = mdcProxy.ConfiguracionBursatilidads

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de configuracion bursatilidad ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
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
Public Class CamposBusquedaConfiguracionBursatilidad
    Implements INotifyPropertyChanged

    Private _strBursatilidad As String
    <Display(Name:="Bursatilidad")> _
    Public Property strBursatilidad As String
        Get
            Return _strBursatilidad
        End Get
        Set(ByVal value As String)
            _strBursatilidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strBursatilidad"))
        End Set
    End Property

    Private _strlogEntidadVigilada As String
    Public Property strlogEntidadVigilada As String
        Get
            'Esta propiedad se le hace binding al combo y se cambia la variable logica segun venga
            Select Case _strlogEntidadVigilada
                Case Is = Nothing
                    logEntidadVigilada = Nothing
                Case Is = "SI"
                    logEntidadVigilada = True
                Case Is = "NO"
                    logEntidadVigilada = False
            End Select
            Return _strlogEntidadVigilada
        End Get
        Set(ByVal value As String)
            _strlogEntidadVigilada = value
            'Se asigna el valor que viene del combo a la variable Boolean
            logEntidadVigilada = CType(value, Boolean)

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strlogEntidadVigilada"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logEntidadVigilada"))
        End Set
    End Property




    'Se declara la variable como Nullable para que pueda enviar Null al procedimiento cuando no se haga una seleccion.
    Private _logEntidadVigilada As Nullable(Of Boolean)
    <Display(Name:="Vigilada")>
    Public Property logEntidadVigilada As Nullable(Of Boolean)
        Get

            Return _logEntidadVigilada
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            _logEntidadVigilada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logEntidadVigilada"))
        End Set
    End Property

    Private _strClaseInversion As String
    <Display(Name:="Clase Inversión")>
    Public Property strClaseInversion As String
        Get
            Return _strClaseInversion
        End Get
        Set(ByVal value As String)
            _strClaseInversion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strClaseInversion"))
        End Set
    End Property

    Private _strClaseContable As String
    <Display(Name:="Clase Contable")>
    Public Property strClaseContable As String
        Get
            Return _strClaseContable
        End Get
        Set(ByVal value As String)
            _strClaseContable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strClaseContable"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
