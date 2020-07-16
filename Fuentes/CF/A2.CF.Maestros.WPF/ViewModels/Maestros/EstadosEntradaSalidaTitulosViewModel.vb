Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA.Web.CFMaestros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.Collections.ObjectModel

''' <summary>
''' ViewModel para la pantalla EstadosEntradaSalidaTitulos perteneciente al proyecto de Cálculos Financieros.
''' </summary>
''' Creado por       : Jorge Peña (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : 23 de agosto/2017
''' Pruebas CB       : Jorge Peña - 23 de agosto/2017 - Resultado Ok ''' 
''' <remarks></remarks>

Public Class EstadosEntradaSalidaTitulosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As EstadosEntradaSalidaTitulos
    Private mdcProxy As MaestrosCFDomainContext  ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As EstadosEntradaSalidaTitulos
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

    Private _DiccionarioCombosA2 As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombosA2() As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombosA2
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombosA2 = value
        End Set
    End Property

    ''' <summary>
    ''' Lista de EstadosEntradaSalidaTitulos que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of EstadosEntradaSalidaTitulos)
    Public Property ListaEncabezado() As EntitySet(Of EstadosEntradaSalidaTitulos)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of EstadosEntradaSalidaTitulos))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de EstadosEntradaSalidaTitulos para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de EstadosEntradaSalidaTitulos que se encuentra seleccionado para editar y nuevo
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As EstadosEntradaSalidaTitulos
    Public Property EncabezadoSeleccionado() As EstadosEntradaSalidaTitulos
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As EstadosEntradaSalidaTitulos)
            _EncabezadoSeleccionado = value
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private Sub _EncabezadoSeleccionado_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        Try
            If Editando = True And Not IsNothing(EncabezadoSeleccionado.strTopico) Then
                Select Case e.PropertyName
                    Case "strTopico"
                        ConsultarConsecutivo()
                    Case "strEstadoActual"
                        If _EncabezadoSeleccionado.strEstadoActual = "B" Then 'BLOQUEADA
                            HabilitarMotivoBloqueo = True
                        Else
                            HabilitarMotivoBloqueo = False 'PENDIENTE
                            EncabezadoSeleccionado.strMotivoBloqueo = Nothing
                        End If
                End Select
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el cambio de la propiedad.", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Elemento de la lista de EstadosEntradaSalidaTitulos que se encuentra seleccionado para la busqueda avanzada
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionadoBusqueda As EstadosEntradaSalidaTitulos
    Public Property EncabezadoSeleccionadoBusqueda() As EstadosEntradaSalidaTitulos
        Get
            Return _EncabezadoSeleccionadoBusqueda
        End Get
        Set(ByVal value As EstadosEntradaSalidaTitulos)
            _EncabezadoSeleccionadoBusqueda = value
            MyBase.CambioItem("EncabezadoSeleccionadoBusqueda")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private WithEvents _cb As CamposBusquedaEstadosEntradaSalidaTitulos
    Public Property cb() As CamposBusquedaEstadosEntradaSalidaTitulos
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaEstadosEntradaSalidaTitulos)
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

    Private _HabilitarEstado As Boolean = False
    Public Property HabilitarEstado() As Boolean
        Get
            Return _HabilitarEstado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEstado = value
            MyBase.CambioItem("HabilitarEstado")
        End Set
    End Property

    Private _HabilitarMotivoBloqueo As Boolean = False
    Public Property HabilitarMotivoBloqueo() As Boolean
        Get
            Return _HabilitarMotivoBloqueo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarMotivoBloqueo = value
            MyBase.CambioItem("HabilitarMotivoBloqueo")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New EstadosEntradaSalidaTitulos

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of EstadosEntradaSalidaTitulos)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.strRetorno = String.Empty
                objNvoEncabezado.strDescripcion = String.Empty
                objNvoEncabezado.strTopico = String.Empty
                objNvoEncabezado.strMecanismo = Nothing
                objNvoEncabezado.strEstadoActual = Nothing
                objNvoEncabezado.strMotivoBloqueo = Nothing
                objNvoEncabezado.logActivo = False
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado
            HabilitarEncabezado = True
            HabilitarEstado = False
            HabilitarMotivoBloqueo = False
            EncabezadoSeleccionado.strEstadoActual = "D"

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
            If Not IsNothing(cb.strDescripcion) Or Not IsNothing(cb.strTopico) Or Not IsNothing(cb.strMecanismo) Then 'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, cb.strDescripcion, cb.strTopico, cb.strMecanismo)
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
                _EncabezadoSeleccionado.strUsuario = Program.Usuario

                ' Validar si el registro ya existe en la lista
                intNroOcurrencias = (From e In ListaEncabezado Where e.intIDEstadosEntradaSalidaTitulos = _EncabezadoSeleccionado.intIDEstadosEntradaSalidaTitulos Select e).Count

                If intNroOcurrencias = 0 Then
                    strAccion = ValoresUserState.Ingresar.ToString()
                    ListaEncabezado.Add(_EncabezadoSeleccionado)
                End If

                ' _EncabezadoSeleccionado.strTasaMoneda = _EncabezadoSeleccionado.strTasaMoneda.Trim()
                ' Enviar cambios al servidor
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
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If Not EncabezadoSeleccionado.logActivo Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Este registro se encuentra inactivo, debe activarlo primero si lo desea modificar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

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

            If _EncabezadoSeleccionado.strEstadoActual = "B" Then 'BLOQUEADA
                HabilitarMotivoBloqueo = True
            Else
                HabilitarMotivoBloqueo = False  'PENDIENTE
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
                HabilitarMotivoBloqueo = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If mdcProxy.EstadosEntradaSalidaTitulos.Where(Function(i) i.intIDEstadosEntradaSalidaTitulos = EncabezadoSeleccionado.intIDEstadosEntradaSalidaTitulos And CBool(EncabezadoSeleccionado.logActivo) = False).Count > 0 Then
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción activa la opción seleccionada. ¿Confirma la activación de este registro?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)

                Else
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción inactiva la opción seleccionada. ¿Confirma la inactivación de este registro?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
                End If
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

                    If mdcProxy.EstadosEntradaSalidaTitulos.Where(Function(i) i.intIDEstadosEntradaSalidaTitulos = EncabezadoSeleccionado.intIDEstadosEntradaSalidaTitulos).Count > 0 Then
                        mdcProxy.EstadosEntradaSalidaTitulos.Remove(mdcProxy.EstadosEntradaSalidaTitulos.Where(Function(i) i.intIDEstadosEntradaSalidaTitulos = EncabezadoSeleccionado.intIDEstadosEntradaSalidaTitulos).First)
                    End If

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
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaEstadosEntradaSalidaTitulos
            objCB.strDescripcion = String.Empty
            objCB.strTopico = String.Empty
            objCB.strMecanismo = String.Empty
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
    Private Function ObtenerRegistroAnterior() As EstadosEntradaSalidaTitulos
        Dim objEncabezado As EstadosEntradaSalidaTitulos = New EstadosEntradaSalidaTitulos

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of EstadosEntradaSalidaTitulos)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDEstadosEntradaSalidaTitulos = _EncabezadoSeleccionado.intIDEstadosEntradaSalidaTitulos
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

                'Valida el retorno
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strRetorno) Then
                    strMsg = String.Format("{0}{1} + El retorno es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la descripción
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strDescripcion) Then
                    strMsg = String.Format("{0}{1} + La descripción es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tópico
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTopico) Then
                    strMsg = String.Format("{0}{1} + El tópico es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el mecanismo
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strMecanismo) Then
                    strMsg = String.Format("{0}{1} + El mecanismo es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el mecanismo
                If Not String.IsNullOrEmpty(EncabezadoSeleccionado.strTopico) Then
                    If String.IsNullOrEmpty(_EncabezadoSeleccionado.strEstadoActual) And _EncabezadoSeleccionado.strTopico = "ESTADOSDEENTRADA" Then
                        strMsg = String.Format("{0}{1} + El estado custodia es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If

                'Valida el tipo de bloqueo
                If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strEstadoActual) Then
                    If _EncabezadoSeleccionado.strEstadoActual = "B" And String.IsNullOrEmpty(_EncabezadoSeleccionado.strMotivoBloqueo) Then
                        strMsg = String.Format("{0}{1} + El tipo de bloqueo es un campo requerido.", strMsg, vbCrLf)
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

    Private Sub ConsultarConsecutivo()
        Try
            If Not IsNothing(EncabezadoSeleccionado) Then
                If (EncabezadoSeleccionado.intIDEstadosEntradaSalidaTitulos) = 0 And Not String.IsNullOrEmpty(EncabezadoSeleccionado.strTopico) Then

                    If mdcProxy Is Nothing Then
                        mdcProxy = inicializarProxyMaestros()
                    End If

                    'mdcProxy.EstadosEntradaSalidaTitulos.Clear()

                    mdcProxy.EstadosEntradaSalidaTitulos_Consecutivo_Consultar(EncabezadoSeleccionado.strTopico, Program.Usuario,
                              AddressOf TerminoEstadosEntradaSalidaTitulos_Consecutivo_Consultar, "")
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el retorno en el maestro de listas con el tópico: " & EncabezadoSeleccionado.strTopico,
                                                             Me.ToString(), "ConsultarConsecutivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoEstadosEntradaSalidaTitulos_Consecutivo_Consultar(lo As InvokeOperation(Of String))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el retorno en el maestro de listas con el tópico " & EncabezadoSeleccionado.strTopico,
                                                 Me.ToString(), "TerminoEstadosEntradaSalidaTitulos_Consecutivo_Consultar", Application.Current.ToString(), Program.Maquina, lo.Error)
            ElseIf lo.Value <> String.Empty Then
                EncabezadoSeleccionado.strRetorno = lo.Value
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el retorno en el maestro de listas con el tópico: " & EncabezadoSeleccionado.strTopico,
                                                             Me.ToString(), "TerminoEstadosEntradaSalidaTitulos_Consecutivo_Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

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
                HabilitarMotivoBloqueo = False

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
        Dim objRet As LoadOperation(Of EstadosEntradaSalidaTitulos)
        Dim dcProxy As MaestrosCFDomainContext

        Try
            dcProxy = inicializarProxyMaestros()

            objRet = Await dcProxy.Load(dcProxy.EstadosEntradaSalidaTitulos_ConsultarPorDefectoSyncQuery(Program.Usuario)).AsTask

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
    ''' Consultar de forma sincrónica los datos de EstadosEntradaSalidaTitulos
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pstrDescripcion As String = "",
                                               Optional ByVal pstrTopico As String = "",
                                               Optional ByVal pstrMecanismo As String = "",
                                               Optional ByVal pstrMotivoBloqueo As String = "",
                                               Optional ByVal pstrEstadoActual As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of EstadosEntradaSalidaTitulos)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyMaestros()
            End If

            mdcProxy.EstadosEntradaSalidaTitulos.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.EstadosEntradaSalidaTitulos_FiltrarSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.EstadosEntradaSalidaTitulos_ConsultarSyncQuery(pstrDescripcion:=pstrDescripcion,
                                                                                                        pstrTopico:=pstrTopico,
                                                                                                        pstrMecanismo:=pstrMecanismo,
                                                                                                                        pstrMotivoBloqueo:=pstrMotivoBloqueo,
                                                                                                                        pstrEstadoActual:=pstrEstadoActual,
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
                    ListaEncabezado = mdcProxy.EstadosEntradaSalidaTitulos

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de EstadosEntradaSalidaTitulos ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
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
Public Class CamposBusquedaEstadosEntradaSalidaTitulos
    Implements INotifyPropertyChanged

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

    Private _strTopico As String
    Public Property strTopico() As String
        Get
            Return _strTopico
        End Get
        Set(ByVal value As String)
            _strTopico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTopico"))
        End Set
    End Property

    Private _strMecanismo As String
    Public Property strMecanismo() As String
        Get
            Return _strMecanismo
        End Get
        Set(ByVal value As String)
            _strMecanismo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strMecanismo"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class