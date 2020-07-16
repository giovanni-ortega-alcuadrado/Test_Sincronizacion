Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

''' <summary>
''' ViewModel para la pantalla Indicadores perteneciente al proyecto de Cálculos Financieros.
''' </summary>
''' Creado por       : Jorge Peña (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : Febrero 21/2014
''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok ''' 
''' <remarks></remarks>

Public Class IndicadoresViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As Indicadores
    Private mdcProxyActualizar As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As Indicadores
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
    ''' Lista de Indicadores que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of Indicadores)
    Public Property ListaEncabezado() As EntitySet(Of Indicadores)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of Indicadores))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Indicadores para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de Indicadores que se encuentra seleccionado para editar y nuevo
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As Indicadores
    Public Property EncabezadoSeleccionado() As Indicadores
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As Indicadores)
            _EncabezadoSeleccionado = value
            If Not IsNothing(_EncabezadoSeleccionado) Then

                If _EncabezadoSeleccionado.strTipoIndicador = "1" Then 'TASA
                    VerTasas = Visibility.Visible
                    VerMonedas = Visibility.Collapsed

                ElseIf _EncabezadoSeleccionado.strTipoIndicador = "2" Then 'MONEDA
                    VerTasas = Visibility.Collapsed
                    VerMonedas = Visibility.Visible

                End If
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property


    ''' <summary>
    ''' Elemento de la lista de Indicadores que se encuentra seleccionado para la busqueda avanzada
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionadoBusqueda As Indicadores
    Public Property EncabezadoSeleccionadoBusqueda() As Indicadores
        Get
            Return _EncabezadoSeleccionadoBusqueda
        End Get
        Set(ByVal value As Indicadores)
            _EncabezadoSeleccionadoBusqueda = value
            If Not IsNothing(_EncabezadoSeleccionadoBusqueda) Then

                If _EncabezadoSeleccionadoBusqueda.strTipoIndicador = "1" Then 'TASA
                    VerTasas = Visibility.Visible
                    VerMonedas = Visibility.Collapsed

                ElseIf _EncabezadoSeleccionadoBusqueda.strTipoIndicador = "2" Then 'MONEDA
                    VerTasas = Visibility.Collapsed
                    VerMonedas = Visibility.Visible

                End If
            End If
            MyBase.CambioItem("EncabezadoSeleccionadoBusqueda")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private WithEvents _cb As CamposBusquedaIndicadores
    Public Property cb() As CamposBusquedaIndicadores
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaIndicadores)
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

    Private _VerMonedas As Visibility = Visibility.Collapsed
    Public Property VerMonedas() As Visibility
        Get
            Return _VerMonedas
        End Get
        Set(ByVal value As Visibility)
            _VerMonedas = value
            MyBase.CambioItem("VerMonedas")
        End Set
    End Property

    Private _VerTasas As Visibility = Visibility.Collapsed
    Public Property VerTasas() As Visibility
        Get
            Return _VerTasas
        End Get
        Set(ByVal value As Visibility)
            _VerTasas = value
            MyBase.CambioItem("VerTasas")
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

        Dim objNvoEncabezado As New Indicadores

        Try
            If mdcProxyActualizar.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of Indicadores)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.strDescripcion = String.Empty
                objNvoEncabezado.dtmFechaArchivo = Date.Now()
                objNvoEncabezado.dblValor = 0
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
            If cb.dtmFechaArchivo IsNot Nothing Then
                Await ConsultarEncabezado(False, String.Empty, cb.strTipoIndicador, cb.strTasaMoneda, CDate(cb.dtmFechaArchivo), cb.dblValor)
            Else
                Await ConsultarEncabezado(False, String.Empty, cb.strTipoIndicador, cb.strTasaMoneda, Nothing, cb.dblValor)
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

            If ValidarRegistro() Then
                ' Incializar los mensajes de validación
                _EncabezadoSeleccionado.strMsgValidacion = String.Empty
                ' Validar si el registro ya existe en la lista
                intNroOcurrencias = (From e In ListaEncabezado Where e.lngID = _EncabezadoSeleccionado.lngID Select e).Count

                If intNroOcurrencias = 0 Then
                    strAccion = ValoresUserState.Ingresar.ToString()
                    ListaEncabezado.Add(_EncabezadoSeleccionado)
                End If

                _EncabezadoSeleccionado.strTasaMoneda = _EncabezadoSeleccionado.strTasaMoneda.Trim()
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
    ''' 
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If mdcProxyActualizar.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ValidarTasaMonedaVacia() = False Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("La tasa / moneda no existe dentro de la lista configurada por favor vuelva a seleccionar para modificar el registro.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
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
    ''' Cambia la busqueda en el diccionario del INDICADOR para que tome el ID y no la descripción.
    ''' Esto por el cambio en el view
    ''' </summary>
    ''' 
    Private Function ValidarTasaMonedaVacia() As Boolean
        Dim logPasoValidacion As Boolean = False

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If Not IsNothing(DiccionarioCombosA2) Then
                    If VerTasas = Visibility.Visible Then
                        If DiccionarioCombosA2.ContainsKey("INDICADOR") Then
                            If DiccionarioCombosA2("INDICADOR").Where(Function(i) i.Descripcion = _EncabezadoSeleccionado.strTasaMoneda).Count > 0 Then
                                logPasoValidacion = True
                            End If
                        End If
                    ElseIf VerMonedas = Visibility.Visible Then
                        If DiccionarioCombosA2.ContainsKey("CFmonedas") Then
                            If DiccionarioCombosA2("CFmonedas").Where(Function(i) i.ID = _EncabezadoSeleccionado.strTasaMoneda).Count > 0 Then
                                logPasoValidacion = True
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el registro", Me.ToString(), "TasaMonedaVacia", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return logPasoValidacion
    End Function

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

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra el indicador seleccionado. ¿Confirma el borrado de este indicador?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
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

                    If mdcProxyActualizar.Indicadores.Where(Function(i) i.lngID = EncabezadoSeleccionado.lngID).Count > 0 Then
                        mdcProxyActualizar.Indicadores.Remove(mdcProxyActualizar.Indicadores.Where(Function(i) i.lngID = EncabezadoSeleccionado.lngID).First)
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
    ''' 
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaIndicadores
            objCB.strTipoIndicador = String.Empty
            objCB.strDescripcion = String.Empty
            objCB.dtmFechaArchivo = Date.Now()
            objCB.dblValor = 0
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
    Private Function ObtenerRegistroAnterior() As Indicadores
        Dim objEncabezado As Indicadores = New Indicadores

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of Indicadores)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.lngID = _EncabezadoSeleccionado.lngID
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
                'Valida el tipo de indicador
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipoIndicador) Then
                    strMsg = String.Format("{0}{1} + El tipo de indicador es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la Descripcion
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTasaMoneda) Then
                    strMsg = String.Format("{0}{1} + La descripción es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la fecha
                If IsNothing(_EncabezadoSeleccionado.dtmFechaArchivo) Then
                    strMsg = String.Format("{0}{1} + La fecha es un campo requerido.", strMsg, vbCrLf)
                End If

                If ValidarTasaMonedaVacia() = False Then
                    strMsg = String.Format("{0}{1} + La tasa / moneda no existe dentro de la lista configurada.", strMsg, vbCrLf)
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
    Private Async Sub consultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of Indicadores)
        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarIndicadoresPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
    ''' Consultar de forma sincrónica los datos de Indicadores
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pstrTipoIndicador As String = "",
                                               Optional ByVal pstrDescripcion As String = Nothing,
                                               Optional ByVal pdtmFechaArchivo As Date? = Nothing,
                                               Optional ByVal pdblValor As Double = 0) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of Indicadores)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxyActualizar Is Nothing Then
                mdcProxyActualizar = inicializarProxyCalculosFinancieros()
            End If

            mdcProxyActualizar.Indicadores.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.FiltrarIndicadoresSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.ConsultarIndicadoresSyncQuery(pstrTipoIndicador:=pstrTipoIndicador,
                                                                                                        pstrDescripcion:=pstrDescripcion,
                                                                                                        pdtmFechaArchivo:=pdtmFechaArchivo,
                                                                                                        pdblValor:=pdblValor,
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
                    ListaEncabezado = mdcProxyActualizar.Indicadores

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de Indicadores ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

    Private Sub _EncabezadoSeleccionado_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        If e.PropertyName = "strTipoIndicador" Then
            If _EncabezadoSeleccionado.strTipoIndicador = "1" Then 'TASA
                VerTasas = Visibility.Visible
                VerMonedas = Visibility.Collapsed

            ElseIf _EncabezadoSeleccionado.strTipoIndicador = "2" Then 'MONEDA
                VerTasas = Visibility.Collapsed
                VerMonedas = Visibility.Visible

            End If
        End If
    End Sub

    Private Sub _cb_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _cb.PropertyChanged
        If e.PropertyName = "strTipoIndicador" Then
            If cb.strTipoIndicador = "1" Then 'TASA
                VerTasas = Visibility.Visible
                VerMonedas = Visibility.Collapsed
            ElseIf cb.strTipoIndicador = "2" Then 'MONEDA
                VerTasas = Visibility.Collapsed
                VerMonedas = Visibility.Visible
            End If
        End If

    End Sub
End Class

''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
''' 
Public Class CamposBusquedaIndicadores
    Implements INotifyPropertyChanged

    Private _strTipoIndicador As String
    Public Property strTipoIndicador() As String
        Get
            Return _strTipoIndicador
        End Get
        Set(ByVal value As String)
            _strTipoIndicador = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoIndicador"))
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

    Private _strTasaMoneda As String
    Public Property strTasaMoneda() As String
        Get
            Return _strTasaMoneda
        End Get
        Set(ByVal value As String)
            _strTasaMoneda = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTasaMoneda"))
        End Set
    End Property

    Private _dtmFechaArchivo As System.Nullable(Of System.DateTime)
    Public Property dtmFechaArchivo() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaArchivo
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaArchivo"))
        End Set
    End Property

    Private _dblValor As Double
    Public Property dblValor() As Double
        Get
            Return _dblValor
        End Get
        Set(ByVal value As Double)
            _dblValor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValor"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class