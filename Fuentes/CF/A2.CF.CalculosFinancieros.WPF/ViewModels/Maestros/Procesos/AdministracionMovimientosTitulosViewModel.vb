Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

''' <summary>
''' ViewModel para la pantalla MvtoCustodiasAplicados perteneciente al proyecto de Cálculos Financieros.
''' </summary>
''' Creado por       : Jorge Peña (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : 20 Agosto/2015
''' Pruebas CB       : Jorge Peña - 20 Agosto/2015 - Resultado Ok 
''' <remarks></remarks>

Public Class AdministracionMovimientosTitulosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As MvtoCustodiasAplicados
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As MvtoCustodiasAplicados
    Dim NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO As String = "Código"
#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
        Index = 0
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID caso de prueba:  CP0001
    ''' Fecha:              20 Agosto/2015
    ''' Pruebas CB:         Jorge Peña - 20 Agosto/2015 - Resultado Ok 
    ''' </history>
    Public Function inicializar() As Boolean

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefectoSync()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                'Await ConsultarEncabezado(True, String.Empty)

                If String.IsNullOrEmpty(NOMBRE_ETIQUETA_COMITENTE) Then
                    NOMBRE_ETIQUETA_COMITENTE = NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO
                End If

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
    ''' Lista de MvtoCustodiasAplicados que se encuentran cargadas en el grid del formulario
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0002
    ''' Fecha:              20 Agosto/2015
    ''' Pruebas CB:         Jorge Peña - 20 Agosto/2015 - Resultado Ok 
    ''' </history>
    Private _ListaEncabezado As EntitySet(Of MvtoCustodiasAplicados)
    Public Property ListaEncabezado() As EntitySet(Of MvtoCustodiasAplicados)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of MvtoCustodiasAplicados))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de MvtoCustodiasAplicados para navegar sobre el grid con paginación
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

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso detalle de codificacion de activos
    ''' </summary>
    Private _ListaDetalle As List(Of MovimientosParticipacionesFondosDetalle)
    Public Property ListaDetalle() As List(Of MovimientosParticipacionesFondosDetalle)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of MovimientosParticipacionesFondosDetalle))
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

    Private _NOMBRE_ETIQUETA_COMITENTE As String = Program.STR_NOMBRE_ETIQUETA_COMITENTE
    Public Property NOMBRE_ETIQUETA_COMITENTE() As String
        Get
            Return _NOMBRE_ETIQUETA_COMITENTE
        End Get
        Set(ByVal value As String)
            _NOMBRE_ETIQUETA_COMITENTE = value
            MyBase.CambioItem("NOMBRE_ETIQUETA_COMITENTE")
        End Set
    End Property

    Private _strIdEspecie As String = String.Empty
    Public Property strIdEspecie() As String
        Get
            Return _strIdEspecie
        End Get
        Set(ByVal value As String)
            _strIdEspecie = value
            MyBase.CambioItem("strIdEspecie")
        End Set
    End Property

    Private _lngIDComitente As String = String.Empty
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = LTrim(RTrim(value))

            If Not String.IsNullOrEmpty(lngIDComitente) Then
                ConsultarDatosPortafolio()
            End If

            MyBase.CambioItem("lngIDComitente")
        End Set
    End Property

    Private _strNombreComitente As String = String.Empty
    Public Property strNombreComitente() As String
        Get
            Return _strNombreComitente
        End Get
        Set(ByVal value As String)
            _strNombreComitente = value
            MyBase.CambioItem("strNombreComitente")
        End Set
    End Property

    Private _BorrarCliente As Boolean = False
    Public Property BorrarCliente() As Boolean
        Get
            Return _BorrarCliente
        End Get
        Set(ByVal value As Boolean)
            _BorrarCliente = value
            MyBase.CambioItem("BorrarCliente")
        End Set
    End Property

    Private _BorrarEspecie As Boolean = False
    Public Property BorrarEspecie() As Boolean
        Get
            Return _BorrarEspecie
        End Get
        Set(ByVal value As Boolean)
            _BorrarEspecie = value
            MyBase.CambioItem("BorrarEspecie")
        End Set
    End Property

    Private _strIsin As String = String.Empty
    Public Property strIsin() As String
        Get
            Return _strIsin
        End Get
        Set(ByVal value As String)
            _strIsin = value
            MyBase.CambioItem("strIsin")
        End Set
    End Property

    Private _dtmFechaMovimiento As System.Nullable(Of Date)
    Public Property dtmFechaMovimiento() As System.Nullable(Of Date)
        Get
            Return _dtmFechaMovimiento
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _dtmFechaMovimiento = value
            MyBase.CambioItem("dtmFechaMovimiento")
        End Set
    End Property

    Private _strTipo As String = String.Empty
    Public Property strTipo() As String
        Get
            Return _strTipo
        End Get
        Set(ByVal value As String)
            _strTipo = value
            MyBase.CambioItem("strTipo")
        End Set
    End Property

    Private _strEstado As String = String.Empty
    Public Property strEstado() As String
        Get
            Return _strEstado
        End Get
        Set(ByVal value As String)
            _strEstado = value
            MyBase.CambioItem("strEstado")
        End Set
    End Property

    Private _Index As Integer
    Public Property Index() As Integer
        Get
            Return _Index
        End Get
        Set(ByVal value As Integer)
            _Index = value
            MyBase.CambioItem("Index")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    Public Async Sub ConfirmarBuscarRegistro()
        Try
            'Valida el estado salida
            'If IsNothing(dtmFechaMovimiento) Then
            '    A2Utilidades.Mensajes.mostrarMensaje("La fecha de movimiento es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If

            If Not IsNothing(strTipo) Or Not IsNothing(lngIDComitente) Or
                Not IsNothing(strIdEspecie) Or Not IsNothing(strIsin) Then
                'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, strTipo, lngIDComitente, strIdEspecie, dtmFechaMovimiento, strIsin, strEstado)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0009
    ''' Fecha:              20 Agosto/2015
    ''' Pruebas CB:         Jorge Peña - 20 Agosto/2015 - Resultado Ok 
    ''' </history>
    Public Sub CambiarEstado()
        Try

            If ValidarRegistro() Then
                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción activa o inactiva los registros seleccionados. ¿Confirma la activación o inactivación de estos registros?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "CambiarEstado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcutá cuando el usuario confirma la eliminación del encabezado activo.
    ''' Ejecuta el proceso de eliminación en la base de datos
    ''' </summary>
    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then

                IsBusy = True
                Program.VerificarCambiosProxyServidor(mdcProxy)
                mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para seleccionar o retirar la selección de todas las casillas tipo check en la columna de cobro
    ''' </summary>
    ''' <param name="blnIsChequed">Parámetro tipo Boolean</param>
    Public Sub SeleccionarTodos(blnIsChequed As Boolean)
        Try
            If ListaEncabezado IsNot Nothing Then
                For Each it In ListaEncabezado
                    it.logActivo = blnIsChequed
                Next
                MyBase.CambioItem("ListaEncabezado")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cobrar todas las utilidades.", Me.ToString(), "CobrarTodasUtilidades", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta el nombre y la fecha de portafolio de un cliente.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function ConsultarDatosPortafolio() As Task
        Try
            Dim objRet As LoadOperation(Of DatosPortafolios)
            Dim dcProxy As CalculosFinancierosDomainContext

            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarDatosPortafolioSyncQuery(_lngIDComitente, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        strNombreComitente = objRet.Entities.First.strNombre
                    Else
                        strNombreComitente = Nothing
                    End If
                End If
            Else
                strNombreComitente = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosPortafolio. ", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function
#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(ListaEncabezado) Then
                If Not ListaEncabezado.Count > 0 Then
                    strMsg = String.Format("{0}{1} Debe haber por lo menos un registro para activar o inactivar", strMsg, vbCrLf)
                End If
            Else
                strMsg = String.Format("{0}{1} Debe haber por lo menos un registro para activar o inactivar", strMsg, vbCrLf)
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
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
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

                MyBase.TerminoSubmitChanges(So)
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico

                ConfirmarBuscarRegistro()
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
        Dim objRet As LoadOperation(Of MvtoCustodiasAplicados)
        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarMvtoCustodiasAplicadosPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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

            lngIDComitente = String.Empty
            strNombreComitente = String.Empty
            strIdEspecie = String.Empty
            'strTipo = String.Empty
            dtmFechaMovimiento = Date.Now()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de MvtoCustodiasAplicados
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Public Async Function ConsultarEncabezado(Optional ByVal plogFiltrar As Boolean = False,
                                                Optional ByVal pstrFiltro As String = "",
                                                Optional ByVal pstrTipo As String = "",
                                                Optional ByVal plngIDComitente As String = "",
                                                Optional ByVal pstrIdEspecie As String = "",
                                                Optional ByVal pdtmFechaMovimiento As System.Nullable(Of Date) = Nothing,
                                                Optional ByVal pstrIsin As String = "",
                                                Optional ByVal pstrEstado As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of MvtoCustodiasAplicados)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.MvtoCustodiasAplicados.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarMvtoCustodiasAplicadosSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarMvtoCustodiasAplicadosSyncQuery(pstrTipo:=pstrTipo,
                                                                                               plngIDComitente:=plngIDComitente,
                                                                                               pstrIDEspecie:=pstrIdEspecie,
                                                                                               pdtmFechaMovimiento:=pdtmFechaMovimiento,
                                                                                               pstrISIN:=pstrIsin,
                                                                                               pstrEstado:=pstrEstado,
                                                                                               pstrUsuario:=Program.Usuario,
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
                    ListaEncabezado = mdcProxy.MvtoCustodiasAplicados

                    If Not objRet.Entities.Count > 0 Then
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de MvtoCustodiasAplicados ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

End Class


