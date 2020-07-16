Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFProcesarPortafolios
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports System.Collections.ObjectModel

Public Class ProcesarPortafolioViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

    ''' <summary>
    ''' ViewModel para la pantalla Procesar Portafolio perteneciente al proyecto de Cálculos Financieros.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion. 
    ''' Fecha            : Febrero 21/2014
    ''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok 
    ''' </history>

#Region "Constantes"
    Private Const STR_PARAMETROS_EXPORTAR As String = "[FECHAPROCESO]=[[FECHAPROCESO]]|[TIPOPROCESO]=[[TIPOPROCESO]]|[TIPOPORTAFOLIO]=[[TIPOPORTAFOLIO]]|[FECHA_VALORACION_INICIAL]=[[FECHA_VALORACION_INICIAL]]|[FECHA_VALORACION_FINAL]=[[FECHA_VALORACION_FINAL]]|[LNGIDCOMITENTE]=[[LNGIDCOMITENTE]]|[MODULO]=[[MODULO]]|[USUARIO]=[[USUARIO]]"
    Private Const STR_CARPETA As String = "EXPORTARINFVAL"
    Private Const STR_PROCESO As String = "ExportarValoracion"

    Private Enum PARAMETROSEXPORTAR
        FECHAPROCESO
        FECHA_VALORACION_INICIAL
        FECHA_VALORACION_FINAL
        LNGIDCOMITENTE
        TIPOPORTAFOLIO
        TIPOPROCESO
        MODULO
        USUARIO
    End Enum

    Private Const STR_PROCESO_AVANCE As String = "ExportarAvance"

#End Region

