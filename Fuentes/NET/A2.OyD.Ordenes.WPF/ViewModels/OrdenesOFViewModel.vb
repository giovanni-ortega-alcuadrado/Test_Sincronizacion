Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports A2ComunesControl

Public Class OrdenesOFViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        cb = New CamposBusquedaOrdenOF(Me)
    End Sub

    Private Sub inicializarServicio()

        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OrdenesDomainContext()
                dcProxy1 = New OrdenesDomainContext()
                dcProxyConsultas = New OrdenesDomainContext()
                dcProxyMaestros = New MaestrosDomainContext()
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                mdcProxyUtilidad02 = New UtilidadesDomainContext()
                dcProxyImportaciones = New ImportacionesDomainContext()
            Else
                dcProxy = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyConsultas = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyMaestros = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            End If

            '---------------------------------------------------------------------------------------------------------------------
            '-- Definir tipo de orden que manejará el control
            '---------------------------------------------------------------------------------------------------------------------
            If IsNothing(Me.ClaseOrden) Then
                _mstrClaseOrden = String.Empty
            End If

            Select Case Me.ClaseOrden
                Case ClasesOrden.A
                    Me._mintClaseEspecie = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
                    Me._mlogEsRentaFija = False
                    Me._mlogEsRentaVariable = True
                Case ClasesOrden.C
                    Me._mintClaseEspecie = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
                    Me._mlogEsRentaFija = True
                    Me._mlogEsRentaVariable = False
                    If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                        RF_TTV_Regreso = "4"
                        RF_TTV_Salida = "3"
                        RF_REPO = "RP"
                        RF_Simulatena_Regreso = "2"
                        RF_Simulatena_Salida = "1"
                        RF_MERCADO_REPO = "E"
                        RF_MERCADO_PRIMARIO = "P"
                        RF_MERCADO_RENOVACION = "R"
                        RF_MERCADO_SECUNDARIO = "S"
                    Else
                        RF_TTV_Regreso = Program.RF_TTV_Regreso
                        RF_TTV_Salida = Program.RF_TTV_Salida
                        RF_REPO = Program.RF_REPO
                        RF_Simulatena_Regreso = Program.RF_Simulatena_Regreso
                        RF_Simulatena_Salida = Program.RF_Simulatena_Salida
                        RF_MERCADO_REPO = Program.RF_Mercado_Repo
                        RF_MERCADO_PRIMARIO = Program.RF_Mercado_Primario
                        RF_MERCADO_RENOVACION = Program.RF_Mercado_Renovacion
                        RF_MERCADO_SECUNDARIO = Program.RF_Mercado_Secundario
                    End If
                Case Else
                    Me._mintClaseEspecie = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T
                    Me._mlogEsRentaFija = False
                    Me._mlogEsRentaVariable = False
            End Select

            mdtmFechaCierreSistema = DateAdd(DateInterval.Year, -5, Now()).Date

            '---------------------------------------------------------------------------------------------------------------------
            '-- Cargar datos iniciales de la lista de órdenes
            '---------------------------------------------------------------------------------------------------------------------
            If Not Program.IsDesignMode() Then
                IsBusy = True
                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.listaVerificaparametroQuery("", "Ordenes", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerParametros, Nothing)
                dcProxyMaestros.Load(dcProxyMaestros.InstalacionFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInstalacion, "FiltroInicial")

                mlogInicializado = True

                HabilitarBotones()
            End If

            MyBase.CambioItem("RentaFijaVisible")
            MyBase.CambioItem("RentaVariableVisible")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "OrdenesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        validarPermisosAdicionar()
        ValidarPermisosDuplicar()

    End Sub

    Private Sub TerminoTraerInstalacion(ByVal lo As LoadOperation(Of Instalacio))
        If Not lo.HasError Then
            ListaInstalacion = dcProxyMaestros.Instalacios
            strIDComitentePermitidoOF = LTrim(RTrim((From obj In ListaInstalacion Select obj.IdContraparteOTC).FirstOrDefault().ToString()))
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Instalacion", _
                                             Me.ToString(), "TerminoTraerInstalacio", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerParametros(ByVal lo As LoadOperation(Of OYDUtilidades.valoresparametro))
        Try
            If lo.HasError = False Then
                objListaParametros = lo.Entities.ToList

                dcProxy.Load(dcProxy.OrdenesOFFiltrarQuery(Me.ClaseOrden.ToString(), "", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "")
                dcProxy1.Load(dcProxy1.TraerOrdenesOFPorDefectoQuery(Me.ClaseOrden.ToString(), Program.Usuario, "", Program.HashConexion), AddressOf TerminoTraerOrdenesPorDefecto_Completed, "Default")
            Else
                lo.MarkErrorAsHandled()
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la lista de parametros", Me.ToString(), "Terminotraerparametrolista", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los parametros de ordenes.", Me.ToString(), "TerminoTraerParametros", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Eventos"
    Friend Event seleccionarTipoOrden()
#End Region

#Region "Constantes"

    ''' <summary>
    ''' A : Acciones
    ''' C : Crediticia
    ''' </summary>
    Public Enum ClasesOrden
        A '// Acciones
        C '// Crediticia
    End Enum

    ''' <summary>
    ''' C : Compra
    ''' R : Recompra
    ''' V : Venta
    ''' S : Reventa
    ''' </summary>
    Public Enum TiposOrden
        C '// Compra
        R '// Recompra
        V '// Venta
        S '// Reventa
    End Enum

    ''' <summary>
    ''' L : Límite
    ''' M : Mercado
    ''' C : Condicionada
    ''' </summary>
    Public Enum TipoLimite As Byte
        L
        M
        C
    End Enum

    ''' <summary>
    ''' C : Cantidad Minima
    ''' K : Fill and Kill
    ''' F : Fill or Kill
    ''' N : Ninguna
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TipoEjecucion As Byte
        C
        K
        F
        N
    End Enum

    Private Const MSTR_MC_ACCION_INGRESAR As String = "I"
    Private Const MSTR_MC_ACCION_ACTUALIZAR As String = "U"
    Private Const MSTR_MC_ACCION_BORRAR As String = "D"

    Private Const MSTR_MC_ORDEN_DOBLE As String = "D"
    Private Const MSTR_MC_ORDEN_SENCILLA As String = "S"

    Private Const MSTR_ACCION_EDITAR As String = "editar"
    Private Const MSTR_ACCION_ANULAR As String = "anular"
    Private Const MSTR_ACCION_APROBAR As String = "aprobar"
    Private Const MSTR_ACCION_BUSCAR As String = "buscar"
    Private Const MSTR_ACCION_FILTRAR As String = "filtrar"
    Private Const MSTR_ACCION_CALCULAR_DIAS As String = "dias"
    Private Const MSTR_ACCION_CALCULAR_FECHA As String = "fecha"

    Private Const MSTR_ACC_CONSULTA_RECEPTORES_CLT As String = "receptorescliente"
    Friend Const MSTR_CALCULAR_DIAS_ORDEN As String = "vencimiento_orden"
    Friend Const MSTR_CALCULAR_DIAS_TITULO As String = "vencimiento_titulo"

    Private MSTR_MODULO_OYD_ORDENES As String = "O"

    Private MINT_LONG_MAX_CODIGO_OYD As Byte = 17
    Private MINT_LONG_MAX_NEMOTECNICO As Byte = 15

    '(SLB) Constante para el Tópico de Instrucciones Ordenes
    Public Const STR_TOPICO_INST_ORDENES_COMPRA As String = "INST_ORDENES_C"
    Public Const STR_TOPICO_INST_ORDENES_VENTA As String = "INST_ORDENES_V"

#End Region

#Region "Variables"

    Private mlogInicializado As Boolean = False
    Private mlogRecalcularFechas As Boolean = True 'CCM20120305. Pemrite activar o desactivar el recálculo de las fechas de vigencia y vencimiento y días de vigencia y palzo

    Private mlogDuplicarOrden As Boolean = False ' Indica si se está duplicando una orden
    Private mlogPuedeDuplicarOrden As Boolean = False ' Indica si el usuario tiene autorización para duplicar órdenes

    Private mlogDuplicarDatosParam As Boolean = False 'CCM20120305. Indica si ya se inicializaron los parámetros que definen algunas características de configuración de las órdenes
    Private mlogDuplicarDatosPrecio As Boolean = True 'CCM20120305. Por defecto duplica el precio de la orden

    Private _mlogHabilitarFechaElaboracion As Boolean = True 'SLB20121128. Habilitar Fecha Elaboración
    Public _mlogMostrarTodosReceptores As Boolean = True 'SLB20121128. Por defecto debe mostrar todos los receptores
    Private _mlogCargarReceptorClientes As Boolean = True
    Private _mlogRequiereIngresoOrdenantes As Boolean = False

    Private mlogUsuarioPuedeIngresar As Boolean = True 'CCM20120305. Indica que el usuario tiene el botón Nuevo activo en la barra de herramientas

    Private mintUltimoId As Integer = -200000000

    Private mstrAccionOrden As String = String.Empty '// Este indicador ayuda a controla la ejecución de consultas inutiles durante el ingreso especialmente
    Private mstrTipoLimitePorDef As String = "M" 'CCM20120305. El tipo de natruraleza o tipo límite es Mercado (M)
    Private mstrFormaPagoPorDef As String = "C" 'SLB20130621. la forma de pago por defecto es Cheque (C)
    Private intPorcentajeCantidadVisible As Integer = 50 'SLB20121126. El porcentaje minimo de la Cantidad Visible sola para acciones

    Private mlogHABILITAR_DEPOSITO_VENTASOF As Boolean = False  'Santiago Vergara - Marzo 20/2014 - Se migra cambio en la validación de cuenta depósito 
    Private mlogCUENTADEPOSITOORDENES As Boolean = False 'Santiago Vergara - Marzo 20/2014 - Se migra cambio en la validación de cuenta depósito 

    Private mdtmFechaCierreSistema As Date

    Public Property cb As CamposBusquedaOrdenOF

    Private OrdenOFPorDefecto As OyDOrdenes.OrdenesOF
    Private OrdenOFAnterior As OyDOrdenes.OrdenesOF

    Private dcProxy As OrdenesDomainContext
    Private dcProxy1 As OrdenesDomainContext
    Private dcProxyConsultas As OrdenesDomainContext
    Private dcProxyMaestros As MaestrosDomainContext


    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext

    Private _mobjReceptorTomaSeleccionadoAntes As OYDUtilidades.BuscadorGenerico
    Private _mobjNemotecnicoSeleccionadoAntes As OYDUtilidades.BuscadorEspecies
    Private _mobjComitenteSeleccionadoAntes As OYDUtilidades.BuscadorClientes

    Private _strObjetoAnterior As String
    Private _strObjetoAnteriorValidar As String = " "

    'SLB se adiciona estas variables, para asignarles el valor del archivo de parametros.
    Private RF_TTV_Regreso As String = ""
    Private RF_TTV_Salida As String = ""
    Private RF_REPO As String = ""
    Private RF_Simulatena_Regreso As String = ""
    Private RF_Simulatena_Salida As String = ""

    Private RF_MERCADO_REPO As String = ""
    Private RF_MERCADO_PRIMARIO As String = ""
    Private RF_MERCADO_RENOVACION As String = ""
    Private RF_MERCADO_SECUNDARIO As String = ""

    Private _mlogFechaCierreSistema As Boolean = False

    Dim objDicUsuarioOperador As List(Of OYDUtilidades.ItemCombo) = Nothing
    Dim ExisteUsuarioOperador As Boolean = True


    Private dcProxyImportaciones As ImportacionesDomainContext
    Private _NroProceso As System.Nullable(Of Double)
    Private _DispatcherTimerOrdenesLEO As System.Windows.Threading.DispatcherTimer
    Private lstMensajeEstadoGeneracionOrdenLEO As New List(Of String)

    Dim intDiasVigenciaDuracionInmediata As Integer
    Private strIDComitentePermitidoOF As String
    Public logRegistroLibroOrdenes As Boolean
    Public dblValorCantidadLibroOrden As Double

    Dim TipoOrdenModificada As String = String.Empty
    Dim intIDOrdenModificada As Integer = 0
    Dim logRefrescarDetalles As Boolean = True

    Private objListaParametros As List(Of OYDUtilidades.valoresparametro)
#End Region

#Region "Propiedades para manejo según la clase de la orden"

    Private Sub validarClaseOrden()
        If (_mlogEsRentaFija And _mlogEsRentaVariable) Or _mstrClaseOrden.Equals(String.Empty) Then
            '/ Validar que por algún problema no se habiliten las dos opciones simultáneamente
            _mlogEsRentaFija = False
            _mlogEsRentaVariable = False
            A2Utilidades.Mensajes.mostrarMensaje("Existe un problema con la orden y el sistema no puede identificarla. Por favor cierre la forma de órdenes e ingrese nuevamente", Program.TituloSistema)
            IsBusy = True
        End If
    End Sub

    Private _mstrClaseOrden As ClasesOrden
    ''' <summary>
    ''' Indica si se está trabajando con órdenes de acciones o de renta fija. Solamente se asigna valor cuando se inicializa el control.
    ''' </summary>
    Public Property ClaseOrden As ClasesOrden
        Get
            validarClaseOrden()
            Return (_mstrClaseOrden)
        End Get
        Set(ByVal value As ClasesOrden)
            If mlogInicializado = False Then ' Esta propiedad no se deja modificar despúes de inicializado el control
                _mstrClaseOrden = value
                inicializarServicio()
            End If
        End Set
    End Property

    Private _mintClaseEspecie As A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie
    ''' <summary>
    ''' Propiedad para filtrar las especies disponibles en la orden
    ''' </summary>
    Public ReadOnly Property ClaseEspecie As A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie
        Get
            Return (_mintClaseEspecie)
        End Get
    End Property

    ''' <summary>
    ''' Propiedad que define el título de la forma según la calse de orden
    ''' </summary>
    Public ReadOnly Property TituloForma As String
        Get
            If _mlogEsRentaFija Then
                Return ("Órdenes de Renta Fija Otras Firmas")
            ElseIf _mlogEsRentaVariable Then
                Return ("Órdenes de Acciones Otras Firmas")
            Else
                Return ("Órdenes")
            End If
        End Get
    End Property

    Private _mlogEsRentaVariable As Boolean = False
    ''' <summary>
    ''' Indica si se está trabajando con órdenes de renta variable (true) o no (false)
    ''' </summary>
    Public ReadOnly Property EsRentaVariable As Boolean
        Get
            validarClaseOrden()
            Return (_mlogEsRentaVariable)
        End Get
    End Property

    Private _mlogEsRentaFija As Boolean = False
    ''' <summary>
    ''' Indica si se está trabajando con órdenes de renta fija (true) o no (false)
    ''' </summary>
    Public ReadOnly Property EsRentaFija As Boolean
        Get
            validarClaseOrden()
            Return (_mlogEsRentaFija)
        End Get
    End Property

    ''' <summary>
    ''' Permite visualizar o no los controles propios de renta fija
    ''' </summary>
    Public ReadOnly Property RentaFijaVisible As System.Windows.Visibility
        Get
            validarClaseOrden()

            If (_mlogEsRentaFija) Then
                Return (System.Windows.Visibility.Visible)
            Else
                Return (System.Windows.Visibility.Collapsed)
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica si se está trabajando con órdenes de renta variable (true) o no (false)
    ''' </summary>
    Public ReadOnly Property RentaVariableVisible As System.Windows.Visibility
        Get
            validarClaseOrden()

            If (_mlogEsRentaVariable) Then
                Return (System.Windows.Visibility.Visible)
            Else
                Return (System.Windows.Visibility.Collapsed)
            End If
        End Get
    End Property

#End Region

#Region "Propiedades que definen atributos de la orden"

    Private _mstrIdComitente As String
    Public Property ComitenteOrden() As String
        Get
            Return (_mstrIdComitente)
        End Get
        Set(ByVal value As String)
            If value.Equals(String.Empty) Then
                _mstrIdComitente = value
                ComitenteSeleccionado = Nothing
            ElseIf Not Versioned.IsNumeric(value) Then
                A2Utilidades.Mensajes.mostrarMensaje("El código del comitente debe ser un valor numérico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                If _ComitenteSeleccionado Is Nothing Then
                    _mstrIdComitente = String.Empty
                Else
                    _mstrIdComitente = _ComitenteSeleccionado.IdComitente
                End If
            ElseIf value.ToString.Length() > MINT_LONG_MAX_CODIGO_OYD Then
                A2Utilidades.Mensajes.mostrarMensaje("La longitud máxima del código del comitente es de 17 caracteres", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                If _ComitenteSeleccionado Is Nothing Then
                    _mstrIdComitente = String.Empty
                Else
                    _mstrIdComitente = _ComitenteSeleccionado.IdComitente
                End If
            Else
                _mstrIdComitente = value

                If Not _OrdenOFSelected Is Nothing AndAlso (_ComitenteSeleccionado Is Nothing OrElse Not value.Equals(_ComitenteSeleccionado.IdComitente)) Then
                    'SLB se le adiciona buscarComitenteOrden para poder identificar si el Comitente esta inactivo.
                    buscarComitente(Right(Space(17) & value, MINT_LONG_MAX_CODIGO_OYD), "buscarComitenteOrden")
                End If
            End If

            MyBase.CambioItem("ComitenteOrden")
        End Set
    End Property

    Private _mstrNemotecnicoOrden As String
    Public Property NemotecnicoOrden() As String
        Get
            Return (_mstrNemotecnicoOrden)
        End Get
        Set(ByVal value As String)
            If value.Equals(String.Empty) Then
                _mstrNemotecnicoOrden = value
                NemotecnicoSeleccionado = Nothing
            ElseIf value.ToString.Length() > MINT_LONG_MAX_NEMOTECNICO Then
                A2Utilidades.Mensajes.mostrarMensaje("La longitud máxima del nemotécnico de la especie es de 15 caracteres", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                If _NemotecnicoSeleccionado Is Nothing Then
                    _mstrNemotecnicoOrden = String.Empty
                Else
                    _mstrNemotecnicoOrden = _NemotecnicoSeleccionado.Nemotecnico
                End If
            Else
                _mstrNemotecnicoOrden = value
                If Not _OrdenOFSelected Is Nothing AndAlso (_NemotecnicoSeleccionado Is Nothing OrElse Not value.ToUpper().Equals(_NemotecnicoSeleccionado.Nemotecnico.ToUpper())) Then
                    buscarNemotecnico(value)
                End If
            End If

            MyBase.CambioItem("NemotecnicoOrden")
        End Set
    End Property

    Private _mintDiasVigencia As Integer = 0
    ''' <summary>
    ''' Días de vigencia de la orden. Es un dato calculado que no se almacena con la orden. El usuario lo puede modificar directamente o se 
    ''' calcula con la fecha de la orden (elaboración) y la fecha de vigencia.
    ''' </summary>
    Public Property DiasVigencia() As Integer
        Get
            Return (_mintDiasVigencia)
        End Get
        Set(ByVal value As Integer)
            If Not _mintDiasVigencia.Equals(value) Then
                _mintDiasVigencia = value

                If Not IsNothing(Me.OrdenOFSelected) Then
                    Try
                        mlogRecalcularFechas = False 'CCM20120305
                        calcularDiasOrden(MSTR_CALCULAR_DIAS_ORDEN, _mintDiasVigencia)
                    Catch ex As Exception
                        mlogRecalcularFechas = True 'CCM20120305
                    End Try
                End If

                MyBase.CambioItem("DiasVigencia")
            End If
        End Set
    End Property

    Private _mintDiasPlazo As Integer = 0
    ''' <summary>
    ''' Días de plazo del título de la orden. Es un dato calculado que no se almacena con la orden. Solamente para renta fija y no puede ser modificado por el usuario.
    ''' Se calcula con la fecha de emisión del título y la fecha de vencimiento.
    ''' </summary>
    Public ReadOnly Property DiasPlazo() As Integer
        Get
            Return (_mintDiasPlazo)
        End Get
    End Property

    Private _mstrEstadoOrden As String = String.Empty
    Public Property EstadoOrden() As String
        Get
            Return (_mstrEstadoOrden)
        End Get
        Set(ByVal value As String)
            Dim objEstados As List(Of OYDUtilidades.ItemCombo)
            If Application.Current.Resources.Contains(Me.ListaCombosEsp) Then
                objEstados = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_ESTADOS_ORDEN"))

                If IsNothing(objEstados) Then
                    _mstrEstadoOrden = ""
                Else
                    _mstrEstadoOrden = (From obj In objEstados Where obj.ID = value Select obj).First.Descripcion
                End If
            Else
                _mstrEstadoOrden = ""
            End If
            MyBase.CambioItem("EstadoOrden")
        End Set
    End Property

    Private _ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            Dim logIgual As Boolean = False

            If Not IsNothing(_ComitenteSeleccionado) AndAlso _ComitenteSeleccionado.Equals(value) Then
                Exit Property
            Else
                Ordenantes = Nothing
                CuentasDeposito = Nothing
            End If

            If logIgual Then
                'buscarOrdenanteSeleccionado()
                'buscarCuentaDepositoSeleccionada()
            Else
                _ComitenteSeleccionado = value

                If Not value Is Nothing Then
                    If mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                        '// Solamente si se está ingresando o actualizando se debe cambiar el código del comitente y buscar los nuevos receptores de la orden
                        _OrdenOFSelected.IDComitente = _ComitenteSeleccionado.CodigoOYD
                        _OrdenOFSelected.FormaPago = _ComitenteSeleccionado.CodFormaPago

                        '// Consultar los receptores que se pueden asignar a la orden
                        consultarReceptoresComitente()
                    ElseIf mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Then
                        _OrdenOFSelected.IDComitente = _ComitenteSeleccionado.CodigoOYD
                        consultarReceptoresComitente()
                    End If

                    '// Actualizar el campo para que se vea el código del comitente seleccionado
                    ComitenteOrden = _ComitenteSeleccionado.IdComitente

                    consultarOrdenantes()
                    consultarCuentasDeposito()

                    '(SLB) Consultar las Cuentas de los Clientes para las Instrucciones de la Orden
                    consultarCuentasCliente()
                End If
                MyBase.CambioItem("ComitenteSeleccionado")
            End If
        End Set
    End Property

    Private _NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            Dim logIgual As Boolean = False

            If Not _NemotecnicoSeleccionado Is Nothing Then
                If _NemotecnicoSeleccionado.Equals(value) Then
                    logIgual = True
                End If
            End If

            If Not logIgual Then
                _NemotecnicoSeleccionado = value

                If Not value Is Nothing Then
                    If mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Or mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Then
                        _OrdenOFSelected.Nemotecnico = _NemotecnicoSeleccionado.Nemotecnico
                    End If

                    '// Actualizar el campo para que se vea el nemotecnico seleccionado
                    NemotecnicoOrden = _NemotecnicoSeleccionado.Nemotecnico
                End If

                MyBase.CambioItem("NemotecnicoSeleccionado")
            End If
        End Set
    End Property

#End Region

#Region "Propiedades para presentación del ordenante"

    Private _Ordenantes As List(Of OYDUtilidades.BuscadorOrdenantes)
    Public Property Ordenantes As List(Of OYDUtilidades.BuscadorOrdenantes)
        Get
            Return (_Ordenantes)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorOrdenantes))
            _Ordenantes = value

            If value Is Nothing Then
                OrdenanteSeleccionado = Nothing
            Else
                buscarOrdenanteSeleccionado()
            End If

            MyBase.CambioItem("Ordenantes")
        End Set
    End Property

    Private _mobjOrdenanteSeleccionado As OYDUtilidades.BuscadorOrdenantes
    Public Property OrdenanteSeleccionado() As OYDUtilidades.BuscadorOrdenantes
        Get
            Return (_mobjOrdenanteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorOrdenantes)
            _mobjOrdenanteSeleccionado = value
            If mstrAccionOrden.Equals(MSTR_MC_ACCION_INGRESAR) Or mstrAccionOrden.Equals(MSTR_MC_ACCION_ACTUALIZAR) Then
                If Not _mobjOrdenanteSeleccionado Is Nothing Then
                    _OrdenOFSelected.IDOrdenante = _mobjOrdenanteSeleccionado.IdOrdenante
                End If
            End If
            MyBase.CambioItem("OrdenanteSeleccionado")
        End Set
    End Property

#End Region

#Region "Propiedades para presentación de la cuenta depósito"

    Private _CuentasDeposito As List(Of OYDUtilidades.BuscadorCuentasDeposito)
    Public Property CuentasDeposito As List(Of OYDUtilidades.BuscadorCuentasDeposito)
        Get
            Return (_CuentasDeposito)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorCuentasDeposito))
            _CuentasDeposito = value

            If value Is Nothing Then
                CtaDepositoSeleccionada = Nothing
            Else
                buscarCuentaDepositoSeleccionada()
            End If

            MyBase.CambioItem("CuentasDeposito")
        End Set
    End Property

    Private _mobjCtaDepositoSeleccionada As OYDUtilidades.BuscadorCuentasDeposito
    Public Property CtaDepositoSeleccionada() As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (_mobjCtaDepositoSeleccionada)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorCuentasDeposito)
            If Not IsNothing(_mobjCtaDepositoSeleccionada) AndAlso _mobjCtaDepositoSeleccionada.Equals(value) Then
                Exit Property
            End If

            _mobjCtaDepositoSeleccionada = value

            If mstrAccionOrden.Equals(MSTR_MC_ACCION_INGRESAR) Or mstrAccionOrden.Equals(MSTR_MC_ACCION_ACTUALIZAR) Then
                If Not _mobjCtaDepositoSeleccionada Is Nothing Then
                    _OrdenOFSelected.UBICACIONTITULO = _mobjCtaDepositoSeleccionada.Deposito
                    _OrdenOFSelected.CuentaDeposito = _mobjCtaDepositoSeleccionada.NroCuentaDeposito
                    If Editando Then
                        consultarBeneficiariosCliente()
                    End If
                End If
            End If

            MyBase.CambioItem("CtaDepositoSeleccionada")
        End Set
    End Property


    Private _CuentaDepositoDeceval As List(Of OYDUtilidades.BuscadorCuentasDeposito)
    Public Property CuentaDepositoDeceval() As List(Of OYDUtilidades.BuscadorCuentasDeposito)
        Get
            Return _CuentaDepositoDeceval
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorCuentasDeposito))
            _CuentaDepositoDeceval = value
            MyBase.CambioItem("CuentaDepositoDeceval")
        End Set
    End Property

    Private _CuentasDepositoPago As List(Of OyDOrdenes.CuentasDepositoPago)
    Public Property CuentasDepositoPago() As List(Of OyDOrdenes.CuentasDepositoPago)
        Get
            Return _CuentasDepositoPago
        End Get
        Set(ByVal value As List(Of OyDOrdenes.CuentasDepositoPago))
            _CuentasDepositoPago = value
            MyBase.CambioItem("CuentasDepositoPago")
        End Set
    End Property

    Private _CuentasDepositoPagoSeleccionada As List(Of OyDOrdenes.CuentasDepositoPago)

    Private _CuentasClientes As List(Of OyDOrdenes.CuentasClientes)
    Public Property CuentasClientes() As List(Of OyDOrdenes.CuentasClientes)
        Get
            Return _CuentasClientes
        End Get
        Set(ByVal value As List(Of OyDOrdenes.CuentasClientes))
            _CuentasClientes = value
            MyBase.CambioItem("CuentasClientes")
        End Set
    End Property

    Private _TiposComisiones As List(Of OyDOrdenes.Comisiones)
    Public Property TiposComisiones() As List(Of OyDOrdenes.Comisiones)
        Get
            Return _TiposComisiones
        End Get
        Set(ByVal value As List(Of OyDOrdenes.Comisiones))
            _TiposComisiones = value
            MyBase.CambioItem("TiposComisiones")
        End Set
    End Property

#End Region

#Region "Propiedades adicionales para control de visualización"

    Private _mlogOpcionActiva As Boolean = False
    ''' <summary>
    ''' Permite habilitar o deshabilitar controles del formulario (IsEnabled, IsReadonly)
    ''' </summary>
    Public ReadOnly Property OpcionActiva() As Boolean
        Get
            'If _mlogOpcionActiva Then
            '    MessageBox.Show("Opcion activa:True")
            'End If
            Return (_mlogOpcionActiva)
        End Get
    End Property

    ''' <summary>
    ''' Permite habilitar o deshabilitar el botón de enviar por SAE (enrutar orden)
    ''' </summary>
    Public ReadOnly Property IngresoOrdenActivo() As Visibility
        Get
            If mlogUsuarioPuedeIngresar Then
                Return (Visibility.Visible)
            Else
                Return (Visibility.Collapsed)
            End If
        End Get
    End Property


    Private _DuplicarOrdenBoton As Visibility = Visibility.Visible
    Public Property DuplicarOrdenBoton() As Visibility
        Get
            Return _DuplicarOrdenBoton
        End Get
        Set(ByVal value As Visibility)
            _DuplicarOrdenBoton = value
            MyBase.CambioItem("DuplicarOrdenBoton")
        End Set
    End Property

    'Private _UsuarioOperadorCombo As Visibility = Visibility.Collapsed
    'Public Property UsuarioOperadorCombo As Visibility
    '    Get
    '        Return _UsuarioOperadorCombo
    '    End Get
    '    Set(ByVal value As Visibility)
    '        _UsuarioOperadorCombo = value
    '        MyBase.CambioItem("UsuarioOperadorCombo")
    '    End Set
    'End Property

    'Private _UsuarioOperadorText As Visibility = Visibility.Visible
    'Public Property UsuarioOperadorText As Visibility
    '    Get
    '        Return _UsuarioOperadorText
    '    End Get
    '    Set(ByVal value As Visibility)
    '        _UsuarioOperadorText = value
    '        MyBase.CambioItem("UsuarioOperadorText")
    '    End Set
    'End Property

    Private _TabSeleccionado As Byte = 0
    ''' <summary>
    ''' Propiedad para controlar el tab activo del tab control principal
    ''' </summary>
    Public Property TabSeleccionado As Byte
        Get
            Return _TabSeleccionado
        End Get
        Set(ByVal value As Byte)
            _TabSeleccionado = value

            Select Case _TabSeleccionado
                Case 1
                    cargarLiquidaciones()
            End Select

            MyBase.CambioItem("TabSeleccionado")

        End Set
    End Property

    Private _TabSeleccionadoGeneral As Integer = 0
    ''' <summary>
    ''' Propiedad para controlar el tab activo del tab control que contiene los datos generales de la orden
    ''' </summary>
    Public Property TabSeleccionadoGeneral As Integer
        Get
            Return _TabSeleccionadoGeneral
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionadoGeneral = value
            MyBase.CambioItem("TabSeleccionadoGeneral")
        End Set
    End Property

    Private _mstrListaCombosEsp As String = String.Empty
    ''' <summary>
    ''' Nombre de la lista de datos para los combos específicos de órdenes
    ''' </summary>
    Public Property ListaCombosEsp As String
        Get
            Return (_mstrListaCombosEsp)
        End Get
        Set(ByVal value As String)
            _mstrListaCombosEsp = value
        End Set
    End Property

    Private _ListaInstalacion As EntitySet(Of Instalacio)
    Public Property ListaInstalacion() As EntitySet(Of Instalacio)
        Get
            Return _ListaInstalacion
        End Get
        Set(ByVal value As EntitySet(Of Instalacio))
            _ListaInstalacion = value
            MyBase.CambioItem("ListaInstalacion")
        End Set
    End Property

    Private _mstrNombreInicioCombos As String = String.Empty
    ''' <summary>
    ''' Nombre de la lista de datos para los combos específicos de órdenes
    ''' </summary>
    Public Property NombreInicioCombos As String
        Get
            Return (_mstrNombreInicioCombos)
        End Get
        Set(ByVal value As String)
            _mstrNombreInicioCombos = value
        End Set
    End Property

    ''' <summary>
    ''' Permite controlar si los botones para nueva orden de compra y nueva orden de venta están o no activos
    ''' </summary>
    Private _ActivarSeleccionTipoOrden As Boolean = False
    Public Property ActivarSeleccionTipoOrden As Boolean
        Get
            Return (_ActivarSeleccionTipoOrden)
        End Get
        Set(ByVal value As Boolean)
            _ActivarSeleccionTipoOrden = value
            MyBase.CambioItem("ActivarSeleccionTipoOrden")
        End Set
    End Property

    ''' <summary>
    ''' Permite controlar si el botón para duplicar una orden está o no activo
    ''' </summary>
    Private _ActivarDuplicarOrden As Boolean = False
    Public Property ActivarDuplicarOrden As Boolean
        Get
            Return (_ActivarDuplicarOrden)
        End Get
        Set(ByVal value As Boolean)
            _ActivarDuplicarOrden = value
            MyBase.CambioItem("ActivarDuplicarOrden")
        End Set
    End Property

    Private _habilitarFechaElaboracion As Boolean = False
    Public Property habilitarFechaElaboracion() As Boolean
        Get
            Return _habilitarFechaElaboracion
        End Get
        Set(ByVal value As Boolean)
            _habilitarFechaElaboracion = value
            MyBase.CambioItem("habilitarFechaElaboracion")
        End Set
    End Property

    ''' <summary>
    ''' Pone en modo de solo lectura los detalles de la orden.
    ''' </summary>
    ''' <remarks></remarks>
    Private _EditandoDetalle As Boolean = True
    Public Property EditandoDetalle() As Boolean
        Get
            Return _EditandoDetalle
        End Get
        Set(ByVal value As Boolean)
            _EditandoDetalle = value
            MyBase.CambioItem("EditandoDetalle")
        End Set
    End Property


#End Region

#Region "Propiedades para manejo de horas"

    Private _HoraFinVigencia As Date = Now()
    ''' <summary>
    ''' Esta propiedad hace binding con la propiedad Hora fin de vigencia de la orden cuando su duración es a hora
    ''' </summary>
    Public Property HoraFinVigencia As Date
        Get
            Return (_HoraFinVigencia)
        End Get
        Set(ByVal value As Date)
            _HoraFinVigencia = value
            If Editando Then Me._OrdenOFSelected.VigenciaHasta = _HoraFinVigencia.Date
            MyBase.CambioItem("HoraFinVigencia")
        End Set
    End Property

    ''' <summary>
    ''' Convierte el valor de la propiedad HoraFinVigencia en string para enviar a la base de datos o el valor de la base de datos en una fecha para mostrar. Esto según
    ''' el valor del parámetro plogMostrar.
    ''' </summary>
    ''' <param name="pstrHoraMinutos">Valor que viene de la base de datos (hora y minutos)</param>
    ''' <param name="plogMostrar">Si es True convierte el valor de entrada en un tipo fecha válido para mostrarla y si es falso extrae la hora para actualizar el campo</param>
    ''' <remarks></remarks>
    Private Function asignarHoraVigencia(ByVal pstrHoraMinutos As String, ByVal plogMostrar As Boolean)
        Dim intSepHora As Integer = -1
        Dim intSepMin As Integer = -1
        Dim strHora As String = String.Empty
        Dim strMinutos As String = String.Empty
        Dim strSegundos As String = String.Empty

        If plogMostrar Then
            If pstrHoraMinutos Is Nothing Then
                _HoraFinVigencia = Nothing
            Else
                intSepHora = pstrHoraMinutos.IndexOf(":")
                intSepMin = pstrHoraMinutos.LastIndexOf(":")
                If intSepHora > 0 Then
                    strHora = Left(pstrHoraMinutos, intSepHora - 1)
                Else
                    strHora = "0"
                End If

                If intSepMin > intSepHora + 1 Then
                    strMinutos = Mid(pstrHoraMinutos, intSepHora + 1, intSepMin - intSepHora)
                Else
                    strMinutos = "0"
                End If

                If intSepMin > 1 Then
                    strSegundos = Right(pstrHoraMinutos, Len(pstrHoraMinutos) - intSepMin + 1) ' Se suma 1 porque las posiciones se toman desde cero
                Else
                    strSegundos = "0"
                End If

                If Versioned.IsNumeric(strHora) And Versioned.IsNumeric(strMinutos) And Versioned.IsNumeric(strSegundos) Then
                    _HoraFinVigencia = New DateTime(Year(Now), Month(Now), Day(Now), CInt(strHora), CInt(strMinutos), CInt(strSegundos))
                Else
                    _HoraFinVigencia = New DateTime(Year(Now), Month(Now), Day(Now), 0, 0, 0)
                End If
            End If
        ElseIf plogMostrar = False Then
            pstrHoraMinutos = Right("00" & Hour(_HoraFinVigencia).ToString, 2) & ":" & Right("00" & Minute(_HoraFinVigencia).ToString(), 2) & ":" & Right("00" & Second(_HoraFinVigencia).ToString(), 2)
        End If

        Return (pstrHoraMinutos)
    End Function

#End Region

#Region "Propiedades ordenes"

    Private _objTipoId As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property objTipoId() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _objTipoId
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _objTipoId = value
            MyBase.CambioItem("objTipoId")
        End Set
    End Property

    Private _HabilitarMercado As Boolean = False
    Public Property HabilitarMercado() As Boolean
        Get
            Return _HabilitarMercado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarMercado = value
            MyBase.CambioItem("HabilitarMercado")
        End Set
    End Property

    Private _IsBusyLiquidaciones As Boolean
    Public Property IsBusyLiquidaciones() As Boolean
        Get
            Return _IsBusyLiquidaciones
        End Get
        Set(ByVal value As Boolean)
            _IsBusyLiquidaciones = value
            MyBase.CambioItem("IsBusyLiquidaciones")
        End Set
    End Property

    Private _ListaOrdenes As EntitySet(Of OyDOrdenes.OrdenesOF)
    Public Property ListaOrdenes() As EntitySet(Of OyDOrdenes.OrdenesOF)
        Get
            Return _ListaOrdenes
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.OrdenesOF))
            _ListaOrdenes = value
            MyBase.CambioItem("ListaOrdenes")
            MyBase.CambioItem("ListaOrdenesPaged")
        End Set
    End Property

    Public ReadOnly Property ListaOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaOrdenes) Then
                Dim view = New PagedCollectionView(_ListaOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _OrdenOFSelected As OyDOrdenes.OrdenesOF
    Public Property OrdenOFSelected() As OyDOrdenes.OrdenesOF
        Get
            Return _OrdenOFSelected
        End Get
        Set(ByVal value As OyDOrdenes.OrdenesOF)
            Try
                If Not IsNothing(_OrdenOFSelected) AndAlso _OrdenOFSelected.Equals(value) Then
                    Exit Property
                ElseIf logRefrescarDetalles = False Then
                    Exit Property
                End If

                _OrdenOFSelected = value
                leerParametros()

                _mlogOpcionActiva = False
                If Not value Is Nothing Then
                    If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = String.Empty Then
                        'CFMA20180628 se cambio ya que no mostraba la informacion del comitente en la ordenOF
                        'Me.buscarComitente("buscar")
                        Me.buscarComitente(, "buscar")
                        'CFMA20180628
                        _mobjComitenteSeleccionadoAntes = _ComitenteSeleccionado

                        Me.buscarNemotecnico()
                        _mobjNemotecnicoSeleccionadoAntes = _NemotecnicoSeleccionado

                        Me.buscarItem("receptores")

                        '// Consultar los beneficiarios de la cuenta depósito asignada a la orden
                        consultarBeneficiariosOrden()

                        '// Consultar los receptores asignados a la orden
                        consultarReceptorOrdenesOF(OrdenOFSelected.Clase, OrdenOFSelected.Tipo, OrdenOFSelected.NroOrden, OrdenOFSelected.Version, String.Empty)

                        'ValidarUsuarioOperador()
                        '// Calcular días vigencia
                        mlogRecalcularFechas = False 'CCM20120305
                        Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)

                        cargarLiquidaciones()
                    Else
                        If mlogDuplicarOrden = False Then
                            '-- Se está ingresando una nueva orden pero no por la opción de duplicar
                            dcProxy1.ReceptorOrdenesOFs.Clear()
                            ListaReceptorOrdenesOF = dcProxy1.ReceptorOrdenesOFs

                            If Me.ClaseOrden = ClasesOrden.C Then
                                If Not IsNothing(CuentaDepositoDeceval) Then
                                    CuentaDepositoDeceval.Clear()
                                End If
                                'NombreColeccionDetalle = "cmPagosOrdenes"
                                'NuevoRegistroDetalle()
                                'End If
                            End If

                            _mlogOpcionActiva = True
                        End If
                    End If

                    '-- Propiedades que tienen el estado de la orden
                    Me.EstadoOrden = _OrdenOFSelected.Estado

                    TabSeleccionado = 0
                    TabSeleccionadoGeneral = 0

                End If

            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Falló la selección de la orden actual", Me.ToString, "OrdenOFSelected", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioNegocio)
            End Try

            MyBase.CambioItem("OrdenOFSelected")
        End Set
    End Property

    Private _VisibleDuracionFecha As Visibility = Visibility.Collapsed
    Public Property VisibleDuracionFecha As Visibility
        Get
            Return _VisibleDuracionFecha
        End Get
        Set(ByVal value As Visibility)
            _VisibleDuracionFecha = value
            MyBase.CambioItem("VisibleDuracionFecha")
        End Set
    End Property

    Private _VisibleDuracionHora As Visibility = Visibility.Collapsed
    Public Property VisibleDuracionHora As Visibility
        Get
            Return _VisibleDuracionHora
        End Get
        Set(ByVal value As Visibility)
            _VisibleDuracionHora = value
            MyBase.CambioItem("VisibleDuracionHora")
        End Set
    End Property

#End Region

#Region "Resultados Asincrónicos"

    ''' <summary>
    ''' SLB - Este metodo arroja el resultado si la fecha de orden es valida, si se esta Guardando invoca el Metodo calcularDiasOrden.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoVerificarFechaValida(ByVal lo As LoadOperation(Of OyDOrdenes.ValidarDiaHabil))
        Try
            Dim objValidarDiaHabil As OyDOrdenes.ValidarDiaHabil
            If Not lo.HasError Then
                objValidarDiaHabil = dcProxy.ValidarDiaHabils.First
                If Not objValidarDiaHabil.EsDiaHabil Then
                    MostrarMensajeFechaNoValida(objValidarDiaHabil.MenorFechaHabilOrden)
                ElseIf lo.UserState.ToString.Equals("GuardarOrden") Then
                    calcularDiasOrden(MSTR_CALCULAR_DIAS_ORDEN, -1, True)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al completar la validación de los días de vigencia para guardar la orden", Me.ToString(), "calcularDiasHabilesGuardarCompleted", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub MostrarMensajeFechaNoValida(ByVal pdtmFechaOrden As Date)
        A2Utilidades.Mensajes.mostrarMensaje("La Fecha de Elebaroción de la orden (" + _OrdenOFSelected.FechaOrden.Date.ToLongDateString + ") es un día no hábil", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        _OrdenOFSelected.FechaOrden = pdtmFechaOrden
        IsBusy = False
    End Sub

    ''' <summary>
    ''' SLB - Verificar de la fecha de elaboración de la Orden es Valida, invoca el SP uspOyDNet_Ordenes_CalcularDiasHabiles
    ''' </summary>
    ''' <param name="plogGuardarOrden"></param>
    ''' <remarks>SLB20120704</remarks>
    Public Sub VerificarFechaValida(Optional ByVal plogGuardarOrden As Boolean = False)
        Try
            If _OrdenOFSelected.FechaOrden.Date > Now.Date Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de elaboración de la orden debe ser menor o igual a la fecha actual ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                OrdenOFSelected.FechaOrden = Now
                IsBusy = False
                Exit Sub
            End If

            Dim Accion As String = String.Empty
            If plogGuardarOrden Then
                Accion = "GuardarOrden"
            End If
            dcProxy.ValidarDiaHabils.Clear()
            dcProxy.Load(dcProxy.ValidarDiaHabilQuery(Me._OrdenOFSelected.FechaOrden.Date, Program.Usuario, Program.HashConexion), AddressOf TerminoVerificarFechaValida, Accion)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar la fecha de elaboracion de la orden", Me.ToString(), "VerificarFechaValida", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Este procedimiento se ejecuta cuando el usuario va a guardar la orden y se lanza la validación de la fecha de la orden (elaboración) y de vigencia de la orden, además 
    ''' la fecha de emisión y vencimiento del título
    ''' </summary>
    Private Sub calcularDiasHabilesGuardarCompleted(ByVal lo As LoadOperation(Of OyDOrdenes.ValidarFecha))
        Try
            If calcularDiasHabilesValidar(lo) Then
                validarOrden()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al completar la validación de los días de vigencia para guardar la orden", Me.ToString(), "calcularDiasHabilesGuardarCompleted", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Este procedimiento se ejecuta cuando se lanza la validación de la fecha de la orden (elaboración) y de vigencia de la orden, además 
    ''' la fecha de emisión y vencimiento del título, pero siempre y cuando no se esté guardando la orden
    ''' </summary>
    Private Sub calcularDiasHabilesCompleted(ByVal lo As LoadOperation(Of OyDOrdenes.ValidarFecha))
        Try
            calcularDiasHabilesValidar(lo)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al completar la validación de los días de vigencia al editar la orden", Me.ToString(), "calcularDiasHabilesCompleted", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Este procedimiento valida el resultado recibido desde el servidor cuando se modifican las fechas de la orden y/o vigencia de la orden y las 
    ''' fechas de emisión y vencimiento del título
    ''' </summary>
    Private Function calcularDiasHabilesValidar(ByVal lo As LoadOperation(Of OyDOrdenes.ValidarFecha))
        Dim objDatos As OyDOrdenes.ValidarFecha
        Dim strMsg As String = String.Empty
        Dim strAccion As String = String.Empty
        Dim logResultado As Boolean = True

        Try
            If Not lo.HasError Then
                objDatos = lo.Entities.FirstOrDefault
                strAccion = lo.UserState.ToString.ToLower

                If Not objDatos Is Nothing Then
                    If objDatos.EsDiaHabil Then
                        '/ Guardar la fecha de cierre del sistema
                        mdtmFechaCierreSistema = objDatos.FechaCierre

                        If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then
                            If objDatos.FechaFinal <= objDatos.FechaCierre And (mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR) Then
                                A2Utilidades.Mensajes.mostrarMensaje("La fecha de vigencia no puede ser menor a la fecha de cierre del sistema (" & objDatos.FechaCierre.ToLongDateString() & ")." & vbNewLine & vbNewLine & "Por favor modifique la fecha de vigencia de la orden para que sea posterior a la fecha de cierre.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                _mintDiasVigencia = 0
                                Return (False)
                            End If
                        End If

                        If strAccion = MSTR_ACCION_CALCULAR_DIAS Then
                            If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then
                                _mintDiasVigencia = objDatos.NroDias
                                MyBase.CambioItem("DiasVigencia")
                            Else
                                _mintDiasPlazo = objDatos.NroDias
                                MyBase.CambioItem("DiasPlazo")
                            End If
                        Else
                            If Not objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_TITULO Then
                                _OrdenOFSelected.VigenciaHasta = objDatos.FechaFinal
                            End If
                        End If

                        logResultado = True
                    Else 'If objDatos.EsDiaHabil Then
                        If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                            If Not objDatos.FechaHabilMayor Is Nothing Then
                                strMsg = "La fecha hábil siguiente es " & FormatDateTime(objDatos.FechaHabilMayor, Microsoft.VisualBasic.DateFormat.LongDate) & "."
                            End If
                            If Not objDatos.FechaHabilMenor Is Nothing Then
                                strMsg &= "La fecha hábil anterior es " & FormatDateTime(objDatos.FechaHabilMenor, Microsoft.VisualBasic.DateFormat.LongDate) & "."
                            End If

                            If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then
                                strMsg = "La fecha de vigencia no es un día hábil. " & strMsg & "." & vbNewLine & vbNewLine &
                                        "Debe seleccionar un día hábil."
                            Else
                                strMsg = "La fecha de vencimiento no es un día hábil. " & strMsg & "." & vbNewLine & vbNewLine &
                                        "Debe seleccionar un día hábil."
                            End If

                            A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_ORDEN Then
                                If IsNothing(objDatos.NroDias) Then
                                    If IsNothing(OrdenOFSelected.VigenciaHasta) Then
                                        _mintDiasVigencia = 0
                                    Else
                                        ' Se suma uno porque se deben incluir la fecha de la orden y la fecha de vigencia
                                        _mintDiasVigencia = DateDiff(DateInterval.Day, _OrdenOFSelected.FechaOrden, CType(OrdenOFSelected.VigenciaHasta, Date)) + 1
                                    End If
                                Else
                                    _mintDiasVigencia = objDatos.NroDias
                                End If
                                MyBase.CambioItem("DiasVigencia")
                            Else
                                If IsNothing(objDatos.NroDias) Then
                                    _mintDiasPlazo = 0
                                Else
                                    _mintDiasPlazo = objDatos.NroDias
                                End If
                                MyBase.CambioItem("DiasPlazo")
                            End If
                        End If

                        logResultado = False
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación de los días al vencimiento", Me.ToString(), "calcularDiasHabilesCompleted", Program.TituloSistema, Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la validación de los días al vencimiento", Me.ToString(), "calcularDiasHabilesCompleted", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            mlogRecalcularFechas = True 'CCM20120305
            Me.IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Calcula el número de días entre dos fechas. 
    ''' Se suma uno a la diferencia entre las fechas para incluir la fecha inicial y final, si el parámetro Incluir extremos es verdadero.
    ''' </summary>
    ''' <param name="pdtmFechaDesde">Fecha inicial del rango</param>
    ''' <param name="pdtmFechaHasta">Fecha final del rango</param>
    ''' <param name="plogIncluirExtremos">Indica si en la diferencia en días se deben contar la fecha inicial y final (True) o no (False)</param>
    ''' <returns>Número de días entre las fechas.</returns>
    ''' <remarks></remarks>
    Private Function calcularDiasRango(ByVal pdtmFechaDesde As Date, ByVal pdtmFechaHasta As Date, Optional ByVal plogIncluirExtremos As Boolean = True) As Integer
        Dim intDias As Integer
        Try
            intDias = DateDiff(DateInterval.Day, pdtmFechaDesde, pdtmFechaHasta) + IIf(plogIncluirExtremos, 1, 0)
        Catch ex As Exception
            intDias = 0
        End Try
        Return (intDias)
    End Function

    Private Sub TerminoVerificarOrdenModificable(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenOFModificable))
        Dim objDatos As OyDOrdenes.OrdenOFModificable
        Dim logEditar As Boolean = False
        Dim strAccion As String = String.Empty
        Dim strMsg As String = String.Empty

        Try
            If Not lo.HasError Then
                objDatos = lo.Entities.FirstOrDefault
                strAccion = lo.UserState.ToString.ToLower

                If Not objDatos Is Nothing Then
                    If objDatos.Modificable Then
                        If objDatos.UltimaModificacion.Equals(_OrdenOFSelected.FechaActualizacion) Then
                            logEditar = True
                        Else
                            mostrarMensajePregunta("La orden fue modificada después que se realizó la consulta para desplegarla en pantalla. Debe actualizarla para poder modificarla." & vbNewLine & vbNewLine & "¿Desea refrescar los datos de las ordenes actuales?", _
                                                   Program.TituloSistema, _
                                                   "REFRESCARDATOSORDEN", _
                                                   AddressOf validarRefrescarOrdenes, False)
                        End If
                    Else
                        strMsg = objDatos.Mensaje
                        If strAccion = MSTR_ACCION_ANULAR Then
                            strMsg &= vbNewLine & "La orden no puede ser anulada."
                        End If
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación del estado de la orden", Me.ToString(), "TerminoVerificarOrdenModificable", Program.TituloSistema, Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If

            If logEditar Then
                If strAccion = MSTR_ACCION_EDITAR Then
                    If Not IsNothing(_OrdenOFSelected) Then
                        If objDatos.TieneLiquidaciones And (_OrdenOFSelected.Estado = "P" Or _OrdenOFSelected.Estado = "M") Then
                            mostrarMensajePregunta("¿Desea registrar la orden en el libro de órdenes?",
                                                   Program.TituloSistema,
                                                   "LIBROORDENES",
                                                   AddressOf TerminoPreguntarLibroOrdenes, False)
                        Else
                            logRegistroLibroOrdenes = False
                            dblValorCantidadLibroOrden = 0

                            Editando = True
                            EditandoDetalle = False
                            mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR

                            If Me.ClaseOrden = ClasesOrden.C Then
                                Select Case Me.OrdenOFSelected.Objeto
                                    Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_TTV_Salida, RF_TTV_Regreso, RF_REPO
                                        Me.HabilitarMercado = False
                                    Case Else
                                        Me.HabilitarMercado = True
                                End Select
                            End If
                            _strObjetoAnterior = _OrdenOFSelected.Objeto
                            _strObjetoAnteriorValidar = _OrdenOFSelected.Objeto

                            DeshabilitarBotones()
                        End If
                    Else
                        mstrAccionOrden = String.Empty
                    End If
                Else
                    mostrarMensajePregunta("Comentario para la anulación de la orden: ",
                                           Program.TituloSistema,
                                           "ANULARORDEN",
                                           AddressOf TerminoComentariosAnulacion,
                                           True,
                                           "¿Anular la orden?", False, True, True, False)
                End If
            Else
                MyBase.RetornarValorEdicionNavegacion()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la validación del estado de la orden", Me.ToString(), "TerminoVerificarOrdenModificable", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            Me.IsBusy = False
        End Try
    End Sub

    Private Sub TerminoPreguntarLibroOrdenes(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If objResultado.DialogResult Then
                logRegistroLibroOrdenes = True
                dblValorCantidadLibroOrden = _OrdenOFSelected.Cantidad

                Editando = True
                EditandoDetalle = False
                mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR

                If Me.ClaseOrden = ClasesOrden.C Then
                    Select Case Me.OrdenOFSelected.Objeto
                        Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_TTV_Salida, RF_TTV_Regreso, RF_REPO
                            Me.HabilitarMercado = False
                        Case Else
                            Me.HabilitarMercado = True
                    End Select
                End If
                _strObjetoAnterior = _OrdenOFSelected.Objeto
                _strObjetoAnteriorValidar = _OrdenOFSelected.Objeto

                DeshabilitarBotones()
            Else
                MyBase.RetornarValorEdicionNavegacion()
                logRegistroLibroOrdenes = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del libro de ordenes.", Me.ToString(), "TerminoPreguntarLibroOrdenes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerOrdenesPorDefecto_Completed(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenesOF))
        If Not lo.HasError Then
            OrdenOFPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Orden por defecto", _
                                             Me.ToString(), "TerminoTraerOrdenPorDefecto_Completed", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenesOF))
        Try
            If Not lo.HasError Then

                '// Este indicador ayuda a controla la ejecución de consultas inutiles durante el ingreso especialmente
                mstrAccionOrden = String.Empty

                If dcProxy.OrdenesOFs.Count > 0 Then
                    '// Se activan o inactiva acá estos botónes porque siempre que se guarda se actualiza la lista de órdenes
                    ActivarDuplicarOrden = True
                    ActivarSeleccionTipoOrden = True
                Else
                    '// Se activan o inactiva acá estos botónes porque siempre que se guarda se actualiza la lista de órdenes
                    ActivarDuplicarOrden = False
                    ActivarSeleccionTipoOrden = False
                End If

                If (lo.UserState = MSTR_MC_ACCION_INGRESAR And TipoOrdenModificada = MSTR_MC_ORDEN_DOBLE) Or _
                    lo.UserState = MSTR_MC_ACCION_ACTUALIZAR Then
                    logRefrescarDetalles = False
                End If

                ListaOrdenes = dcProxy.OrdenesOFs

                If _ListaOrdenes.Count > 0 Then
                    If lo.UserState = MSTR_MC_ACCION_INGRESAR Then
                        If TipoOrdenModificada = MSTR_MC_ORDEN_DOBLE Then
                            Dim idOrden As Integer = _ListaOrdenes.First.NroOrden - 1
                            logRefrescarDetalles = True
                            OrdenOFSelected = _ListaOrdenes.Where(Function(i) i.NroOrden = idOrden).First
                        End If
                    ElseIf lo.UserState = MSTR_MC_ACCION_ACTUALIZAR Then
                        logRefrescarDetalles = True
                        If _ListaOrdenes.Where(Function(i) i.NroOrden = intIDOrdenModificada).Count > 0 Then
                            OrdenOFSelected = _ListaOrdenes.Where(Function(i) i.NroOrden = intIDOrdenModificada).First
                        Else
                            OrdenOFSelected = _ListaOrdenes.First
                        End If
                    End If
                End If

                If dcProxy.OrdenesOFs.Count = 0 AndAlso (lo.UserState.ToString.ToLower() = MSTR_ACCION_BUSCAR Or lo.UserState.ToString.ToLower() = MSTR_ACCION_FILTRAR) Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontraron registros que cumplan con las condiciones de indicadas", Program.TituloSistema)
                    visNavegando = "Collapsed"
                    MyBase.CambioItem("visNavegando")
                Else
                    MyBase.CambiarFormulario_Forma_Manual()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de la lista de Ordenes", Me.ToString(), "TerminoTraerOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes", Me.ToString(), "TerminoTraerOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
        Finally
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoTraerReceptorOrdenesOF(ByVal lo As LoadOperation(Of OyDOrdenes.ReceptorOrdenesOF))
        If Not lo.HasError Then
            ListaReceptorOrdenesOF = dcProxy1.ReceptorOrdenesOFs
            'SLB20121130 Se adiciona esta logica para saber si los receptores de la orden duplicada se inactivaron
            If lo.UserState.ToString = "verificarReceptoresDuplicar" And _ListaReceptorOrdenesOF.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Receptor inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptorOrdenesOFes", _
                                             Me.ToString(), "TerminoTraerReceptorOrdenesOFes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerBeneficiariosOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.BeneficiarioOrdenesOF))
        If Not lo.HasError Then
            ListaBeneficiariosOrdenesOF = dcProxy.BeneficiarioOrdenesOFs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BeneficiariosOrdenes", _
                                             Me.ToString(), "TerminoTraerBeneficiariosOrdenes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerOrdenantes(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorOrdenantes))
        If Not lo.HasError Then
            Ordenantes = mdcProxyUtilidad01.BuscadorOrdenantes.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes", _
                                             Me.ToString(), "TerminoTraerOrdenantes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCuentasDeposito(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorCuentasDeposito))
        If Not lo.HasError Then
            CuentasDeposito = mdcProxyUtilidad02.BuscadorCuentasDepositos.ToList
            If Me.ClaseOrden = ClasesOrden.C Then
                CuentaDepositoDeceval = (From Cuentas In CuentasDeposito Where Cuentas.Deposito = "D").ToList
            End If

            Dim objNroCuenta As Integer = -1
            Dim strDeposito As String = String.Empty

            If Not IsNothing(OrdenOFSelected) Then
                strDeposito = _OrdenOFSelected.UBICACIONTITULO
                If Not IsNothing(_OrdenOFSelected.CuentaDeposito) Then
                    objNroCuenta = _OrdenOFSelected.CuentaDeposito
                End If
            End If

            If Me.ClaseOrden = ClasesOrden.A Then
                If CuentasDeposito.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = objNroCuenta).Count > 0 Then
                    CtaDepositoSeleccionada = CuentasDeposito.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = objNroCuenta).First
                Else
                    If CuentasDeposito.Where(Function(i) i.Deposito = "D" And i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).Count = 1 Then
                        CtaDepositoSeleccionada = CuentasDeposito.Where(Function(i) i.Deposito = "D" And i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).First
                    End If
                End If
            Else
                If CuentasDeposito.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = objNroCuenta).Count > 0 Then
                    CtaDepositoSeleccionada = CuentasDeposito.Where(Function(i) i.Deposito = strDeposito And IIf(IsNothing(i.NroCuentaDeposito), -1, i.NroCuentaDeposito) = objNroCuenta).First
                Else
                    If CuentasDeposito.Where(Function(i) i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).Count = 1 Then
                        CtaDepositoSeleccionada = CuentasDeposito.Where(Function(i) i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).First
                    End If
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                             Me.ToString(), "TerminoTraerCuentasDeposito", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCuentasClientes(ByVal lo As LoadOperation(Of OyDOrdenes.CuentasClientes))
        If Not lo.HasError Then
            CuentasClientes = dcProxy.CuentasClientes.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                             Me.ToString(), "TerminoTraerCuentasClientes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando termina de guardar los cambios de la orden.
    ''' </summary>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)

        Dim strMsg As String = String.Empty
        Try
            If So.HasError Then
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next

                    If Not strMsg.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                                   Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                    So.MarkErrorAsHandled()
                End If
                IsBusy = False
                MyBase.TerminoSubmitChanges(So)
                Exit Try
            Else
                If So.UserState = MSTR_MC_ACCION_INGRESAR Or So.UserState = MSTR_MC_ACCION_ACTUALIZAR Then

                    If Me.ClaseOrden = ClasesOrden.C Then
                        Select Case _OrdenOFSelected.Objeto
                            Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_TTV_Salida, RF_TTV_Regreso, RF_REPO
                                TipoOrdenModificada = MSTR_MC_ORDEN_DOBLE
                            Case Else
                                TipoOrdenModificada = MSTR_MC_ORDEN_SENCILLA
                        End Select
                    Else
                        TipoOrdenModificada = MSTR_MC_ORDEN_SENCILLA
                    End If


                    If So.UserState = MSTR_MC_ACCION_ACTUALIZAR Then
                        intIDOrdenModificada = _OrdenOFSelected.NroOrden
                    Else
                        intIDOrdenModificada = 0
                    End If

                    MyBase.TerminoSubmitChanges(So)
                    IsBusy = True
                    dcProxy.OrdenesOFs.Clear()
                    dcProxy.Load(dcProxy.OrdenesOFFiltrarQuery(Me.ClaseOrden.ToString, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, So.UserState)
                Else
                    MyBase.TerminoSubmitChanges(So)
                    Filtrar()
                End If

                HabilitarMercado = False
            End If
            habilitarFechaElaboracion = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", Me.ToString(), "TerminoSubmitChanges", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarLiquidacionesOrdenOF(ByVal lo As LoadOperation(Of OyDOrdenes.LiquidacionesOrdenOF))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se cargaron las liquidaciones de la orden porque se presentó un problema durante el proceso.", Me.ToString(), "Autorizar_OrdenCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
                IsBusy = False
            Else
                LiquidacionesOrdenOF = dcProxyConsultas.LiquidacionesOrdenOFs

                If Not IsNothing(_OrdenOFSelected) Then
                    If _OrdenOFSelected.Estado = "P" Or _OrdenOFSelected.Estado = "M" Then
                        SaldoOrden = 0

                        Dim sumaLiquidaciones As Double = 0

                        For Each li In _ListaLiquidacionesOrdenOF
                            sumaLiquidaciones += li.Cantidad
                        Next

                        If sumaLiquidaciones < _OrdenOFSelected.Cantidad Then
                            SaldoOrden = _OrdenOFSelected.Cantidad - sumaLiquidaciones
                        End If
                    Else
                        SaldoOrden = _OrdenOFSelected.Cantidad
                    End If
                Else
                    SaldoOrden = 0
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir las liquidaciones de la orden", Me.ToString(), "TerminoConsultarLiquidacionesOrdenOF", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
            IsBusyLiquidaciones = False
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando finaliza la anulación de la orden en el servidor
    ''' </summary>
    Private Sub TerminoAnularOrden(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenSAE))
        Dim objOrdenSAE As OyDOrdenes.OrdenSAE
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se anuló la orden porque se presentó un problema durante el proceso.", Me.ToString(), "TerminoAnularOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
            Else
                ' (SLB) Modifcado para que trabaje con el Maker and Checker
                objOrdenSAE = dcProxy.OrdenSAEs.First
                If objOrdenSAE.TipoMensaje > 1 Then
                    A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("La anulación de la orden se realizo correctamente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    Filtrar()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la anulación de la orden.", Me.ToString(), "TerminoAnularOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del comitente que tiene asignada la orden
    ''' Se dispara cuando la busqueda del comitente iniciada desde el procedimiento buscarComitente finaliza
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.UserState.ToString = "buscarComitenteOrden" Then

                    If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                        Me.ComitenteSeleccionado = Nothing
                        If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                            A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                        ComitenteOrden = String.Empty
                        dcProxy1.ReceptorOrdenesOFs.Clear()
                        ListaReceptorOrdenesOF = dcProxy1.ReceptorOrdenesOFs
                    Else
                        Me.ComitenteSeleccionado = lo.Entities.ToList.Item(0)
                    End If

                Else
                    Me.ComitenteSeleccionado = lo.Entities.ToList.Item(0)

                    If lo.UserState.ToString = "buscar" Then
                        _mobjComitenteSeleccionadoAntes = _ComitenteSeleccionado
                    End If
                End If
            Else
                Me.ComitenteSeleccionado = Nothing
                If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                ComitenteOrden = String.Empty
                dcProxy1.ReceptorOrdenesOFs.Clear()
                ListaReceptorOrdenesOF = dcProxy1.ReceptorOrdenesOFs
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del nemotecnico que tiene asignada la orden
    ''' Se dispara cuando la busqueda del nemotecnico iniciada desde el procedimiento buscarNemotecnico finaliza
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub buscarNemotecnicoCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Try
            If lo.Entities.ToList.Count > 0 Then
                Me.NemotecnicoSeleccionado = lo.Entities.ToList.Item(0)
                If Editando Then
                    If Not Me.NemotecnicoSeleccionado.Activo Then
                        A2Utilidades.Mensajes.mostrarMensaje("El nemotécnico ingresado se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Me.NemotecnicoSeleccionado = Nothing
                        NemotecnicoOrden = String.Empty
                    End If

                    If Me.ClaseOrden.ToString = ClasesOrden.A.ToString Then
                        If Me._OrdenOFSelected.Objeto <> "IC" Then
                            If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                                A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    Else
                        If Me._OrdenOFSelected.Objeto <> "IC" Or (Not IsNothing(Me.ComitenteSeleccionado) AndAlso Me.ComitenteSeleccionado.CodCategoria <> "2") Then
                            If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                                A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado y el cliente debe ser Inversionista Profesional", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                End If
                _mobjNemotecnicoSeleccionadoAntes = _NemotecnicoSeleccionado
            Else
                Me.NemotecnicoSeleccionado = Nothing
                If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                    A2Utilidades.Mensajes.mostrarMensaje("El nemotécnico ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                NemotecnicoOrden = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del nemotécnico de la orden", Me.ToString(), "buscarNemotecnicoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del receptor toma que tiene asignada la orden
    ''' Se dispara cuando la busqueda del receptor toma iniciada desde el procedimiento buscarItem finaliza
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub buscarGenericoCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Select Case strTipoItem.ToLower()
                    Case "receptores"

                End Select
            Else
                Select Case strTipoItem.ToLower()
                    Case "receptores"

                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarGenericoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si se refrescan los datos de las órdenes que están en pantalla
    ''' </summary>
    Private Sub validarRefrescarOrdenes(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                Me.FiltroVM = Me.OrdenOFSelected.NroOrden
                Filtrar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar si se refrescan los datos de las órdenes en pantalla", Me.ToString(), "validarRefrescarOrdenes", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario ingresa el comentario a la anulación de la orden
    ''' </summary>
    Private Sub TerminoComentariosAnulacion(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IsBusy = True
                dcProxy.OrdenSAEs.Clear()
                dcProxy.Load(dcProxy.AnularOrdenesOFQuery(_OrdenOFSelected.Clase, _OrdenOFSelected.Tipo, _OrdenOFSelected.NroOrden, _OrdenOFSelected.Version, objResultado.Observaciones, _OrdenOFSelected.Notas, Program.Usuario, Program.HashConexion), AddressOf TerminoAnularOrden, MSTR_ACCION_ANULAR)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el rechazo de la orden", Me.ToString(), "TerminoComentariosAnulacion", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <sumary>
    ''' (SLB) Se ejecuta cuando retorna de confirmar si el usuario desea duplicar la orden
    ''' </sumary>
    Private Sub validarDuplicarOrden(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IsBusy = True
                configurarNuevaOrden(_OrdenOFSelected, True, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el duplicar una orden", Me.ToString(), "validarDuplicarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoValidarPermisosDuplicar(ByVal lo As InvokeOperation(Of Boolean))
        If Not lo.HasError Then
            If lo.Value Then
                DuplicarOrdenBoton = Visibility.Visible
            Else
                DuplicarOrdenBoton = Visibility.Collapsed
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar permisos para Duplicar una Orden", _
                     Me.ToString(), "TerminoValidarPermisosDuplicar", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

#Region "Métodos"

    Private Sub ValidarPermisosDuplicar()
        Try
            dcProxy1.ValidarUsuario_DuplicarOrden(Program.Usuario, Program.HashConexion, AddressOf TerminoValidarPermisosDuplicar, "consulta")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar permisos para Duplicar una Orden", Me.ToString(), "ValidarPermisosDuplicar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Habilitar los botones de Duplicar, Enviar SAE, Compra, Venta y Modificación Instrucción.
    ''' </summary>
    ''' <remarks>SLB20130202</remarks>
    Friend Sub HabilitarBotones()
        ActivarDuplicarOrden = True
        ActivarSeleccionTipoOrden = True
    End Sub

    ''' <summary>
    ''' Deshabilitar los botones de Duplicar, Enviar SAE, Compra, Venta y Modificación Instrucción.
    ''' </summary>
    ''' <remarks>SLB20130202</remarks>
    Friend Sub DeshabilitarBotones(Optional ByVal strOpcion As String = "")
        ActivarDuplicarOrden = False
        ActivarSeleccionTipoOrden = False
    End Sub

    ''' <summary>
    ''' Valida si el usuario tienen permisos para adicionar órdenes. 
    ''' La verificación se hace comprobando si el botón Nuevo está o no presente en la barra de herramientas
    ''' </summary>
    ''' <remarks>CCM20120305</remarks>
    Private Sub validarPermisosAdicionar()
        Dim objToolbar As List(Of String) = Nothing
        Dim strDatosBoton As String = String.Empty

        Try
            If Application.Current.Resources.Contains("A2Consola_ToolbarActivo") Then
                objToolbar = CType(Application.Current.Resources("A2Consola_ToolbarActivo"), List(Of String))

                For Each strDatosBoton In objToolbar
                    If strDatosBoton.Substring(0, 5).ToLower() = "nuevo" Then
                        mlogUsuarioPuedeIngresar = True
                        Exit For
                    End If
                Next
            Else
                mlogUsuarioPuedeIngresar = False
            End If
        Catch ex As Exception
            mlogUsuarioPuedeIngresar = False
        End Try
    End Sub

    ''' <summary>
    ''' Consulta en los datos de los combos el grupo de datos PARAM_ORDENES que tiene varios parámetros para configurar el comportamiento de la orden
    ''' </summary>
    ''' <remarks>CCM20120305</remarks>
    Private Sub leerParametros()

        If mlogDuplicarDatosParam Then
            Exit Sub
        End If

        Dim objParametrosDiccionario As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim strValor As String = ""

        If Application.Current.Resources.Contains(Me.ListaCombosEsp) Then
            Try
                If CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey(NombreCategoriaEnDiccionario("O_PARAM_CONFIG_ORDENES")) Then
                    objParametrosDiccionario = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_PARAM_CONFIG_ORDENES"))
                End If
            Catch ex As Exception
                Exit Sub
            End Try

            If Not IsNothing(objParametrosDiccionario) Then

                If CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey(NombreCategoriaEnDiccionario("O_USUARIO_OPERADOR")) Then
                    objDicUsuarioOperador = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_USUARIO_OPERADOR"))
                End If

                Try
                    mstrTipoLimitePorDef = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_TIPO_LIMITE_DEFAULT" Select obj).First.Valor
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_DUPLICAR_DATOS_PRECIO_ACC" Select obj).First.Valor
                    If strValor = "S" Then
                        mlogDuplicarDatosPrecio = True
                    Else
                        mlogDuplicarDatosPrecio = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    If ClasesOrden.A.ToString = Me._OrdenOFSelected.Clase Then
                        strValor = (From obj In objListaParametros Where obj.Parametro = "PORC_CANT_VISIBLE" Select obj).First.Valor
                        If strValor <> String.Empty Then
                            intPorcentajeCantidadVisible = CInt(strValor)
                        End If
                    End If

                Catch ex As Exception

                End Try

                Try
                    strValor = (From obj In objParametrosDiccionario Where obj.ID = "HABILITAR_FECHA_ELABORACION" Select obj).First.Descripcion
                    If strValor = "1" Then
                        _mlogHabilitarFechaElaboracion = True
                    Else
                        _mlogHabilitarFechaElaboracion = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objParametrosDiccionario Where obj.ID = "COMBO_RECEPTORES_CLIENTES" Select obj).First.Descripcion
                    If strValor = "1" Then
                        _mlogMostrarTodosReceptores = False
                    Else
                        _mlogMostrarTodosReceptores = True
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objParametrosDiccionario Where obj.ID = "CARGAR_RECEPTORES_CLIENTES" Select obj).First.Descripcion
                    If strValor = "1" Then
                        _mlogCargarReceptorClientes = True
                    Else
                        _mlogCargarReceptorClientes = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objParametrosDiccionario Where obj.ID = "REQUIERO_INGRESO_ORDENANTES" Select obj).First.Descripcion
                    If strValor = "1" Then
                        _mlogRequiereIngresoOrdenantes = True
                    Else
                        _mlogRequiereIngresoOrdenantes = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "DIAS_VIGENCIA_ORDEN_INMEDIATA" Select obj).First.Valor
                    intDiasVigenciaDuracionInmediata = IIf(IsNothing(strValor), 1, CInt(strValor))
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                'Try
                '    strValor = (From obj In objParametrosDiccionario Where obj.ID = "OYDNet_ORDEN_COMITENTEPERMITIDO_OF" Select obj).First.Descripcion
                '    strIDComitentePermitidoOF = LTrim(RTrim(strValor))
                'Catch ex As Exception
                '    '-- Si hay error se trabaja con el valor por defecto del parámetro
                'End Try


                Try
                    mstrFormaPagoPorDef = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_TIPO_FORMAPAGO_DEFAULT" Select obj).First.Valor
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try


                Try 'Santiago Vergara - Marzo 20/2014 - Se migra cambio en la validación de cuenta depósito 
                    strValor = (From obj In objListaParametros Where obj.Parametro = "CUENTADEPOSITOORDENES" Select obj).First.Valor
                    If strValor = "SI" Then
                        mlogCUENTADEPOSITOORDENES = True
                    Else
                        mlogCUENTADEPOSITOORDENES = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try


                Try 'Santiago Vergara - Marzo 20/2014 - Se migra cambio en la validación de cuenta depósito 
                    strValor = (From obj In objListaParametros Where obj.Parametro = "HABILITAR_DEPOSITO_VENTASOF" Select obj).First.Valor
                    If strValor = "SI" Then
                        mlogHABILITAR_DEPOSITO_VENTASOF = True
                    Else
                        mlogHABILITAR_DEPOSITO_VENTASOF = False
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                mlogDuplicarDatosParam = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Calcula el número de días hábiles entre dos fechas o a partir de la fecha inicial y un número de días calcula la fecha hábil final.
    ''' </summary>
    ''' <param name="pstrTipoCalculo">Indica si el cálculo es para los días de la orden o los días de vencimiento del título</param>
    ''' <param name="pintDias">Indica el número de días entre la fecha inicial y final. Si no se recibe o es menor que cero se calcula el número de días, si se recibe se calcula la fecha final.</param>
    ''' <param name="plogGuardarOrden">Si es verdadero indica que la orden debe ser guardada si pasa las validaciones. Si es falso solamente se hacen las validaciones</param>
    ''' 
    Public Sub calcularDiasOrden(ByVal pstrTipoCalculo As String, Optional ByVal pintDias As Integer = -1, Optional ByVal plogGuardarOrden As Boolean = False)
        Try
            _mlogFechaCierreSistema = False
            If _OrdenOFSelected Is Nothing Then
                Exit Sub
                'ElseIf mstrAccionOrden.Equals(String.Empty) And _mintDiasVigencia > 0 Then 'CCM20120306 - Desactivar esta condición porque no recalcula los días de vigencia
                'Exit Sub
            ElseIf pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO And Me.ClaseOrden = ClasesOrden.A Then
                '/ Los días de vencimiento del título solamente se calculan para renta fija
                Exit Sub
            ElseIf (IsNothing(_OrdenOFSelected.FechaOrden) Or (IsNothing(_OrdenOFSelected.VigenciaHasta) And pintDias <= 0)) And pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_ORDEN Then
                '/ Para calcular la fecha de vigencia o los días de vigencia de la orden es necesario tener la fecha de la orden y los días de vigencia o la fecha de vigencia respectivamente
                Exit Sub
            ElseIf mdtmFechaCierreSistema >= _OrdenOFSelected.VigenciaHasta Then
                _mlogFechaCierreSistema = True
                'CambiarAForma()
                'A2Utilidades.Mensajes.mostrarMensaje("La fecha de la orden no puede ser menor a la fecha de cierre del sistema (" & mdtmFechaCierreSistema.ToLongDateString() & ")." & vbNewLine & vbNewLine & "Por favor modifique la fecha de elaboración de la orden para que sea posterior a la fecha de cierre.", Program.TituloSistema)
                If Editando = False Then
                    Exit Sub
                End If
            ElseIf _OrdenOFSelected.VigenciaHasta < _OrdenOFSelected.FechaOrden.Date Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de la orden no puede ser mayor a la fecha de vigencia." & vbNewLine & vbNewLine & "Por favor modifique la fecha de elaboración o la fecha de vigencia de la orden.", Program.TituloSistema)
                IsBusy = False
                Exit Sub

            End If

            'IsBusy = True

            dcProxy.ValidarFechas.Clear()
            If pintDias <= 0 Then
                ' Calcular los días al vencimiento de la orden a partir de la fecha de elaboración y vencimiento
                If pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO Then
                    If plogGuardarOrden Then
                        'dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenOFSelected.FechaEmision, _OrdenOFSelected.FechaVencimiento, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    Else
                        'dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenOFSelected.FechaEmision, _OrdenOFSelected.FechaVencimiento, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    End If
                Else
                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenOFSelected.FechaOrden.Date, _OrdenOFSelected.VigenciaHasta, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    Else
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenOFSelected.FechaOrden.Date, _OrdenOFSelected.VigenciaHasta, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    End If
                End If
            Else
                ' Calcular la fecha de vencimiento de la orden a partir de la fecha de elaboración y los días al vencimiento
                If pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO Then
                    If plogGuardarOrden Then
                        'dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenOFSelected.FechaEmision, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    Else
                        'dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenOFSelected.FechaEmision, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    End If
                Else
                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenOFSelected.FechaOrden.Date, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    Else
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenOFSelected.FechaOrden.Date, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    End If
                End If
            End If
        Catch ex As Exception
            mlogRecalcularFechas = True 'CCM20120305
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular los días al vencimiento", Me.ToString(), "calcularDiasOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa un nuevo objeto de tipo orden con los valores por defecto para iniciar el ingreso de una nueva orden. El ingreso de una nueva orden se da
    ''' porque el usuario utilice la funcionalidad para crear una nueva orden desde cero o porque duplique una existente.
    ''' </summary>
    ''' <param name="pobjDatosOrden">Datos por defecto para inicalizar la orden</param>
    ''' <param name="plogDuplicar">Si es verdadero indica que se está ducplicando la orden actual, de lo contrario se ingresa una orden desde cero</param>
    ''' <param name="plogCompra">Si es verdadero se ingresa una nueva orden de compra, si es falso una orden de venta</param>
    ''' <remarks>
    ''' Algunos valores para la inicialización de la nueva orden no se toman de la orden recibida como parámetro, sino que siempre se inicializan 
    ''' siempre de la orden por defecto.
    ''' </remarks>
    Private Sub configurarNuevaOrden(ByVal pobjDatosOrden As OyDOrdenes.OrdenesOF, ByVal plogDuplicar As Boolean, ByVal plogCompra As System.Nullable(Of Boolean))

        Dim intDias As Integer = 0

        Try
            mlogRecalcularFechas = False ' 'CCM20120305 - Desactivar cálculo de fechas 

            '// Indicar que se está ingresando una nueva orden para no ejecutar algunas consultas y ejecutar otras que son solamente para el ingreso
            mstrAccionOrden = MSTR_MC_ACCION_INGRESAR

            Dim NewOrden As New OyDOrdenes.OrdenesOF

            If plogDuplicar Then
                NewOrden.Tipo = pobjDatosOrden.Tipo
            Else
                ComitenteSeleccionado = Nothing
                NemotecnicoSeleccionado = Nothing

                ComitenteOrden = String.Empty
                NemotecnicoOrden = String.Empty

                If plogCompra Is Nothing Then
                    NewOrden.Tipo = Nothing
                ElseIf plogCompra Then
                    NewOrden.Tipo = TiposOrden.C.ToString
                Else
                    NewOrden.Tipo = TiposOrden.V.ToString
                End If
            End If

            NewOrden.FechaOrden = Now()
            NewOrden.FechaEstado = Now()
            NewOrden.FechaSistema = Now()

            NewOrden.VigenciaHasta = OrdenOFPorDefecto.VigenciaHasta
            If plogDuplicar Then
                NewOrden.Objeto = pobjDatosOrden.Objeto
                NewOrden.Mercado = pobjDatosOrden.Mercado
                NewOrden.FormaPago = pobjDatosOrden.FormaPago
            Else
                NewOrden.Objeto = ""
                If Me.ClaseOrden = ClasesOrden.C Then
                    NewOrden.Mercado = RF_MERCADO_SECUNDARIO
                    Me.HabilitarMercado = True
                Else
                    NewOrden.Mercado = Nothing
                End If
                NewOrden.FormaPago = mstrFormaPagoPorDef
            End If

            'SLB 20121126
            NewOrden.FechaActualizacion = Now()

            NewOrden.Clase = pobjDatosOrden.Clase
            NewOrden.IDComitente = pobjDatosOrden.IDComitente
            NewOrden.IDOrdenante = pobjDatosOrden.IDOrdenante
            NewOrden.Nemotecnico = pobjDatosOrden.Nemotecnico
            NewOrden.UBICACIONTITULO = pobjDatosOrden.UBICACIONTITULO
            NewOrden.CuentaDeposito = pobjDatosOrden.CuentaDeposito
            NewOrden.Cantidad = pobjDatosOrden.Cantidad
            SaldoOrden = pobjDatosOrden.Cantidad
            NewOrden.FechaEmision = pobjDatosOrden.FechaEmision
            NewOrden.FechaVencimiento = pobjDatosOrden.FechaVencimiento
            NewOrden.TasaNominal = pobjDatosOrden.TasaNominal
            NewOrden.TasaInicial = pobjDatosOrden.TasaInicial
            NewOrden.Modalidad = pobjDatosOrden.Modalidad
            'NewOrden.Mercado = pobjDatosOrden.Mercado
            NewOrden.EfectivaInferior = pobjDatosOrden.EfectivaInferior
            NewOrden.EfectivaSuperior = pobjDatosOrden.EfectivaSuperior
            NewOrden.DiasVencimientoInferior = pobjDatosOrden.DiasVencimientoInferior
            NewOrden.DiasVencimientoSuperior = pobjDatosOrden.DiasVencimientoSuperior
            'NewOrden.Version = pobjDatosOrden.Version
            NewOrden.Version = 0
            NewOrden.Ordinaria = pobjDatosOrden.Ordinaria
            'NewOrden.Objeto = pobjDatosOrden.Objeto
            NewOrden.Repo = pobjDatosOrden.Repo
            NewOrden.CondicionesNegociacion = pobjDatosOrden.CondicionesNegociacion
            NewOrden.Instrucciones = pobjDatosOrden.Instrucciones
            NewOrden.Notas = pobjDatosOrden.Notas
            NewOrden.TipoInversion = pobjDatosOrden.TipoInversion
            'NewOrden.ConsecutivoSwap = pobjDatosOrden.ConsecutivoSwap
            NewOrden.ComisionPactada = pobjDatosOrden.ComisionPactada
            NewOrden.Precio = pobjDatosOrden.Precio
            NewOrden.PrecioInferior = pobjDatosOrden.PrecioInferior
            NewOrden.DividendoCompra = pobjDatosOrden.DividendoCompra
            NewOrden.EnPesos = pobjDatosOrden.EnPesos
            NewOrden.Renovacion = pobjDatosOrden.Renovacion
            NewOrden.ComisionPactada = pobjDatosOrden.ComisionPactada

            '/ Estos campos siempre se inicializan con los por defecto
            NewOrden.DescripcionOrden = OrdenOFPorDefecto.DescripcionOrden
            NewOrden.NombreCliente = OrdenOFPorDefecto.NombreCliente
            NewOrden.Estado = OrdenOFPorDefecto.Estado

            '/ CCM20120305: Para acciones esto campo se inicializan con el por defecto o el del parámetro OYDNet_ORDEN_TIPO_LIMITE_DEFAULT
            If pobjDatosOrden.Clase = ClasesOrden.A.ToString Then
                If Not mstrTipoLimitePorDef.Trim().Equals(String.Empty) And (Not plogDuplicar) Then
                    NewOrden.TipoLimite = mstrTipoLimitePorDef.Trim()
                Else
                    NewOrden.TipoLimite = pobjDatosOrden.TipoLimite
                End If
            Else
                NewOrden.TipoLimite = pobjDatosOrden.TipoLimite
            End If

            '/ CCM20120305: Para acciones estos campos se inicializan con los por defecto o se mantienen los de la orden según parámetro OYDNet_ORDEN_DUPLICAR_DATOS_PRECIO_ACC
            If pobjDatosOrden.Clase = ClasesOrden.A.ToString Then
                If mlogDuplicarDatosPrecio Then
                    NewOrden.Precio = pobjDatosOrden.Precio
                Else
                    NewOrden.Precio = OrdenOFPorDefecto.Precio
                End If
            Else
                NewOrden.Precio = pobjDatosOrden.Precio
            End If

            NewOrden.Usuario = Program.Usuario

            '// Si se está duplicando la orden se activa este indicador para no borrar la lista de receptores y beneficiarios
            mlogDuplicarOrden = plogDuplicar

            OrdenOFAnterior = OrdenOFSelected
            OrdenOFSelected = NewOrden

            'Actualizar los días de vigencia. Se suma uno para incluir la fecha de la orden y la de vigencia
            '_mintDiasVigencia = calcularDiasRango(Me._OrdenSelected.FechaOrden, Me._OrdenSelected.VigenciaHasta)

            'SLB20130621
            Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)

            MyBase.CambioItem("Ordenes")

            DeshabilitarBotones()


            '// Se desactiva el indicador de cuplicar orden
            mlogDuplicarOrden = False

            'ActivarDuplicarOrden = False
            'ActivarEnvioSAE = False
            'ActivarSeleccionTipoOrden = False

            TabSeleccionadoGeneral = 0

            habilitarFechaElaboracion = _mlogHabilitarFechaElaboracion

            Editando = True
            EditandoDetalle = False

            MyBase.CambioItem("Editando")

            'SLB
            _strObjetoAnterior = Me._OrdenOFSelected.Objeto
            _strObjetoAnteriorValidar = " "

            If plogDuplicar And Me.ClaseOrden = ClasesOrden.C Then
                Select Case pobjDatosOrden.Objeto
                    Case RF_Simulatena_Salida, RF_Simulatena_Regreso
                        'Case "1", "2", "SI"
                        Me.HabilitarMercado = False
                    Case RF_TTV_Regreso, RF_TTV_Salida, RF_REPO
                        'Case "RP", "3", "4"
                        Me.HabilitarMercado = False
                        Me.OrdenOFSelected.Repo = True
                    Case Else
                        Me.HabilitarMercado = True
                End Select

            End If

            logRegistroLibroOrdenes = False
            dblValorCantidadLibroOrden = 0

            If visNavegando = "Collapsed" Then
                MyBase.CambiarFormulario_Forma_Manual()
            End If

            SaldoOrden = 0

        Catch ex As Exception
            mlogRecalcularFechas = True 'CCM20120305. Esta variable controla el cálculo de días entre fechas y solamente se debe actualizar cuando finaliza el cálculo o por error
            mlogDuplicarOrden = False
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en configurar la nueva orden", Me.ToString(), "configurarNuevaOrden", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub NuevoRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            '// Presenta la forma para seleccionar si se genera una orden de venta o de compra
            'RaiseEvent seleccionarTipoOrden()
            If Not dcProxy.IsLoading() And Not dcProxy1.IsLoading() And Not mdcProxyUtilidad01.IsLoading() And Not mdcProxyUtilidad02.IsLoading Then
                generarNuevaCompra()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevoRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            IsBusy = True
            dcProxy.OrdenesOFs.Clear()
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.OrdenesOFFiltrarQuery(Me.ClaseOrden.ToString, TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, MSTR_ACCION_FILTRAR)
            Else
                dcProxy.Load(dcProxy.OrdenesOFFiltrarQuery(Me.ClaseOrden.ToString, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "")
            End If
            IsBusy = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", Me.ToString(), "Filtrar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaOrdenOF(Me)
        CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Dim strMsg As String = String.Empty
        Dim strAno As String = String.Empty

        Try
            If Not (IsNothing(cb.Tipo) And IsNothing(cb.NroOrden) And IsNothing(cb.Version) And IsNothing(cb.IDComitente) And IsNothing(cb.IDOrdenante) And
                        IsNothing(cb.FechaOrden) And IsNothing(cb.Nemotecnico) And IsNothing(cb.FormaPago) And IsNothing(cb.Objeto) And IsNothing(cb.CondicionesNegociacion) And
                        cb.TipoInversion <> "  " And IsNothing(cb.TipoTransaccion) And IsNothing(cb.TipoLimite) And IsNothing(cb.VigenciaHasta) And IsNothing(cb.Ejecucion) And IsNothing(cb.Duracion) And IsNothing(cb.Estado) And
                        IsNothing(cb.IdBolsa)) Then

                If Not IsNothing(cb.NroOrden) Then
                    If Versioned.IsNumeric(cb.NroOrden) = False Then
                        strMsg = "El número de la orden debe ser numérico."
                    ElseIf cb.NroOrden <= 0 Then
                        strMsg = "El número de la orden debe ser mayor que cero."
                    End If

                    If Not strMsg.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Exit Sub
                    Else
                        If IsNothing(cb.AnoOrden) Then
                            strAno = Now.Year()
                        Else
                            strAno = cb.AnoOrden.ToString
                        End If
                        cb.NroOrden = strAno & Right("000000" & cb.NroOrden.ToString, 6)
                    End If
                End If

                If Not IsNothing(cb.AnoOrden) And IsNothing(cb.NroOrden) Then
                    cb.NroOrden = cb.AnoOrden
                End If

                If Not IsNothing(cb.Version) Then
                    If Versioned.IsNumeric(cb.Version) = False Then
                        strMsg = "La versión de la orden debe ser numérico."
                    ElseIf cb.NroOrden < 0 Then
                        strMsg = "La versión de la orden debe ser mayor o igual que cero."
                    End If

                    If Not strMsg.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Exit Sub
                    End If
                End If

                ErrorForma = ""
                dcProxy.OrdenesOFs.Clear()
                OrdenOFAnterior = Nothing
                IsBusy = True
                DescripcionFiltroVM = "" 'PENDIENTE DESCRIPCIÓN FILTRO
                cb.Clase = _mstrClaseOrden.ToString()
                dcProxy.Load(dcProxy.OrdenesOFConsultarQuery(cb.Clase, cb.Tipo, cb.NroOrden, cb.Version, cb.IDComitente, cb.IDOrdenante, cb.FechaOrden, cb.FormaPago, cb.Objeto,
                                                            cb.CondicionesNegociacion, cb.TipoInversion, cb.TipoLimite, cb.VigenciaHasta, cb.Estado,
                                                            Program.Usuario(), Program.HashConexion), AddressOf TerminoTraerOrdenes, MSTR_ACCION_BUSCAR)
                MyBase.ConfirmarBuscar()
            End If
        Catch ex As Exception
            cb = New CamposBusquedaOrdenOF(Me)
            CambioItem("cb")
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub guardarOrden(Optional ByVal YaValidoServidor As Boolean = False)

        Dim strLiqAsociadas As String = String.Empty
        Dim strAccion As String = MSTR_MC_ACCION_ACTUALIZAR

        Try
            'SBL20130613 Validaciones de Órdenes en el servidor.
            'If Not YaValidoServidor Then
            '    IsBusy = False
            '    'dcProxy1.tblRespuestaValidaciones.Clear()
            '    'dcProxy1.Load(dcProxy1.ValidarIngresoOrden_BolsaQuery(_OrdenOFSelected.IDOrden, _OrdenOFSelected.NroOrden, _OrdenOFSelected.Tipo, _OrdenOFSelected.Clase, _
            '    '                                        _OrdenOFSelected.IDComitente, _OrdenOFSelected.FechaOrden, _OrdenOFSelected.Ordinaria, _OrdenOFSelected.Objeto, _
            '    '                                        _OrdenOFSelected.Cantidad, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarIngresoOrden, "Validar")
            '    Exit Sub
            'End If

            If Not ListaOrdenes.Contains(OrdenOFSelected) Then
                ListaOrdenes.Add(OrdenOFSelected)
                strAccion = MSTR_MC_ACCION_INGRESAR
            End If

            'strAccion = MSTR_MC_ACCION_INGRESAR
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, strAccion)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la iniciar el proceso de actualización de la orden", Me.ToString(), "guardarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private _ListaResultadoValidacion As List(Of OyDOrdenes.tblRespuestaValidaciones)
    Public Property ListaResultadoValidacion() As List(Of OyDOrdenes.tblRespuestaValidaciones)
        Get
            Return _ListaResultadoValidacion
        End Get
        Set(ByVal value As List(Of OyDOrdenes.tblRespuestaValidaciones))
            _ListaResultadoValidacion = value
            MyBase.CambioItem("ListaResultadoValidacion")
        End Set
    End Property

    Private Sub TerminoValidarIngresoOrden(ByVal lo As LoadOperation(Of OyDOrdenes.tblRespuestaValidaciones))
        Try
            IsBusy = False
            If lo.HasError = False Then

                ListaResultadoValidacion = lo.Entities.ToList

                If ListaResultadoValidacion.Count > 0 Then
                    Dim logExitoso As Boolean = False
                    Dim logContinuaMostrandoMensaje As Boolean = False
                    Dim logRequiereConfirmacion As Boolean = False
                    Dim logRequiereJustificacion As Boolean = False
                    Dim logRequiereAprobacion As Boolean = False
                    Dim logConsultaListaJustificacion As Boolean = False
                    Dim logError As Boolean = False
                    Dim strMensajeExitoso As String = "La orden se actualizó correctamente."
                    Dim strMensajeError As String = "La orden no se pudo actualizar."

                    For Each li In ListaResultadoValidacion
                        If li.Exitoso Then
                            logExitoso = True
                            logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereJustificacion = False
                            logRequiereConfirmacion = False
                            logRequiereAprobacion = False
                            strMensajeExitoso = String.Format("{0}{1} {2}", strMensajeExitoso, vbCrLf, li.Mensaje)
                        ElseIf li.RequiereConfirmacion Then
                            logExitoso = False
                            logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereConfirmacion = True
                        ElseIf li.RequiereJustificacion Then
                            logExitoso = False
                            logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereJustificacion = True
                        ElseIf li.RequiereAprobacion Then
                            logExitoso = False
                            logError = False
                            logContinuaMostrandoMensaje = False
                            logRequiereAprobacion = True
                        ElseIf li.DetieneIngreso Then
                            logError = True
                            logExitoso = False
                            logRequiereJustificacion = False
                            logRequiereConfirmacion = False
                            logContinuaMostrandoMensaje = False
                            logRequiereAprobacion = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)
                        Else
                            logError = True
                            logExitoso = False
                            logRequiereJustificacion = False
                            logRequiereConfirmacion = False
                            logContinuaMostrandoMensaje = False
                            logRequiereAprobacion = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)
                        End If

                    Next

                    If logExitoso And _
                        logContinuaMostrandoMensaje = False And _
                        logRequiereConfirmacion = False And _
                        logRequiereJustificacion = False And _
                        logRequiereAprobacion = False And _
                        logError = False Then

                        guardarOrden(True)
                        'strMensajeExitoso = strMensajeExitoso.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        'strMensajeExitoso = strMensajeExitoso.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        'strMensajeExitoso = strMensajeExitoso.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        'strMensajeExitoso = strMensajeExitoso.Replace("--", String.Format("{0}", vbCrLf))
                    ElseIf logError Then
                        strMensajeError = strMensajeError.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("--", String.Format("{0}", vbCrLf))

                        mostrarMensaje(strMensajeError, "Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                    End If
                Else
                    guardarOrden(True)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngresoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub


    Private Sub validarOrden()
        Dim strMsg As String = String.Empty
        Dim strReceptores As String = String.Empty
        Dim strLiqProbables As String = String.Empty
        Dim strPagosOrdenes As String = String.Empty
        Dim strInstruccionesOrdenes As String = String.Empty
        Dim strAdicionalesOrdenes As String = String.Empty
        Dim dblPorcentajeComision As Double = 0
        Dim intNroLideres As Integer = 0 'CCM20120305 - Validar que exista un receptor líder seleccionado
        Dim lstReceptores As List(Of String) 'CCM20120305
        Dim lstReceptoresRep As List(Of Object) 'CCM20120305
        Dim strMsgPago As String = String.Empty

        Try
            ErrorForma = ""

            'Me.TabSeleccionadoGeneral = OrdenTabs.LEO

            If ComitenteSeleccionado Is Nothing Then
                strMsg &= "+ Verifique el comitente seleccionado porque no se han podido validar sus datos." & vbNewLine
            ElseIf LTrim(RTrim(_OrdenOFSelected.IDComitente)) <> strIDComitentePermitidoOF Then
                strMsg &= "+ El comitente solo puede ser la firma comisionista." & vbNewLine
            End If

            If NemotecnicoSeleccionado Is Nothing Then
                strMsg &= "+ Verifique la especie seleccionada porque no se han podido validar sus datos." & vbNewLine
            End If

            If OrdenanteSeleccionado Is Nothing Then
                strMsg &= "+ Verifique el ordenante seleccionado porque no se han podido validar sus datos." & vbNewLine
            End If

            If IsNothing(_OrdenOFSelected.Tipo) Or String.IsNullOrEmpty(_OrdenOFSelected.Tipo) Then
                strMsg &= "+ Debe ingresar el Tipo de orden (Compra-Venta)." & vbNewLine
            End If

            If IsNothing(_OrdenOFSelected.FechaOrden) Then
                strMsg &= "+ Debe de ingresar la Fecha de elaboración." & vbNewLine
            End If

            If IsNothing(_OrdenOFSelected.VigenciaHasta) Then
                strMsg &= "+ Debe de ingresar la Fecha de vigencia." & vbNewLine
            End If

            If IsNothing(_OrdenOFSelected.FormaPago) Or String.IsNullOrEmpty(_OrdenOFSelected.FormaPago) Then
                strMsg &= "+ Debe ingresar la forma de pago." & vbNewLine
            End If

            If IsNothing(_OrdenOFSelected.CondicionesNegociacion) Or String.IsNullOrEmpty(_OrdenOFSelected.CondicionesNegociacion) Then
                strMsg &= "+ Debe ingresar la condición de negociación." & vbNewLine
            End If

            If IsNothing(_OrdenOFSelected.Objeto) Then
                strMsg &= "+ Debe ingresar la clasificación de la orden." & vbNewLine
            End If

            If IsNothing(_OrdenOFSelected.TipoLimite) Or String.IsNullOrEmpty(_OrdenOFSelected.TipoLimite) Then
                strMsg &= "+ Debe ingresar el Tipo límite - Naturaleza." & vbNewLine
            End If

            If IsNothing(_OrdenOFSelected.Cantidad) Or _OrdenOFSelected.Cantidad = 0 Then
                strMsg &= "+ Debe ingresar la cantidad." & vbNewLine
            End If

            If _OrdenOFSelected.Cantidad < dblValorCantidadLibroOrden And logRegistroLibroOrdenes Then
                strMsg &= "+ No se puede modificar la cantidad por un valor inferior." & vbNewLine
            End If

            'Santiago Vergara - Marzo 20/2014 - Se migra cambio en la validación de cuenta depósito 
            If Not IsNothing(_mobjCtaDepositoSeleccionada) Then
                _OrdenOFSelected.CuentaDeposito = CtaDepositoSeleccionada.NroCuentaDeposito
                _OrdenOFSelected.UBICACIONTITULO = CtaDepositoSeleccionada.Deposito
            End If

            'Santiago Vergara - Marzo 20/2014 - Se migra cambio en la validación de cuenta depósito 
            If _OrdenOFSelected.Tipo = TiposOrden.V.ToString Or (_OrdenOFSelected.Tipo = TiposOrden.C.ToString And mlogHABILITAR_DEPOSITO_VENTASOF) Then

                If mlogCUENTADEPOSITOORDENES Then

                    If String.IsNullOrEmpty(_OrdenOFSelected.UBICACIONTITULO) Then
                        strMsg &= "+ La ubicación del título es requerida." & vbNewLine
                    ElseIf _OrdenOFSelected.UBICACIONTITULO <> "F" And _OrdenOFSelected.UBICACIONTITULO <> "X" And IsNothing(_OrdenOFSelected.CuentaDeposito) Then
                        strMsg &= "+ La cuenta del depósito es requerida." & vbNewLine
                    End If

                End If

            End If

            If _mobjOrdenanteSeleccionado Is Nothing Then
                strMsg &= "+ Debe seleccionar el ordenante de la orden." & vbNewLine
            Else
                'SLB Validación para saber si el Ordenante de la orden se encuentra Inactivo cuando se esta modificando 
                If _mobjOrdenanteSeleccionado.IdOrdenante = "-1" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El ordenante se encuentra Inactivo, seleccione otro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                _OrdenOFSelected.IDOrdenante = OrdenanteSeleccionado.IdOrdenante
            End If


            If Not strMsg.Equals(String.Empty) Then
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Me._OrdenOFSelected.FechaOrden() >= Now() Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de elaboración de la orden no puede ser mayor a la fecha actual.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Me.ClaseOrden = ClasesOrden.A Then
                strMsg = String.Empty

                If _OrdenOFSelected.Precio < _OrdenOFSelected.PrecioInferior Then
                    strMsg &= "+ El valor del precio no puede ser menor que el precio inferior." & vbNewLine
                End If

                If Not strMsg.Equals(String.Empty) Then
                    Me.TabSeleccionadoGeneral = 0
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                'SLB 20130111
                If Me._OrdenOFSelected.Objeto <> "IC" Then
                    If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                        A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

            Else 'Validaciones de Renta Fija

                If IsNothing(_OrdenOFSelected.Mercado) Or String.IsNullOrEmpty(_OrdenOFSelected.Mercado) Then
                    strMsg &= "+ Debe ingresar el Mercado." & vbNewLine
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Me._OrdenOFSelected.Objeto <> "IC" Or Me.ComitenteSeleccionado.CodCategoria <> "2" Then
                    If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                        A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado  y el cliente debe ser Inversionista Profesional", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

                If _OrdenOFSelected.Tipo = TiposOrden.R.ToString() Or _OrdenOFSelected.Tipo = TiposOrden.S.ToString() Then
                    strMsg = "Los tipos de orden Recompra y Reventa son generadas en forma automática por el sistema cuando la orden lo requiere (simultáneas, repos, etc.)." & vbNewLine & vbNewLine & "Debe cambiar el tipo de la orden a compra o venta."
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Me._OrdenOFSelected.Objeto = "  " Then
                    strMsg = "Debe seleccionar la clasificación de la orden"
                End If

                If Me._OrdenOFSelected.Objeto = RF_TTV_Regreso Then
                    strMsg &= "No se permiten hacer ordenes de tipo TTV de regreso, por favor cree primero la orden de salida" & vbNewLine
                End If

                If Me._OrdenOFSelected.Objeto = RF_Simulatena_Regreso Then
                    strMsg &= "No se permiten hacer ordenes de tipo Simultanea de regreso, por favor cree primero la orden de salida" & vbNewLine
                End If

                If _OrdenOFSelected.Mercado = "P" And _OrdenOFSelected.Objeto <> "MP" And HabilitarMercado Then
                    strMsg &= "Cuando en mercado se selecciona Primario la clasificación debe ser Mercado Primario" & vbNewLine
                End If

                If _OrdenOFSelected.EfectivaSuperior < _OrdenOFSelected.EfectivaInferior Then
                    strMsg &= "+ El valor de la efectiva superior no puede ser menor que el valor de la efectiva inferior." & vbNewLine
                End If

                If _OrdenOFSelected.DiasVencimientoSuperior < _OrdenOFSelected.DiasVencimientoInferior Then
                    strMsg &= "+ El valor de los días de vencimiento superior no puede ser menor que el valor de los días de vencimiento inferior." & vbNewLine
                End If

                If _strObjetoAnteriorValidar <> " " Then
                    If _strObjetoAnteriorValidar <> _OrdenOFSelected.Objeto Then
                        Select Case _strObjetoAnteriorValidar
                            Case RF_Simulatena_Salida, RF_Simulatena_Regreso, RF_TTV_Salida, RF_TTV_Regreso, RF_REPO
                                'Case "1", "2", "SI", "3", Program.RF_TTV_Regreso, "RP"
                                If (_OrdenOFSelected.Objeto <> RF_Simulatena_Salida Or _OrdenOFSelected.Objeto <> RF_Simulatena_Regreso Or _OrdenOFSelected.Objeto <> "SI" Or _OrdenOFSelected.Objeto <> RF_TTV_Salida _
                                    Or _OrdenOFSelected.Objeto <> RF_TTV_Regreso Or _OrdenOFSelected.Objeto <> RF_REPO) Then
                                    strMsg &= "EL tipo de Clasificación de esta Orden no se puede modificar debido a que tiene una punta asociada (Orden No. " & CStr(_OrdenOFSelected.NroOrden + 1) & ")" & vbNewLine
                                End If
                            Case Else
                                If (_OrdenOFSelected.Objeto = RF_Simulatena_Salida Or _OrdenOFSelected.Objeto = RF_Simulatena_Regreso Or _OrdenOFSelected.Objeto = "SI" Or _OrdenOFSelected.Objeto = RF_TTV_Salida _
                                    Or _OrdenOFSelected.Objeto = RF_TTV_Regreso Or _OrdenOFSelected.Objeto = RF_REPO) Then
                                    strMsg &= "EL tipo de Clasificación de esta Orden no se puede modificar debido a que no tiene una punta asociada." & vbNewLine
                                End If
                        End Select

                    End If
                End If

                'SLB20130111
                If Me._OrdenOFSelected.Objeto <> "IC" Or Me.ComitenteSeleccionado.CodCategoria <> "2" Then
                    If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                        A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado  y el cliente debe ser Inversionista Profesional.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Me.TabSeleccionadoGeneral = 0
                    Exit Sub
                End If
            End If


            lstReceptores = New List(Of String) 'CCM20120305 
            lstReceptoresRep = New List(Of Object) 'CCM20120305 

            strReceptores = "<receptores>"

            If _ListaReceptorOrdenesOF.Count > 0 Then
                For Each obj In _ListaReceptorOrdenesOF
                    If Not lstReceptores.Contains(obj.IDReceptor) And (obj.Porcentaje > 0 Or obj.Lider) Then 'CCM20120305 - Validar receptores duplicados y solamente considerar el primero
                        lstReceptores.Add(obj.IDReceptor)
                        dblPorcentajeComision += obj.Porcentaje

                        If IsNothing(obj.IDReceptor) Or String.IsNullOrEmpty(obj.IDReceptor) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el receptor", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            If Me.ClaseOrden = ClasesOrden.C Then
                                Me.TabSeleccionadoGeneral = 1
                            End If
                            Exit Sub
                        End If

                        If obj.Lider Then 'CCM20120305 - Validar que solamente exista un lider
                            intNroLideres += 1
                        End If

                        strReceptores &= "<receptor Id=""" & obj.IDReceptor & """ Lider=""" & IIf(obj.Lider, "1", "0") & """ Porcentaje=""" & obj.Porcentaje.ToString() & """ />"
                    Else
                        lstReceptoresRep.Add(obj) 'CCM20120305 - Guardar receptores duplicados
                    End If
                Next
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar los receptores de la orden", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.TabSeleccionadoGeneral = 1
                Exit Sub
            End If

            strReceptores &= "</receptores>"

            If dblPorcentajeComision <> 100.0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El porcentaje de distribución de comisión entre los receptores de la orden debe ser cien (100). En este momento está en " & dblPorcentajeComision.ToString("N0"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.TabSeleccionadoGeneral = 1
                Exit Sub
            End If

            'CCM20120305 - Validar que solamente exista un lider y que por lo menos exista uno seleccionado
            If intNroLideres = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Se debe seleccionar un receptor líder para la distribución de la comisión.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.TabSeleccionadoGeneral = 1
                Exit Sub
            ElseIf intNroLideres > 1 Then
                A2Utilidades.Mensajes.mostrarMensaje("Solamente puede seleccionarse un receptor líder para la distribución de la comisión. En este momento hay " & intNroLideres.ToString("N0") & " receptores seleccionados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.TabSeleccionadoGeneral = 1
                Exit Sub
            End If

            'CCM20120305 - Eliminar receptores duplicados (se deja solamnete el primero en el grid)
            If lstReceptoresRep.Count > 0 Then
                For Each obj In lstReceptoresRep
                    ListaReceptorOrdenesOF.Remove(obj)
                Next
            End If

            _OrdenOFSelected.ReceptoresXML = strReceptores

            'Verifica sí se debe de registrar el libro de ordenes.

            _OrdenOFSelected.RegistrarLibroOrdenes = logRegistroLibroOrdenes

            guardarOrden()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la orden", Me.ToString(), "validarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            IsBusy = True
            mlogRecalcularFechas = False 'CCM20120305
            VerificarFechaValida(True)   'SLB20120704 Primero se Valida si la Fecha es Valida.
            'calcularDiasOrden(MSTR_CALCULAR_DIAS_ORDEN, -1, True)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de la orden", Me.ToString(), "ActualizarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        Try
            If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(Me._OrdenOFSelected) Then
                validarEstadoOrden(MSTR_ACCION_EDITAR)
            End If
            'TabSeleccionadoGeneral = OrdenTabs.LEO
            habilitarFechaElaboracion = False
            _mlogOpcionActiva = False
            MyBase.CambioItem("OpcionActiva")
        Catch ex As Exception
            Me.IsBusy = False
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            IsBusy = True
            ErrorForma = ""
            mstrAccionOrden = String.Empty
            If Not IsNothing(_OrdenOFSelected) Then
                Editando = False
                dcProxy.RejectChanges()
                dcProxy1.RejectChanges()
                mdcProxyUtilidad01.RejectChanges()

                'Editando = False
                EditandoDetalle = True

                If _OrdenOFSelected.EntityState = EntityState.Detached Then
                    OrdenOFSelected = OrdenOFAnterior
                End If

                habilitarFechaElaboracion = False

                'ValidarUsuarioOperador("cancelar")
                HabilitarBotones()

                '// Si se cancela se asignan los valores anteriores de los buscadores
                If ListaOrdenes.Count > 0 Then
                    ComitenteSeleccionado = _mobjComitenteSeleccionadoAntes
                    buscarOrdenanteSeleccionado()
                    buscarCuentaDepositoSeleccionada()
                    NemotecnicoSeleccionado = _mobjNemotecnicoSeleccionadoAntes
                End If

                logRegistroLibroOrdenes = False
                dblValorCantidadLibroOrden = 0
                HabilitarMercado = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If IsNothing(_OrdenOFSelected) Then
                Exit Sub
            End If
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_OrdenOFSelected) Then
                validarEstadoOrden(MSTR_ACCION_ANULAR)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular la orden", Me.ToString(), "BorrarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        MyBase.CancelarBuscar()
    End Sub

    Public Overrides Sub QuitarFiltro()
        MyBase.QuitarFiltro()
    End Sub

    ''' <summary>
    ''' Validar si el estado de la orden permite que la orden sea modificada o anulada. Se tiene en cuenta el estado (campo strEstado), el estodo LEO y el estado SAE.
    ''' </summary>
    ''' <param name="pstrAccion">Indica si se valida la edición o anlación de la orden</param>
    Private Sub validarEstadoOrden(ByVal pstrAccion As String)

        Dim strMsg As String = String.Empty

        If _mlogFechaCierreSistema Then
            MyBase.RetornarValorEdicionNavegacion()
            CambiarAForma()
            A2Utilidades.Mensajes.mostrarMensaje("La fecha de la vigencia no puede ser menor a la fecha de cierre del sistema (" & mdtmFechaCierreSistema.ToLongDateString() & ")." & vbNewLine & vbNewLine & "Por favor modifique la fecha de vigencia de la orden para que sea posterior a la fecha de cierre.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '_mlogFechaCierreSistema = False
            Exit Sub
        End If

        If Me._OrdenOFSelected.Modificable = False Then
            MyBase.RetornarValorEdicionNavegacion()

            If pstrAccion = MSTR_ACCION_EDITAR Then
                strMsg = "Solamente las órdenes en estado pendiente pueden ser modificadas"
            Else
                strMsg = "Solamente las órdenes en estado pendiente pueden ser anuladas"
            End If

            A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

        Else
            'SLB (Inicio) No permite la edición de las ordenes Simultaneas, TTV y REPO de Regreso.
            If Me.ClaseOrden = ClasesOrden.C Then
                If _OrdenOFSelected.Objeto = RF_Simulatena_Regreso Then
                    strMsg = "La Orden No. " + _OrdenOFSelected.NroOrden.ToString + " es una Simultanea de Regreso no puede ser modificada o borrada. Modifique la Orden No. " + (_OrdenOFSelected.NroOrden - 1).ToString + " que es la correspondiente Simúltanea de Salida."
                End If

                If _OrdenOFSelected.Repo And _OrdenOFSelected.Objeto = RF_TTV_Regreso Then
                    strMsg = "La Orden No. " + _OrdenOFSelected.NroOrden.ToString + " es una TTV de Regreso no puede ser modificada o borrada. Modifique la Orden No. " + (_OrdenOFSelected.NroOrden - 1).ToString + " que es la correspondiente TTV de Salida."
                End If

                If _OrdenOFSelected.Repo And _OrdenOFSelected.Objeto = RF_REPO Then
                    If _OrdenOFSelected.Tipo = TiposOrden.S.ToString Or _OrdenOFSelected.Tipo = TiposOrden.R.ToString Then
                        strMsg = "La Orden No. " + _OrdenOFSelected.NroOrden.ToString + " es una REPO de Regreso no puede ser modificada o borrada. Modifique la Orden No. " + (_OrdenOFSelected.NroOrden - 1).ToString + " que es la correspondiente REPO de Salida."
                    End If
                End If
            End If
            'SLB (Fin)

            If strMsg.Equals(String.Empty) Then
                Me.IsBusy = True
                validarOrdenModificable(pstrAccion)
            Else
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        End If

    End Sub

    ''' <summary>
    ''' Valida el estado de la orden en el servidor
    ''' </summary>
    ''' <param name="pstrAccion">Indica si se valida la edición o anulación de la orden</param>
    Private Sub validarOrdenModificable(ByVal pstrAccion As String)
        Try
            dcProxy.OrdenOFModificables.Clear()
            dcProxy.Load(dcProxy.verificarOrdenModificableOFQuery(Me._OrdenOFSelected.Clase, Me._OrdenOFSelected.Tipo, Me._OrdenOFSelected.NroOrden, Me._OrdenOFSelected.Version, MSTR_MODULO_OYD_ORDENES, pstrAccion, Program.Usuario, Program.HashConexion), AddressOf TerminoVerificarOrdenModificable, pstrAccion)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la validación del estado de la orden", Me.ToString(), "ValidarOrdenModificable", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Me.IsBusy = False
        End Try
    End Sub

    Private Function NombreCategoriaEnDiccionario(ByVal pstrCategoria As String) As String
        Return String.Format("{0}_{1}", NombreInicioCombos, pstrCategoria)
    End Function

#End Region

#Region "Eventos de acciones adicionales sobre la orden"

    Public Sub duplicarOrden()
        Try
            If IsNothing(_OrdenOFSelected) Then
                Exit Sub
            End If
            mostrarMensajePregunta("¿Esta seguro de Duplicar la Orden?", _
                                   Program.TituloSistema, _
                                   "DUPLICARORDEN", _
                                   AddressOf validarDuplicarOrden, False)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la duplicación de la orden", Me.ToString(), "duplicarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub RefrescarOrden()
        Try
            If Not Me.OrdenOFSelected Is Nothing Then
                cb = New CamposBusquedaOrdenOF(Me)
                cb.NroOrden = Right(Me.OrdenOFSelected.NroOrden, 6)
                cb.AnoOrden = Left(Me.OrdenOFSelected.NroOrden, 4)
                cb.Version = Me.OrdenOFSelected.Version
                cb.Clase = Me.OrdenOFSelected.Clase

                ConfirmarBuscar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta de actualización de la orden", Me.ToString(), "refrescarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub generarNuevaCompra()
        Try
            IsBusy = True
            configurarNuevaOrden(OrdenOFPorDefecto, False, True)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la generación de una nueva orden de venta", Me.ToString(), "generarNuevaCompra", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub generarNuevaVenta()
        Try
            IsBusy = True
            configurarNuevaOrden(OrdenOFPorDefecto, False, False)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la generación de una nueva orden de compra", Me.ToString(), "generarNuevaVenta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub imprimirOrden()
        Dim strParametros As String = String.Empty
        Dim strReporte As String = String.Empty
        Dim strNroVentana As String = String.Empty

        Try
            If Not _OrdenOFSelected Is Nothing And mstrAccionOrden = String.Empty Then
                If Application.Current.Resources.Contains("A2VReporteOrdenesRS") = False Then
                    A2Utilidades.Mensajes.mostrarMensaje("El reporte para imprimir la orden no está configurado", Program.TituloSistema)
                    Exit Sub
                End If

                strReporte = Application.Current.Resources("A2VReporteOrdenesRS").ToString.Trim()
                strParametros = "&pstrClaseOrden=" & _OrdenOFSelected.Clase & "&pintIdOrden=" & _OrdenOFSelected.NroOrden & "&pintVersion=" & _OrdenOFSelected.Version & "&pdblSaldo=" & Me.SaldoOrden()
                MostrarReporte(strParametros, Me.ToString, strReporte)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la impresión del reporte", Me.ToString(), "imprimirReporte", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Métodos para controlar cambio de campos asociados a buscadores"

    ''' <summary>
    ''' Buscar los datos del comitente que tiene asignada la orden.
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strIdComitente As String = String.Empty

        Try
            If Not Me.OrdenOFSelected Is Nothing Then
                If Not Me.ComitenteSeleccionado Is Nothing And pstrIdComitente.Equals(String.Empty) Then
                    strIdComitente = Me.ComitenteSeleccionado.IdComitente
                End If

                If Not strIdComitente.Equals(Me.OrdenOFSelected.IDComitente) Then
                    If pstrIdComitente.Trim.Equals(String.Empty) Then
                        strIdComitente = Me.OrdenOFSelected.IDComitente
                    Else
                        strIdComitente = pstrIdComitente
                    End If

                    If strIDComitentePermitidoOF <> LTrim(RTrim(strIdComitente)) And Editando Then
                        mostrarMensaje("El comitente solo puede ser la firma comisionista.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If

                    If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                        mdcProxyUtilidad01.BuscadorClientes.Clear()
                        mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del nemotecnico que tiene asignado la orden.
    ''' </summary>
    ''' <param name="pstrNemotecnico">Nemotécnico que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    Friend Sub buscarNemotecnico(Optional ByVal pstrNemotecnico As String = "")
        Dim strNemotecnico As String = String.Empty

        Try
            If Not Me.OrdenOFSelected Is Nothing Then
                If Not Me.NemotecnicoSeleccionado Is Nothing And pstrNemotecnico.Equals(String.Empty) Then
                    strNemotecnico = Me.NemotecnicoSeleccionado.Nemotecnico
                End If

                'If Not strNemotecnico.Equals(Me.OrdenOFSelected.Nemotecnico) Then
                If pstrNemotecnico.Trim.Equals(String.Empty) Then
                    strNemotecnico = Me.OrdenOFSelected.Nemotecnico
                Else
                    strNemotecnico = pstrNemotecnico
                End If
                mdcProxyUtilidad01.BuscadorEspecies.Clear()
                '-- CCM20120108: Incluir la clase para evitar que al digitar se traigan especies de una clase diferente a la de la orden
                strNemotecnico = System.Web.HttpUtility.UrlEncode(strNemotecnico)

                'SLB Cuando es Renta Fija se debe verificar si la clasificación es REPO o TTV.
                If Me.ClaseOrden = ClasesOrden.C Then
                    Select Case Me.OrdenOFSelected.Objeto
                        Case RF_TTV_Salida, RF_TTV_Regreso, RF_REPO
                            'Case "3", "4", "RP" '"1", "2","SI",
                            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarNemotecnicoEspecificoQuery("A", strNemotecnico, Program.Usuario, Program.HashConexion), AddressOf buscarNemotecnicoCompleted, "") 'Accion
                        Case Else
                            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarNemotecnicoEspecificoQuery("C", strNemotecnico, Program.Usuario, Program.HashConexion), AddressOf buscarNemotecnicoCompleted, "") 'Renta Fija
                    End Select
                Else
                    mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarNemotecnicoEspecificoQuery(Me.ClaseOrden.ToString(), strNemotecnico, Program.Usuario, Program.HashConexion), AddressOf buscarNemotecnicoCompleted, "")
                End If

                'End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del nemotécnico de la orden", Me.ToString(), "buscarNemotecnico", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del receptor toma que tiene asignado la orden.
    ''' </summary>
    ''' <param name="pstrIdItem">Receptor toma que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.OrdenOFSelected Is Nothing Then
                Select Case pstrTipoItem.ToLower()
                    Case "receptores"
                        pstrIdItem = pstrIdItem.Trim()
                    Case Else
                        logConsultar = False
                End Select

                If logConsultar Then
                    mdcProxyUtilidad01.BuscadorGenericos.Clear()
                    mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el buscador de " & pstrTipoItem, Me.ToString(), "buscarItem", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Actualizar el comitente de la orden con los datos del comitente recibido como parámetro
    ''' </summary>
    ''' <param name="pobjComitente">Comitente enviado como parámetro</param>
    Public Sub actualizarComitenteOrden(ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If strIDComitentePermitidoOF = LTrim(RTrim(pobjComitente.IdComitente)) Then
            Me.ComitenteSeleccionado = pobjComitente
        Else
            mostrarMensaje("El comitente solo puede ser la firma comisionista.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If
    End Sub

    ''' <summary>
    ''' Actualizar elemento buscado con los datos recibidos como parámetro
    ''' </summary>
    ''' <param name="pstrTipoItem">Tipo de objeto que se recibe</param>
    ''' <param name="pobjItem">Item enviado como parámetro</param>
    Public Sub actualizarItemOrden(ByVal pstrTipoItem As String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrTipoItem.ToLower
                Case "idreceptor"
                    Me.ReceptorOrdenesOFSelected.IDReceptor = pobjItem.IdItem
                    Me.ReceptorOrdenesOFSelected.Nombre = pobjItem.Nombre
            End Select
        End If
    End Sub

    ''' <summary>
    ''' Actualizar el nemotécnico de la orden con los datos del nemotecnico recibido como parámetro
    ''' </summary>
    ''' <param name="pobjNemotecnico">Nemotécnico enviado como parámetro</param>
    Public Sub actualizarNemotecnicoOrden(ByVal pobjNemotecnico As OYDUtilidades.BuscadorEspecies)
        Me.NemotecnicoSeleccionado = pobjNemotecnico
        If Me._OrdenOFSelected.Objeto <> "IC" And Me.ClaseOrden.ToString = ClasesOrden.A.ToString Then
            If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        End If

        If Me.ClaseOrden.ToString() = ClasesOrden.C.ToString() Then
            If Me._OrdenOFSelected.Objeto <> "IC" Or (Not IsNothing(Me.ComitenteSeleccionado) AndAlso Me.ComitenteSeleccionado.CodCategoria <> "2") Then
                If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                    A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado  y el cliente debe ser Inversionista Profesional", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        End If

    End Sub

#End Region

#Region "Tablas hijas"

#Region "Liquidaciones de la Orden"

    Private _ListaLiquidacionesOrdenOF As EntitySet(Of OyDOrdenes.LiquidacionesOrdenOF)
    Public Property LiquidacionesOrdenOF() As EntitySet(Of OyDOrdenes.LiquidacionesOrdenOF)
        Get
            Return (_ListaLiquidacionesOrdenOF)
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.LiquidacionesOrdenOF))
            _ListaLiquidacionesOrdenOF = value
            MyBase.CambioItem("LiquidacionesOrdenOF")
        End Set
    End Property

    Private _SaldoOrden As Double? = 0
    Public Property SaldoOrden() As Double?
        Get
            Return _SaldoOrden
        End Get
        Set(ByVal value As Double?)
            _SaldoOrden = value
            MyBase.CambioItem("SaldoOrden")
        End Set
    End Property


#End Region

#Region "Beneficarios"

    '******************************************************** BeneficiariosOrdenes 
    Private _ListaBeneficiariosOrdenesOF As EntitySet(Of OyDOrdenes.BeneficiarioOrdenesOF)
    Public Property ListaBeneficiariosOrdenesOF() As EntitySet(Of OyDOrdenes.BeneficiarioOrdenesOF)
        Get
            Return _ListaBeneficiariosOrdenesOF
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.BeneficiarioOrdenesOF))
            _ListaBeneficiariosOrdenesOF = value
            MyBase.CambioItem("ListaBeneficiariosOrdenesOF")
            MyBase.CambioItem("BeneficiariosOrdenesOFPaged")
        End Set
    End Property

    Public ReadOnly Property BeneficiariosOrdenesOFPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBeneficiariosOrdenesOF) Then
                Dim view = New PagedCollectionView(_ListaBeneficiariosOrdenesOF)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _BeneficiariosOrdenOFSelected As OyDOrdenes.BeneficiarioOrdenesOF
    Public Property BeneficiariosOrdenOFSelected() As OyDOrdenes.BeneficiarioOrdenesOF
        Get
            Return _BeneficiariosOrdenOFSelected
        End Get
        Set(ByVal value As OyDOrdenes.BeneficiarioOrdenesOF)
            _BeneficiariosOrdenOFSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("BeneficiariosOrdenOFSelected")
            End If
        End Set
    End Property

#End Region

#Region "Receptores"

    '******************************************************** ReceptorOrdenesOFes 
    Private _ListaReceptorOrdenesOF As EntitySet(Of OyDOrdenes.ReceptorOrdenesOF)
    Public Property ListaReceptorOrdenesOF() As EntitySet(Of OyDOrdenes.ReceptorOrdenesOF)
        Get
            Return _ListaReceptorOrdenesOF
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.ReceptorOrdenesOF))
            _ListaReceptorOrdenesOF = value
            If Not IsNothing(value) Then
                ReceptorOrdenesOFSelected = _ListaReceptorOrdenesOF.FirstOrDefault
            End If
            MyBase.CambioItem("ListaReceptorOrdenesOF")
            MyBase.CambioItem("ListaReceptorOrdenesOFPaged")
        End Set
    End Property

    Public ReadOnly Property ListaReceptorOrdenesOFPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaReceptorOrdenesOF) Then
                Dim view = New PagedCollectionView(_ListaReceptorOrdenesOF)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ReceptorOrdenesOFSelected As OyDOrdenes.ReceptorOrdenesOF
    Public Property ReceptorOrdenesOFSelected() As OyDOrdenes.ReceptorOrdenesOF
        Get
            Return _ReceptorOrdenesOFSelected
        End Get
        Set(ByVal value As OyDOrdenes.ReceptorOrdenesOF)
            If Not IsNothing(value) Then
                _ReceptorOrdenesOFSelected = value
                MyBase.CambioItem("ReceptorOrdenesOFSelected")
            End If
        End Set
    End Property

    'Private _ListaReceptorOrdenesOFesAnterior As New EntitySet(Of OyDOrdenes.ReceptorOrdenesOF)

