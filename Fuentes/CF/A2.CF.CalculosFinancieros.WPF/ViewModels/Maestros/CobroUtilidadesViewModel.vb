Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports A2.OyD.OYDServer.RIA.Web.CFProcesarPortafolios
Imports A2CFProcesarPortafolio

Public Class CobroUtilidadesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"

    Public ViewCobroUtilidades As CobroUtilidadesView = Nothing
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxyProcesar As ProcesarPortafoliosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios


    Dim NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO As String = "Código"
#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        inicializar()
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Descripción:    Creacion.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          24 de Agosto/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - 24 de Agosto/2014 - Resultado Ok 
    ''' </history>
    Public Function inicializar() As Boolean

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                ConsultarValoresPorDefecto()

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Propiedad para determinar la fila seleccionada y poder capturar los eventos de los campos modificados
    ''' </summary>
    Private WithEvents _UtilidadesCustodiasSelected As ProcesarUtilidadesCustodias
    Public Property UtilidadesCustodiasSelected() As ProcesarUtilidadesCustodias
        Get
            Return _UtilidadesCustodiasSelected
        End Get
        Set(ByVal value As ProcesarUtilidadesCustodias)

            If Not IsNothing(_UtilidadesCustodiasSelected) AndAlso _UtilidadesCustodiasSelected.Equals(value) Then
                Exit Property
            End If

            _UtilidadesCustodiasSelected = value
            MyBase.CambioItem("UtilidadesCustodiasSelected")
        End Set
    End Property

    ''' <summary>
    ''' Lista de UtilidadesCustodias que se encuentran cargadas en el grid del formulario modal (childWindow)
    ''' </summary>
    Private WithEvents _ListaUtilidadesCustodias As List(Of ProcesarUtilidadesCustodias)
    Public Property ListaUtilidadesCustodias() As List(Of ProcesarUtilidadesCustodias)
        Get
            Return _ListaUtilidadesCustodias
        End Get
        Set(ByVal value As List(Of ProcesarUtilidadesCustodias))
            _ListaUtilidadesCustodias = value
            MyBase.CambioItem("ListaUtilidadesCustodias")
            MyBase.CambioItem("ListaEncabezadoPaginada")
            If Not IsNothing(_ListaUtilidadesCustodias) And IsNothing(_UtilidadesCustodiasSelected) Then
                UtilidadesCustodiasSelected = _ListaUtilidadesCustodias.FirstOrDefault
            End If
        End Set
    End Property


    ''' <summary>
    ''' Colección que pagina la lista de ChoquesTasasInteres para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    Public ReadOnly Property ListaEncabezadoPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaUtilidadesCustodias) Then
                Dim view = New PagedCollectionView(_ListaUtilidadesCustodias)
                _ListaEncabezadoPaginada = view
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _dtmFechaProceso As System.Nullable(Of System.DateTime)
    Public Property dtmFechaProceso() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaProceso
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaProceso = value
            MyBase.CambioItem("dtmFechaProceso")
        End Set
    End Property

    Private WithEvents _lngIDComitente As String = String.Empty
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = value
            If lngIDComitente <> "" Then
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

    Private _strTipoCompania As String
    Public Property strTipoCompania() As String
        Get
            Return _strTipoCompania
        End Get
        Set(ByVal value As String)
            _strTipoCompania = value
            MyBase.CambioItem("strTipoCompania")
        End Set
    End Property

    Private _strEstadosRendimientos As String
    Public Property strEstadosRendimientos() As String
        Get
            Return _strEstadosRendimientos
        End Get
        Set(ByVal value As String)
            _strEstadosRendimientos = value
            MyBase.CambioItem("strEstadosRendimientos")
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

    ''' <summary>
    ''' Creado por:     Javier Eduardo Pardo Moreno
    ''' Descripción:    Propiedad para cargar la lista del tópico TIPOCOMPANIA y agregarle el item 'Todos' sin modificar datos, sino únicamente en esta pantalla
    ''' Fecha:          Septiembre 08/2015
    ''' ID del cambio:  JEPM20150908
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaTipoCompania As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTipoCompania As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTipoCompania
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTipoCompania = value
            MyBase.CambioItem("ListaTipoCompania")
        End Set
    End Property

    ''' <summary>
    ''' Creado por:     Javier Eduardo Pardo Moreno
    ''' Descripción:    Propiedad para cargar la lista del tópico ESTADOS_RENDIMIENTOS y agregarle el item 'Todos' sin modificar datos, sino únicamente en esta pantalla
    ''' Fecha:          Septiembre 09/2015
    ''' ID del cambio:  JEPM20150909
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaEstadosRendimientos As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaEstadosRendimientos As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaEstadosRendimientos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaEstadosRendimientos = value
            MyBase.CambioItem("ListaEstadosRendimientos")
        End Set
    End Property

    Private _logCobrarTodos As Boolean
    Public Property logCobrarTodos() As Boolean
        Get
            Return _logCobrarTodos
        End Get
        Set(ByVal value As Boolean)
            _logCobrarTodos = value
            CobrarTodasUtilidades(_logCobrarTodos)
            If _logCobrarTodos Then
                logAnularTodos = False
            End If
            MyBase.CambioItem("logCobrarTodos")
        End Set
    End Property

    Private _logAnularTodos As Boolean
    Public Property logAnularTodos() As Boolean
        Get
            Return _logAnularTodos
        End Get
        Set(ByVal value As Boolean)
            _logAnularTodos = value
            AnularTodasUtilidades(_logAnularTodos)
            If _logAnularTodos Then
                logCobrarTodos = False
            End If
            MyBase.CambioItem("logAnularTodos")
        End Set
    End Property

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ActualizarRegistro
    ''' </summary>
    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------

            'Valida la fecha de proceso
            If IsNothing(dtmFechaProceso) Then
                strMsg = String.Format("{0}{1} + La fecha de proceso es un campo requerido.", strMsg, vbCrLf)
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

    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Sub ConsultarValoresPorDefecto()
        Try
            dtmFechaProceso = Date.Now()
            lngIDComitente = String.Empty
            strNombreComitente = String.Empty

            If String.IsNullOrEmpty(NOMBRE_ETIQUETA_COMITENTE) Then
                NOMBRE_ETIQUETA_COMITENTE = NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO
            End If

            CargaListas()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarValoresPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Creado por:     Javier Eduardo Pardo Moreno
    ''' Descripción:    Método para refrescar la caché de los combos para ser llamado desde el botón de ControlRefrescarCache_PLUS
    ''' Fecha:          Septiembre 09/2015
    ''' ID del cambio:  JEPM20150909
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaListas()
        If Application.Current.Resources.Contains(Program.NombreListaCombos) Then

            'Consultar si contiene el elemento ESTADOS_RENDIMIENTOS
            If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("ESTADOS_RENDIMIENTOS") Then
                If Not IsNothing(CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("ESTADOS_RENDIMIENTOS")) Then
                    Dim objListaEstadosRendimientos As List(Of OYDUtilidades.ItemCombo)
                    objListaEstadosRendimientos = New List(Of OYDUtilidades.ItemCombo)(CType(Application.Current.Resources(Program.NombreListaCombos), 
                                 Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("ESTADOS_RENDIMIENTOS"))
                    objListaEstadosRendimientos.Add(New ItemCombo() With {
                                                                    .ID = "(TODOS)",
                                                                    .Retorno = "T",
                                                                    .Descripcion = "Todos",
                                                                    .Categoria = "ESTADOS_RENDIMIENTOS"
                                                                })
                    Me.ListaEstadosRendimientos = objListaEstadosRendimientos
                    Me.strEstadosRendimientos = objListaEstadosRendimientos.LastOrDefault().ID
                End If
            End If

            If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("TIPOCOMPANIA") Then
                If Not IsNothing(CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOCOMPANIA")) Then
                    Dim objListaTipoCompania As List(Of OYDUtilidades.ItemCombo)
                    objListaTipoCompania = New List(Of OYDUtilidades.ItemCombo)(CType(Application.Current.Resources(Program.NombreListaCombos), 
                                 Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOCOMPANIA"))
                    objListaTipoCompania.Add(New ItemCombo() With {
                                                                    .ID = "(TODOS)",
                                                                    .Retorno = "T",
                                                                    .Descripcion = "Todos",
                                                                    .Categoria = "TIPOCOMPANIA"
                                                                })
                    Me.ListaTipoCompania = objListaTipoCompania
                    Me.strTipoCompania = objListaTipoCompania.LastOrDefault().ID
                End If
            End If

        End If
    End Sub

#End Region

#Region "Métodos públicos del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Método para seleccionar o retirar la selección de todas las casillas tipo check en la columna de cobro
    ''' </summary>
    ''' <param name="blnIsChequed">Parámetro tipo Boolean</param>
    Public Sub CobrarTodasUtilidades(blnIsChequed As Boolean)
        Try
            If ListaUtilidadesCustodias IsNot Nothing Then
                For Each it In ListaUtilidadesCustodias
                    it.logCobro = blnIsChequed
                    If it.logCobro Then
                        it.logAnulado = False
                        If it.dblValorCobrado = 0 Then
                            it.dblValorCobrado = it.dblValorCalculado
                        End If
                    End If
                Next
                MyBase.CambioItem("ListaUtilidadesCustodias")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cobrar todas las utilidades.", Me.ToString(), "CobrarTodasUtilidades", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para seleccionar o retirar la selección de todas las casillas tipo check en la columna de anular
    ''' </summary>
    ''' <param name="blnIsChequed">Parámetro tipo Boolean</param>
    Public Sub AnularTodasUtilidades(blnIsChequed As Boolean)
        Try
            If ListaUtilidadesCustodias IsNot Nothing Then
                For Each it In ListaUtilidadesCustodias
                    it.logAnulado = blnIsChequed
                    If it.logAnulado Then
                        it.logCobro = False
                    End If
                Next
                MyBase.CambioItem("ListaUtilidadesCustodias")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular todas las utilidades.", Me.ToString(), "AnularTodasUtilidades", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function ConsultarDatosPortafolio() As Task
        Try
            Dim objRet As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros.DatosPortafolios)
            Dim mdcProxy As CalculosFinancierosDomainContext

            mdcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarDatosPortafolioSyncQuery(lngIDComitente, Program.Usuario, Program.HashConexion)).AsTask

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
                        A2Utilidades.Mensajes.mostrarMensaje("El " + NOMBRE_ETIQUETA_COMITENTE + " no existe o no es válido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        lngIDComitente = String.Empty
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

    Public Sub LimpiarGrid()
        Try
            ListaUtilidadesCustodias = Nothing
            Me.BorrarCliente = True
            Me.lngIDComitente = Nothing
            Me.strNombreComitente = Nothing
            Me.BorrarCliente = False

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el grid", _
                                                             Me.ToString(), "LimpiarGrid", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function RefresacarCache() As Task
        Try
            IsBusy = True

            Dim A2VM As New A2UtilsViewModel
            Await A2VM.inicializarCombos(String.Empty, String.Empty, True)

            CargaListas()

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Refresacar Cache. ", Me.ToString(), "RefresacarCache", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function
#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Se ejecuta al finalizar el proceso de valoración
    ''' </summary>
    ''' <param name="lo"></param>
    Private Async Function TerminoUtilidadesCustodiasActualizar(lo As InvokeOperation(Of String)) As Task
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó procesar valoracion", _
                                                 Me.ToString(), "TerminoUtilidadesCustodiasActualizar", Application.Current.ToString(), Program.Maquina, lo.Error)
            Else
                Await UtilidadesCustodiasConsultar()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó procesar valoracion", _
                                                             Me.ToString(), "TerminoUtilidadesCustodiasActualizar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Resultados Sincrónicos del encabezado - REQUERIDO"

    ''' <history>
    ''' Descripción      : Se adiciona la lógica para hacer el llamado al método "ValidarCobroUtilidadesPendientesSync" antes de consultar las custodias
    ''' Creado por       : Santiago Upegui G. (Alcuadrado S.A.)
    ''' Fecha            : Junio 31/2015
    ''' Pruebas CB       : Santiago Upegui G. (Alcuadrado S.A.) - Junio 31/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Modificado por   : Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 10/2015
    ''' Descripción      : Se envía el parámetro strReconstruccion vacío para evitar actualizar el parámetro CF_RECONSTRUIR_MOVIMIENTOS
    ''' Pruebas CB       : Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 10/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Modificado por   : Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 23/2015
    ''' Descripción      : Se retira la funcionalidad ValidarOperacionesPendientesQuery ya que no es requerida para esta pantalla
    ''' Pruebas CB       : Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 23/2015 - Resultado Ok 
    ''' </history>
    Public Async Function UtilidadesCustodiasConsultar() As Task
        Dim objRet As LoadOperation(Of ProcesarUtilidadesCustodias)

        Try
            If mdcProxyProcesar Is Nothing Then
                mdcProxyProcesar = inicializarProxyProcesarPortafolios()
            End If

            mdcProxyProcesar.ProcesarUtilidadesCustodias.Clear()

            ListaUtilidadesCustodias = New List(Of ProcesarUtilidadesCustodias)

            If ValidarDatos() Then

                If Not IsBusy Then
                    IsBusy = True
                End If

                Dim objRet2 As InvokeOperation(Of Boolean)

                objRet2 = Await mdcProxyProcesar.ValidarCobroUtilidadesPendientesSync(pdtmFechaValoracion:=dtmFechaProceso,
                                                                                pstrIdEspecie:=Nothing,
                                                                                plngIDComitente:=lngIDComitente,
                                                                                pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion).AsTask()

                If Not objRet2 Is Nothing Then
                    If objRet2.HasError Then
                        If objRet2.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta.", Me.ToString(), "UtilidadesCustodiasConsultar", Program.TituloSistema, Program.Maquina, objRet2.Error)
                        End If

                        objRet2.MarkErrorAsHandled()
                    Else
                        Dim strTempstrTipoCompania As String = ""
                        Dim strTempstrEstadosRendimientos As String = ""

                        If strTipoCompania <> "(TODOS)" Then
                            strTempstrTipoCompania = strTipoCompania
                        End If
                        If strEstadosRendimientos <> "(TODOS)" Then
                            strTempstrEstadosRendimientos = strEstadosRendimientos
                        End If

                        objRet = Await mdcProxyProcesar.Load(mdcProxyProcesar.UtilidadesCustodiasConsultarQuery(pdtmFechaValoracion:=dtmFechaProceso,
                                                                        pstrIdEspecie:=Nothing,
                                                                        plngIDComitente:=lngIDComitente,
                                                                        pstrTipoCompania:=strTempstrTipoCompania,
                                                                        pstrEstado:=strTempstrEstadosRendimientos,
                                                                        pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
                        If Not objRet Is Nothing Then
                                If objRet.HasError Then
                                    If objRet.Error Is Nothing Then
                                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                                    Else
                                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "UtilidadesCustodiasConsultar", Program.TituloSistema, Program.Maquina, objRet.Error)
                                    End If

                                    objRet.MarkErrorAsHandled()
                                Else
                                ListaUtilidadesCustodias = mdcProxyProcesar.ProcesarUtilidadesCustodias.ToList
                            End If
                            End If
                        End If

                    End If

                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las Utilidades de Custodias.", Me.ToString(), "UtilidadesCustodiasConsultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Método del botón aceptar de la ventana modal para enviar la información digitada a la base de datos
    ''' </summary>
    Public Sub RealizarProceso()

        Dim objRetornovalor As String = String.Empty
        Try
            If Not IsNothing(ListaUtilidadesCustodias) Then

                If ListaUtilidadesCustodias.Count > 0 Then

                    If mdcProxy Is Nothing Then
                        mdcProxy = inicializarProxyCalculosFinancieros()
                    End If

                    Dim xmlCompleto As String = String.Empty
                    Dim xmlDetalle As String = String.Empty

                    Dim ValidarCobroUtilidad As Boolean = True

                    IsBusy = True

                    If Not IsNothing(ListaUtilidadesCustodias) Then

                        xmlCompleto = "<CobroUtilidades>"

                        For Each objeto In (From c In ListaUtilidadesCustodias)
                            If (objeto.logCobro And objeto.dblValorCobrado = 0) Then
                                ValidarCobroUtilidad = False
                                Exit For
                            ElseIf (objeto.logCobro Or objeto.logAnulado) Then
                                xmlDetalle = "<Detalle intID=""" & objeto.intID &
                                             """ dblValorCobrado=""" & objeto.dblValorCobrado &
                                             """ logCobrado=""" & objeto.logCobro &
                                             """ logAnulado=""" & objeto.logAnulado & """ ></Detalle>"
                                xmlCompleto = xmlCompleto & xmlDetalle
                            End If
                        Next
                        xmlCompleto = xmlCompleto & "</CobroUtilidades>"

                    End If

                    If ValidarCobroUtilidad Then
                        mdcProxyProcesar.UtilidadesCustodiasActualizar(pxmlCobroUtilidades:=xmlCompleto, pstrUsuario:=Program.Usuario, pdtmFechaValoracion:=dtmFechaProceso, pstrIdEspecie:=Nothing, plngIDComitente:=lngIDComitente, pstrTipoPortafolio:=Nothing, pstrTipoProceso:=Nothing, pstrInfoConexion:=Program.HashConexion, callback:=AddressOf TerminoUtilidadesCustodiasActualizar, userState:="")
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("El valor a cobrar debe ser diferente de cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If

                    IsBusy = False

                End If

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cobro de utilidades.", Me.ToString(), "UtilidadesCustodiasActualizar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

#End Region


    Private Sub _UtilidadesCustodiasSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _UtilidadesCustodiasSelected.PropertyChanged
        If e.PropertyName = "logAnulado" Then
            If _UtilidadesCustodiasSelected.logAnulado Then
                UtilidadesCustodiasSelected.logCobro = False
            End If
        End If

        If e.PropertyName = "logCobro" Then
            If _UtilidadesCustodiasSelected.logCobro Then
                UtilidadesCustodiasSelected.logAnulado = False
            End If
        End If

    End Sub
End Class