#Region "Variables - REQUERIDO"

    Public ViewProcesarPortafolio As ProcesarPortafolioView = Nothing

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Métodos utilizados para la ventana modal con el cobro de utilidades pendientes
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjVM As ProcesoCobroUtilidadesViewModel
    Private ViewProcesoCobroUtilidades As ProcesoCobroUtilidadesView

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As ProcesarPortafoliosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxy2 As ProcesarPortafoliosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mintLapsoRecargaAutomaticaProcesarPortafolio As Integer
    Private mstrHabilitarRecargaAutomaticaProcesarPortafolio As String
    Private mlogActivarIsLoading As Boolean = False
    Private mstrEliminarCierreTodosLosPortafolios As String
    Private mstrReconstruirMovimientos As String
    Private logCierreContinuo As Boolean = False
    Private mintCierreContinuo_LimiteDias As Integer = 5
    Private mstrCierreContinuo_LimiteDias_Permitir As String
    Dim NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO As String = "Código"

    Dim intAcumuladorDias As Integer = 0
    Dim intDiferenciaDias As Integer = 0
    Dim dtmFechaValoracion As System.Nullable(Of System.DateTime)
    Dim logMostrarMensajeReconstruirMov As Boolean = True
    Dim logMostrarMensajeFechasSinValorar As Boolean = True
    Dim logMostrarMensajeFechaDeCierreInferior As Boolean = True
    Dim logIniciarJobValoracion As Boolean = False
    Dim logEliminarDatosResultadoMotor As Boolean = True
    Dim logErrorAlTerminarProcesar As Boolean = False 'JEPM20181004

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        'If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
        '    strTipoPortafolio = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOPORTAFOLIO_VAL").FirstOrDefault.ID
        'End If
        'IndexTipoProceso = 1
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
        strTipoProceso = "T" 'JEPM20181002
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Jorge Peña
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 21/2014
    ''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: 
    ''' Descripción:       Se agrega el llamado al método ConsultarParametros para implementar el times.
    ''' Responsable:       Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 22/2014
    ''' Pruebas CB:        Jorge Peña - Abril 22/2014 - Resultados OK
    ''' </history> 
    Public Function inicializar() As Boolean
        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                ConsultarValoresPorDefecto()

                ConsultarParametros()

                If String.IsNullOrEmpty(NOMBRE_ETIQUETA_COMITENTE) Then
                    NOMBRE_ETIQUETA_COMITENTE = NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO
                End If

                strTipoProceso = "T" 'JEPM20181002

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

    Private _dtmFechaValoracionInicial As System.Nullable(Of System.DateTime)
    Public Property dtmFechaValoracionInicial() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaValoracionInicial
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaValoracionInicial = value
            MyBase.CambioItem("dtmFechaValoracionInicial")
        End Set
    End Property

    Private _dtmFechaValoracionFinal As System.Nullable(Of System.DateTime)
    Public Property dtmFechaValoracionFinal() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaValoracionFinal
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaValoracionFinal = value
            MyBase.CambioItem("dtmFechaValoracionFinal")
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

    Private _strTipoPortafolioSeleccionado As String
    Public Property strTipoPortafolioSeleccionado() As String
        Get
            Return _strTipoPortafolioSeleccionado
        End Get
        Set(ByVal value As String)
            _strTipoPortafolioSeleccionado = value
            MyBase.CambioItem("strTipoPortafolioSeleccionado")

            If _strTipoPortafolioSeleccionado = "(Todos)" Then
                strTipoPortafolio = String.Empty
            Else
                strTipoPortafolio = _strTipoPortafolioSeleccionado
            End If
        End Set
    End Property

    Private _strTipoPortafolio As String = String.Empty
    Public Property strTipoPortafolio() As String
        Get
            Return _strTipoPortafolio
        End Get
        Set(ByVal value As String)
            _strTipoPortafolio = value
            MyBase.CambioItem("strTipoPortafolio")
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

    Private _strTipoProceso As String = String.Empty
    Public Property strTipoProceso() As String
        Get
            Return _strTipoProceso
        End Get
        Set(ByVal value As String)
            _strTipoProceso = value
            MyBase.CambioItem("strTipoProceso")
        End Set
    End Property

    Private _IndexTipoPortafolio As Integer
    Public Property IndexTipoPortafolio() As Integer
        Get
            Return _IndexTipoPortafolio
        End Get
        Set(ByVal value As Integer)
            _IndexTipoPortafolio = value
            MyBase.CambioItem("IndexTipoPortafolio")
        End Set
    End Property

    Private _IndexTipoProceso As Integer
    Public Property IndexTipoProceso() As Integer
        Get
            Return _IndexTipoProceso
        End Get
        Set(ByVal value As Integer)
            _IndexTipoProceso = value
            MyBase.CambioItem("IndexTipoProceso")
        End Set
    End Property

    Private _VisibilidadBoton As Visibility = Visibility.Visible
    Public Property VisibilidadBoton() As Visibility
        Get
            Return _VisibilidadBoton
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadBoton = value
            MyBase.CambioItem("VisibilidadBoton")
        End Set
    End Property

    Private _HabilitarBoton As Boolean = True
    Public Property HabilitarBoton() As Boolean
        Get
            Return _HabilitarBoton
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBoton = value
            MyBase.CambioItem("HabilitarBoton")
        End Set
    End Property

    'Se crean las propiedades necesarias para poder habilitar y desactivar los controles SM20150918
    Private _IsBusyProcesando As Boolean = False
    Public Property IsBusyProcesando() As Boolean
        Get
            Return _IsBusyProcesando
        End Get
        Set(ByVal value As Boolean)
            _IsBusyProcesando = value
            MyBase.CambioItem("IsBusyProcesando")
        End Set
    End Property

    Private _EliminarBoton As Boolean = False
    Public Property EliminarBoton() As Boolean
        Get
            Return _EliminarBoton
        End Get
        Set(ByVal value As Boolean)
            _EliminarBoton = value
            MyBase.CambioItem("EliminarBoton")
        End Set
    End Property

    Private _InformeBoton As Boolean = False
    Public Property InformeBoton() As Boolean
        Get
            Return _InformeBoton
        End Get
        Set(ByVal value As Boolean)
            _InformeBoton = value
            MyBase.CambioItem("InformeBoton")
        End Set
    End Property

    '''<summary>
    ''' Propiedad para inhabilitar el botón refrescar cuando el parámetro mstrHabilitarRecargaAutomaticaProcesarPortafolio está activo.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: 
    ''' Responsable:       Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 22/2014
    ''' Pruebas CB:        Jorge Peña - Abril 22/2014 - Resultados OK
    ''' </history> 
    Private _HabilitarBotonRefrescar As Boolean = True
    Public Property HabilitarBotonRefrescar() As Boolean
        Get
            Return _HabilitarBotonRefrescar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotonRefrescar = value
            MyBase.CambioItem("HabilitarBotonRefrescar")
        End Set
    End Property


    ''' <summary>
    ''' Lista de ProcesarPortafolio que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of ProcesarPortafolio)
    Public Property ListaEncabezado() As EntitySet(Of ProcesarPortafolio)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of ProcesarPortafolio))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de ProcesarPortafolio para navegar sobre el grid con paginación
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

    Private _logCerrarPortafolios As Boolean = True
    Public Property logCerrarPortafolios() As Boolean
        Get
            Return _logCerrarPortafolios
        End Get
        Set(ByVal value As Boolean)
            _logCerrarPortafolios = value
            MyBase.CambioItem("logCerrarPortafolios")
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
    ''' Descripción:    Propiedad para el checkbox chkContabilizar en Falso por defecto
    ''' Fecha:          Septiembre 29/2015
    ''' ID del cambio:  JEPM20150929
    ''' </summary>
    Private _logContabilizar As Boolean = False
    Public Property logContabilizar() As Boolean
        Get
            Return _logContabilizar
        End Get
        Set(ByVal value As Boolean)
            _logContabilizar = value
            MyBase.CambioItem("logContabilizar")
        End Set
    End Property

    ''' <summary>
    ''' Creado por:     
    ''' Descripción:    Propiedad para el checkbox chkReconstruir
    ''' Fecha:          Diciembre 10/2015
    ''' ID del cambio:  GAG20151210
    ''' </summary>
    Private _logReconstruir As Boolean = False
    Public Property logReconstruir() As Boolean
        Get
            Return _logReconstruir
        End Get
        Set(ByVal value As Boolean)
            _logReconstruir = value
            MyBase.CambioItem("logReconstruir")
        End Set
    End Property

#End Region

#Region "Propiedades de la Especie"

    Private _NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            _NemotecnicoSeleccionado = value
            strIdEspecie = String.Empty
            MyBase.CambioItem("strIdEspecie")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Método para confirmar si continua el proceso si hay fechas sin valoración inferiores a la fecha de valoración.
    ''' </summary>
    ''' <history>
    ''' Modificado por   : Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 10/2015
    ''' Descripción      : Se envía el parámetro strReconstruccion para actualizar el parámetro CF_RECONSTRUIR_MOVIMIENTOS
    ''' Pruebas CB       : Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 10/2015 - Resultado Ok 
    ''' </history>
    Private Async Sub ConfirmarFechaValoracionInferior(ByVal sender As Object, ByVal e As EventArgs)
        Try
            'Se inicializa en true por defecto, por el cierre continuo
            Dim logPresionaronBotonSI As Boolean = True

            'Se utiliza la variable logPresionaronBotonSI cuando no es cierre continuo porque el sender viene nothing
            If Not IsNothing(sender) Then
                If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    logPresionaronBotonSI = False
                Else
                    logPresionaronBotonSI = True
                End If
            End If

            If Not logPresionaronBotonSI Then

                'CerrarTemporizador()
                IsBusy = False
                HabilitarBoton = True
                IsBusyProcesando = False
                EliminarBoton = True
                InformeBoton = True
            End If

            If logPresionaronBotonSI Then
                mdcProxy.ProcesarUtilidadesCustodias.Clear()

                Dim objRet As InvokeOperation(Of Boolean)

                objRet = Await mdcProxy.ValidarCobroUtilidadesPendientesSync(dtmFechaValoracion, strIdEspecie, lngIDComitente, Program.Usuario, Program.HashConexion).AsTask()

                If logCierreContinuo Then
                    objRet = Nothing
                End If

                If Not IsNothing(objRet) Then
                    If objRet.Value Then
                        mobjVM = New ProcesoCobroUtilidadesViewModel
                        ViewProcesoCobroUtilidades = New ProcesoCobroUtilidadesView(mobjVM, dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, Program.Usuario)
                        AddHandler ViewProcesoCobroUtilidades.Closed, AddressOf ViewProcesoCobroUtilidades_Closed
                        Program.Modal_OwnerMainWindowsPrincipal(ViewProcesoCobroUtilidades)
                        ViewProcesoCobroUtilidades.ShowDialog()
                    Else
                        Dim objRet2 As LoadOperation(Of ProcesarPortafolio)

                        If ValidarDatos() Then
                            mdcProxy.ProcesarPortafolios.Clear()

                            objRet2 = Await mdcProxy.Load(mdcProxy.ValidarOperacionesPendientesQuery(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, CStr(IIf(logReconstruir, "SI", "NO")), Program.Usuario, Program.HashConexion)).AsTask()

                            If Not objRet2 Is Nothing Then
                                If objRet2.HasError Then
                                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el informe de procesamiento", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                                End If
                            End If

                            ListaEncabezado = mdcProxy.ProcesarPortafolios
                        End If

                        mdcProxy.ValidarFechaCierrePortafolio(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, logCerrarPortafolios, Program.Usuario, Program.HashConexion, _
                                                              AddressOf TerminoValidarFechaCierrePortafolio, "")
                    End If
                End If

                'Cuando es cierre continuo
                If IsNothing(objRet) Then
                    Dim objRet2 As LoadOperation(Of ProcesarPortafolio)

                    If ValidarDatos() Then
                        mdcProxy.ProcesarPortafolios.Clear()
                        objRet2 = Await mdcProxy.Load(mdcProxy.ValidarOperacionesPendientesQuery(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, CStr(IIf(logReconstruir, "SI", "NO")), Program.Usuario, Program.HashConexion)).AsTask()

                        If Not objRet2 Is Nothing Then
                            If objRet2.HasError Then
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el informe de procesamiento.", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                            End If
                        End If

                        ListaEncabezado = mdcProxy.ProcesarPortafolios

                        For Each li In ListaEncabezado
                            If li.strDescripcion.Contains("tiene operaciones pendientes") And Not String.IsNullOrEmpty(lngIDComitente) Then
                                IsBusy = False
                                IsBusyProcesando = False
                                HabilitarBoton = True
                                InformeBoton = True
                                intAcumuladorDias = 0
                                intDiferenciaDias = 0
                                logIniciarJobValoracion = False
                                logEliminarDatosResultadoMotor = False
                                Exit Sub
                            End If
                        Next
                    End If

                    mdcProxy.ValidarFechaCierrePortafolio(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, logCerrarPortafolios, Program.Usuario, Program.HashConexion, _
                                                          AddressOf TerminoValidarFechaCierrePortafolio, "")
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar fecha valoracion inferior", _
                                                             Me.ToString(), "ConfirmarFechaValoracionInferior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ejecuta el cierre del portafolio si el usuario presiona el botón de la ventana emergente que valida si existen portafolios con fecha de cierre inferior.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_10
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Julio 4/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 4/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP0005
    ''' Descripción      : Se agrega el parámetro logCerrarPortafolios. 
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 16/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Diciembre 16/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: 
    ''' Descripción      : Se agrega el parámetro logContabilizar. 
    ''' Responsable      : Javier Pardo (Alcuadrado S.A.)
    ''' Fecha            : Septiembre 29/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Septiembre 29/2015 - Resultado Ok 
    ''' </history>
    Private Async Sub ConfirmarCerrarPortafolio(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objRet As LoadOperation(Of ProcesarPortafolio)

            'Se inicializa en true por defecto, por el cierre continuo
            Dim logPresionaronBotonSI As Boolean = True

            'Se utiliza la variable logPresionaronBotonSI cuando es cierre continuo porque el sender viene nothing
            If Not IsNothing(sender) Then
                If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    logPresionaronBotonSI = False
                Else
                    logPresionaronBotonSI = True
                End If
            End If

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyProcesarPortafolios()
            End If

            IsBusy = True
            mlogActivarIsLoading = True

            'Cuando presiona el botón NO
            If Not logPresionaronBotonSI Then

                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyProcesarPortafolios()
                End If

                mdcProxy.ProcesarPortafolios.Clear()
                objRet = Await mdcProxy.Load(mdcProxy.CerrarFechaCierrePortafolioQuery(dtmFechaValoracion, strTipoPortafolio, lngIDComitente, False, logCerrarPortafolios, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el resultado de eliminar", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                End If

                ListaEncabezado = mdcProxy.ProcesarPortafolios
                MyBase.CambioItem("ListaEncabezado")
            End If

            If logPresionaronBotonSI Then   'Cuando presiona el botón SI
                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyProcesarPortafolios()
                End If

                mdcProxy.ProcesarPortafolios.Clear()
                objRet = Await mdcProxy.Load(mdcProxy.CerrarFechaCierrePortafolioQuery(dtmFechaValoracion, strTipoPortafolio, lngIDComitente, True, logCerrarPortafolios, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el resultado de eliminar.", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                End If

                ListaEncabezado = mdcProxy.ProcesarPortafolios
                MyBase.CambioItem("ListaEncabezado")

            End If

            IsBusy = False
            HabilitarBoton = False
            ReiniciaTimer()

            If logCierreContinuo Then
                intAcumuladorDias = intAcumuladorDias + 1

                If intAcumuladorDias > intDiferenciaDias Then
                    logIniciarJobValoracion = True
                End If
            Else
                logIniciarJobValoracion = True
            End If

            mdcProxy.ProcesarPortafolio(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, logContabilizar, Program.Usuario, logIniciarJobValoracion, logEliminarDatosResultadoMotor, Program.HashConexion,
                                        AddressOf TerminoProcesarValoracion, "")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el confirmar cerrar portafolio", Me.ToString(), "ConfirmarCerrarPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para capturar la respuesta del usuario para eliminar el portafolio para todos los clientes o uno en específico
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_11
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Julio 4/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 4/2014 - Resultado Ok 
    ''' </history>
    Private Async Sub ConfirmarTerminoValidarParametro_EliminarCierreTodosLosPortafolios(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objRet As LoadOperation(Of ProcesarPortafolio)

            IsBusy = True
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyProcesarPortafolios()
            End If

            If mdcProxy2 Is Nothing Then
                mdcProxy2 = inicializarProxyProcesarPortafolios()
            End If

            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                mdcProxy.ProcesarPortafolios.Clear()
                objRet = Await mdcProxy.Load(mdcProxy.EliminarCalculosQuery(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, True, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar la devolución de cierre", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                End If

                ListaEncabezado = mdcProxy.ProcesarPortafolios
                MyBase.CambioItem("ListaEncabezado")
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar la devolución de cierre.", Me.ToString(), "ConfirmarTerminoValidarParametro_EliminarCierreTodosLosPortafolios", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta el nombre y la fecha de portafolio de un cliente.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function ConsultarDatosPortafolio(ByVal plngIDComitente As String) As Task
        Try
            Dim objRet As LoadOperation(Of DatosPortafolios)
            Dim dcProxy As ProcesarPortafoliosDomainContext

            dcProxy = inicializarProxyProcesarPortafolios()

            objRet = Await dcProxy.Load(dcProxy.ConsultarDatosPortafolioSyncQuery(plngIDComitente, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        strNombreComitente = objRet.Entities.First.strNombre
                    Else
                        lngIDComitente = Nothing
                        strNombreComitente = Nothing
                    End If
                End If
            Else
                lngIDComitente = Nothing
                strNombreComitente = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Valida si puede continuar el proceso validando el check de reconstrucción.
    ''' </summary>
    Private Sub ConfirmarReconstruirMovimiento(ByVal sender As Object, ByVal e As EventArgs)

        Try
            'Se inicializa en true por defecto, por el cierre continuo
            Dim logPresionaronBotonSI As Boolean = True

            'Se utiliza la variable logPresionaronBotonSI cuando es cierre continuo porque el sender viene nothing
            If Not IsNothing(sender) Then
                If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    logPresionaronBotonSI = False
                Else
                    logPresionaronBotonSI = True
                End If
            End If

            If Not logPresionaronBotonSI Then
                IsBusy = False
                HabilitarBoton = True
                IsBusyProcesando = False
                EliminarBoton = True
                InformeBoton = True
            End If

            If logPresionaronBotonSI Then
                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyProcesarPortafolios()
                End If

                mdcProxy.ValidarFechaValoracionInferiorSync(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, Program.Usuario, Program.HashConexion,
                                                            AddressOf TerminoValidarFechaValoracionInferior, "")
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar la reconstrucción de movimientos",
                                                             Me.ToString(), "ConfirmarReconstruirMovimiento", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub ConfirmarLimiteDiasCierreContinuo(ByVal sender As Object, ByVal e As EventArgs)
        Try
            'Se inicializa en true por defecto, por el cierre continuo
            Dim logPresionaronBotonSI As Boolean = True

            'Se utiliza la variable logPresionaronBotonSI cuando no es cierre continuo porque el sender viene nothing
            If Not IsNothing(sender) Then
                If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    logPresionaronBotonSI = False
                Else
                    logPresionaronBotonSI = True
                End If
            End If

            If Not logPresionaronBotonSI Then

                'CerrarTemporizador()
                IsBusy = False
                HabilitarBoton = True
                IsBusyProcesando = False
                EliminarBoton = True
                InformeBoton = True
                Exit Sub
            End If

            If logPresionaronBotonSI Then
                'Se habilita el IsBusyProcesando y se desahabilitan los botones de eliminar y del informe SM20150918
                IsBusyProcesando = True
                HabilitarBoton = False
                EliminarBoton = False
                InformeBoton = False

                If (dtmFechaValoracionInicial < dtmFechaValoracionFinal) Then
                    logCierreContinuo = True
                Else
                    logCierreContinuo = False
                End If

                dtmFechaValoracion = DateAdd(DateInterval.Day, intAcumuladorDias, CType(dtmFechaValoracionInicial, Date))

                mdcProxy.ProcesarUtilidadesCustodias.Clear()

                If intAcumuladorDias <= intDiferenciaDias Then
                    If logCierreContinuo Then

                        'Cuando es cierre continuo solamente debe mostrarse el mensaje la primera vez
                        If dtmFechaValoracion = dtmFechaValoracionInicial Then
                            logMostrarMensajeReconstruirMov = True
                        Else
                            logMostrarMensajeReconstruirMov = False
                        End If

                        logMostrarMensajeFechasSinValorar = False
                        logMostrarMensajeFechaDeCierreInferior = False
                    End If
                Else
                    IsBusy = False
                    HabilitarBoton = True
                    'Se desabilita el Isbusy y se habilitan los botones de eliminar y del informe luego de terminar todo el proceso SM20150918
                    IsBusyProcesando = False
                    EliminarBoton = True
                    InformeBoton = True
                    intAcumuladorDias = 0
                    intDiferenciaDias = 0
                    logIniciarJobValoracion = False
                    logEliminarDatosResultadoMotor = True
                    Exit Sub
                End If

                If logReconstruir Then
                    If logCierreContinuo And Not logMostrarMensajeReconstruirMov Then
                        ConfirmarReconstruirMovimiento(Nothing, Nothing)
                    Else
                        A2Utilidades.Mensajes.mostrarMensajePregunta(String.Format("Esta activa la opción ""Reconstruir Movimiento"".{0}¿Está seguro de reconstruir todos los movimientos?", vbCrLf), Program.TituloSistema, ValoresUserState.Borrar.ToString(),
                                                                     AddressOf ConfirmarReconstruirMovimiento, False)
                    End If
                Else
                    mdcProxy.ValidarFechaValoracionInferiorSync(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, Program.Usuario, Program.HashConexion,
                                                                AddressOf TerminoValidarFechaValoracionInferior, "")
                End If
                IsBusy = False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar fecha valoracion inferior",
                                                             Me.ToString(), "ConfirmarFechaValoracionInferior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Valida si continua el cierre para todos los clientes.
    ''' Creado por       : Jhon Alexis Echavarria (Alcuadrado S.A.)
    ''' Fecha            : Sep 27/2016
    ''' </summary>
    Private Sub ConfirmarCierreTodosLosClientes(ByVal sender As Object, ByVal e As EventArgs)

        Try
            'Se inicializa en true por defecto, por el cierre continuo
            Dim logPresionaronBotonSI As Boolean = True

            'Se utiliza la variable logPresionaronBotonSI cuando no es cierre continuo porque el sender viene nothing
            If Not IsNothing(sender) Then
                If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    logPresionaronBotonSI = False
                Else
                    logPresionaronBotonSI = True
                End If
            End If

            If Not logPresionaronBotonSI Then

                'CerrarTemporizador()
                IsBusy = False
                HabilitarBoton = True
                IsBusyProcesando = False
                EliminarBoton = True
                InformeBoton = True
                Exit Sub
            End If

            If logPresionaronBotonSI Then

                If (intDiferenciaDias >= mintCierreContinuo_LimiteDias) And mstrCierreContinuo_LimiteDias_Permitir = "SI" Then
                    'Solo debo de mostrar este mensaje una solo vez, así sea para el cierre continuo
                    If intAcumuladorDias = 0 Then
                        A2Utilidades.Mensajes.mostrarMensajePregunta("El límite de días para realizar el cierre continuo no puede superar el permitido (" & mintCierreContinuo_LimiteDias & " día(s)). ¿ Desea conitnuar con el proceso ?", Program.TituloSistema, ValoresUserState.Borrar.ToString(),
                                                                     AddressOf ConfirmarLimiteDiasCierreContinuo, False)
                    Else
                        ConfirmarLimiteDiasCierreContinuo(Nothing, Nothing)
                    End If
                Else
                    'Se habilita el IsBusyProcesando y se desahabilitan los botones de eliminar y del informe SM20150918
                    IsBusyProcesando = True
                    HabilitarBoton = False
                    EliminarBoton = False
                    InformeBoton = False

                    If (dtmFechaValoracionInicial < dtmFechaValoracionFinal) Then
                        logCierreContinuo = True
                    Else
                        logCierreContinuo = False
                    End If

                    dtmFechaValoracion = DateAdd(DateInterval.Day, intAcumuladorDias, CType(dtmFechaValoracionInicial, Date))

                    mdcProxy.ProcesarUtilidadesCustodias.Clear()

                    'Solo debo de permitir eliminar los de la tabla CF.tblProcesarResultadoMotor la primera vez se que llame al sp [CF].[uspCalculosFinancieros_ValoracionPortafolio]
                    If intAcumuladorDias > 0 Then
                        logEliminarDatosResultadoMotor = False
                    End If

                    If intAcumuladorDias <= intDiferenciaDias Then
                        If logCierreContinuo Then

                            'Cuando es cierre continuo solamente debe mostrarse el mensaje la primera vez
                            If dtmFechaValoracion = dtmFechaValoracionInicial Then
                                logMostrarMensajeReconstruirMov = True
                            Else
                                logMostrarMensajeReconstruirMov = False
                            End If

                            logMostrarMensajeFechasSinValorar = False
                            logMostrarMensajeFechaDeCierreInferior = False
                        End If
                    Else
                        IsBusy = False
                        HabilitarBoton = True
                        'Se desabilita el Isbusy y se habilitan los botones de eliminar y del informe luego de terminar todo el proceso SM20150918
                        IsBusyProcesando = False
                        EliminarBoton = True
                        InformeBoton = True
                        intAcumuladorDias = 0
                        intDiferenciaDias = 0
                        logIniciarJobValoracion = False
                        logEliminarDatosResultadoMotor = True
                        Exit Sub
                    End If

                    If logReconstruir Then
                        If logCierreContinuo And Not logMostrarMensajeReconstruirMov Then
                            ConfirmarReconstruirMovimiento(Nothing, Nothing)
                        Else
                            A2Utilidades.Mensajes.mostrarMensajePregunta(String.Format("Esta activa la opción ""Reconstruir Movimiento"".{0}¿Está seguro de reconstruir todos los movimientos?", vbCrLf), Program.TituloSistema, ValoresUserState.Borrar.ToString(),
                                                                         AddressOf ConfirmarReconstruirMovimiento, False)
                        End If
                    Else
                        mdcProxy.ValidarFechaValoracionInferiorSync(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, Program.Usuario, Program.HashConexion,
                                                                    AddressOf TerminoValidarFechaValoracionInferior, "")
                    End If
                End If
                IsBusy = False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar fecha valoracion inferior",
                                                             Me.ToString(), "ConfirmarCierreTodosLosClientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub


#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            'Valida la fecha de valoración
            If IsNothing(dtmFechaValoracionInicial) Then
                strMsg = String.Format("{0}{1} + La fecha de valoración inicial es un campo requerido.", strMsg, vbCrLf)
            End If

            If IsNothing(dtmFechaValoracionFinal) Then
                strMsg = String.Format("{0}{1} + La fecha de valoración final es un campo requerido.", strMsg, vbCrLf)
            End If

            If (dtmFechaValoracionInicial > dtmFechaValoracionFinal) Then
                strMsg = String.Format("{0}{1} + La fecha de valoración inicial no puede ser mayor a la fecha de valoración final.", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                IsBusy = False
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ' ''' <summary>
    ' ''' Consulta los valores por defecto para un nuevo encabezado
    ' ''' </summary>
    Private Sub ConsultarValoresPorDefecto()
        Try
            dtmFechaValoracionInicial = Date.Now()
            dtmFechaValoracionFinal = Date.Now()
            lngIDComitente = String.Empty
            CargaListas()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarValoresPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Método para realizar la valoración de las utilidades, si hay cobros pendientes levanta una ventana modal
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Editado por      : Germán Arbey González Osorio
    ''' Descripción      : Se genera el código para recibir el mensaje que indica si levanta la ventana modal y la muestra dependiendo el caso
    ''' Fecha            : Abril 01/2014
    ''' Pruebas CB       : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  Id_6, Id_7, Id_8
    ''' Descripción:        Se agrega el método ReiniciaTimer para que el proceso de informe de valoración sea automático.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              23 de Abril/2014
    ''' Pruebas CB:         Jorge Peña / 23 de Abril/2014 - Resultados OK
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  Id_10
    ''' Creado por:         Jorge Peña (Alcuadrado S.A.)
    ''' Descripción:        Se agrega el TimeSpan de 30 a 60 porque estaba generando problemas de tiempos.
    ''' Fecha:              Julio 4/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - Julio 4/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Modificado por:     Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Descripción:        Se retira el evento mobjVM.FinalizaValoracion debido a que no es necesaria su funcionalidad y no maneja
    '''                     adecuadamente la variable isBusy
    ''' Fecha:              Septiembre 8/2014
    ''' Pruebas CB:         Germán Arbey González Osorio (Alcuadrado S.A.) - Septiembre 8/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP0005
    ''' Descripción      : Se elimina la validación cuando el campo "Cerrar portafolios" está marcado porque debe de validar la fecha de portafolio siempre.
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 16/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Diciembre 16/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descripción      : Pregunta primero si existen fechas sin valorar inferiores a la fecha seleccionada
    ''' Responsable      : Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 12/2015
    ''' Pruebas CB       : Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 12/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descripción      : Si el check de reconstrucción esta activo, el usuario debe indicar si continua con el proceso
    ''' Responsable      : Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 21/2015
    ''' Pruebas CB       : Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 21/2015 - Resultado Ok 
    ''' </history>
    Public Sub ProcesarValoracion()
        Try
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyProcesarPortafolios()
                DirectCast(mdcProxy.DomainClient, WebDomainClient(Of ProcesarPortafoliosDomainContext.IProcesarPortafoliosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0)
            End If

            If mdcProxy.IsLoading Then
                If Not logCierreContinuo Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                'Exit Sub      OJOOOOOO preguntar que se puede hacer
            End If

            If Not IsNothing(ListaEncabezado) Then
                If InformeBoton Then
                    ListaEncabezado = Nothing
                End If
            End If



            If ValidarDatos() Then

                intDiferenciaDias = CInt(DateDiff(DateInterval.Day, CType(dtmFechaValoracionInicial, Date), CType(dtmFechaValoracionFinal, Date)))

                'Solo debo de mostrar este mensaje una solo vez, así sea para el cierre continuo
                If intAcumuladorDias = 0 Then
                    If (intDiferenciaDias >= mintCierreContinuo_LimiteDias) And mstrCierreContinuo_LimiteDias_Permitir = "NO" Then
                        A2Utilidades.Mensajes.mostrarMensaje("El límite de días para realizar el cierre continuo no puede superar el permitido (" & mintCierreContinuo_LimiteDias & " día(s)).", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If


                'Solo debo de mostrar este mensaje una solo vez, así sea para el cierre continuo  JAEZ 20160927
                If String.IsNullOrEmpty(lngIDComitente) Then
                    If intAcumuladorDias = 0 Then
                        A2Utilidades.Mensajes.mostrarMensajePregunta("¿Está seguro de realizar el proceso para todos los clientes?", Program.TituloSistema, ValoresUserState.Borrar.ToString(),
                                                                       AddressOf ConfirmarCierreTodosLosClientes, False)
                    Else
                        ConfirmarCierreTodosLosClientes(Nothing, Nothing)
                    End If

                Else

                    If (intDiferenciaDias >= mintCierreContinuo_LimiteDias) And mstrCierreContinuo_LimiteDias_Permitir = "SI" Then
                        'Solo debo de mostrar este mensaje una solo vez, así sea para el cierre continuo
                        If intAcumuladorDias = 0 Then
                            A2Utilidades.Mensajes.mostrarMensajePregunta("El límite de días para realizar el cierre continuo no puede superar el permitido (" & mintCierreContinuo_LimiteDias & " día(s)). ¿ Desea conitnuar con el proceso ?", Program.TituloSistema, ValoresUserState.Borrar.ToString(),
                                                                         AddressOf ConfirmarLimiteDiasCierreContinuo, False)
                        Else
                            ConfirmarLimiteDiasCierreContinuo(Nothing, Nothing)
                        End If
                    Else
                        'Se habilita el IsBusyProcesando y se desahabilitan los botones de eliminar y del informe SM20150918
                        IsBusyProcesando = True
                        HabilitarBoton = False
                        EliminarBoton = False
                        InformeBoton = False

                        If (dtmFechaValoracionInicial < dtmFechaValoracionFinal) Then
                            logCierreContinuo = True
                        Else
                            logCierreContinuo = False
                        End If

                        dtmFechaValoracion = DateAdd(DateInterval.Day, intAcumuladorDias, CType(dtmFechaValoracionInicial, Date))

                        mdcProxy.ProcesarUtilidadesCustodias.Clear()

                        'Solo debo de permitir eliminar los de la tabla CF.tblProcesarResultadoMotor la primera vez se que llame al sp [CF].[uspCalculosFinancieros_ValoracionPortafolio]
                        If intAcumuladorDias > 0 Then
                            logEliminarDatosResultadoMotor = False
                        End If

                        If intAcumuladorDias <= intDiferenciaDias Then
                            If logCierreContinuo Then

                                'Cuando es cierre continuo solamente debe mostrarse el mensaje la primera vez
                                If dtmFechaValoracion = dtmFechaValoracionInicial Then
                                    logMostrarMensajeReconstruirMov = True
                                Else
                                    logMostrarMensajeReconstruirMov = False
                                End If

                                logMostrarMensajeFechasSinValorar = False
                                logMostrarMensajeFechaDeCierreInferior = False
                            End If
                        Else
                            IsBusy = False
                            HabilitarBoton = True
                            'Se desabilita el Isbusy y se habilitan los botones de eliminar y del informe luego de terminar todo el proceso SM20150918
                            IsBusyProcesando = False
                            EliminarBoton = True
                            InformeBoton = True
                            intAcumuladorDias = 0
                            intDiferenciaDias = 0
                            logIniciarJobValoracion = False
                            logEliminarDatosResultadoMotor = True
                            Exit Sub
                        End If

                        If logReconstruir Then
                            If logCierreContinuo And Not logMostrarMensajeReconstruirMov Then
                                ConfirmarReconstruirMovimiento(Nothing, Nothing)
                            Else
                                A2Utilidades.Mensajes.mostrarMensajePregunta(String.Format("Está activa la opción ""Reconstruir Movimiento"".{0}¿Está seguro de reconstruir todos los movimientos?", vbCrLf), Program.TituloSistema, ValoresUserState.Borrar.ToString(),
                                                                             AddressOf ConfirmarReconstruirMovimiento, False)
                            End If
                        Else
                            mdcProxy.ValidarFechaValoracionInferiorSync(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, Program.Usuario, Program.HashConexion,
                                                                        AddressOf TerminoValidarFechaValoracionInferior, "")
                        End If
                    End If

                End If
            End If

            IsBusy = False

            logErrorAlTerminarProcesar = False 'Reiniciar variable para detectar si el sp de valoración retornó o no un mensaje de error con el fin de mostrar el mensaje al finalizar de ejecución exitosa del proceso

        Catch ex As Exception
            IsBusy = False
            HabilitarBoton = True
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al procesar la valoración", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para eliminar el portafolio de acuerdo a los parámetros de entrada, el resultado de la eliminación se muestra en el grid.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  Id_6
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              16 de Junio/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 16 de Junio/2014 - Resultados OK
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: Id_12, Id_13, Id_14
    ''' Descripción:     : Se traslado el llamado al método EliminarCalculosQuery a la función ConfirmarTerminoValidarParametro_EliminarCierreTodosLosPortafolios ya que la respuesta a la pregunta es asincrónica y se necesitaba que el proceso de devolución fuera lineal y contínuo.
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Julio 4/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 4/2014 - Resultado Ok 
    ''' </history>
    Public Sub EliminarCalculos()
        Try
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyProcesarPortafolios()
            End If

            If ValidarDatos() Then
                IsBusy = True

                If mstrEliminarCierreTodosLosPortafolios.ToUpper = "SI" Then
                    mdcProxy.ValidarParametro_EliminarCierreTodosLosPortafolios(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, True, Program.Usuario, Program.HashConexion,
                                                                                AddressOf TerminoValidarParametro_EliminarCierreTodosLosPortafolios, "")
                Else
                    mdcProxy.ValidarParametro_EliminarCierreTodosLosPortafolios(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, False, Program.Usuario, Program.HashConexion,
                                                                                AddressOf TerminoValidarParametro_EliminarCierreTodosLosPortafolios, "")
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            HabilitarBoton = True
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al eliminar los cálculos", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Controlador de evento para activar el boton Procesar al terminar de valorar desde la ventana de Cobros
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Editado por      : Germán Arbey González Osorio
    ''' Descripción      : Controlador de evento para activar el boton Procesar al terminar de valorar desde la ventana de Cobros
    ''' Fecha            : Abril 01/2014
    ''' Pruebas CB       : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  Id_6, Id_7, Id_8
    ''' Descripción:        Se agrega el método CerrarTemporizador para que cuando proceso de informe de valoración sea automático se cierre.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              23 de Abril/2014
    ''' Pruebas CB:         Jorge Peña / 23 de Abril/2014 - Resultados OK
    ''' </history>
    Private Sub ViewProcesoCobroUtilidades_FinalizaValoracion(ByVal sender As Object, ByVal e As EventArgs)
        HabilitarBoton = True
        CerrarTemporizador()

    End Sub

    ''' <summary>
    ''' Controlador de evento que captura el momento en que se cierra la ventana de los cobros de utilidades
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Editado por      : Germán Arbey González Osorio
    ''' Descripción      : Controlador de evento que captura el momento en que se cierra la ventana de los cobros de utilidades
    ''' Fecha            : Abril 01/2014
    ''' Pruebas CB       : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  Id_6, Id_7, Id_8
    ''' Descripción:        Se agrega el método CerrarTemporizador para que cuando proceso de informe de valoración sea automático se cierre.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              23 de Abril/2014
    ''' Pruebas CB:         Jorge Peña / 23 de Abril/2014 - Resultados OK
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  Id_10
    ''' Creado por:         Jorge Peña (Alcuadrado S.A.)
    ''' Descripción:        Se agrega la validación de operaciones pendientes, validación de cierres de portafolio y llamado al método de procesar valoración.
    ''' Fecha:              Julio 4/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - Julio 4/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  CP0001, CP0002, CP0003
    ''' Creado por:         Jorge Peña (Alcuadrado S.A.)
    ''' Descripción:        Se elimina la validación "Hay cobros de utilidades pendientes por cobrar, por lo tanto no se realiza el proceso de valoración."
    ''' Fecha:              Octubre 30/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - Octubre 30/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP0005
    ''' Descripción      : Se elimina la validación cuando el campo "Cerrar portafolios" está marcado porque debe de validar la fecha de portafolio siempre.
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 16/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Diciembre 16/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descripción      : ''' Descripción      : Se envía el parámetro strReconstruccion para actualizar el parámetro CF_RECONSTRUIR_MOVIMIENTOS
    ''' Responsable      : Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 10/2015
    ''' Pruebas CB       : Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 10/2015 - Resultado Ok 
    ''' </history>
    Private Async Sub ViewProcesoCobroUtilidades_Closed(ByVal sender As Object, ByVal e As EventArgs)

        Dim objRet As LoadOperation(Of ProcesarPortafolio)

        If mdcProxy Is Nothing Then
            mdcProxy = inicializarProxyProcesarPortafolios()
        End If

        If ValidarDatos() Then
            mdcProxy.ProcesarPortafolios.Clear()
            objRet = Await mdcProxy.Load(mdcProxy.ValidarOperacionesPendientesQuery(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, CStr(IIf(logReconstruir, "SI", "NO")), Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el informe de procesamiento.", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                End If
            End If

            ListaEncabezado = mdcProxy.ProcesarPortafolios
        End If

        mdcProxy.ValidarFechaCierrePortafolio(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, logCerrarPortafolios, Program.Usuario, Program.HashConexion,
                                              AddressOf TerminoValidarFechaCierrePortafolio, "")
    End Sub

    Public Async Function ConsultarInformeProcesamiento() As Task
        Try
            Dim objRet As LoadOperation(Of ProcesarPortafolio)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyProcesarPortafolios()
            End If

            If mdcProxy.IsLoading Then
                If Not logCierreContinuo Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                Exit Function      'OJOOOOOO preguntar que se puede hacer
            End If

            'IsBusy = True
            mdcProxy.ProcesarPortafolios.Clear()
            objRet = Await mdcProxy.Load(mdcProxy.ConsultarAvanceProcesamientoQuery(dtmFechaValoracionInicial, dtmFechaValoracionFinal, strTipoProceso, strTipoPortafolio, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el informe de procesamiento", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                End If
            End If

            ListaEncabezado = mdcProxy.ProcesarPortafolios
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el informe de procesamiento.", Me.ToString(), "ConsultarInformeProcesamiento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Realiza la exportación  de los resultados de valoración a Excel.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto Cruz.
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 24/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 24/2014 - Resultado Ok 
    ''' 
    ''' Editado por      : Juan Carlos Soto Cruz.
    ''' Descripción      : Se formatea la fecha dd/mm/yyyy.
    ''' Fecha            : Marzo 07/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Marzo 07/2014 - Resultado Ok 
    ''' 
    ''' Editado por      : Germán Arbey González Osorio.
    ''' Descripción      : Se adiciona el parametro strFechaValoracion a la url de valoración para que el sistema pueda diferenciar
    '''                    los archivos y el usuario pueda abrir el archivo sin guardar y ver la información correcta.
    ''' Fecha            : Julio 16/2014
    ''' Pruebas CB       : Germán Arbey González Osorio - Julio 16/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  CP0004
    ''' Creado por:         Jorge Peña (Alcuadrado S.A.)
    ''' Descripción:        Se agrega la instrucción "strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.TIPOPROCESO), strTipoProceso)"
    ''' Fecha:              Octubre 30/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - Octubre 30/2014 - Resultado Ok 
    ''' </history>
    Public Async Sub ExportarInformeValoracion()
        Try
            IsBusy = True

            Dim objRet As LoadOperation(Of GenerarArchivosPlanos)
            Dim dcProxyUtil As UtilidadesDomainContext
            Dim strParametros As String = STR_PARAMETROS_EXPORTAR
            Dim dia As String = String.Empty
            Dim mes As String = String.Empty
            Dim ano As String = String.Empty
            Dim strFechaValoracionInicial As String = String.Empty
            Dim strFechaValoracionFinal As String = String.Empty

            If Not ValidarDatos() Then Exit Sub

            dcProxyUtil = inicializarProxyUtilidadesOYD()


            '_dtmFechaValoracionInicial
            If (_dtmFechaValoracionInicial.Value.Day < 10) Then
                dia = "0" + _dtmFechaValoracionInicial.Value.Day.ToString
            Else
                dia = _dtmFechaValoracionInicial.Value.Day.ToString
            End If

            If (_dtmFechaValoracionInicial.Value.Month < 10) Then
                mes = "0" + _dtmFechaValoracionInicial.Value.Month.ToString
            Else
                mes = _dtmFechaValoracionInicial.Value.Month.ToString
            End If

            ano = _dtmFechaValoracionInicial.Value.Year.ToString

            strFechaValoracionInicial = ano + mes + dia


            '_dtmFechaValoracionFinal
            If (_dtmFechaValoracionFinal.Value.Day < 10) Then
                dia = "0" + _dtmFechaValoracionFinal.Value.Day.ToString
            Else
                dia = _dtmFechaValoracionFinal.Value.Day.ToString
            End If

            If (_dtmFechaValoracionFinal.Value.Month < 10) Then
                mes = "0" + _dtmFechaValoracionFinal.Value.Month.ToString
            Else
                mes = _dtmFechaValoracionFinal.Value.Month.ToString
            End If

            ano = _dtmFechaValoracionFinal.Value.Year.ToString

            strFechaValoracionFinal = ano + mes + dia

            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.FECHA_VALORACION_INICIAL), strFechaValoracionInicial)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.FECHA_VALORACION_FINAL), strFechaValoracionFinal)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.LNGIDCOMITENTE), lngIDComitente)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.TIPOPROCESO), strTipoProceso)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.USUARIO), Program.Usuario)

            objRet = Await dcProxyUtil.Load(dcProxyUtil.GenerarArchivoPlanoSyncQuery(STR_CARPETA, STR_PROCESO, strParametros, "InformeValoracion", "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la generación del archivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación del archivo.", Me.ToString(), "ExportarInformeValoracion", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    IsBusy = False
                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objResultado = objRet.Entities.First

                        If objResultado.Exitoso Then
                            Program.VisorArchivosWeb_DescargarURL(objResultado.RutaArchivoPlano & "?date=" & strFechaValoracionInicial & DateTime.Now.ToString("HH:mm:ss"))
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(objResultado.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                    IsBusy = False
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso ExportarInformeValoracion",
                                                             Me.ToString(), "ExportarInformeValoracion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ValorAReemplazar(ByVal pintTipoCampo As PARAMETROSEXPORTAR) As String
        Return String.Format("[[{0}]]", pintTipoCampo.ToString)
    End Function

    ''' <summary>
    ''' Realiza la exportación  del avance de procesamiento de valoración a Excel.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto Cruz.
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 24/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 24/2014 - Resultado Ok 
    ''' </history>
    Public Async Sub ExportarAvanceProcesamiento()
        Try
            IsBusy = True

            Dim objRet As LoadOperation(Of GenerarArchivosPlanos)
            Dim dcProxyUtil As UtilidadesDomainContext
            Dim strParametros As String = STR_PARAMETROS_EXPORTAR
            Dim dia As String = String.Empty
            Dim mes As String = String.Empty
            Dim ano As String = String.Empty
            Dim strFechaValoracionInicial As String = String.Empty
            Dim strFechaValoracionFinal As String = String.Empty

            If Not ValidarDatos() Then Exit Sub

            dcProxyUtil = inicializarProxyUtilidadesOYD()

            '_dtmFechaValoracionInicial
            If (_dtmFechaValoracionInicial.Value.Day < 10) Then
                dia = "0" + _dtmFechaValoracionInicial.Value.Day.ToString
            Else
                dia = _dtmFechaValoracionInicial.Value.Day.ToString
            End If

            If (_dtmFechaValoracionInicial.Value.Month < 10) Then
                mes = "0" + _dtmFechaValoracionInicial.Value.Month.ToString
            Else
                mes = _dtmFechaValoracionInicial.Value.Month.ToString
            End If

            ano = _dtmFechaValoracionInicial.Value.Year.ToString

            strFechaValoracionInicial = ano + mes + dia


            '_dtmFechaValoracionFinal
            If (_dtmFechaValoracionFinal.Value.Day < 10) Then
                dia = "0" + _dtmFechaValoracionFinal.Value.Day.ToString
            Else
                dia = _dtmFechaValoracionFinal.Value.Day.ToString
            End If

            If (_dtmFechaValoracionFinal.Value.Month < 10) Then
                mes = "0" + _dtmFechaValoracionFinal.Value.Month.ToString
            Else
                mes = _dtmFechaValoracionFinal.Value.Month.ToString
            End If

            ano = _dtmFechaValoracionFinal.Value.Year.ToString

            strFechaValoracionFinal = ano + mes + dia

            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.FECHA_VALORACION_INICIAL), strFechaValoracionInicial)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.FECHA_VALORACION_FINAL), strFechaValoracionFinal)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.TIPOPORTAFOLIO), strTipoPortafolio)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.MODULO), strTipoProceso)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.USUARIO), Program.Usuario)

            objRet = Await dcProxyUtil.Load(dcProxyUtil.GenerarArchivoPlanoSyncQuery(STR_CARPETA, STR_PROCESO_AVANCE, strParametros, "AvanceProcesamiento", "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la generación del archivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación del archivo.", Me.ToString(), "ExportarInformeValoracion", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    IsBusy = False
                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objResultado = objRet.Entities.First

                        If objResultado.Exitoso Then
                            Program.VisorArchivosWeb_DescargarURL(objResultado.RutaArchivoPlano)
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(objResultado.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso ExportarAvanceProcesamiento",
                                                             Me.ToString(), "ExportarAvanceProcesamiento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Este metodo se ejecuta al terminar de realizar la valoración
    ''' </summary>
    ''' <param name="lo">Objeto de tipo InvokeOperation(Of String)</param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Descripción  : Este metodo se ejecuta al terminar de realizar la valoración
    ''' Fecha        : Abril 11/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    ''' <history> 
    ''' Modificado por : Javier Eduardo Pardo Moreno
    ''' Descripción  : Se apaga el IsBusy al teminar la valoración
    ''' Fecha        : Octubre 01/2018
    ''' Pruebas CB   : Javier Eduardo Pardo Moreno - Octubre 01/2018 - Resultado OK
    ''' </history>
    Private Sub TerminoProcesarValoracion(lo As InvokeOperation(Of String))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó procesar valoracion",
                                                 Me.ToString(), "TerminoProcesarValoracion", Application.Current.ToString(), Program.Maquina, lo.Error)
                'JEPM20181001
                IsBusy = False
                IsBusyProcesando = False
                HabilitarBoton = True
                InformeBoton = True
                intAcumuladorDias = 0
                intDiferenciaDias = 0
                logIniciarJobValoracion = False
                logEliminarDatosResultadoMotor = False

                logErrorAlTerminarProcesar = True

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó procesar valoracion.",
                                                             Me.ToString(), "TerminoProcesarValoracion", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            CerrarTemporizador()
        End Try
    End Sub

    ''' <summary>
    ''' Este metodo se ejecuta al terminar de validar si existen fechas sin valorar inferiores a la fecha seleccionada
    ''' </summary>
    ''' <history>
    ''' Descripción      : Se envía el parámetro strReconstruccion para actualizar el parámetro CF_RECONSTRUIR_MOVIMIENTOS
    ''' Responsable      : Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 10/2015
    ''' Pruebas CB       : Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 10/2015 - Resultado Ok 
    ''' </history>
    Private Async Sub TerminoValidarFechaValoracionInferior(lo As InvokeOperation(Of String))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó validar fecha valoracion inferior",
                                                 Me.ToString(), "TerminoValidarFechaValoracionInferior", Application.Current.ToString(), Program.Maquina, lo.Error)

                IsBusy = False
                IsBusyProcesando = False
                HabilitarBoton = True
                InformeBoton = True
                intAcumuladorDias = 0
                intDiferenciaDias = 0
                logIniciarJobValoracion = False
                logEliminarDatosResultadoMotor = False

            ElseIf lo.Value <> String.Empty Then
                If logCierreContinuo And Not logMostrarMensajeFechasSinValorar Then
                    ConfirmarFechaValoracionInferior(Nothing, Nothing)
                Else
                    A2Utilidades.Mensajes.mostrarMensajePregunta(lo.Value, Program.TituloSistema, ValoresUserState.Borrar.ToString(),
                                                                 AddressOf ConfirmarFechaValoracionInferior, False)
                End If
            ElseIf lo.Value = String.Empty Then

                Dim objRet As InvokeOperation(Of Boolean)

                objRet = Await mdcProxy.ValidarCobroUtilidadesPendientesSync(dtmFechaValoracion, strIdEspecie, lngIDComitente, Program.Usuario, Program.HashConexion).AsTask()

                If logCierreContinuo Then
                    objRet = Nothing
                End If

                If Not IsNothing(objRet) Then
                    If objRet.Value Then
                        mobjVM = New ProcesoCobroUtilidadesViewModel
                        ViewProcesoCobroUtilidades = New ProcesoCobroUtilidadesView(mobjVM, dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, Program.Usuario)
                        AddHandler ViewProcesoCobroUtilidades.Closed, AddressOf ViewProcesoCobroUtilidades_Closed
						Program.Modal_OwnerMainWindowsPrincipal(ViewProcesoCobroUtilidades)
						ViewProcesoCobroUtilidades.ShowDialog()
					Else
                        Dim objRet2 As LoadOperation(Of CFProcesarPortafolios.ProcesarPortafolio)

                        If ValidarDatos() Then
                            mdcProxy.ProcesarPortafolios.Clear()

                            objRet2 = Await mdcProxy.Load(mdcProxy.ValidarOperacionesPendientesQuery(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, CStr(IIf(logReconstruir, "SI", "NO")), Program.Usuario, Program.HashConexion)).AsTask()

                            If Not objRet2 Is Nothing Then
                                If objRet2.HasError Then
                                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el informe de procesamiento.", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                                End If
                            End If

                            ListaEncabezado = mdcProxy.ProcesarPortafolios
                        End If

                        mdcProxy.ValidarFechaCierrePortafolio(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, logCerrarPortafolios, Program.Usuario, Program.HashConexion,
                                                              AddressOf TerminoValidarFechaCierrePortafolio, "")
                    End If
                End If

                'Cuando es cierre continuo
                If IsNothing(objRet) Then
                    Dim objRet2 As LoadOperation(Of CFProcesarPortafolios.ProcesarPortafolio)

                    If ValidarDatos() Then
                        'mdcProxy.ProcesarPortafolios.Clear()
                        mdcProxy = inicializarProxyProcesarPortafolios()
                        objRet2 = Await mdcProxy.Load(mdcProxy.ValidarOperacionesPendientesQuery(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, CStr(IIf(logReconstruir, "SI", "NO")), Program.Usuario, Program.HashConexion)).AsTask()

                        If Not objRet2 Is Nothing Then
                            If objRet2.HasError Then
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el informe de procesamiento", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                            End If
                        End If

                        ListaEncabezado = mdcProxy.ProcesarPortafolios

                        For Each li In ListaEncabezado
                            If li.strDescripcion.Contains("tiene operaciones pendientes") And Not String.IsNullOrEmpty(lngIDComitente) Then
                                IsBusy = False
                                IsBusyProcesando = False
                                HabilitarBoton = True
                                InformeBoton = True
                                intAcumuladorDias = 0
                                intDiferenciaDias = 0
                                logIniciarJobValoracion = False
                                logEliminarDatosResultadoMotor = False
                                Exit Sub
                            End If
                        Next
                    End If

                    mdcProxy.ValidarFechaCierrePortafolio(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, logCerrarPortafolios, Program.Usuario, Program.HashConexion,
                                                          AddressOf TerminoValidarFechaCierrePortafolio, "")
                End If

            End If
        Catch ex As Exception
            IsBusy = False
            IsBusyProcesando = False
            HabilitarBoton = True
            InformeBoton = True
            intAcumuladorDias = 0
            intDiferenciaDias = 0
            logIniciarJobValoracion = False
            logEliminarDatosResultadoMotor = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó validar fecha valoracion inferior",
                                                             Me.ToString(), "TerminoValidarFechaValoracionInferior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID caso de prueba:  Id_10
    ''' Creado por:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              Julio 4/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - Julio 4/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP0005
    ''' Descripción      : Se coloca el código en esta función porque el mensaje de pregunta es asincrónico y no se puede hacer asincrónico.
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 16/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Diciembre 16/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descripción      : Se retira el texto "¿Desea continuar?" del mensaje que se genera al cerrar portafolios
    ''' Responsable      : Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha            : Julio 15/2015
    ''' Pruebas CB       : Germán Arbey González Osorio (Alcuadrado S.A.) - Julio 15/2015 - Resultado Ok 
    ''' </history>
    Private Async Sub TerminoValidarFechaCierrePortafolio(lo As InvokeOperation(Of String))
        Try
            Dim objRet As LoadOperation(Of ProcesarPortafolio)

            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó procesar valoracion",
                                                 Me.ToString(), "TerminoValidarFechaCierrePortafolio", Application.Current.ToString(), Program.Maquina, lo.Error)
            ElseIf lo.Value = "EXISTEN_OPERACIONES_VALORADAS" Then
                Await ConsultarInformeProcesamiento()
                HabilitarBoton = True
                EliminarBoton = True
                InformeBoton = True
                Exit Sub
            ElseIf lo.Value <> String.Empty Then
                If logCierreContinuo And Not logMostrarMensajeFechaDeCierreInferior Then
                    ConfirmarCerrarPortafolio(Nothing, Nothing)
                Else
                    A2Utilidades.Mensajes.mostrarMensajePregunta(lo.Value, Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarCerrarPortafolio, False)
                End If
            ElseIf lo.Value = String.Empty Then
                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyProcesarPortafolios()
                End If

                'Await es porque es sincrónico
                mdcProxy.ProcesarPortafolios.Clear()
                objRet = Await mdcProxy.Load(mdcProxy.CerrarFechaCierrePortafolioQuery(dtmFechaValoracion, strTipoPortafolio, lngIDComitente, True, logCerrarPortafolios, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el resultado de eliminar.", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                End If

                ListaEncabezado = mdcProxy.ProcesarPortafolios
                MyBase.CambioItem("ListaEncabezado")

                HabilitarBoton = False
                ReiniciaTimer()

                If logCierreContinuo Then
                    intAcumuladorDias = intAcumuladorDias + 1

                    If intAcumuladorDias > intDiferenciaDias Then
                        logIniciarJobValoracion = True
                    End If
                Else
                    logIniciarJobValoracion = True
                End If

                mdcProxy.ProcesarPortafolio(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, logContabilizar, Program.Usuario, logIniciarJobValoracion, logEliminarDatosResultadoMotor, Program.HashConexion,
                                            AddressOf TerminoProcesarValoracion, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó procesar valoracion.",
                                                             Me.ToString(), "TerminoValidarFechaCierrePortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que indica que terminó de devolver la fecha de valoración
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_12, Id_13, Id_14
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Julio 4/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 4/2014 - Resultado Ok 
    ''' </history>
    Private Async Sub TerminoValidarParametro_EliminarCierreTodosLosPortafolios(lo As InvokeOperation(Of String))
        Try
            Dim strMsg As String = String.Empty
            Dim objRet As LoadOperation(Of ProcesarPortafolio)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyProcesarPortafolios()
            End If

            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            IsBusy = False

            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso de devolver la fecha de cierre", _
                                                 Me.ToString(), "TerminoValidarParametro_EliminarCierreTodosLosPortafolios", Application.Current.ToString(), Program.Maquina, lo.Error)
            ElseIf lo.Value <> String.Empty And mstrEliminarCierreTodosLosPortafolios.ToUpper = "SI" Then
                A2Utilidades.Mensajes.mostrarMensajePregunta(lo.Value, Program.TituloSistema, ValoresUserState.Borrar.ToString(), _
                                                             AddressOf ConfirmarTerminoValidarParametro_EliminarCierreTodosLosPortafolios)
            ElseIf mstrEliminarCierreTodosLosPortafolios.ToUpper = "NO" Then
                If String.IsNullOrEmpty(lngIDComitente) Then
                    strMsg = String.Format("{0}{1} + El código OyD es un campo requerido.", strMsg, vbCrLf)
                End If

                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                mdcProxy.ProcesarPortafolios.Clear()
                objRet = Await mdcProxy.Load(mdcProxy.EliminarCalculosQuery(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, False, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar la devolución de cierre.", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                End If

                ListaEncabezado = mdcProxy.ProcesarPortafolios
                MyBase.CambioItem("ListaEncabezado")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso de devolver la fecha de cierre.", _
                                                             Me.ToString(), "TerminoValidarParametro_EliminarCierreTodosLosPortafolios", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que indica que terminó de devolver el proceso de fecha de valoración
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_14
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Julio 4/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 4/2014 - Resultado Ok 
    ''' </history>
    Private Sub TerminoProcesoDevolverFechaCierreCF(lo As InvokeOperation(Of String))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso de devolver la fecha de cierre CF", _
                                                 Me.ToString(), "TerminoProcesoDevolverFechaCierreCF", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso de devolver la fecha de cierre CF.", _
                                                             Me.ToString(), "TerminoProcesoDevolverFechaCierreCF", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Timer Refrescar pantalla"

    '''<summary>
    ''' Método utilizado para que el proceso de informe de valoración sea automático, para activarlo se neceitan dos parámetro:
    ''' mstrHabilitarRecargaAutomaticaProcesarPortafolio: indica si el proceso debe estar o no activo.
    ''' mintLapsoRecargaAutomaticaProcesarPortafolio: tiempo en segundos que tarda el proceso en reiniciarse.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  Id_6, Id_7, Id_8
    ''' Descripción:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              23 de Abril/2014
    ''' Pruebas CB:         Jorge Peña / 23 de Abril/2014 - Resultados OK
    ''' </history>
    Private _myDispatcherTimerProcesarPortafolio As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer
    Public Sub ReiniciaTimer()
        Try
            If Not String.IsNullOrEmpty(mstrHabilitarRecargaAutomaticaProcesarPortafolio) And Not IsNothing(mintLapsoRecargaAutomaticaProcesarPortafolio) Then
                If mstrHabilitarRecargaAutomaticaProcesarPortafolio = "SI" Then
                    If _myDispatcherTimerProcesarPortafolio Is Nothing Then
                        _myDispatcherTimerProcesarPortafolio = New System.Windows.Threading.DispatcherTimer
                        _myDispatcherTimerProcesarPortafolio.Interval = New TimeSpan(0, 0, 0, 0, mintLapsoRecargaAutomaticaProcesarPortafolio * 1000)
                        AddHandler _myDispatcherTimerProcesarPortafolio.Tick, AddressOf Me.Each_Tick
                    End If
                    _myDispatcherTimerProcesarPortafolio.Start()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar reiniciar el timer.", Me.ToString, "ReiniciaTimer", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    ''' Método utilizado para finalizar el hilo del temporizador.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  Id_6, Id_7, Id_8
    ''' Descripción:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              23 de Abril/2014
    ''' Pruebas CB:         Jorge Peña / 23 de Abril/2014 - Resultados OK
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  Id_10
    ''' Descripción:        Se agrega la instrucción "HabilitarBoton = True" para habilitar el botón Procesar.
    ''' Creado por:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              Julio 4/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - Julio 4/2014 - Resultado Ok 
    ''' </history>
    Public Async Sub CerrarTemporizador()
        Try
            Await RecargarPantallaProcesarPortafolio()
            HabilitarBoton = True

            If Not IsNothing(_myDispatcherTimerProcesarPortafolio) Then
                _myDispatcherTimerProcesarPortafolio.Stop()
                RemoveHandler _myDispatcherTimerProcesarPortafolio.Tick, AddressOf Me.Each_Tick
                _myDispatcherTimerProcesarPortafolio = Nothing
            End If

            If logCierreContinuo Then
                'intAcumuladorDias = intAcumuladorDias + 1

                ProcesarValoracion()

            Else
                IsBusy = False
                HabilitarBoton = True
                'Se desabilita el Isbusy y se habilitan los botones de eliminar y del informe luego de terminar todo el proceso SM20150918
                IsBusyProcesando = False
                EliminarBoton = True
                InformeBoton = True
                intAcumuladorDias = 0
                intDiferenciaDias = 0
                logIniciarJobValoracion = False
                logEliminarDatosResultadoMotor = True

            End If

            'JEPM20181004 Informar al usuario cuando el proceso se haya realizado exitosamente. Cuando se realiza cierre continuo
            If intAcumuladorDias = 0 And intAcumuladorDias = intDiferenciaDias And logErrorAlTerminarProcesar = False Then

                IsBusy = False
                HabilitarBoton = True
                IsBusyProcesando = False
                EliminarBoton = True
                InformeBoton = True
                logIniciarJobValoracion = False
                logEliminarDatosResultadoMotor = True

                A2Utilidades.Mensajes.mostrarMensaje("Se completó el proceso de cierre activo. Por favor revise los mensajes en el detalle del 'Avance del proceso \ Incidencias'.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Personalizado)

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar parar el timer.", Me.ToString, "pararTemporizador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Recarga el grid cuando se este en un tiempo de espera sin notificaciones y sin refrescar la pantalla.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  Id_6, Id_7, Id_8
    ''' Descripción:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              23 de Abril/2014
    ''' Pruebas CB:         Jorge Peña / 23 de Abril/2014 - Resultados OK
    ''' </history>
    Private Async Sub Each_Tick(sender As Object, e As EventArgs)

        If Not mdcProxy.IsLoading Then
            Await RecargarPantallaProcesarPortafolio()
        End If
    End Sub

#End Region

#Region "Métodos Privados de la pantalla"

    ''' <summary>
    ''' Consulta los datos que arroja el proceso de valoración.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  Id_6, Id_7, Id_8
    ''' Descripción:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              23 de Abril/2014
    ''' Pruebas CB:         Jorge Peña / 23 de Abril/2014 - Resultados OK
    ''' </history>
    Public Async Function RecargarPantallaProcesarPortafolio() As Task
        Try
            Await ConsultarInformeProcesamiento()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recargar la pantalla procesar portafolio.", Me.ToString(), "RecargarPantallaOrdenes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    Private Sub CargaListas()
        If Application.Current.Resources.Contains(Program.NombreListaCombos) Then
            If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("TIPOCOMPANIA") Then
                If Not IsNothing(CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOCOMPANIA")) Then
                    Dim objListaTipoCompania As List(Of OYDUtilidades.ItemCombo)
                    objListaTipoCompania = New List(Of OYDUtilidades.ItemCombo)(CType(Application.Current.Resources(Program.NombreListaCombos), 
                                 Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOCOMPANIA"))
                    objListaTipoCompania.Add(New ItemCombo() With {
                                                                    .ID = "(Todos)",
                                                                    .Retorno = "T",
                                                                    .Descripcion = "Todos",
                                                                    .Categoria = "TIPOCOMPANIA"
                                                                })
                    Me.ListaTipoCompania = objListaTipoCompania
                    Me.strTipoPortafolioSeleccionado = objListaTipoCompania.LastOrDefault().ID
                End If
            End If

        End If
    End Sub

#End Region

#Region "Consultar parametros y campos obligatorios"

    ''' <summary>
    ''' Método para consultar los parámetros para recargar y habilitar automáticamente el proceso de valoración.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  Id_6, Id_7, Id_8
    ''' Descripción:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              23 de Abril/2014
    ''' Pruebas CB:         Jorge Peña / 23 de Abril/2014 - Resultados OK
    ''' </history>
    ''' <history>
    ''' Descripción:     : Se agrega el parámetro EliminarCierreTodosLosPortafolios.
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Julio 4/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 4/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descripción:     : Se agrega el parámetro CF_RECONSTRUIR_MOVIMIENTOS.
    ''' Creado por       : Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 10/2015
    ''' Pruebas CB       : Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 10/2015 - Resultado Ok 
    ''' </history>
    Private Sub ConsultarParametros()
        Try
            Dim dcProxyUtil As UtilidadesDomainContext

            dcProxyUtil = inicializarProxyUtilidadesOYD()
            dcProxyUtil.Verificaparametro("LapsoRecargaAutomaticaProcesarPortafolio", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "LapsoRecargaAutomaticaProcesarPortafolio")
            dcProxyUtil.Verificaparametro("HabilitarRecargaAutomaticaProcesarPortafolio", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "HabilitarRecargaAutomaticaProcesarPortafolio")
            dcProxyUtil.Verificaparametro("EliminarCierreTodosLosPortafolios", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "EliminarCierreTodosLosPortafolios")
            dcProxyUtil.Verificaparametro("CF_RECONSTRUIR_MOVIMIENTOS", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "CF_RECONSTRUIR_MOVIMIENTOS")
            dcProxyUtil.Verificaparametro("CIERRECONTINUO_LIMITEDIAS", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "CIERRECONTINUO_LIMITEDIAS")
            dcProxyUtil.Verificaparametro("CIERRECONTINUO_LIMITEDIAS_PERMITIR", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "CIERRECONTINUO_LIMITEDIAS_PERMITIR")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método consultar parámetros", _
                                                             Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de asignar el resultado de los parámetros consultados en la tabla de parámetros.
    ''' </summary>
    ''' <param name="lo">Valor del parámetro</param>
    ''' <history>
    ''' ID caso de prueba:  Id_6, Id_7, Id_8
    ''' Descripción:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              23 de Abril/2014
    ''' Pruebas CB:         Jorge Peña / 23 de Abril/2014 - Resultados OK
    ''' </history>
    ''' <history>
    ''' Descripción:     : Se agrega el parámetro EliminarCierreTodosLosPortafolios.
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Julio 4/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 4/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descripción:     : Se agrega el parámetro CF_RECONSTRUIR_MOVIMIENTOS.
    ''' Creado por       : Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha            : Diciembre 10/2015
    ''' Pruebas CB       : Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 10/2015 - Resultado Ok 
    ''' </history>
    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Select Case lo.UserState.ToString
                Case "LapsoRecargaAutomaticaProcesarPortafolio"
                    mintLapsoRecargaAutomaticaProcesarPortafolio = CInt(lo.Value)
                Case "HabilitarRecargaAutomaticaProcesarPortafolio"
                    mstrHabilitarRecargaAutomaticaProcesarPortafolio = lo.Value.ToString
                    If mstrHabilitarRecargaAutomaticaProcesarPortafolio = "SI" Then
                        HabilitarBotonRefrescar = False
                    End If
                Case "EliminarCierreTodosLosPortafolios"
                    mstrEliminarCierreTodosLosPortafolios = lo.Value.ToString
                Case "CF_RECONSTRUIR_MOVIMIENTOS"
                    mstrReconstruirMovimientos = lo.Value.ToString
                    If mstrReconstruirMovimientos = "SI" Then
                        logReconstruir = True
                    Else
                        logReconstruir = False
                    End If
                Case "CIERRECONTINUO_LIMITEDIAS"
                    mintCierreContinuo_LimiteDias = CInt(lo.Value)
                Case "CIERRECONTINUO_LIMITEDIAS_PERMITIR"
                    mstrCierreContinuo_LimiteDias_Permitir = lo.Value.ToString
            End Select
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", _
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

#End Region

    'Public Event PropertyChanged1(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

End Class