#End Region

#Region "Adicionar Hijos"

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptorOrdenesOF"
                Dim NewReceptorOrdenesOF As New OyDOrdenes.ReceptorOrdenesOF

                NewReceptorOrdenesOF.ClaseOrden = Me._OrdenOFSelected.Clase
                NewReceptorOrdenesOF.TipoOrden = Me._OrdenOFSelected.Tipo
                NewReceptorOrdenesOF.NroOrden = Me._OrdenOFSelected.NroOrden
                NewReceptorOrdenesOF.Version = Me._OrdenOFSelected.Version
                NewReceptorOrdenesOF.FechaActualizacion = Now()
                NewReceptorOrdenesOF.Usuario = Program.Usuario
                NewReceptorOrdenesOF.Lider = False
                NewReceptorOrdenesOF.Porcentaje = 0
                NewReceptorOrdenesOF.Nombre = ""

                ListaReceptorOrdenesOF.Add(NewReceptorOrdenesOF)
                ReceptorOrdenesOFSelected = NewReceptorOrdenesOF

                MyBase.CambioItem("ListaReceptorOrdenesOF")

        End Select
    End Sub

#End Region

#Region "Borrar Hijos"

    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptorOrdenesOF"
                If Not IsNothing(_ListaReceptorOrdenesOF) Then
                    If Not _ReceptorOrdenesOFSelected Is Nothing Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptorOrdenesOFSelected, ListaReceptorOrdenesOF.ToList)

                        If ListaReceptorOrdenesOF.Contains(_ReceptorOrdenesOFSelected) Then
                            ListaReceptorOrdenesOF.Remove(_ReceptorOrdenesOFSelected)
                        End If
                        If ListaReceptorOrdenesOF.Count > 0 Then
                            Program.PosicionarItemLista(ReceptorOrdenesOFSelected, ListaReceptorOrdenesOF.ToList, intRegistroPosicionar)
                        Else
                            ReceptorOrdenesOFSelected = Nothing
                        End If
                        MyBase.CambioItem("ListaReceptorOrdenesOF")
                    End If
                End If
        End Select
    End Sub

#End Region

#End Region

#Region "Consultar datos asociados a la orden o al comitente"

    ''' <summary>
    ''' Consultar las liquidaciones asociadas a la orden
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cargarLiquidaciones()
        If mstrAccionOrden <> MSTR_MC_ACCION_INGRESAR And mlogDuplicarOrden = False Then
            If Not IsNothing(OrdenOFSelected) Then
                IsBusyLiquidaciones = True
                dcProxyConsultas.LiquidacionesOrdenOFs.Clear()
                dcProxyConsultas.Load(dcProxyConsultas.ConsultarLiquidacionesAsociadasOrdenOFQuery(OrdenOFSelected.Clase, OrdenOFSelected.Tipo, OrdenOFSelected.NroOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarLiquidacionesOrdenOF, "consultarLiquidaciones")
            End If
        Else
            dcProxyConsultas.LiquidacionesOrdenOFs.Clear()
        End If
    End Sub

    ''' <summary>
    ''' Se verifica si los receptores estan activos de la orden duplica 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub verificarReceptorOrdenesOFDuplicar()
        dcProxy1.ReceptorOrdenesOFs.Clear()
        dcProxy1.Load(dcProxy1.Verificar_ReceptoresOrdenesOF_OrdenQuery(OrdenOFSelected.Clase, OrdenOFSelected.Tipo, OrdenOFSelected.NroOrden, OrdenOFSelected.Version, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptorOrdenesOF, "verificarReceptoresDuplicar")
    End Sub

    ''' <summary>
    ''' Consultar los receptores asociados a la orden
    ''' </summary>
    Private Sub consultarReceptorOrdenesOF(ByVal pstrClaseOrden As String, ByVal pstrTipo As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrUserState As String)
        'If Not IsNothing(dcProxy1.ReceptorOrdenesOFs) Then
        '    dcProxy1.ReceptorOrdenesOFs.Clear()
        'End If
        dcProxy1.ReceptorOrdenesOFs.Clear()
        dcProxy1.Load(dcProxy1.Traer_ReceptoresOrdenesOF_OrdenesOFQuery(pstrClaseOrden, pstrTipo, pintNroOrden, pintVersion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptorOrdenesOF, "")
    End Sub

    ''' <summary>
    ''' Consultar los receptores del cliente para seleccionar los asociados a la orden
    ''' </summary>
    Private Sub consultarReceptoresComitente()
        If _mlogCargarReceptorClientes Then
            dcProxy1.ReceptorOrdenesOFs.Clear()
            dcProxy1.Load(dcProxy1.Traer_ReceptoresOrdenesOF_ClienteQuery(_OrdenOFSelected.Clase, _OrdenOFSelected.Tipo, _ComitenteSeleccionado.CodigoOYD, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptorOrdenesOF, MSTR_ACC_CONSULTA_RECEPTORES_CLT)
        End If
    End Sub

    ''' <summary>
    ''' Consultar los ordenantes del comitente asociado a la orden
    ''' </summary>
    Private Sub consultarOrdenantes()
        mdcProxyUtilidad01.BuscadorOrdenantes.Clear()
        mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarOrdenantesComitenteQuery(_ComitenteSeleccionado.IdComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenantes, "")
    End Sub

    ''' <summary>
    ''' Consultar las cuentas depósito del comitente asociado a la orden
    ''' </summary>
    Private Sub consultarCuentasDeposito()
        mdcProxyUtilidad02.BuscadorCuentasDepositos.Clear()
        mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarCuentasDepositoComitenteQuery(_ComitenteSeleccionado.IdComitente, True, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDeposito, "")
    End Sub

    ''' <summary>
    ''' Consultar las cuentas del cliente para las instrucciones
    ''' </summary>
    Private Sub consultarCuentasCliente()
        dcProxy.CuentasClientes.Clear()
        dcProxy.Load(dcProxy.Consultar_CuentasClientesQuery(_ComitenteSeleccionado.IdComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasClientes, "")
    End Sub

    ''' <summary>
    ''' Seleccionar los datos del ordenante asociado a la orden de la lista de ordenantes
    ''' </summary>
    Private Sub buscarOrdenanteSeleccionado()
        If Not _Ordenantes Is Nothing And Not _OrdenOFSelected Is Nothing Then
            If Not Editando Then
                If Not _OrdenOFSelected.IDOrdenante Is Nothing Then
                    If (From obj In _Ordenantes Where obj.IdOrdenante = _OrdenOFSelected.IDOrdenante Select obj).ToList.Count > 0 Then
                        OrdenanteSeleccionado = (From cta In _Ordenantes Where cta.IdOrdenante.Trim() = _OrdenOFSelected.IDOrdenante.Trim() Select cta).ToList.ElementAt(0)
                    Else
                        'SLB20121205 Cuando el Ordenante se encuentra Inactivo se debe mostrar en el pantalla, a pesar de que en la lista no exista.
                        Dim strDescripcionOrdenante = LTrim(_OrdenOFSelected.IDOrdenante) & " - " & _OrdenOFSelected.NombreOrdenante.ToString
                        Ordenantes.Add(New OYDUtilidades.BuscadorOrdenantes With {.DescripcionOrdenante = strDescripcionOrdenante, .IdOrdenante = "-1"})
                        OrdenanteSeleccionado = Ordenantes.Last
                        Ordenantes.Remove(Ordenantes.Last)
                        'OrdenanteSeleccionado = Nothing
                    End If
                Else
                    If _Ordenantes.Count = 1 Then
                        OrdenanteSeleccionado = _Ordenantes.First
                    End If
                End If
            Else 'SLB se adiciona la seleccion del Ordenante Lider por defecto.
                If (From obj In _Ordenantes Select obj).ToList.Count > 0 Then
                    If _mlogRequiereIngresoOrdenantes Then
                        If (From obj In _Ordenantes Select obj).ToList.Count = 1 Then
                            OrdenanteSeleccionado = _Ordenantes.First
                        Else
                            OrdenanteSeleccionado = Nothing
                        End If
                    Else
                        If (From cta In _Ordenantes Where cta.Lider = True Select cta).ToList.Count > 0 Then
                            OrdenanteSeleccionado = (From cta In _Ordenantes Where cta.Lider = True Select cta).ToList.ElementAt(0)
                        Else
                            OrdenanteSeleccionado = _Ordenantes.First
                        End If
                    End If
                Else
                    OrdenanteSeleccionado = Nothing
                    A2Utilidades.Mensajes.mostrarMensaje("Ordenante Inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            OrdenanteSeleccionado = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Seleccionar los datos de la cuenta depósito asociada a la orden de la lista de cuentas depósito
    ''' </summary>
    Private Sub buscarCuentaDepositoSeleccionada()
        If Not _OrdenOFSelected Is Nothing And Not CuentasDeposito Is Nothing Then
            If Editando Then
                If (From cta In CuentasDeposito Select cta).ToList.Count = 1 Then
                    CtaDepositoSeleccionada = CuentasDeposito.First
                Else
                    CtaDepositoSeleccionada = Nothing
                End If
            Else
                If Not _OrdenOFSelected.CuentaDeposito Is Nothing Then
                    If (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _OrdenOFSelected.CuentaDeposito Select cta).ToList.Count > 0 Then
                        CtaDepositoSeleccionada = (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _OrdenOFSelected.CuentaDeposito Select cta).ToList.ElementAt(0)
                    Else
                        CtaDepositoSeleccionada = Nothing
                    End If
                Else
                    If _CuentasDeposito.Count = 1 Then
                        CtaDepositoSeleccionada = _CuentasDeposito.First
                    ElseIf (From cta In CuentasDeposito Where IsNothing(cta.NroCuentaDeposito) And cta.Deposito = _OrdenOFSelected.UBICACIONTITULO Select cta).ToList.Count > 0 Then
                        CtaDepositoSeleccionada = (From cta In CuentasDeposito Where IsNothing(cta.NroCuentaDeposito) And cta.Deposito = _OrdenOFSelected.UBICACIONTITULO Select cta).ToList.ElementAt(0)
                    End If
                End If
            End If
        Else
            CtaDepositoSeleccionada = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Consultar los beneficiarios de la cuenta depósito asignada a la orden
    ''' </summary>
    Private Sub consultarBeneficiariosOrden()
        dcProxy.BeneficiarioOrdenesOFs.Clear()
        dcProxy.Load(dcProxy.Traer_BeneficiariosOrdenes_OrdenesOFQuery(OrdenOFSelected.Clase, OrdenOFSelected.Tipo, OrdenOFSelected.NroOrden, OrdenOFSelected.Version, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosOrdenes, "")
    End Sub

    ''' <summary>
    ''' Consultar los beneficiarios de la cuenta depósito asignada al cliente
    ''' </summary>
    Private Sub consultarBeneficiariosCliente()
        dcProxy.BeneficiarioOrdenesOFs.Clear()
        If Not IsNothing(OrdenOFSelected.CuentaDeposito) Then
            dcProxy.Load(dcProxy.Traer_BeneficiariosOrdenesOF_ClienteQuery(OrdenOFSelected.IDComitente, OrdenOFSelected.CuentaDeposito, OrdenOFSelected.UBICACIONTITULO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosOrdenes, "")
        End If
    End Sub

#End Region

#Region "Property Changed"

    ''' <summary>
    ''' Este evento se dispara cuando alguna propiedad de la orden activa cambia
    ''' Se utiliza para calcular los días de vigencia y/o días de vencimiento cuando cambia alguna de las fechas involucradas en el cálculo
    ''' </summary>
    Private Sub _OrdenOFSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _OrdenOFSelected.PropertyChanged

        If e.PropertyName.ToLower.Equals("objeto") Then
            If Not _OrdenOFSelected.Objeto = String.Empty Then
                _OrdenOFSelected.Ordinaria = False
            Else
                _OrdenOFSelected.Ordinaria = True
            End If
        End If

        If Me.ClaseOrden = ClasesOrden.C Then
            If e.PropertyName.ToLower.Equals("objeto") Then
                Me.OrdenOFSelected.Repo = False
                Select Case _OrdenOFSelected.Objeto
                    Case RF_Simulatena_Salida, RF_Simulatena_Regreso
                        Me.OrdenOFSelected.Mercado = RF_MERCADO_SECUNDARIO
                        Me.HabilitarMercado = False
                    Case RF_TTV_Salida, RF_TTV_Regreso
                        Me.OrdenOFSelected.Mercado = RF_MERCADO_SECUNDARIO
                        Me.HabilitarMercado = False
                        Me.OrdenOFSelected.Repo = True
                    Case RF_REPO
                        Me.OrdenOFSelected.Mercado = RF_MERCADO_REPO
                        Me.HabilitarMercado = False
                        Me.OrdenOFSelected.Repo = True
                    Case "MP"
                        Me.OrdenOFSelected.Mercado = RF_MERCADO_PRIMARIO '"P"
                        Me.HabilitarMercado = True
                        Me.OrdenOFSelected.Repo = False
                    Case Else
                        'objTipoId = CType(Application.Current.Resources("OrdenesView_Ord_RentaFija"), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("O_MERCADO").Where(Function(li) li.ID.Equals("P") Or li.ID.Equals("S") Or li.ID.Equals("R")).ToList
                        Me.OrdenOFSelected.Mercado = RF_MERCADO_SECUNDARIO
                        Me.HabilitarMercado = True
                        Me.OrdenOFSelected.Repo = False
                        'Case Else
                        '    'objTipoId = CType(Application.Current.Resources("OrdenesView_Ord_RentaFija"), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("O_MERCADO").Where(Function(li) li.ID.Equals("P") Or li.ID.Equals("S") Or li.ID.Equals("R")).ToList
                        '    Me.OrdenOFSelected.Mercado = RF_MERCADO_PRIMARIO '"P"
                        '    Me.HabilitarMercado = True
                End Select

                If _strObjetoAnterior <> _OrdenOFSelected.Objeto Then
                    Select Case _strObjetoAnterior
                        Case RF_TTV_Salida, RF_TTV_Regreso, RF_REPO
                            'Case "3", "4", "RP"
                            If (_OrdenOFSelected.Objeto <> RF_TTV_Salida And _OrdenOFSelected.Objeto <> RF_TTV_Regreso And _OrdenOFSelected.Objeto <> RF_REPO) Then
                                NemotecnicoOrden = String.Empty
                            End If
                        Case Else
                            If (_OrdenOFSelected.Objeto = RF_TTV_Salida Or _OrdenOFSelected.Objeto = RF_TTV_Regreso Or _OrdenOFSelected.Objeto = RF_REPO) Then
                                NemotecnicoOrden = String.Empty
                            End If
                    End Select
                End If
                _strObjetoAnterior = _OrdenOFSelected.Objeto
            End If
        End If

        If mlogRecalcularFechas = False Then 'CCM20120305
            mlogRecalcularFechas = True
            Exit Sub
        End If

        Try
            mlogRecalcularFechas = False 'CCM20120305

            Select Case e.PropertyName.ToLower()
                'Case "fechaorden"
                '    Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
                Case "vigenciahasta"
                    Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
                Case "fechavencimiento"
                    Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_TITULO, -1)
                Case "fechaemision"
                    Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_TITULO, -1)
            End Select
        Catch ex As Exception
            mlogRecalcularFechas = True 'CCM20120305
        End Try

    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaOrdenOF
    Implements INotifyPropertyChanged

#Region "Eventos"
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- DEFINICIÓN DE EVENTOS
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- Indicar cualquier cambio en una propiedad del objeto
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#End Region

#Region "Variables"
    Private mobjVM As OrdenesOFViewModel
#End Region

#Region "Propiedades"

    <Description("Renta fija o renta variable")> _
    Public Property Clase As String = String.Empty
    <Description("Tipo de operación de la orden: compra, venta, ...")> _
    Public Property Tipo As String = String.Empty
    Public Property AnoOrden As String = Nothing
    Public Property NroOrden As System.Nullable(Of Integer)
    Public Property Version As System.Nullable(Of Integer) = 0
    Public Property FechaOrden As System.Nullable(Of Date)
    Public Property Nemotecnico As String = String.Empty
    Public Property FormaPago As String = String.Empty
    Public Property Objeto As String = Nothing
    Public Property CondicionesNegociacion As String
    Public Property TipoInversion As String = "  "
    Public Property TipoTransaccion As String = String.Empty
    Public Property TipoLimite As String = String.Empty
    Public Property VigenciaHasta As System.Nullable(Of Date)
    Public Property Ejecucion As String = String.Empty
    Public Property Duracion As String = String.Empty
    Public Property Estado As String = String.Empty
    Public Property IdBolsa As System.Nullable(Of Integer)

    Private _ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            _ComitenteSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ComitenteSeleccionado"))
            mobjVM.CambioItem("cb")
        End Set
    End Property

    Private _OrdenanteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property OrdenanteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_OrdenanteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            _OrdenanteSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("OrdenanteSeleccionado"))
            mobjVM.CambioItem("cb")
        End Set
    End Property

    Private _NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            _NemotecnicoSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NemotecnicoSeleccionado"))
            mobjVM.CambioItem("cb")
        End Set
    End Property

    Private _IDComitente As String = String.Empty
    Public Property IDComitente() As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitente"))
        End Set
    End Property

    Private _IDOrdenante As String = String.Empty
    Public Property IDOrdenante() As String
        Get
            Return _IDOrdenante
        End Get
        Set(ByVal value As String)
            _IDOrdenante = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDOrdenante"))
        End Set
    End Property

#End Region

#Region "Inicializaciones"

    Public Sub New(ByRef pobjVMPadre As OrdenesOFViewModel)
        mobjVM = pobjVMPadre
    End Sub

#End Region

End Class

