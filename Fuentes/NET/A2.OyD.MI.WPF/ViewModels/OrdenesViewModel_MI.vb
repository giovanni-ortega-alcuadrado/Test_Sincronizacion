Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: OrdenesViewModel.vb
'Generado el : 07/21/2011 08:36:53
'Propiedad de Alcuadrado S.A. 2010
'
' Adaptar código generado a necesidades del sistema: Cristian Ciceri Muñetón - Julio/2011
'           - Modificación del diseño
'           - Propiedad para identificar si trabaja órdenes de acciones o de renta fija
'           - Referencia al view model mediante la variable mobjVM
'           - Incluir carga de combos específicos
'           - Manejador de error en el control
'           - Ajustes a funcionalidad del negocio
'
' ATENCIÓN: El maker and checker de este control tiene algunas diferencias con el estándar de otros controles
' ========
'             - Para saber si se tiene maker and checker se consulta en la carga de combos específicos y no se retorna con cada registro
'             - La barra de herramientas (a2controlmenu) es una versión diferente
'

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Public Class OrdenesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        cb = New CamposBusquedaOrden(Me)
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
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                mdcProxyUtilidad02 = New UtilidadesDomainContext()
            Else
                'dcProxy = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                'dcProxy1 = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                'dcProxyConsultas = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy = New OrdenesDomainContext(New System.Uri(Program.RutaServicioOrdenes))
                dcProxy1 = New OrdenesDomainContext(New System.Uri(Program.RutaServicioOrdenes))
                dcProxyConsultas = New OrdenesDomainContext(New System.Uri(Program.RutaServicioOrdenes))
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
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

                mlogInicializado = True

                HabilitarBotones()
                'ActivarDuplicarOrden = True
                'ActivarEnvioSAE = True
                'ActivarSeleccionTipoOrden = True

            End If

            MyBase.CambioItem("RentaFijaVisible")
            MyBase.CambioItem("RentaVariableVisible")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "OrdenesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        validarPermisosAdicionar()

    End Sub

    Private Sub TerminoTraerParametros(ByVal lo As LoadOperation(Of OYDUtilidades.valoresparametro))
        Try
            If lo.HasError = False Then
                objListaParametros = lo.Entities.ToList

                dcProxy.Load(dcProxy.OrdenesFiltrar_MIQuery(Me.ClaseOrden.ToString(), "", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, "")
                dcProxy1.Load(dcProxy1.TraerOrden_MIPorDefectoQuery(Me.ClaseOrden.ToString(), Program.Usuario, "", Program.HashConexion), AddressOf TerminoTraerOrdenesPorDefecto_Completed, "Default")
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

    ''' <summary>
    ''' A - Hasta Hora
    ''' C - Hasta Cancelación
    ''' D - Día
    ''' F - Hasta Fecha
    ''' I - Inmediata
    ''' S - Sesion
    ''' </summary>
    Public Enum TipoDuracion As Byte
        A
        C
        D
        F
        I
        S
    End Enum

    ''' <summary>
    ''' PA : Pendiente por aprobar
    ''' NA : No aprobado (rechazado)
    ''' A  : Aprobado
    ''' PV : Con versión por aprobbar
    ''' </summary>
    Public Enum EstadoMakerChecker As Byte
        PA
        NA
        A
        PV
    End Enum

    Private Enum OrdenTabs As Byte
        LEO
        Caracteristicas
        Otros
        Observaciones
        Receptores
        Benficiarios
        Instrucciones
        LiqPosibles
    End Enum

    Private Const MSTR_MC_BOTON_TEXTO_VERSION_APROBADA As String = "Ver versión aprobada"
    Private Const MSTR_MC_BOTON_TEXTO_VERSION_POR_APROBAR As String = "Ver versión por aprobar"

    Private Const MSTR_MC_ACCION_INGRESAR As String = "I"
    Private Const MSTR_MC_ACCION_ACTUALIZAR As String = "U"
    Private Const MSTR_MC_ACCION_BORRAR As String = "D"

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
    Friend Const MSTR_VALIDAR_DIA_FECHA_ELA_ORDEN As String = "fecha_elaboracion_orden"

    Private MSTR_MODULO_OYD_ORDENES As String = "O"

    Private MSTR_MENSAJE_EXCEPCION_RDIP As String = "El rating del producto no corresponde al perfil del Cliente." & vbNewLine & vbNewLine & "¿Desea continuar?"

    Private MINT_LONG_MAX_CODIGO_OYD As Byte = 17
    Private MINT_LONG_MAX_NEMOTECNICO As Byte = 15

    Public Const MSTR_CTADEPOSITO_TITULO_FISICO As String = "F"
    Public Const MSTR_CTADEPOSITO_TITULO_EXTERIOR As String = "X"

#End Region

#Region "Variables"

    Private mlogInicializado As Boolean = False
    Private mlogRecalcularFechas As Boolean = True 'CCM20120305. Pemrite activar o desactivar el recálculo de las fechas de vigencia y vencimiento y días de vigencia y palzo
    Private mlogCambioBolsa As Boolean = True

    Private mlogDuplicarOrden As Boolean = False ' Indica si se está duplicando una orden
    Private mlogPuedeDuplicarOrden As Boolean = False ' Indica si el usuario tiene autorización para duplicar órdenes

    Private mlogDuplicarDatosParam As Boolean = False 'CCM20120305. Indica si ya se inicializaron los parámetros que definen algunas características de configuración de las órdenes
    Private mlogDuplicarDatosLeo As Boolean = False 'CCM20120305. Por defecto no duplica datos de LEO
    Private mlogDuplicarDatosPrecio As Boolean = True 'CCM20120305. Por defecto duplica el precio de la orden

    Private _mlogHabilitarFechaElaboracion As Boolean = True 'SLB20121128. Habilitar Fecha Elaboración
    Public _mlogMostrarTodosReceptores As Boolean = True 'SLB20121128. Por defecto debe mostrar todos los receptores
    Private _mlogCargarReceptorClientes As Boolean = True
    Private _mlogRequiereIngresoOrdenantes As Boolean = False

    Private mlogUsuarioPuedeIngresar As Boolean = True 'CCM20120305. Indica que el usuario tiene el botón Nuevo activo en la barra de herramientas

    Private mlogSAEActivo As Boolean = True ' Indica si la funcionalidad para enrutar órdenes por SAE está activa
    Private mlogSAEActivoClaseOrden As Boolean = True ' Indica si la funcionalidad para enrutar órdenes por SAE está activa para la clase de orden (renta fija, acciones)
    Private mlogPuedeEnrutarSAE As Boolean = True ' Indica si el usuario está autorizado para enrutar órdenes por SAE

    Private mintUltimoId As Integer = -200000000

    Private mstrAccionOrden As String = String.Empty '// Este indicador ayuda a controla la ejecución de consultas inutiles durante el ingreso especialmente
    Private mstrTipoLimitePorDef As String = "M" 'CCM20120305. El tipo de natruraleza o tipo límite es Mercado (M)
    Private mstrValidarCuidadSeteo As String = "NO" 'SLB20130726 Validar la información de la cuidad de Seteo de las Órdenes
    Private mstrCuidadSeteo As String
    Private logValidarCuentaDeposito As Boolean = False 'adicionado por juan david correa para validar la cuenta deposito

    Private logRegistroLibroOrdenes As Boolean = False 'SLB20130801 Registro en Libro de Órdenes
    Private dblValorCantidadLibroOrden As Double = 0

    Private mdtmFechaCierreSistema As Date

    Public Property cb As CamposBusquedaOrden

    Private OrdenPorDefecto As OyDOrdenes.Orden_MI
    Private OrdenAnterior As OyDOrdenes.Orden_MI

    Private dcProxy As OrdenesDomainContext
    Private dcProxy1 As OrdenesDomainContext
    Private dcProxyConsultas As OrdenesDomainContext

    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext

    Private _mobjReceptorTomaSeleccionadoAntes As OYDUtilidades.BuscadorGenerico
    Private _mobjNemotecnicoSeleccionadoAntes As OYDUtilidades.BuscadorEspecies
    Private _mobjComitenteSeleccionadoAntes As OYDUtilidades.BuscadorClientes

    Private _mlogFechaCierreSistema As Boolean = False

    Private objListaParametros As List(Of OYDUtilidades.valoresparametro)
    Public objA2VMLocal As A2UtilsViewModelEsp
    Public logActualizarFechaRecepcion As Boolean = True

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
                Return ("Órdenes de Renta Fija")
            ElseIf _mlogEsRentaVariable Then
                Return ("Órdenes de Acciones Mercado Integrado")
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

                If Not _OrdenSelected Is Nothing AndAlso (_ComitenteSeleccionado Is Nothing OrElse Not value.Equals(_ComitenteSeleccionado.IdComitente)) Then
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

                If Not _OrdenSelected Is Nothing AndAlso (_NemotecnicoSeleccionado Is Nothing OrElse Not value.ToUpper().Equals(_NemotecnicoSeleccionado.Nemotecnico.ToUpper())) Then
                    '(SLB) se adiciona la validacion de la Bolsa 
                    If Not _OrdenSelected.IdBolsa.Equals(0) Then
                        buscarNemotecnico(value, "buscar")
                    Else
                        validarBolsaParaEspecie()
                    End If
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

                If Not IsNothing(Me.OrdenSelected) Then
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

    Private _mstrEstadoSAE As String = String.Empty
    ''' <summary>
    ''' Estado de la orden cuando ha sido enviada por SAE (Bus de integración)
    ''' </summary>
    Public ReadOnly Property EstadoSAE As String
        Get
            Return (_mstrEstadoSAE)
        End Get
    End Property

    Private _mstrNroOrdenSAE As String = String.Empty
    ''' <summary>
    ''' Número de la orden cuando ha sido enviada por SAE (Bus de integración)
    ''' </summary>
    Public ReadOnly Property NroOrdenSAE As String
        Get
            Return (_mstrNroOrdenSAE)
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

    Private _mstrEstadoOrdenLeo As String = String.Empty
    Public Property EstadoOrdenLeo() As String
        Get
            Return (_mstrEstadoOrdenLeo)
        End Get
        Set(ByVal value As String)
            Dim objEstados As List(Of OYDUtilidades.ItemCombo)
            If Application.Current.Resources.Contains(Me.ListaCombosEsp) Then
                objEstados = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_ESTADOS_ORDEN_LEO"))

                If IsNothing(objEstados) Then
                    _mstrEstadoOrdenLeo = ""
                Else
                    _mstrEstadoOrdenLeo = (From obj In objEstados Where obj.ID = value Select obj).First.Descripcion
                End If
            Else
                _mstrEstadoOrdenLeo = ""
            End If
            MyBase.CambioItem("EstadoOrdenLeo")
        End Set
    End Property

    'Private _mdblSaldo As Double = 0
    'Public ReadOnly Property SaldoOrden() As Double
    '    Get
    '        Return (_mdblSaldo)
    '    End Get
    'End Property

    Private _SaldoOrden As Double?
    Public Property SaldoOrden As Double?
        Get
            Return _SaldoOrden
        End Get
        Set(ByVal value As Double?)
            _SaldoOrden = value
            MyBase.CambioItem("SaldoOrden")
        End Set
    End Property

    Private _ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            Dim logIgual As Boolean = False

            'If Not _ComitenteSeleccionado Is Nothing Then
            '    If _ComitenteSeleccionado.Equals(value) Then
            '        logIgual = True
            '    End If
            'Else
            '    Ordenantes = Nothing
            '    CuentasDeposito = Nothing
            'End If

            If Not IsNothing(_ComitenteSeleccionado) AndAlso _ComitenteSeleccionado.Equals(value) Then
                consultarOrdenantes()
                consultarCuentasDeposito()
                Exit Property
            Else
                Ordenantes = Nothing
                CuentasDeposito = Nothing
            End If


            If logIgual Then
                buscarOrdenanteSeleccionado()
                buscarCuentaDepositoSeleccionada()
            Else
                _ComitenteSeleccionado = value

                If Not value Is Nothing Then
                    If mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                        '// Solamente si se está ingresando o actualizando se debe cambiar el código del comitente y buscar los nuevos receptores de la orden
                        _OrdenSelected.IDComitente = _ComitenteSeleccionado.CodigoOYD
                        _OrdenSelected.FormaPago = _ComitenteSeleccionado.CodFormaPago

                        '// Consultar los receptores que se pueden asignar a la orden
                        consultarReceptoresComitente()
                    ElseIf mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Then
                        _OrdenSelected.IDComitente = _ComitenteSeleccionado.CodigoOYD
                        consultarReceptoresComitente()
                    End If

                    '// Actualizar el campo para que se vea el código del comitente seleccionado
                    ComitenteOrden = _ComitenteSeleccionado.IdComitente

                    consultarOrdenantes()
                    consultarCuentasDeposito()
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
                        _OrdenSelected.Nemotecnico = _NemotecnicoSeleccionado.Nemotecnico
                    End If

                    '// Actualizar el campo para que se vea el nemotecnico seleccionado
                    NemotecnicoOrden = _NemotecnicoSeleccionado.Nemotecnico
                End If

                MyBase.CambioItem("NemotecnicoSeleccionado")
            End If
        End Set
    End Property

    Private _ReceptorTomaSeleccionado As OYDUtilidades.BuscadorGenerico
    Public Property ReceptorTomaSeleccionado As OYDUtilidades.BuscadorGenerico
        Get
            Return (_ReceptorTomaSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorGenerico)
            _mobjReceptorTomaSeleccionadoAntes = _ReceptorTomaSeleccionado
            _ReceptorTomaSeleccionado = value
            MyBase.CambioItem("ReceptorTomaSeleccionado")
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
                    _OrdenSelected.IDOrdenante = _mobjOrdenanteSeleccionado.IdOrdenante
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
                    _OrdenSelected.UBICACIONTITULO = _mobjCtaDepositoSeleccionada.Deposito
                    _OrdenSelected.CuentaDeposito = _mobjCtaDepositoSeleccionada.NroCuentaDeposito
                    If Editando Then
                        consultarBeneficiariosCliente()
                    End If
                End If
            End If

            MyBase.CambioItem("CtaDepositoSeleccionada")
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
    Public ReadOnly Property SAEActivo() As Visibility
        Get
            If Me.mlogSAEActivo And Me.mlogSAEActivoClaseOrden And Me.mlogPuedeEnrutarSAE Then
                Return (Visibility.Visible)
            Else
                Return (Visibility.Collapsed)
            End If
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

    ''' <summary>
    ''' Permite controlar si el botón para envío de una orden a través de SAE está o no activo
    ''' </summary>
    Private _ActivarEnvioSAE As Boolean = False
    Public Property ActivarEnvioSAE As Boolean
        Get
            Return (_ActivarEnvioSAE)
        End Get
        Set(ByVal value As Boolean)
            _ActivarEnvioSAE = value
            MyBase.CambioItem("ActivarEnvioSAE")
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

    Private _EditandoRegistro As Boolean = False
    ''' <summary>
    ''' Se crea esta variable para el registro en Libro de Órdenes.
    ''' </summary>
    ''' <remarks>SLB20130801</remarks>
    Public Property EditandoRegistro As Boolean
        Get
            Return _EditandoRegistro
        End Get
        Set(ByVal value As Boolean)
            _EditandoRegistro = value
            MyBase.CambioItem("EditandoRegistro")
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

#Region "Propiedades Maker and checker"

#Region "Propiedades generales que no cambian por registro"

    Private _ListaComboEstadoMaker As ObservableCollection(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboEstadoMaker() As ObservableCollection(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComboEstadoMaker
        End Get
        Set(ByVal value As ObservableCollection(Of OYDUtilidades.ItemCombo))
            _ListaComboEstadoMaker = value
            MyBase.CambioItem("ListaComboEstadoMaker")
        End Set
    End Property

    Private _ListaComboAccionMaker As ObservableCollection(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboAccionMaker() As ObservableCollection(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComboAccionMaker
        End Get
        Set(ByVal value As ObservableCollection(Of OYDUtilidades.ItemCombo))
            _ListaComboAccionMaker = value
            MyBase.CambioItem("ListaComboAccionMaker")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que define si se maneja o no maker and checker. Es necesaria para activar los botones de Aprobar, Rechazar y Ver versión
    ''' </summary>
    ''' <remarks></remarks>
    Private _mlogManejaMakerAndChecker As Boolean = False
    Public ReadOnly Property ManejaMakerAndChecker As Boolean
        Get
            Return (_mlogManejaMakerAndChecker)
        End Get
    End Property

    ' La propiedad se hace de solo lectura para que no pueda ser manipulada desde fuera del view model
    Private Sub cambiarManejaMakerAndChecker(ByVal plogManejaMakerAndChecker As Boolean)
        If Not _mlogManejaMakerAndChecker.Equals(plogManejaMakerAndChecker) Then
            _mlogManejaMakerAndChecker = plogManejaMakerAndChecker
            'VisibleMakerAndCheker = Visibility.Visible
            MyBase.CambioItem("ManejaMakerAndChecker")
        End If
    End Sub

    ''' <summary>
    ''' Se ocultan los controles que no aplican cuando no se utiliza el Maker and Checker
    ''' </summary>
    ''' <param name="plogManejaMakerAndChecker"></param>
    ''' <remarks>SLB20120704</remarks>
    Private Sub mostrarCamposMakerAndChecker(ByVal plogManejaMakerAndChecker As Boolean)
        If Not plogManejaMakerAndChecker Then
            cb.VisibleMakerAndCheker = Visibility.Collapsed
        End If
    End Sub

#End Region

#Region "Propiedades que cambian por registro"

    ''' <summary>
    ''' Propiedad que define el texto del botón que permite intercambiar desde la versión aprobada del registro a la no aprobada
    ''' </summary>
    ''' <remarks></remarks>
    Private _mstrTextoCambioARegistroOriginal As String = MSTR_MC_BOTON_TEXTO_VERSION_APROBADA
    Public ReadOnly Property TextoCambioARegistroOriginal As String
        Get
            Return (_mstrTextoCambioARegistroOriginal)
        End Get
    End Property

    Private Sub cambiarTextoCambioARegistroOriginal(ByVal pstrTextoCambioARegistroOriginal As String)
        If Not _mstrTextoCambioARegistroOriginal.Equals(pstrTextoCambioARegistroOriginal) AndAlso Not pstrTextoCambioARegistroOriginal.Equals(String.Empty) Then
            _mstrTextoCambioARegistroOriginal = pstrTextoCambioARegistroOriginal
            MyBase.CambioItem("TextoCambioARegistroOriginal")
        End If
    End Sub

    ''' <summary>
    ''' Propiedad que define el texto del botón que permite intercambiar desde la versión no aprobada del registro a la aprobada
    ''' </summary>
    ''' <remarks></remarks>
    Private _mstrTextoCambioARegistroModificado As String = MSTR_MC_BOTON_TEXTO_VERSION_POR_APROBAR
    Public ReadOnly Property TextoCambioARegistroModificado As String
        Get
            Return (_mstrTextoCambioARegistroModificado)
        End Get
    End Property

    Private Sub cambiarTextoCambioARegistroModificado(ByVal pstrTextoCambioARegistroModificado As String)
        If Not _mstrTextoCambioARegistroModificado.Equals(pstrTextoCambioARegistroModificado) AndAlso Not pstrTextoCambioARegistroModificado.Equals(String.Empty) Then
            _mstrTextoCambioARegistroModificado = pstrTextoCambioARegistroModificado
            MyBase.CambioItem("TextoCambioARegistroModificado")
        End If
    End Sub

    ''' <summary>
    ''' Propiedad que define si activan o no los botones de aprobar y rechazar para el registro seleccionado. Cuando el registro
    ''' seleccionado es el que está pendiente de aprobar, es decir, es el que tiene los cambios, los botones de aprobar y rechazar
    ''' se deben activar pero si el registro activo es el orginal (sin los cambios) estos botones no se deben activar.
    ''' </summary>
    ''' <remarks></remarks>
    Private _mlogRegistroActivoPorAprobar As Boolean = False
    Public ReadOnly Property RegistroActivoPorAprobar As Boolean
        Get
            Return (_mlogRegistroActivoPorAprobar)
        End Get
    End Property

    ' La propiedad se hace de solo lectura para que no pueda ser manipulada desde fuera del view model
    Private Sub cambiarRegistroActivoPorAprobar(ByVal pstrEstadoMakerChecker As String, ByVal pstrAccionMakerCheker As String, ByVal plogTieneOrdenRelacionada As Boolean)
        Dim logRegistroActivoPorAprobar As Boolean = False '// Activar o no botones de aprobar y rechazar
        Dim logRegistroTieneCambios As Boolean = False '// Ver o no el botón para cambiar entre versiones (aprobda y pendiente)
        Dim logVerDescripcionMakerChecker As Boolean = False '// Ver o no el texto en el formulario que muestra el estado del maker and checker
        'Dim strTextoBotonVersionAprobada As String = String.Empty '// Texto del botón para cambiar entre versiones aprobada y pendiente

        '// Cuando no se tiene estado definido (es nothing) se asume que el documento está aprobado y no tiene versione por aprobar. 
        '// No se activan los botones de aprobar y rechazar ni ver versión. Tampoco se muestra el estado del maker and checker
        If Not IsNothing(pstrEstadoMakerChecker) Then
            '// Cuando se tiene estado definido se debe evaluar que hacer.

            If pstrEstadoMakerChecker.ToUpper.Equals(EstadoMakerChecker.PA.ToString()) Then
                '// Cuando los datos están en proceso de aprobación el registro está en estado pendiente. OJO: Solamente se considera este estado, los ya aprobados y rechazados se deben descartar en la consulta.
                If pstrAccionMakerCheker.ToUpper().Equals(MSTR_MC_ACCION_INGRESAR) Then
                    '// Cuando la acción es ingrerso no se muestra el botón de ver versión aprobada porque no existe una versión previa
                    logRegistroTieneCambios = False
                Else
                    '// Cuando la acción NO es ingrerso se muestra el botón de ver versión aprobada porque debe existir una versión previa sobre la cual se hicieron las modificaciones
                    logRegistroTieneCambios = True
                End If

                logRegistroActivoPorAprobar = True '// Se deben ver los botones de aprobar y rechazar porque el registro esta por aprobar
                logVerDescripcionMakerChecker = True '// Ver el texto en el formulario que muestra el estado del maker and checker
            ElseIf plogTieneOrdenRelacionada And (pstrEstadoMakerChecker.ToUpper.Equals(EstadoMakerChecker.A.ToString()) Or pstrEstadoMakerChecker.ToUpper.Equals(String.Empty)) Then
                '// Cuando se tiene una orden relacionada (versión pendiente de aprobar) y no es la versión pendiente de aprobar significa que está en la versión aprobada pero
                ' que tiene una versión por aprobar.
                logRegistroActivoPorAprobar = False
                logRegistroTieneCambios = True
                'logVerDescripcionMakerChecker = True '// Ver el texto en el formulario que muestra el estado del maker and checker
            ElseIf pstrEstadoMakerChecker.ToUpper.Equals(EstadoMakerChecker.NA.ToString()) Then
                '// Cuando la orden ha sido rechazada no se deba activar ninguna opción y no se debe dejar modificar
                logVerDescripcionMakerChecker = True '// Ver el texto en el formulario que muestra el estado del maker and checker
            Else
                '// Cuando se tiene un estado diferente se asume que el registro está aprobado y no tiene versiones por aprobar. 
                '// No se activan los botones de aprobar y rechazar ni de ver versión. Tampoco se muestra el estado del maker and checker.
                logRegistroActivoPorAprobar = False
            End If
        End If

        If Not _mlogRegistroActivoPorAprobar.Equals(logRegistroActivoPorAprobar) Then
            _mlogRegistroActivoPorAprobar = logRegistroActivoPorAprobar
            MyBase.CambioItem("RegistroActivoPorAprobar")
        End If

        cambiarRegistroTieneCambios(logRegistroTieneCambios)
        cambiarVerDescripcionMakerChecker(logVerDescripcionMakerChecker)
    End Sub

    ''' <summary>
    ''' Propiedad que define si activa o no el botón para ver la versión aprobada o no aprobada del registro activos cuando 
    ''' este tiene una versión no aprobada.  Cuando el registro seleccionado es el que está pendiente de aprobar, 
    ''' es decir, es el que tiene los cambios, o el registro activo es la versión orginal del modificado (sin los cambios) 
    ''' este botón se deben activar.
    ''' 
    ''' IMPORTANTE: En el caso de un nuevo registro, éste queda por aprobar pero no tiene versión aprobada y 
    '''             por lo tanto el botón nodebe verse.
    ''' </summary>
    ''' <remarks></remarks>
    Private _mlogRegistroTieneCambios As Boolean = False
    Public ReadOnly Property RegistroTieneCambios As Boolean
        Get
            Return (_mlogRegistroTieneCambios)
        End Get
    End Property

    Private Sub cambiarRegistroTieneCambios(ByVal plogVerBotonVersionAprobada As Boolean)
        If Not _mlogRegistroTieneCambios.Equals(plogVerBotonVersionAprobada) Then
            _mlogRegistroTieneCambios = plogVerBotonVersionAprobada
            MyBase.CambioItem("RegistroTieneCambios")
        End If
    End Sub

    ''' <summary>
    ''' Texto que muestra el estado del registro activo según la funcionalidad del Maker and Checker.
    ''' </summary>
    ''' <remarks></remarks>
    Private _mintVerDescMakerChecker As System.Windows.Visibility = Visibility.Collapsed
    Public ReadOnly Property VerDescripcionMakerChecker As System.Windows.Visibility
        Get
            Return (_mintVerDescMakerChecker)
        End Get
    End Property

    Private Sub cambiarVerDescripcionMakerChecker(ByVal plogVerDescMakerChecker As Boolean)
        If plogVerDescMakerChecker Then
            _mintVerDescMakerChecker = Visibility.Visible
        Else
            _mintVerDescMakerChecker = Visibility.Collapsed
        End If
        MyBase.CambioItem("VerDescripcionMakerChecker")
    End Sub

#End Region

#End Region

#Region "Propiedades ordenes"

    Private _ListaOrdenes As EntitySet(Of OyDOrdenes.Orden_MI)
    Public Property ListaOrdenes() As EntitySet(Of OyDOrdenes.Orden_MI)
        Get
            Return _ListaOrdenes
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.Orden_MI))
            _ListaOrdenes = value
            MyBase.CambioItem("ListaOrdenes")
            MyBase.CambioItem("ListaOrdenesPaged")
            'If Not IsNothing(value) Then
            '    OrdenSelected = ListaOrdenes.FirstOrDefault
            'End If
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

    Private WithEvents _OrdenSelected As OyDOrdenes.Orden_MI
    Public Property OrdenSelected() As OyDOrdenes.Orden_MI
        Get
            Return _OrdenSelected
        End Get
        Set(ByVal value As OyDOrdenes.Orden_MI)
            Try
                If Not IsNothing(_OrdenSelected) AndAlso _OrdenSelected.Equals(value) Then
                    Exit Property
                End If
                _OrdenSelected = value

                leerParametros()

                _mlogOpcionActiva = False
                If Not value Is Nothing Then

                    If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = String.Empty Then
                        If Not String.IsNullOrEmpty(OrdenSelected.IDComitente) Then
                            Me.buscarComitente(OrdenSelected.IDComitente, "buscar")
                        End If
                        _mobjComitenteSeleccionadoAntes = _ComitenteSeleccionado

                        Me.buscarNemotecnico()
                        _mobjNemotecnicoSeleccionadoAntes = _NemotecnicoSeleccionado

                        Me.buscarItem("receptores")

                        '-- Actualizar el estado del maker and checker para la orden
                        If Me.ManejaMakerAndChecker Then
                            cambiarRegistroActivoPorAprobar(Me._OrdenSelected.EstadoMakerChecker, Me._OrdenSelected.AccionMakerAndChecker, Me._OrdenSelected.TieneOrdenRelacionada)
                        End If

                        '// Consultar los beneficiarios de la cuenta depósito asignada a la orden
                        consultarBeneficiariosOrden()
                        '// Consultar los receptores asignados a la orden
                        consultarReceptoresOrden()

                        '// Consultar órden SAE
                        'consultarOrdenSAE()

                        '// Calcular días vigencia
                        mlogRecalcularFechas = False 'CCM20120305
                        Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)

                        ConsultarSaldoOrden()
                    Else
                        If mlogDuplicarOrden = False Then
                            '-- Se está ingresando una nueva orden pero no por la opción de duplicar
                            dcProxy.BeneficiariosOrdens.Clear()
                            dcProxy1.ReceptoresOrdens.Clear()
                            'SLB20140204 Cuando no hay registros en la tabla de ordenes de MILA
                            ListaReceptoresOrdenes = dcProxy1.ReceptoresOrdens

                            _mlogOpcionActiva = True
                        End If
                    End If

                    '-- Propiedades que tienen el estado de la orden
                    Me.EstadoOrden = _OrdenSelected.Estado
                    Me.EstadoOrdenLeo = _OrdenSelected.EstadoLEO

                    asignarHoraVigencia(_OrdenSelected.HoraVigencia, True)
                End If

                TabSeleccionado = 0
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Falló la selección de la orden actual", Me.ToString, "OrdenSelected", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioNegocio)
            End Try

            MyBase.CambioItem("OrdenSelected")
        End Set
    End Property


    Private _VisibleMakerAndCheker As Visibility = Visibility.Collapsed
    Public Property VisibleMakerAndCheker() As Visibility
        Get
            Return _VisibleMakerAndCheker
        End Get
        Set(ByVal value As Visibility)
            _VisibleMakerAndCheker = value
            MyBase.CambioItem("VisibleMakerAndCheker")
        End Set
    End Property
    Private _OrdenSelectedcl As New OyDOrdenes.Orden_MI
    Public Property OrdenSelectedcl() As OyDOrdenes.Orden_MI
        Get
            Return _OrdenSelectedcl
        End Get
        Set(ByVal value As OyDOrdenes.Orden_MI)
            _OrdenSelectedcl = value
            MyBase.CambioItem("OrdenSelectedcl")
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
                'objValidarDiaHabil = lo.Entities.FirstOrDefault
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
        A2Utilidades.Mensajes.mostrarMensaje("La Fecha de Elebaroción de la orden (" + _OrdenSelected.FechaOrden.Date.ToLongDateString + ") es un día no hábil", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        _OrdenSelected.FechaOrden = pdtmFechaOrden
        IsBusy = False
    End Sub

    ''' <summary>
    ''' SLB - Verificar de la fecha de elaboración de la Orden es Valida, invoca el SP uspOyDNet_Ordenes_CalcularDiasHabiles
    ''' </summary>
    ''' <param name="plogGuardarOrden"></param>
    ''' <remarks>SLB20120704</remarks>
    Public Sub VerificarFechaValida(Optional ByVal plogGuardarOrden As Boolean = False)
        Try
            If Editando Then
                If _OrdenSelected.FechaOrden.Date > Now.Date Then
                    A2Utilidades.Mensajes.mostrarMensaje("La fecha de elaboración de la orden debe ser menor o igual a la fecha actual ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    OrdenSelected.FechaOrden = Now
                    IsBusy = False
                    Exit Sub
                End If

                Dim Accion As String = String.Empty
                If plogGuardarOrden Then
                    Accion = "GuardarOrden"
                End If
                dcProxy.ValidarDiaHabils.Clear()
                dcProxy.Load(dcProxy.ValidarDiaHabilQuery(Me._OrdenSelected.FechaOrden.Date, Program.Usuario, Program.HashConexion), AddressOf TerminoVerificarFechaValida, Accion)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar la fecha de elaboracion de la orden", Me.ToString(), "VerificarFechaValida", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub calcularFechaRecepcion(Optional ByVal plogGuardarOrden As Boolean = False)
        Try
            If Editando Then
                'SLB Manejo de la fecha de recepción
                If IsNothing(_OrdenSelected.FechaRecepcion) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor seleccione una fecha de toma", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    _OrdenSelected.FechaRecepcion = CType(_OrdenSelected.FechaRecepcion, Date).AddHours(_OrdenSelected.FechaSistema.Hour).AddMinutes(_OrdenSelected.FechaSistema.Minute).AddSeconds(_OrdenSelected.FechaSistema.Second)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar la fecha de recepción de la orden", Me.ToString(), "calcularFechaRecepcion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Este procedimiento se ejecuta cuando el usuario va a guardar la orden y se lanza la validación de la fecha de la orden (elaboración) y de vigencia de la orden, además 
    ''' la fecha de emisión y vencimiento del título
    ''' </summary>
    ''' 
    Private Sub calcularDiasHabilesGuardarCompleted(ByVal lo As LoadOperation(Of OyDOrdenes.ValidarFecha))
        Try
            If calcularDiasHabilesValidar(lo) Then
                validarOrden()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al completar la validación de los días de vigencia para guardar la orden", Me.ToString(), "calcularDiasHabilesGuardarCompleted", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
            'Finally
            '    IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Este procedimiento se ejecuta cuando se lanza la validación de la fecha de la orden (elaboración) y de vigencia de la orden, además 
    ''' la fecha de emisión y vencimiento del título, pero siempre y cuando no se esté guardando la orden
    ''' </summary>
    ''' 
    Private Sub calcularDiasHabilesCompleted(ByVal lo As LoadOperation(Of OyDOrdenes.ValidarFecha))
        Try
            calcularDiasHabilesValidar(lo)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al completar la validación de los días de vigencia al editar la orden", Me.ToString(), "calcularDiasHabilesCompleted", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
            'Finally
            '    IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Este procedimiento valida el resultado recibido desde el servidor cuando se modifican las fechas de la orden y/o vigencia de la orden y las 
    ''' fechas de emisión y vencimiento del título
    ''' </summary>
    ''' 
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
                                A2Utilidades.Mensajes.mostrarMensaje("La fecha de vigencia no puede ser menor o igual a la fecha de cierre del sistema (" & objDatos.FechaCierre.ToLongDateString() & ")." & vbNewLine & vbNewLine & "Por favor modifique la fecha de vigencia de la orden para que sea posterior a la fecha de cierre.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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
                            If objDatos.TipoCalculo.ToLower() = MSTR_CALCULAR_DIAS_TITULO Then
                                _OrdenSelected.FechaVencimiento = objDatos.FechaFinal
                            Else
                                _OrdenSelected.VigenciaHasta = objDatos.FechaFinal
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
                                    If IsNothing(OrdenSelected.VigenciaHasta) Then
                                        _mintDiasVigencia = 0
                                    Else
                                        ' Se suma uno porque se deben incluir la fecha de la orden y la fecha de vigencia
                                        _mintDiasVigencia = DateDiff(DateInterval.Day, _OrdenSelected.FechaOrden, CType(OrdenSelected.VigenciaHasta, Date)) + 1
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
    ''' 
    Private Function calcularDiasRango(ByVal pdtmFechaDesde As Date, ByVal pdtmFechaHasta As Date, Optional ByVal plogIncluirExtremos As Boolean = True) As Integer
        Dim intDias As Integer
        Try
            intDias = DateDiff(DateInterval.Day, pdtmFechaDesde, pdtmFechaHasta) + IIf(plogIncluirExtremos, 1, 0)
        Catch ex As Exception
            intDias = 0
        End Try

        Return (intDias)
    End Function

    Private Sub TerminoVerificarOrdenModificable(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenModificable))
        Dim objDatos As OyDOrdenes.OrdenModificable
        Dim logEditar As Boolean = False
        Dim strAccion As String = String.Empty
        Dim strMsg As String = String.Empty

        Try
            If Not lo.HasError Then
                objDatos = lo.Entities.FirstOrDefault
                strAccion = lo.UserState.ToString.ToLower

                If Not objDatos Is Nothing Then
                    If objDatos.Modificable Then
                        If objDatos.UltimaModificacion.Equals(_OrdenSelected.FechaActualizacion) Then
                            logEditar = True
                        Else
                            'C1.Silverlight.C1MessageBox.Show("La orden fue modificada después que se realizó la consulta para desplegarla en pantalla. Debe actualizarla para poder modificarla." & vbNewLine & vbNewLine & "¿Desea refrescar los datos de las ordenes actuales?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarRefrescarOrdenes)
                            mostrarMensajePregunta("La orden fue modificada después que se realizó la consulta para desplegarla en pantalla. Debe actualizarla para poder modificarla." & vbNewLine & vbNewLine & "¿Desea refrescar los datos de las ordenes actuales?",
                                                   Program.TituloSistema,
                                                   "REFRESCARORDEN",
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
                    If Not IsNothing(_OrdenSelected) Then
                        If objDatos.TieneLiquidaciones And (_OrdenSelected.Estado = "P" Or _OrdenSelected.Estado = "M") Then '
                            mostrarMensajePregunta("¿Desea registrar la orden en el libro de órdenes?",
                                                   Program.TituloSistema,
                                                   "LIBROORDENES",
                                                   AddressOf TerminoPreguntarLibroOrdenes, False)
                        Else
                            Editando = True
                            EditandoRegistro = True
                            mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR

                            DeshabilitarBotones()

                        End If
                    Else
                        mstrAccionOrden = String.Empty
                    End If
                Else
                    'C1.Silverlight.C1PromptBox.Show("Comentario para la anulación de la orden: ", Program.TituloSistema,Program.Usuario, Program.HashConexion, AddressOf TerminoComentariosAnulacion)
                    mostrarMensajePregunta("Comentario para la anulación de la orden: ",
                                           Program.TituloSistema,
                                           "ANULARORDEN",
                                           AddressOf TerminoComentariosAnulacion, True, "¿Anular orden?", False, True, True, False)
                End If
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
                dblValorCantidadLibroOrden = _OrdenSelected.Cantidad
                Editando = True
                DeshabilitarBotones()
            Else
                MyBase.RetornarValorEdicionNavegacion()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del libro de ordenes.", Me.ToString(), "TerminoPreguntarLibroOrdenes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerOrdenesPorDefecto_Completed(ByVal lo As LoadOperation(Of OyDOrdenes.Orden_MI))
        If Not lo.HasError Then
            OrdenPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Orden por defecto",
                                             Me.ToString(), "TerminoTraerOrdenPorDefecto_Completed", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    'Private Sub TerminoTraerOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.Orden))
    Private Sub TerminoTraerOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.Orden_MI))
        Try
            If Not lo.HasError Then

                '// Este indicador ayuda a controla la ejecución de consultas inutiles durante el ingreso especialmente
                mstrAccionOrden = String.Empty

                If dcProxy.Orden_MIs.Count > 0 Then
                    '// Antes de asignar la colección se verifica el maker and checker para asegurar que la orden seleccionada ya esté actualizada
                    cambiarManejaMakerAndChecker(dcProxy.Orden_MIs.Last.ManejaMakerAndChecker)

                    '// Se activan o inactiva acá estos botónes porque siempre que se guarda se actualiza la lista de órdenes
                    ActivarDuplicarOrden = True
                    ActivarEnvioSAE = True
                    ActivarSeleccionTipoOrden = True
                Else
                    '// Si no hay registros se desativa el maker and checker
                    cambiarManejaMakerAndChecker(False)

                    '// Se activan o inactiva acá estos botónes porque siempre que se guarda se actualiza la lista de órdenes
                    ActivarDuplicarOrden = False
                    ActivarEnvioSAE = False
                    ActivarSeleccionTipoOrden = False
                End If

                ListaOrdenes = dcProxy.Orden_MIs

                If dcProxy.Orden_MIs.Count = 0 AndAlso (lo.UserState.ToString.ToLower() = MSTR_ACCION_BUSCAR Or lo.UserState.ToString.ToLower() = MSTR_ACCION_FILTRAR) Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontraron registros que cumplan con las condiciones de indicadas", Program.TituloSistema)
                    MyBase.CambiarALista()
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

    Private Sub TerminoTraerBeneficiariosOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.BeneficiariosOrden))
        If Not lo.HasError Then
            ListaBeneficiariosOrdenes = dcProxy.BeneficiariosOrdens
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BeneficiariosOrdenes",
                                             Me.ToString(), "TerminoTraerBeneficiariosOrdenes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerReceptoresOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.ReceptoresOrden))
        If Not lo.HasError Then
            ListaReceptoresOrdenes = dcProxy1.ReceptoresOrdens
            'SLB20121130 Se adiciona esta logica para saber si los receptores de la orden duplicada se inactivaron
            If lo.UserState.ToString = "verificarReceptoresDuplicar" And ListaReceptoresOrdenes.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Receptor inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresOrdenes",
                                             Me.ToString(), "TerminoTraerReceptoresOrdenes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerOrdenantes(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorOrdenantes))
        If Not lo.HasError Then
            Ordenantes = mdcProxyUtilidad01.BuscadorOrdenantes.ToList

            If Not _OrdenSelected.IDOrdenante Is Nothing Then
                If (From obj In _Ordenantes Where obj.IdOrdenante = _OrdenSelected.IDOrdenante Select obj).ToList.Count > 0 Then
                    OrdenanteSeleccionado = (From cta In _Ordenantes Where cta.IdOrdenante.Trim() = _OrdenSelected.IDOrdenante.Trim() Select cta).ToList.ElementAt(0)
                Else
                    'SLB20121205 Cuando el Ordenante se encuentra Inactivo se debe mostrar en el pantalla, a pesar de que en la lista no exista.
                    'Dim strDescripcionOrdenante = LTrim(_OrdenSelected.IDOrdenante) & " - " & _OrdenSelected.NombreOrdenante.ToString
                    'Ordenantes.Add(New OYDUtilidades.BuscadorOrdenantes With {.DescripcionOrdenante = strDescripcionOrdenante, .IdOrdenante = "-1"})
                    'OrdenanteSeleccionado = Ordenantes.Last
                    'Ordenantes.Remove(Ordenantes.Last)
                    OrdenanteSeleccionado = Nothing
                    _OrdenSelected.IDOrdenante = Nothing
                End If
            Else
                If _Ordenantes.Count = 1 Then
                    OrdenanteSeleccionado = Ordenantes.First
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes",
                                             Me.ToString(), "TerminoTraerOrdenantes", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCuentasDeposito(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorCuentasDeposito))
        If Not lo.HasError Then
            CuentasDeposito = mdcProxyUtilidad02.BuscadorCuentasDepositos.ToList
            If Not Editando Then
                If Not _OrdenSelected.CuentaDeposito Is Nothing Then
                    If (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _OrdenSelected.CuentaDeposito Select cta).ToList.Count > 0 Then
                        CtaDepositoSeleccionada = (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _OrdenSelected.CuentaDeposito Select cta).ToList.ElementAt(0)
                    Else
                        CtaDepositoSeleccionada = Nothing
                    End If
                Else
                    'AJUSTE20141017SV - Se adiciona lógica para que seleccione también el dato cuando solo venga la ubicación del título 
                    If _CuentasDeposito.Count = 1 Then
                        CtaDepositoSeleccionada = CuentasDeposito.First
                    ElseIf (From cta In CuentasDeposito Where IsNothing(cta.NroCuentaDeposito) And cta.Deposito = _OrdenSelected.UBICACIONTITULO Select cta).ToList.Count > 0 Then
                        CtaDepositoSeleccionada = (From cta In CuentasDeposito Where IsNothing(cta.NroCuentaDeposito) And cta.Deposito = _OrdenSelected.UBICACIONTITULO Select cta).ToList.ElementAt(0)
                    End If
                End If

                If _CuentasDeposito.Count = 1 Then
                    CtaDepositoSeleccionada = CuentasDeposito.First
                End If
            Else
                If (From cta In CuentasDeposito Select cta).ToList.Count = 1 Then
                    CtaDepositoSeleccionada = CuentasDeposito.First
                Else
                    CtaDepositoSeleccionada = Nothing
                End If
            End If
            'AJUSTE20141017SV: corrección para estabblecer la cuenta por defecto
            'If Me.ClaseOrden = ClasesOrden.A Then
            '    If CuentasDeposito.Where(Function(i) i.Deposito = "D" And i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).Count = 1 Then
            '        CtaDepositoSeleccionada = CuentasDeposito.Where(Function(i) i.Deposito = "D" And i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).First
            '    End If
            'Else
            '    If CuentasDeposito.Where(Function(i) i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).Count = 1 Then
            '        CtaDepositoSeleccionada = CuentasDeposito.Where(Function(i) i.NroCuentaDeposito <> 0 And Not i.NroCuentaDeposito Is Nothing).First
            '    End If
            'End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito",
                                             Me.ToString(), "TerminoTraerCuentasDeposito", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoEnrutarOrdenSAE(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenSAE))
        Dim objOrdenSAE As OyDOrdenes.OrdenSAE
        Dim logConsultarSAE As Boolean = False

        Try
            Me._mstrNroOrdenSAE = String.Empty
            If Not lo.HasError Then
                If dcProxy1.OrdenSAEs.ToList.Count > 0 Then
                    objOrdenSAE = dcProxy1.OrdenSAEs.First
                    If objOrdenSAE.TipoMensaje > 1 Then
                        Me._mstrEstadoSAE = String.Empty
                        A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema & " - Error en enrutamiento", A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    ElseIf objOrdenSAE.TipoMensaje > 0 Then
                        Me._mstrEstadoSAE = objOrdenSAE.EstadoSAE
                        A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema & " - Advertencias en enrutamiento", A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Orden enrutada exitosamente a través de SAE", Program.TituloSistema)
                        Me._mstrEstadoSAE = objOrdenSAE.EstadoSAE
                    End If
                Else
                    Me._mstrEstadoSAE = String.Empty
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir respueta del enrutamiento de la orden por SAE", Me.ToString(), "TerminoEnrutarOrdenSAE", Program.TituloSistema, Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????

                Me._mstrEstadoSAE = String.Empty
            End If

            MyBase.CambioItem("NroOrdenSAE")
            MyBase.CambioItem("EstadoSAE")

            'If logConsultarSAE Then
            '    consultarOrdenSAE()
            'End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el enrutamiento de la orden por SAE", Me.ToString(), "TerminoEnrutarOrdenSAE", Program.TituloSistema, Program.Maquina, ex)
        Finally
            Me.IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarOrdenSAE(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenSAE))
        Dim objOrdenSAE As OyDOrdenes.OrdenSAE
        Try
            If Not lo.HasError Then
                If dcProxy1.OrdenSAEs.ToList.Count > 0 Then
                    objOrdenSAE = dcProxy1.OrdenSAEs.First
                    Me._mstrNroOrdenSAE = objOrdenSAE.NroOrdenSAE
                    Me._mstrEstadoSAE = objOrdenSAE.EstadoSAE

                    Me.mlogPuedeEnrutarSAE = objOrdenSAE.SAEUsrTienePermisos
                    Me.mlogSAEActivoClaseOrden = objOrdenSAE.SAEActivoClaseOrden
                    Me.mlogSAEActivo = objOrdenSAE.SAEActivo

                    MyBase.CambioItem("SAEActivo")
                Else
                    Me._mstrNroOrdenSAE = String.Empty
                    Me._mstrEstadoSAE = String.Empty

                    Me.mlogSAEActivo = False
                    Me.mlogSAEActivoClaseOrden = False
                    Me.mlogPuedeEnrutarSAE = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la consulta de la orden en SAE", Me.ToString(), "TerminoConsultarOrdenSAE", Program.TituloSistema, Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????

                Me._mstrNroOrdenSAE = String.Empty
                Me._mstrEstadoSAE = String.Empty

                Me.mlogSAEActivo = False
                Me.mlogSAEActivoClaseOrden = False
                Me.mlogPuedeEnrutarSAE = False
            End If

            MyBase.CambioItem("NroOrdenSAE")
            MyBase.CambioItem("EstadoSAE")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de la orden en SAE", Me.ToString(), "TerminoConsultarOrdenSAE", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando termina de guardar los cambios de la orden.
    ''' </summary>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)

        Dim strMsg As String = String.Empty
        Try
            IsBusy = True
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
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                                   Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                    So.MarkErrorAsHandled()
                End If
                IsBusy = False
                Exit Try
            Else
                Editando = False
                EditandoRegistro = False
                habilitarFechaElaboracion = False
                ' (SLB) Mostar mensajes cuando no se utiliza al Maker and Checker
                If Not _mlogManejaMakerAndChecker Then
                    Select Case So.UserState
                        Case "I"
                            A2Utilidades.Mensajes.mostrarMensaje("Se ha creado correctamente la Orden Nro " & CStr(_OrdenSelected.NroOrden), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                            MyBase.QuitarFiltroDespuesGuardar()
                            MyBase.FiltroVM = String.Empty
                            Filtrar()
                        Case "U"
                            A2Utilidades.Mensajes.mostrarMensaje("Se ha modificado correctamente la Orden Nro " & CStr(_OrdenSelected.NroOrden), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    End Select
                    HabilitarBotones()
                    'ActivarDuplicarOrden = True
                    'ActivarEnvioSAE = True
                    'ActivarSeleccionTipoOrden = True
                Else
                    MyBase.QuitarFiltroDespuesGuardar()
                    MyBase.FiltroVM = String.Empty
                    '(SLB) Cuando se utiliza el Maker and Checker se debe recargar la lista de ordenes para refresacar con los cambios realizados en base de datos.
                    Filtrar()
                End If
                '// Actualizar la lista de ordenes para refresacar con los cambios realizados en base de datos. 
                'Filtrar()

            End If

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", Me.ToString(), "TerminoSubmitChanges", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el proceso de autorización de la orden finaliza
    ''' </summary>
    Private Sub TerminoAutorizarOrden(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenSAE))
        Dim objOrdenSAE As OyDOrdenes.OrdenSAE
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se cambió el estado de la orden porque se presentó un problema durante el proceso.", Me.ToString(), "Autorizar_OrdenCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
                IsBusy = False
            Else
                objOrdenSAE = dcProxy.OrdenSAEs.First
                If objOrdenSAE.TipoMensaje > 1 Then
                    A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    IsBusy = False
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(objOrdenSAE.Msg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    MyBase.QuitarFiltroDespuesGuardar()
                    MyBase.FiltroVM = String.Empty
                    Filtrar()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la aprobación de la orden.", Me.ToString(), "TerminoAutorizarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
            'Finally
            '    IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarLiquidacionesOrden(ByVal lo As LoadOperation(Of OyDOrdenes.LiquidacionesOrden))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se cargaron las liquidaciones de la orden porque se presentó un problema durante el proceso.", Me.ToString(), "Autorizar_OrdenCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
                IsBusy = False
                IsBusyLiquidaciones = False
            Else
                LiquidacionesOrden = dcProxyConsultas.LiquidacionesOrdens
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir las liquidaciones de la orden", Me.ToString(), "TerminoConsultarLiquidacionesOrden", Program.TituloSistema, Program.Maquina, ex)
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
                ElseIf objOrdenSAE.Msg.Contains("OrdenAnuladaSinMaker,") Then
                    Dim Mensaje = Split(objOrdenSAE.Msg, ",")
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(1), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    MyBase.QuitarFiltroDespuesGuardar()
                    MyBase.FiltroVM = String.Empty
                    Filtrar()
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("La anulación de la orden está pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    MyBase.QuitarFiltroDespuesGuardar()
                    MyBase.FiltroVM = String.Empty
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
                        dcProxy1.ReceptoresOrdens.Clear()
                        ListaReceptoresOrdenes = dcProxy1.ReceptoresOrdens
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
                dcProxy1.ReceptoresOrdens.Clear()
                ListaReceptoresOrdenes = dcProxy1.ReceptoresOrdens
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
                    If Me._OrdenSelected.Objeto <> "IC" Then
                        If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                            A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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
                        Me.ReceptorTomaSeleccionado = lo.Entities.ToList.Item(0)
                        Me._mobjReceptorTomaSeleccionadoAntes = Me._ReceptorTomaSeleccionado
                End Select
            Else
                Select Case strTipoItem.ToLower()
                    Case "receptores"
                        Me.ReceptorTomaSeleccionado = Nothing
                        Me._mobjReceptorTomaSeleccionadoAntes = Me._ReceptorTomaSeleccionado
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarGenericoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si el usuario si aprueba la orden con una excepción RDIP
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub validarExcepcionRDIPAprobar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                aprobarOrden(True)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el riesgo en la aprobación de la orden", Me.ToString(), "validarExcepcionRDIPAprobar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si el usuario guarda la orden con una excepción RDIP
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub validarExcepcionRDIPGuardar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                guardarOrden()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el guardado de la orden", Me.ToString(), "validarExcepcionRDIPGuardar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si se refrescan los datos de las órdenes que están en pantalla
    ''' </summary>
    Private Sub validarRefrescarOrdenes(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                Me.FiltroVM = Me.OrdenSelected.NroOrden
                Filtrar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar si se refrescan los datos de las órdenes en pantalla", Me.ToString(), "validarRefrescarOrdenes", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si el usuario aprueba la orden 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub validarAprobarOrden(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If objResultado.DialogResult Then
                aprobarOrdenConfirmado()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar la aprobación de la orden", Me.ToString(), "validarAprobarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando retorna de confirmar si el usuario rechaza la orden
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub validarRechazoOrden(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                aprobarOrden(False)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el rechazo de la orden", Me.ToString(), "validarRechazoOrden", Program.TituloSistema, Program.Maquina, ex)
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
                dcProxy.Load(dcProxy.AnularOrden_MIQuery(_OrdenSelected.Clase, _OrdenSelected.Tipo, _OrdenSelected.NroOrden, _OrdenSelected.Version, objResultado.Observaciones, "", Program.Usuario, Program.HashConexion), AddressOf TerminoAnularOrden, MSTR_ACCION_ANULAR)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el rechazo de la orden", Me.ToString(), "validarRechazoOrden", Program.TituloSistema, Program.Maquina, ex)
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
                configurarNuevaOrden(_OrdenSelected, True, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el duplicar una orden", Me.ToString(), "validarDuplicarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

#End Region

#Region "Métodos"

    Private Sub HabilitarBotones()
        ActivarDuplicarOrden = True
        ActivarEnvioSAE = True
        ActivarSeleccionTipoOrden = True
        'ActivarModInstruccion = True
    End Sub

    Private Sub DeshabilitarBotones(Optional ByVal strOpcion As String = "")
        ActivarDuplicarOrden = False
        ActivarEnvioSAE = False
        ActivarSeleccionTipoOrden = False
        'If Not strOpcion = "ModificarInstrucciones" Then
        '    ActivarModInstruccion = False
        'End If
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
                Try
                    mstrTipoLimitePorDef = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_MI_TIPO_LIMITE_DEFAULT" Select obj).First.Valor
                    If IsNothing(mstrTipoLimitePorDef) Then
                        mstrTipoLimitePorDef = "M"
                    End If
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "OYDNet_ORDEN_MI_DUPLICAR_DATOS_LEO" Select obj).First.Valor
                    If strValor = "S" Then
                        mlogDuplicarDatosLeo = True
                    Else
                        mlogDuplicarDatosLeo = False
                    End If
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
                    mstrValidarCuidadSeteo = (From obj In objListaParametros Where obj.Parametro = "ValidarCiudadSeteo" Select obj).First.Valor.ToUpper
                Catch ex As Exception
                    '-- Si hay error se trabaja con el valor por defecto del parámetro
                End Try

                Try
                    strValor = (From obj In objListaParametros Where obj.Parametro = "CUENTADEPOSITOORDENES" Select obj).First.Valor
                    If strValor = "1" Or strValor = "SI" Then
                        logValidarCuentaDeposito = True
                    Else
                        logValidarCuentaDeposito = False
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
            If _OrdenSelected Is Nothing Then
                Exit Sub
                'ElseIf mstrAccionOrden.Equals(String.Empty) And _mintDiasVigencia > 0 Then 'CCM20120306 - Desactivar esta condición porque no recalcula los días de vigencia
                'Exit Sub
            ElseIf pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO And Me.ClaseOrden = ClasesOrden.A Then
                '/ Los días de vencimiento del título solamente se calculan para renta fija
                Exit Sub
            ElseIf (IsNothing(_OrdenSelected.FechaOrden) Or (IsNothing(_OrdenSelected.VigenciaHasta) And pintDias <= 0)) And pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_ORDEN Then
                '/ Para calcular la fecha de vigencia o los días de vigencia de la orden es necesario tener la fecha de la orden y los días de vigencia o la fecha de vigencia respectivamente
                Exit Sub
            ElseIf (IsNothing(_OrdenSelected.FechaVencimiento) Or (IsNothing(_OrdenSelected.FechaEmision) And pintDias <= 0)) And pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO Then
                '/ Para calcular la fecha de vencimiento o los días de plazo del título es necesario tener la fecha de emisión y el plazo o la fecha de vencimiento respectivamente
                Exit Sub
                '(SLB) Se adiciona 
            ElseIf Me._OrdenSelected.FechaOrden.Date > Now.Date Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de elaboración de la orden debe ser menor o igual a la fecha actual.", Program.TituloSistema)
                _OrdenSelected.FechaOrden = Now.Date
                Exit Sub
            ElseIf mdtmFechaCierreSistema >= _OrdenSelected.VigenciaHasta Then
                _mlogFechaCierreSistema = True
                'CambiarAForma()
                'A2Utilidades.Mensajes.mostrarMensaje("La fecha de vigencia de la orden no puede ser menor a la fecha de cierre del sistema (" & mdtmFechaCierreSistema.ToLongDateString() & ")." & vbNewLine & vbNewLine & "Por favor modifique la fecha de elaboración de la orden para que sea posterior a la fecha de cierre.", Program.TituloSistema)
                If Editando = False Then
                    Exit Sub
                End If
            ElseIf _OrdenSelected.VigenciaHasta < _OrdenSelected.FechaOrden.Date Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de la orden no puede ser mayor a la fecha de vigencia." & vbNewLine & vbNewLine & "Por favor modifique la fecha de elaboración o la fecha de vigencia de la orden.", Program.TituloSistema)
                Exit Sub
            ElseIf Not IsNothing(_OrdenSelected.FechaEmision) And Not IsNothing(_OrdenSelected.FechaVencimiento) Then
                If _OrdenSelected.FechaVencimiento < _OrdenSelected.FechaEmision Then
                    A2Utilidades.Mensajes.mostrarMensaje("La fecha de emisión del título no puede ser mayor a la fecha de vencimiento." & vbNewLine & vbNewLine & "Por favor modifique la fecha de emisión o la fecha de vencimiento del título.", Program.TituloSistema)
                    Exit Sub
                End If
            End If

            'IsBusy = True

            dcProxy.ValidarFechas.Clear()
            If pintDias <= 0 Then
                ' Calcular los días al vencimiento de la orden a partir de la fecha de elaboración y vencimiento
                If pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO Then
                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaEmision, _OrdenSelected.FechaVencimiento, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    Else
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaEmision, _OrdenSelected.FechaVencimiento, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    End If
                Else
                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaOrden.Date, _OrdenSelected.VigenciaHasta, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    Else
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaOrden.Date, _OrdenSelected.VigenciaHasta, -1, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_DIAS)
                    End If
                End If
            Else
                ' Calcular la fecha de vencimiento de la orden a partir de la fecha de elaboración y los días al vencimiento
                If pstrTipoCalculo.ToLower = MSTR_CALCULAR_DIAS_TITULO Then

                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaEmision, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    Else
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaEmision, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    End If
                Else
                    If plogGuardarOrden Then
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaOrden.Date, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesGuardarCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    Else
                        dcProxy.Load(dcProxy.CalcularDiasHabilesQuery(pstrTipoCalculo, _OrdenSelected.FechaOrden.Date, Nothing, pintDias, Program.Usuario, Program.HashConexion), AddressOf calcularDiasHabilesCompleted, MSTR_ACCION_CALCULAR_FECHA)
                    End If
                End If
                'If pstrTipoCalculo.ToLower = MSTR_ACCION_CALCULAR_FECHA Then

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
    Private Sub configurarNuevaOrden(ByVal pobjDatosOrden As OyDOrdenes.Orden_MI, ByVal plogDuplicar As Boolean, ByVal plogCompra As System.Nullable(Of Boolean))

        Dim intDias As Integer = 0

        Try
            mlogRecalcularFechas = False ' 'CCM20120305 - Desactivar cálculo de fechas 

            '// Indicar que se está ingresando una nueva orden para no ejecutar algunas consultas y ejecutar otras que son solamente para el ingreso
            mstrAccionOrden = MSTR_MC_ACCION_INGRESAR

            Dim NewOrden As New OyDOrdenes.Orden_MI

            If plogDuplicar Then
                NewOrden.Tipo = pobjDatosOrden.Tipo
                verificarReceptoresOrdenDuplicar()
            Else
                ComitenteSeleccionado = Nothing
                NemotecnicoSeleccionado = Nothing
                ReceptorTomaSeleccionado = Nothing

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

            'SLB 20121126
            NewOrden.FechaActualizacion = Now()

            'SLB
            If plogDuplicar Then
                NewOrden.Moneda = pobjDatosOrden.Moneda
                NewOrden.TasaConversion = pobjDatosOrden.TasaConversion
                NewOrden.TasaConversionDolares = pobjDatosOrden.TasaConversionDolares
                NewOrden.PrecioInferior = pobjDatosOrden.PrecioInferior
                NewOrden.ComisionPesos = pobjDatosOrden.ComisionPesos
                NewOrden.FechaCumplimiento = pobjDatosOrden.FechaCumplimiento
                NewOrden.EnPesos = pobjDatosOrden.EnPesos
                NewOrden.DividendoCompra = pobjDatosOrden.DividendoCompra
            Else
                NewOrden.Moneda = OrdenPorDefecto.Moneda
                NewOrden.TasaConversion = OrdenPorDefecto.TasaConversion
                NewOrden.TasaConversionDolares = 1
                NewOrden.PrecioInferior = OrdenPorDefecto.PrecioInferior
                NewOrden.ComisionPesos = pobjDatosOrden.ComisionPesos
            End If

            '(SLB) se comenta
            NewOrden.VigenciaHasta = OrdenPorDefecto.VigenciaHasta
            'If plogDuplicar Then
            '    NewOrden.VigenciaHasta = pobjDatosOrden.VigenciaHasta
            'Else
            '    NewOrden.VigenciaHasta = OrdenPorDefecto.VigenciaHasta
            'End If

            NewOrden.Clase = pobjDatosOrden.Clase
            NewOrden.IDComitente = pobjDatosOrden.IDComitente
            NewOrden.IDOrdenante = pobjDatosOrden.IDOrdenante
            NewOrden.Nemotecnico = pobjDatosOrden.Nemotecnico
            NewOrden.UBICACIONTITULO = pobjDatosOrden.UBICACIONTITULO
            NewOrden.CuentaDeposito = pobjDatosOrden.CuentaDeposito
            NewOrden.Cantidad = pobjDatosOrden.Cantidad
            SaldoOrden = pobjDatosOrden.Cantidad
            NewOrden.CantidadMinima = pobjDatosOrden.CantidadMinima
            NewOrden.CantidadVisible = pobjDatosOrden.CantidadVisible
            NewOrden.FechaEmision = pobjDatosOrden.FechaEmision
            NewOrden.FechaVencimiento = pobjDatosOrden.FechaVencimiento
            NewOrden.TasaNominal = pobjDatosOrden.TasaNominal
            NewOrden.TasaInicial = pobjDatosOrden.TasaInicial
            NewOrden.Modalidad = pobjDatosOrden.Modalidad
            NewOrden.Mercado = pobjDatosOrden.Mercado
            NewOrden.TasaCompraVenta = pobjDatosOrden.TasaCompraVenta
            NewOrden.IndicadorEconomico = pobjDatosOrden.IndicadorEconomico
            NewOrden.PuntosIndicador = pobjDatosOrden.PuntosIndicador
            NewOrden.EfectivaInferior = pobjDatosOrden.EfectivaInferior
            NewOrden.EfectivaSuperior = pobjDatosOrden.EfectivaSuperior
            NewOrden.DiasVencimientoInferior = pobjDatosOrden.DiasVencimientoInferior
            NewOrden.DiasVencimientoSuperior = pobjDatosOrden.DiasVencimientoSuperior
            NewOrden.FormaPago = pobjDatosOrden.FormaPago
            NewOrden.Version = pobjDatosOrden.Version
            NewOrden.Ordinaria = pobjDatosOrden.Ordinaria
            NewOrden.Objeto = pobjDatosOrden.Objeto
            NewOrden.Repo = pobjDatosOrden.Repo
            NewOrden.CondicionesNegociacion = pobjDatosOrden.CondicionesNegociacion
            NewOrden.Instrucciones = pobjDatosOrden.Instrucciones
            NewOrden.Notas = pobjDatosOrden.Notas
            'NewOrden.TipoInversion = pobjDatosOrden.TipoInversion
            NewOrden.TipoInversion = String.Empty
            NewOrden.TipoTransaccion = pobjDatosOrden.TipoTransaccion
            NewOrden.Ejecucion = pobjDatosOrden.Ejecucion
            NewOrden.Duracion = pobjDatosOrden.Duracion
            NewOrden.HoraVigencia = pobjDatosOrden.HoraVigencia
            NewOrden.CostoAdicionalesOrden = pobjDatosOrden.CostoAdicionalesOrden
            NewOrden.IdBolsa = pobjDatosOrden.IdBolsa
            NewOrden.Eca = pobjDatosOrden.Eca
            NewOrden.OrdenEscrito = pobjDatosOrden.OrdenEscrito
            NewOrden.ConsecutivoSwap = pobjDatosOrden.ConsecutivoSwap
            NewOrden.NegocioEspecial = pobjDatosOrden.NegocioEspecial
            NewOrden.ComisionPactada = pobjDatosOrden.ComisionPactada
            NewOrden.IDProducto = pobjDatosOrden.IDProducto
            NewOrden.IdCiudadSeteo = pobjDatosOrden.IdCiudadSeteo

            '/ Estos campos siempre se inicializan con los por defecto
            NewOrden.TieneOrdenRelacionada = OrdenPorDefecto.TieneOrdenRelacionada
            NewOrden.DescripcionOrden = OrdenPorDefecto.DescripcionOrden
            NewOrden.NombreCliente = OrdenPorDefecto.NombreCliente
            NewOrden.SitioIngreso = OrdenPorDefecto.SitioIngreso
            NewOrden.IpOrigen = OrdenPorDefecto.IpOrigen
            NewOrden.AccionMakerAndChecker = OrdenPorDefecto.AccionMakerAndChecker
            NewOrden.EstadoMakerChecker = OrdenPorDefecto.EstadoMakerChecker
            NewOrden.EstadoLEO = OrdenPorDefecto.EstadoLEO
            NewOrden.EstadoOrdenBus = OrdenPorDefecto.EstadoOrdenBus
            NewOrden.Estado = OrdenPorDefecto.Estado

            '/ CCM20120305: Para acciones esto campo se inicializan con el por defecto o el del parámetro OYDNet_ORDEN_TIPO_LIMITE_DEFAULT
            If pobjDatosOrden.Clase = ClasesOrden.A.ToString Then
                If Not mstrTipoLimitePorDef.Trim().Equals(String.Empty) Then
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
                    NewOrden.PrecioStop = pobjDatosOrden.PrecioStop
                Else
                    NewOrden.Precio = OrdenPorDefecto.Precio
                    NewOrden.PrecioStop = OrdenPorDefecto.PrecioStop
                End If
            Else
                NewOrden.Precio = pobjDatosOrden.Precio
                NewOrden.PrecioStop = pobjDatosOrden.PrecioStop
            End If

            '/ CCM20120305: Estos campos se inicializan con los por defecto o se mantienen con los de la orden según parámetro OYDNet_ORDEN_DUPLICAR_DATOS_LEO
            If mlogDuplicarDatosLeo Then
                NewOrden.CanalRecepcion = pobjDatosOrden.CanalRecepcion
                NewOrden.FechaRecepcion = pobjDatosOrden.FechaRecepcion
                'NewOrden.UsuarioOperador = pobjDatosOrden.UsuarioOperador
                NewOrden.MedioVerificable = pobjDatosOrden.MedioVerificable
                NewOrden.IdReceptorToma = pobjDatosOrden.IdReceptorToma
                NewOrden.NroExtensionToma = pobjDatosOrden.NroExtensionToma
            Else
                NewOrden.CanalRecepcion = OrdenPorDefecto.CanalRecepcion
                NewOrden.FechaRecepcion = OrdenPorDefecto.FechaRecepcion
                'NewOrden.UsuarioOperador = OrdenPorDefecto.UsuarioOperador
                NewOrden.MedioVerificable = OrdenPorDefecto.MedioVerificable
                NewOrden.IdReceptorToma = OrdenPorDefecto.IdReceptorToma
                NewOrden.NroExtensionToma = OrdenPorDefecto.NroExtensionToma
            End If

            NewOrden.UsuarioOperador = OrdenPorDefecto.UsuarioOperador

            NewOrden.Usuario = Program.Usuario
            NewOrden.UsuarioIngreso = Program.Usuario

            '// Si se está duplicando la orden se activa este indicador para no borrar la lista de receptores y beneficiarios
            mlogDuplicarOrden = plogDuplicar


            OrdenAnterior = OrdenSelected
            OrdenSelected = NewOrden
            '(SLB)
            'Me.calcularDiasOrden(OrdenesViewModel.MSTR_ACCION_CALCULAR_FECHA, 1)
            'Actualizar los días de vigencia. Se suma uno para incluir la fecha de la orden y la de vigencia
            '_mintDiasVigencia = calcularDiasRango(Me._OrdenSelected.FechaOrden, Me._OrdenSelected.VigenciaHasta)


            Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)

            If mlogDuplicarOrden Then
                VerificarFechaValida()
            Else
                If Not pobjDatosOrden.EsHabil Then
                    MostrarMensajeFechaNoValida(pobjDatosOrden.FechaOrden)
                End If
            End If

            MyBase.CambioItem("Ordenes")

            '// Se desactiva el indicador de cuplicar orden
            mlogDuplicarOrden = False

            Me._mstrNroOrdenSAE = ""
            MyBase.CambioItem("NroOrdenSAE")
            Me._mstrEstadoSAE = ""
            MyBase.CambioItem("EstadoSAE")

            DeshabilitarBotones()
            TabSeleccionadoGeneral = OrdenTabs.LEO

            Editando = True
            EditandoRegistro = True
            habilitarFechaElaboracion = _mlogHabilitarFechaElaboracion

            MyBase.CambioItem("Editando")

            logRegistroLibroOrdenes = False
            dblValorCantidadLibroOrden = 0

            If visNavegando = "Collapsed" Then
                MyBase.CambiarFormulario_Forma_Manual()
            End If

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
    ''' <summary>
    ''' Metodo para poder filtar las Ordenes de Mila, invoca el SP uspOyDNet_Ordenes_Ordenes_MI_Filtrar
    ''' </summary>
    ''' <remarks>SLB20120704_Documentado</remarks>
    Public Overrides Sub Filtrar()
        Try
            IsBusy = True
            dcProxy.Orden_MIs.Clear()
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.OrdenesFiltrar_MIQuery(Me.ClaseOrden.ToString, TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, MSTR_ACCION_FILTRAR)
            Else
                dcProxy.Load(dcProxy.OrdenesFiltrar_MIQuery(Me.ClaseOrden.ToString, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenes, MSTR_ACCION_FILTRAR)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", Me.ToString(), "Filtrar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        Try
            If Not IsNothing(objA2VMLocal) Then
                If Not IsNothing(objA2VMLocal.DiccionarioCombosA2) Then
                    If objA2VMLocal.DiccionarioCombosA2.ContainsKey("EST_MAKER_CHEKER") Then
                        Dim objListaEstadoMaker = objA2VMLocal.DiccionarioCombosA2("EST_MAKER_CHEKER")

                        For Each li In objListaEstadoMaker
                            If String.IsNullOrEmpty(li.ID) Then
                                li.ID = "VACIO"
                                li.Descripcion = "(Todos)"
                            End If
                        Next

                        ListaComboEstadoMaker = objListaEstadoMaker
                    End If

                    If objA2VMLocal.DiccionarioCombosA2.ContainsKey("ACC_MAKER_CHEKER") Then
                        Dim objListaAccionMaker = objA2VMLocal.DiccionarioCombosA2("ACC_MAKER_CHEKER")

                        For Each li In objListaAccionMaker
                            If String.IsNullOrEmpty(li.ID) Then
                                li.ID = "VACIO"
                                li.Descripcion = "(Todos)"
                            End If
                        Next

                        ListaComboAccionMaker = objListaAccionMaker
                    End If
                End If
            End If
            cb = New CamposBusquedaOrden(Me)
            cb.EstadoMakerChecker = "VACIO"
            cb.AccionMakerChecker = "VACIO"
            '(SLB) Se adiciona
            mostrarCamposMakerAndChecker(_mlogManejaMakerAndChecker)
            CambioItem("cb")
            MyBase.Buscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", Me.ToString(), "Filtrar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Metodo para poder buscar las Ordenes de Mila, invoca el SP uspOyDNet_Ordenes_Ordenes_MI_Consultar
    ''' </summary>
    ''' <remarks>SLB20120704_Documentado</remarks>
    Public Overrides Sub ConfirmarBuscar()
        Dim strMsg As String = String.Empty
        Dim strAno As String = String.Empty

        Try
            Dim strEstadoMaker As String = Nothing
            Dim strAccionMaker As String = Nothing

            If cb.EstadoMakerChecker <> "VACIO" Then
                strEstadoMaker = cb.EstadoMakerChecker
            End If
            If cb.AccionMakerChecker <> "VACIO" Then
                strAccionMaker = cb.AccionMakerChecker
            End If

            If Not (IsNothing(cb.Tipo) And IsNothing(cb.NroOrden) And IsNothing(cb.Version) And IsNothing(cb.IDComitente) And IsNothing(cb.IDOrdenante) And
                        IsNothing(cb.FechaOrden) And IsNothing(cb.Nemotecnico) And IsNothing(cb.FormaPago) And IsNothing(cb.Objeto) And IsNothing(cb.CondicionesNegociacion) And
                        IsNothing(cb.TipoInversion) And IsNothing(cb.TipoTransaccion) And IsNothing(cb.CanalRecepcion) And IsNothing(cb.MedioVerificable) And IsNothing(cb.FechaHoraRecepcion) And
                        IsNothing(cb.TipoLimite) And IsNothing(cb.VigenciaHasta) And IsNothing(cb.Ejecucion) And IsNothing(cb.Duracion) And IsNothing(cb.Estado) And IsNothing(cb.EstadoOrdenBus) And
                        IsNothing(cb.IdBolsa) And IsNothing(strEstadoMaker)) Then

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
                dcProxy.Orden_MIs.Clear()
                OrdenAnterior = Nothing
                IsBusy = True
                DescripcionFiltroVM = "" 'PENDIENTE DESCRIPCIÓN FILTRO
                cb.Clase = _mstrClaseOrden.ToString()
                'SLB - Se le adiciona el parametro de busqueda de la Bolsa (IdBolsa)
                dcProxy.Load(dcProxy.OrdenesConsultar_MIQuery(cb.Clase, cb.Tipo, cb.NroOrden, cb.Version, cb.IDComitente, cb.IDOrdenante, cb.FechaOrden, cb.FormaPago, cb.Objeto,
                                            cb.CondicionesNegociacion, cb.TipoInversion, cb.CanalRecepcion, cb.MedioVerificable,
                                            cb.FechaHoraRecepcion, cb.TipoLimite, cb.VigenciaHasta, cb.Estado, cb.EstadoOrdenBus, strEstadoMaker, strAccionMaker,
                                            cb.IdBolsa, Program.Usuario(), Program.HashConexion), AddressOf TerminoTraerOrdenes, MSTR_ACCION_BUSCAR)
                MyBase.ConfirmarBuscar()
            End If
        Catch ex As Exception
            cb = New CamposBusquedaOrden(Me)
            CambioItem("cb")
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub guardarOrden(Optional ByVal YaBuscoCuidaSeteo As Boolean = False, Optional ByVal YaValidoCuidadSeteo As Boolean = False)

        Dim strLiqAsociadas As String = String.Empty
        Dim strAccion As String = MSTR_MC_ACCION_ACTUALIZAR

        Try

            If mstrValidarCuidadSeteo = "SI" Then
                If Not YaBuscoCuidaSeteo Then
                    If IsNothing(_OrdenSelected.IdCiudadSeteo) Then
                        IsBusy = True
                        dcProxy.ConsultarCuidadSeteo(ListaReceptoresOrdenes.First.IDReceptor, Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarCuidadSeteo, "Consulta")
                        Exit Sub
                    Else
                        If Not IsNothing(CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_CIUDAD_SETEO")).Where(Function(l) l.ID = _OrdenSelected.IdCiudadSeteo)) Then
                            mstrCuidadSeteo = CType(Application.Current.Resources(Me.ListaCombosEsp), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(NombreCategoriaEnDiccionario("O_CIUDAD_SETEO")).Where(Function(l) l.ID = _OrdenSelected.IdCiudadSeteo).First.Descripcion
                        End If
                    End If
                End If

                If Not YaValidoCuidadSeteo Then
                    mostrarMensajePregunta("Ciudad Seteo de la orden: " & mstrCuidadSeteo,
                       Program.TituloSistema,
                       "Ciudad Seteo",
                       AddressOf TerminoPreguntaCiudadSeteo, False)
                    Exit Sub
                End If
            End If

            _OrdenSelected.HoraVigencia = asignarHoraVigencia("", False)

            If Not ListaOrdenes.Contains(OrdenSelected) Then
                dcProxy.RejectChanges()
                ListaOrdenes.Add(OrdenSelected)
                strAccion = MSTR_MC_ACCION_INGRESAR
            End If

            'strAccion = MSTR_MC_ACCION_INGRESAR

            IsBusy = True
            Program.VerificarCambiosProxyServidorOrdenes(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, strAccion)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la iniciar el proceso de actualización de la orden", Me.ToString(), "guardarOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarCuidadSeteo(ByVal lo As InvokeOperation(Of String))
        Try
            IsBusy = False
            If Not lo.HasError Then
                If Not IsNothing(lo.Value) Then
                    Dim res = lo.Value.Split("+")
                    If Not String.IsNullOrEmpty(res(0)) Then
                        If Versioned.IsNumeric(res(0)) Then _OrdenSelected.IdCiudadSeteo = CInt(res(0))
                    End If
                    mstrCuidadSeteo = res(1)
                End If
                guardarOrden(True)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar la ciudad de seteo.", Me.ToString(), "TerminoConsultarCuidadSeteo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar la ciudad de seteo.", Me.ToString(), "TerminoConsultarCuidadSeteo", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    Private Sub TerminoPreguntaCiudadSeteo(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                guardarOrden(True, True)
            Else
                Me.TabSeleccionadoGeneral = OrdenTabs.Otros
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar el guardado de la orden", Me.ToString(), "TerminoPreguntaCiudadSeteo", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub



    Private Sub validarOrden()
        Dim strMsg As String = String.Empty
        Dim strReceptores As String = String.Empty
        Dim dblPorcentajeComision As Double = 0
        Dim intNroLideres As Integer = 0 'CCM20120305 - Validar que exista un receptor líder seleccionado
        Dim lstReceptores As List(Of String) 'CCM20120305
        Dim lstReceptoresRep As List(Of Object) 'CCM20120305

        Try
            ErrorForma = ""
            'If String.IsNullOrEmpty(Me._OrdenSelected.CanalRecepcion) Then
            '    strMsg &= "+ El canal de recepción de la orden es requerido." & vbNewLine
            'End If
            'If String.IsNullOrEmpty(Me._OrdenSelected.EstadoLEO) Then
            '    strMsg &= "+ El usuario operador es requerido." & vbNewLine
            'End If
            'If String.IsNullOrEmpty(Me._OrdenSelected.UsuarioOperador) Then
            '    strMsg &= "+ El usuario operador es requerido." & vbNewLine
            'End If
            'If String.IsNullOrEmpty(Me._OrdenSelected.MedioVerificable) Then
            '    strMsg &= "+ El medio verificable de la orden es requerido." & vbNewLine
            'End If
            'If IsNothing(Me._OrdenSelected.FechaRecepcion) Then
            '    strMsg &= "+ La fecha de recepción de la orden es requerida." & vbNewLine
            'End If
            'If String.IsNullOrEmpty(Me._OrdenSelected.FormaPago) Then
            '    strMsg &= "+ La forma de pago para la orden es requerida." & vbNewLine
            'End If

            'If String.IsNullOrEmpty(Me._OrdenSelected.Nemotecnico) Then
            '    strMsg &= "+ La especie de la orden es requerida." & vbNewLine
            'End If
            'If String.IsNullOrEmpty(Me._OrdenSelected.IDOrdenante) Then
            '    strMsg &= "+ El ordenante de la orden es requerido." & vbNewLine
            'End If
            'If String.IsNullOrEmpty(Me._OrdenSelected.IDComitente) Then
            '    strMsg &= "+ El comitente de la orden es requerido." & vbNewLine
            'End If
            'If IsNothing(Me._OrdenSelected.Objeto) Then
            '    strMsg &= "+ La clasificación de la orden es requerida." & vbNewLine
            'End If
            'If String.IsNullOrEmpty(Me._OrdenSelected.CondicionesNegociacion) Then
            '    strMsg &= "+ La condición de negociación para la orden es requerida." & vbNewLine
            'End If
            'If String.IsNullOrEmpty(Me._OrdenSelected.TipoLimite) Then
            '    strMsg &= "+ La naturaleza de la orden es requerida." & vbNewLine
            'End If
            'If IsNothing(Me._OrdenSelected.FechaOrden) Then
            '    strMsg &= "+ La fecha de elaboración de la orden es requerida." & vbNewLine
            'End If
            'If IsNothing(Me._OrdenSelected.VigenciaHasta) Then
            '    strMsg &= "+ La vigencia de la orden es requerida." & vbNewLine
            'End If
            'If Not strMsg.Equals(String.Empty) Then
            '    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If

            'Me.TabSeleccionadoGeneral = OrdenTabs.LEO

            'If ComitenteSeleccionado Is Nothing Then
            '    strMsg &= "+ Verifique el comitente seleccionado porque no se han podido validar sus datos." & vbNewLine
            'End If

            'If NemotecnicoSeleccionado Is Nothing Then
            '    strMsg = "+ Verifique la especie seleccionada porque no se han podido validar sus datos."
            'End If

            'If _mobjOrdenanteSeleccionado Is Nothing Then
            '    strMsg = "+ Debe seleccionar el ordenante de la orden."
            'Else
            '    'SLB Validación para saber si el Ordenante de la orden se encuentra Inactivo cuando se esta modificando 
            '    If _mobjOrdenanteSeleccionado.IdOrdenante = "-1" Then
            '        A2Utilidades.Mensajes.mostrarMensaje("El ordenante se encuentra Inactivo, seleccione otro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        Exit Sub
            '    End If
            '    _OrdenSelected.IDOrdenante = OrdenanteSeleccionado.IdOrdenante
            'End If

            'If _mobjCtaDepositoSeleccionada Is Nothing Then
            '    strMsg = "+ Debe seleccionar una cuenta depósito para la orden."
            'Else
            '    _OrdenSelected.CuentaDeposito = CtaDepositoSeleccionada.NroCuentaDeposito
            '    _OrdenSelected.UBICACIONTITULO = CtaDepositoSeleccionada.Deposito
            'End If

            'If Not strMsg.Equals(String.Empty) Then
            '    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If

            If IsNothing(_OrdenSelected.Tipo) Or String.IsNullOrEmpty(_OrdenSelected.Tipo) Then
                strMsg &= "+ Debe ingresar el tipo de orden (Compra-Venta)." & vbNewLine
            End If

            If ComitenteSeleccionado Is Nothing Then
                strMsg &= "+ Verifique el comitente seleccionado porque no se han podido validar sus datos." & vbNewLine
            End If

            If _mobjOrdenanteSeleccionado Is Nothing Then
                strMsg &= "+ Debe seleccionar el ordenante de la orden." & vbNewLine
            Else
                'SLB Validación para saber si el Ordenante de la orden se encuentra Inactivo cuando se esta modificando 
                If _mobjOrdenanteSeleccionado.IdOrdenante = "-1" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El ordenante se encuentra Inactivo, seleccione otro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                _OrdenSelected.IDOrdenante = OrdenanteSeleccionado.IdOrdenante
            End If

            If IsNothing(_OrdenSelected.FechaOrden) Then
                strMsg &= "+ Debe de ingresar la fecha de elaboración." & vbNewLine
            End If

            If IsNothing(_OrdenSelected.VigenciaHasta) Then
                strMsg &= "+ Debe de ingresar la fecha de vigencia." & vbNewLine
            End If

            If IsNothing(_OrdenSelected.Objeto) Then
                strMsg &= "+ Debe ingresar la clasificación de la orden." & vbNewLine
            End If

            'If IsNothing(_OrdenSelected.TipoLimite) Or String.IsNullOrEmpty(_OrdenSelected.TipoLimite) Then
            '    strMsg &= "+ Debe ingresar el tipo límite de la orden." & vbNewLine
            'End If

            If IsNothing(_OrdenSelected.CondicionesNegociacion) Or String.IsNullOrEmpty(_OrdenSelected.CondicionesNegociacion) Then
                strMsg &= "+ Debe ingresar la condición de negociación." & vbNewLine
            End If

            If Not strMsg.Equals(String.Empty) Then
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            strMsg = String.Empty

            If IsNothing(_OrdenSelected.FormaPago) Or String.IsNullOrEmpty(_OrdenSelected.FormaPago) Then
                'strMsg &= "+ Debe ingresar la forma de pago." & vbNewLine
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la forma de pago.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                Exit Sub
            End If

            If NemotecnicoSeleccionado Is Nothing Then
                strMsg &= "+ Verifique la especie seleccionada porque no se han podido validar sus datos." & vbNewLine
            End If

            If IsNothing(_OrdenSelected.Cantidad) Or _OrdenSelected.Cantidad = 0 Then
                strMsg &= "+ Debe ingresar " & IIf(Me.ClaseOrden = ClasesOrden.A, "la cantidad.", "el valor nominal") & vbNewLine
            End If

            'SLB20130801 Registro de Libro de Órdenes
            If _OrdenSelected.Cantidad < dblValorCantidadLibroOrden And logRegistroLibroOrdenes Then
                strMsg &= "+ No se puede modificar " & IIf(Me.ClaseOrden = ClasesOrden.A, "la cantidad.", "el valor nominal") & " por un valor inferior." & vbNewLine
            End If

            'SLB20130730 Validación hay que modificarla
            If _mobjCtaDepositoSeleccionada Is Nothing Then
                strMsg &= "+ Debe seleccionar una cuenta depósito o ubicación del titulo para la orden." & vbNewLine
            Else
                If logValidarCuentaDeposito Then
                    If _mobjCtaDepositoSeleccionada.Deposito <> MSTR_CTADEPOSITO_TITULO_FISICO And
                        _mobjCtaDepositoSeleccionada.Deposito <> MSTR_CTADEPOSITO_TITULO_EXTERIOR And
                        IsNothing(_mobjCtaDepositoSeleccionada.NroCuentaDeposito) Then
                        strMsg &= "+ Debe seleccionar un nro de cuenta depósito para la orden." & vbNewLine
                    Else
                        _OrdenSelected.CuentaDeposito = CtaDepositoSeleccionada.NroCuentaDeposito
                        _OrdenSelected.UBICACIONTITULO = CtaDepositoSeleccionada.Deposito
                    End If
                Else
                    _OrdenSelected.CuentaDeposito = CtaDepositoSeleccionada.NroCuentaDeposito
                    _OrdenSelected.UBICACIONTITULO = CtaDepositoSeleccionada.Deposito
                End If
            End If

            If Not strMsg.Equals(String.Empty) Then
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            strMsg = String.Empty

            'Inicio LEO
            If IsNothing(_OrdenSelected.FechaRecepcion) Then
                strMsg &= "+ Debe de ingresar la fecha de toma." & vbNewLine
            End If

            If IsNothing(_OrdenSelected.CanalRecepcion) Or String.IsNullOrEmpty(_OrdenSelected.CanalRecepcion) Then
                strMsg &= "+ Debe ingresar el canal de recepción." & vbNewLine
            End If

            If IsNothing(_OrdenSelected.UsuarioOperador) Or String.IsNullOrEmpty(_OrdenSelected.UsuarioOperador) Then
                strMsg &= "+ Debe ingresar el usuario operador." & vbNewLine
            End If

            If IsNothing(_OrdenSelected.MedioVerificable) Or String.IsNullOrEmpty(_OrdenSelected.MedioVerificable) Then
                strMsg &= "+ Debe ingresar el medio verificable." & vbNewLine
                'Else
                '    'SLB20130806
                '    If _OrdenSelected.MedioVerificable.Equals("T") Then
                '        If IsNothing(_OrdenSelected.NroExtensionToma) Or String.IsNullOrEmpty(_OrdenSelected.NroExtensionToma) Then
                '            strMsg &= "+ Debe ingresar el Nro extensión." & vbNewLine
                '        End If
                '    End If
            End If

            If Not strMsg.Equals(String.Empty) Then
                Me.TabSeleccionadoGeneral = OrdenTabs.LEO
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            'Fin LEO

            If Me._OrdenSelected.FechaOrden() >= Now() Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de elaboración de la orden no puede ser mayor a la fecha actual.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'SLB Validacion Productos Valores
            If Me._OrdenSelected.FormaPago = "E" And Me._OrdenSelected.IDProducto.Equals(0) Then
                A2Utilidades.Mensajes.mostrarMensaje("El producto financiero es requerido cuando es realizan ordenes en Efectivo. Por favor ingreselo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.TabSeleccionadoGeneral = OrdenTabs.Otros
                Exit Sub
            End If

            If Me._OrdenSelected.Objeto <> "IC" Then
                If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                    A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If Me.ClaseOrden = ClasesOrden.A Then
                strMsg = String.Empty

                If Me._OrdenSelected.Duracion Is Nothing OrElse Me._OrdenSelected.Duracion.Trim().Equals(String.Empty) Then
                    strMsg &= "+ La duración de la orden en la bolsa en requerida." & vbNewLine
                End If

                If Me._OrdenSelected.Ejecucion Is Nothing OrElse Me._OrdenSelected.Ejecucion.Trim().Equals(String.Empty) Then
                    strMsg &= "+ El tipo de ejecución de la orden en la bolsa en requerido." & vbNewLine
                End If

                If Not strMsg.Equals(String.Empty) Then
                    Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                '(SLB)
                If Me._OrdenSelected.Moneda <= 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la Moneda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'TabSeleccionadoGeneral = OrdenTabs.Otros
                    Exit Sub
                End If

                If Me._OrdenSelected.TasaConversion <= 0 Or Me._OrdenSelected.TasaConversion Is Nothing Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la Tasa de Conversión en Pesos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'TabSeleccionadoGeneral = OrdenTabs.Otros
                    Exit Sub
                End If

                If Me._OrdenSelected.Cantidad <= 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("La cantidad debe ser un valor mayor a cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If IsNothing(Me._OrdenSelected.Precio) Then
                    _OrdenSelected.Precio = 0
                End If
                If Me._OrdenSelected.Precio < Me._OrdenSelected.PrecioInferior Then
                    A2Utilidades.Mensajes.mostrarMensaje("El Precio debe ser mayor o igual al Precio Inferior", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Me._OrdenSelected.CantidadVisible >= Me._OrdenSelected.Cantidad Then
                    A2Utilidades.Mensajes.mostrarMensaje("La cantidad visible debe ser menor que la cantidad de la orden.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                'If Me._OrdenSelected.Ejecucion.ToUpper() = TipoEjecucion.C.ToString() And Me._OrdenSelected.CantidadMinima = 0 Then
                '    A2Utilidades.Mensajes.mostrarMensaje("Cuando el tipo de ejecución es cantidad mínima se debe indicar el valor de la cantidad mínima", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                '    Exit Sub
                'ElseIf Me._OrdenSelected.Ejecucion.ToUpper() <> TipoEjecucion.C.ToString() Then
                '    Me._OrdenSelected.CantidadMinima = 0
                'End If

                If Me._OrdenSelected.Ejecucion.ToUpper() = TipoEjecucion.C.ToString() And Me._OrdenSelected.CantidadMinima > Me._OrdenSelected.Cantidad Then
                    A2Utilidades.Mensajes.mostrarMensaje("La cantidad mínima debe ser máximo igual a la cantidad de la orden", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                    Exit Sub
                End If

                'If Me._OrdenSelected.TipoLimite.ToUpper() = TipoLimite.M.ToString() And Me._OrdenSelected.Precio > 0 Then
                '    A2Utilidades.Mensajes.mostrarMensaje("Para órdenes a mercado no se debe establecer precio. Se asignará cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    _OrdenSelected.Precio = 0
                '    _OrdenSelected.PrecioStop = 0
                'End If

                If Me._OrdenSelected.TipoLimite.ToUpper() = TipoLimite.M.ToString() And (Me._OrdenSelected.Ejecucion.ToUpper() <> TipoEjecucion.N.ToString Or Me._OrdenSelected.Duracion.ToUpper() <> TipoDuracion.I.ToString()) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Para órdenes a mercado la duración debe ser Inmediata y el tipo de ejecución Ninguna. El sistema asignó estos valores a estas propiedades de la orden." & vbNewLine & vbNewLine & "Si está de acuerdo guarde nuevamente la orden o de lo contrario haga las modificaciones necesarias en la naturaleza, duración y ejecución de la orden y luego guradela.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    _OrdenSelected.Duracion = TipoDuracion.I.ToString()
                    _OrdenSelected.Ejecucion = TipoEjecucion.N.ToString()
                    Me.TabSeleccionadoGeneral = OrdenTabs.Caracteristicas
                    Exit Sub
                End If

                'CCM20120305 - Validar precio stop solamente si es mayor que cero. Si es cero no se ejecutan las validaciones
                'If Me._OrdenSelected.PrecioStop > 0 Then
                '    If Me._OrdenSelected.Precio < Me._OrdenSelected.PrecioStop And Me._OrdenSelected.Tipo.ToUpper() = TiposOrden.V.ToString() Then
                '        A2Utilidades.Mensajes.mostrarMensaje("El precio stop para una orden de venta debe ser menor que el precio de venta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '        Exit Sub
                '    ElseIf Me._OrdenSelected.Precio > Me._OrdenSelected.PrecioStop And Me._OrdenSelected.Tipo.ToUpper() = TiposOrden.C.ToString() Then
                '        A2Utilidades.Mensajes.mostrarMensaje("El precio stop para una orden de compra debe ser mayor que el precio de compra.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '        Exit Sub
                '    End If
                'End If
            Else
                If _OrdenSelected.Tipo = TiposOrden.R.ToString() Or _OrdenSelected.Tipo = TiposOrden.S.ToString() Then
                    strMsg = "Los tipos de orden Recompra y Reventa son generadas en forma automática por el sistema cuando la orden lo requiere (simultáneas, repos, etc.)." & vbNewLine & vbNewLine & "Debe cambiar el tipo de la orden a compra o venta."
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Me._OrdenSelected.Cantidad <= 0 Then
                    strMsg = "+ El valor nominal debe ser mayor que cero."
                End If

                If Me._OrdenSelected.Mercado Is Nothing Then
                    strMsg = "+ El mercado en el cual se negocia la orden es requerido."
                End If

                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If


            If Not Me._OrdenSelected.FechaRecepcion Is Nothing AndAlso Me._OrdenSelected.FechaRecepcion < mdtmFechaCierreSistema Then
                strMsg = "La fecha de toma (fecha de recepción de la orden) no puede ser menor a la fecha de cierre del sistema."
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            lstReceptores = New List(Of String) 'CCM20120305 
            lstReceptoresRep = New List(Of Object) 'CCM20120305 

            strReceptores = "<receptores>"
            If ListaReceptoresOrdenes.Count > 0 Then
                For Each obj In ListaReceptoresOrdenes
                    If Not lstReceptores.Contains(obj.IDReceptor) And (obj.Porcentaje > 0 Or obj.Lider) Then 'CCM20120305 - Validar receptores duplicados y solamente considerar el primero
                        lstReceptores.Add(obj.IDReceptor)
                        dblPorcentajeComision += obj.Porcentaje

                        If IsNothing(obj.IDReceptor) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el receptor", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Me.TabSeleccionadoGeneral = OrdenTabs.Receptores
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
                Me.TabSeleccionadoGeneral = OrdenTabs.Receptores
                Exit Sub
            End If

            strReceptores &= "</receptores>"

            If dblPorcentajeComision <> 100.0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El porcentaje de distribución de comisión entre los receptores de la orden debe ser cien (100). En este momento está en " & dblPorcentajeComision.ToString("N0"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.TabSeleccionadoGeneral = OrdenTabs.Receptores
                Exit Sub
            End If

            'CCM20120305 - Validar que solamente exista un lider y que por lo menos exista uno seleccionado
            If intNroLideres = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Se debe seleccionar un receptor líder para la distribución de la comisión.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.TabSeleccionadoGeneral = OrdenTabs.Receptores
                Exit Sub
            ElseIf intNroLideres > 1 Then
                A2Utilidades.Mensajes.mostrarMensaje("Solamente puede seleccionarse un receptor líder para la distribución de la comisión. En este momento hay " & intNroLideres.ToString("N0") & " receptores seleccionados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Me.TabSeleccionadoGeneral = OrdenTabs.Receptores
                Exit Sub
            End If

            'CCM20120305 - Eliminar receptores duplicados (se deja solamnete el primero en el grid)
            If lstReceptoresRep.Count > 0 Then
                For Each obj In lstReceptoresRep
                    ListaReceptoresOrdenes.Remove(obj)
                Next
            End If

            _OrdenSelected.ReceptoresXML = strReceptores

            strMsg = String.Empty
            'strMsg = validarExcepcionRiesgo()

            If strMsg.Equals(String.Empty) Then
                mstrCuidadSeteo = String.Empty
                guardarOrden()
            Else
                'C1.Silverlight.C1MessageBox.Show(strMsg, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarExcepcionRDIPGuardar)
                mostrarMensajePregunta(strMsg,
                                       Program.TituloSistema,
                                       "EXEPCIONESRDIP",
                                       AddressOf validarExcepcionRDIPGuardar, True, "¿Desea continuar?")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la orden", Me.ToString(), "validarOrden", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
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

            If Not IsNothing(Me._OrdenSelected) Then
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
            ErrorForma = ""
            mstrAccionOrden = String.Empty
            If Not IsNothing(_OrdenSelected) Then
                dcProxy.RejectChanges()
                dcProxy1.RejectChanges()

                Editando = False
                EditandoRegistro = False
                habilitarFechaElaboracion = False

                MyBase.CambioItem("Editando")
                If _OrdenSelected.EntityState = EntityState.Detached Then
                    OrdenSelected = OrdenAnterior
                End If

                HabilitarBotones()
                'ActivarDuplicarOrden = True
                'ActivarEnvioSAE = True
                'ActivarSeleccionTipoOrden = True

                '// Si se cancela se asignan los valores anteriores de los buscadores
                ComitenteSeleccionado = _mobjComitenteSeleccionadoAntes
                NemotecnicoSeleccionado = _mobjNemotecnicoSeleccionadoAntes
                ReceptorTomaSeleccionado = _mobjReceptorTomaSeleccionadoAntes
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Este método valida el Estado de la Orden, si no esta pendiente de aprobacion o rechazada, Anula la Orden
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub BorrarRegistro()
        Try
            If IsNothing(_OrdenSelected) Then
                Exit Sub
            End If
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_OrdenSelected) Then
                validarEstadoOrden(MSTR_ACCION_ANULAR)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular la orden", Me.ToString(), "BorrarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub VersionRegistro()
        Try
            If Not Me.OrdenSelected Is Nothing Then
                cb = New CamposBusquedaOrden(Me)
                cb.NroOrden = Right(Me.OrdenSelected.NroOrden, 6)
                cb.AnoOrden = Left(Me.OrdenSelected.NroOrden, 4)
                cb.Version = 0
                cb.Clase = Me.OrdenSelected.Clase

                If RegistroActivoPorAprobar = False And RegistroTieneCambios Then
                    cb.EstadoMakerChecker = EstadoMakerChecker.PA.ToString()
                ElseIf RegistroActivoPorAprobar Then
                    cb.EstadoMakerChecker = EstadoMakerChecker.A.ToString()
                End If

                ConfirmarBuscar()
            End If

            MyBase.VersionRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar consulta para ver la versión de la orden", Me.ToString(), "VersionRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo que se encarga de aprobar o rechazar la modificacion que se realizo en algun registro (Solo aplica para Maker and Checker)
    ''' </summary>
    ''' <param name="plogAprobar"></param>
    ''' <remarks></remarks>
    Private Sub aprobarOrden(ByVal plogAprobar As Boolean)
        Try
            IsBusy = True
            dcProxy.OrdenSAEs.Clear()
            dcProxy.Load(dcProxy.AutorizarOrden_MIQuery(_OrdenSelected.Clase, _OrdenSelected.Tipo, _OrdenSelected.NroOrden, _OrdenSelected.Version, plogAprobar, _OrdenSelected.CalificacionEspecie, _OrdenSelected.PerfilComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoAutorizarOrden, MSTR_ACCION_APROBAR)
            MyBase.AprobarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar el proceso de aprobación de la orden", Me.ToString(), "aprobarOrden", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario ha confirmado la aprobación de la orden y se pasa a validar si cumple o no con las excepciones RDIP. 
    ''' </summary>
    Private Sub aprobarOrdenConfirmado()
        Dim strMsg As String
        Try
            strMsg = validarExcepcionRiesgo()
            If Not strMsg.Equals(String.Empty) Then
                'C1.Silverlight.C1MessageBox.Show(strMsg, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarExcepcionRDIPAprobar)
                mostrarMensajePregunta(strMsg,
                                       Program.TituloSistema,
                                       "APROBARORDENCONFIRMADO",
                                       AddressOf validarExcepcionRDIPAprobar, True, "¿Desea continuar?")
            Else
                aprobarOrden(True)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el proceso de aprobación de la orden", Me.ToString(), "aprobarOrdenConfirmado", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón aprobar
    ''' </summary>
    Public Overrides Sub AprobarRegistro()
        Try
            If _OrdenSelected Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la orden que desea aprobar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If _OrdenSelected.FechaOrden < mdtmFechaCierreSistema Then
                    A2Utilidades.Mensajes.mostrarMensaje("La orden no puede ser aprobada porque su fecha de vigencia es anterior a la fecha de cierre del sistema (" & Format(mdtmFechaCierreSistema, "dd/MMM/yyyy") & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    'C1.Silverlight.C1MessageBox.Show("¿Aprobar la orden " & _OrdenSelected.NroOrden & "?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarExcepcionRDIPAprobar)
                    mostrarMensajePregunta("¿Aprobar la orden " & _OrdenSelected.NroOrden & "?",
                                           Program.TituloSistema,
                                           "APROBARREGISTRO",
                                           AddressOf validarExcepcionRDIPAprobar, False)


                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el proceso de aprobación de la orden", Me.ToString(), "AprobarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida si el nivel de riesgo del cliente es mayor o no al nivel de riesgo asociado a la especie que se negocia.
    ''' </summary>
    ''' <returns>
    ''' Cuando el nivel del cliente es mayor al de la especie se retorna una cadena vacia (no se incumple con la regla), de lo contrario se retorna el mensaje de validación correspondiente.
    ''' </returns>
    Private Function validarExcepcionRiesgo() As String
        Dim intRiesgoEspecie As Integer = -1
        Dim intRiesgoCliente As Integer = -2
        Dim strMsg As String = String.Empty

        If Not NemotecnicoSeleccionado.CodClasificacionRiesgo Is Nothing AndAlso Not NemotecnicoSeleccionado.CodClasificacionRiesgo.Trim.Equals(String.Empty) AndAlso Versioned.IsNumeric(NemotecnicoSeleccionado.CodClasificacionRiesgo) Then
            intRiesgoEspecie = CInt(NemotecnicoSeleccionado.CodClasificacionRiesgo)
        End If

        If Not ComitenteSeleccionado.CodClasificacionInversionista Is Nothing AndAlso Not ComitenteSeleccionado.CodClasificacionInversionista.Trim.Equals(String.Empty) AndAlso Versioned.IsNumeric(ComitenteSeleccionado.CodClasificacionInversionista) Then
            intRiesgoCliente = CInt(ComitenteSeleccionado.CodClasificacionInversionista)
        End If

        If intRiesgoEspecie < 0 And intRiesgoCliente < 0 Then
            strMsg = "Ni la calificación de riesgo para la especie y ni para el cliente están definidas." & vbNewLine & vbNewLine
        ElseIf intRiesgoCliente < 0 Then
            intRiesgoCliente = intRiesgoEspecie - 1
            strMsg = "La calificación de riesgo para el cliente no está definida. Se asumira un nivel menor que el asociado a la especie." & vbNewLine & vbNewLine
        ElseIf intRiesgoCliente < 0 Then
            intRiesgoEspecie = intRiesgoCliente + 1
            strMsg = "La calificación de riesgo para la especie no está definida. Se asumira un nivel de riesgo mayor que el asociado al cliente." & vbNewLine & vbNewLine
        End If

        If intRiesgoEspecie > intRiesgoCliente Then
            strMsg &= MSTR_MENSAJE_EXCEPCION_RDIP
            _OrdenSelected.PerfilComitente = intRiesgoCliente
            _OrdenSelected.CalificacionEspecie = intRiesgoEspecie
        End If

        Return (strMsg)
    End Function

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón rechazar
    ''' </summary>
    Public Overrides Sub RechazarRegistro()
        Try
            If _OrdenSelected Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la orden que desea rechazar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                'C1.Silverlight.C1MessageBox.Show("¿Rechazar la orden " & _OrdenSelected.NroOrden & "?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarRechazoOrden)
                mostrarMensajePregunta("¿Rechazar la orden " & _OrdenSelected.NroOrden & "?",
                                       Program.TituloSistema,
                                       "RECHAZARORDEN",
                                       AddressOf validarRechazoOrden, False)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el proceso de rechazo de la orden", Me.ToString(), "RechazarRegistro", Program.TituloSistema, Program.Maquina, ex)
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

        Dim strMsg As String

        If _mlogFechaCierreSistema Then
            CambiarAForma()
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("La fecha de la vigencia no puede ser menor a la fecha de cierre del sistema (" & mdtmFechaCierreSistema.ToLongDateString() & ")." & vbNewLine & vbNewLine & "Por favor modifique la fecha de vigencia de la orden para que sea posterior a la fecha de cierre.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '_mlogFechaCierreSistema = False
            Exit Sub
        End If

        If Me._OrdenSelected.Modificable = False Then
            If Me._OrdenSelected.EstadoMakerChecker = EstadoMakerChecker.PA.ToString() Then
                If pstrAccion = MSTR_ACCION_EDITAR Then
                    strMsg = "La orden está pendiente por aprobar y no puede ser modificada"
                Else
                    strMsg = "La orden está pendiente por aprobar y no puede ser anulada"
                End If

                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            ElseIf Me._OrdenSelected.EstadoMakerChecker = EstadoMakerChecker.NA.ToString() Then
                If pstrAccion = MSTR_ACCION_EDITAR Then
                    strMsg = "La orden fue rechazada y no puede ser modificada"
                Else
                    strMsg = "La orden fue rechazada y no puede ser anulada"
                End If

                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If pstrAccion = MSTR_ACCION_EDITAR Then
                    strMsg = "Solamente las órdenes en estado pendiente pueden ser modificadas"
                Else
                    strMsg = "Solamente las órdenes en estado pendiente pueden ser anuladas"
                End If

                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
            MyBase.RetornarValorEdicionNavegacion()
        Else
            Me.IsBusy = True
            validarOrdenModificable(pstrAccion)
        End If

    End Sub

    ''' <summary>
    ''' Valida el estado de la orden en el servidor, invoca el SP uspOyDNet_Ordenes_VerificarEstadoOrden_MI_Consultar
    ''' </summary>
    ''' <param name="pstrAccion">Indica si se valida la edición o anulación de la orden</param>
    Private Sub validarOrdenModificable(ByVal pstrAccion As String)
        Try
            dcProxy.OrdenModificables.Clear()
            dcProxy.Load(dcProxy.verificarOrdenModificable_MIQuery(Me._OrdenSelected.Clase, Me._OrdenSelected.Tipo, Me._OrdenSelected.NroOrden, Me._OrdenSelected.Version, MSTR_MODULO_OYD_ORDENES, pstrAccion, Program.Usuario, Program.HashConexion), AddressOf TerminoVerificarOrdenModificable, pstrAccion)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la validación del estado de la orden", Me.ToString(), "ValidarOrdenModificable", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Me.IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Consulta el Saldo de la Orden
    ''' </summary>
    ''' <remarks>SLB20130826</remarks>
    Private Sub ConsultarSaldoOrden()
        Try
            If Me.ClaseOrden = ClasesOrden.A Then
                If Not (Me._OrdenSelected.Version > 0 Or Me._OrdenSelected.Estado.Equals("C") Or Me._OrdenSelected.Estado.Equals("A")) Then
                    dcProxy.ConsultarSaldoOrden(Me._OrdenSelected.Clase, Me._OrdenSelected.Tipo, Me._OrdenSelected.NroOrden, Me._OrdenSelected.Version, "MI", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarSaldoOrden, "")
                Else
                    SaldoOrden = 0
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la validación del estado de la orden", Me.ToString(), "ValidarOrdenModificable", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Me.IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarSaldoOrden(ByVal lo As InvokeOperation(Of Double))
        If Not lo.HasError Then
            SaldoOrden = lo.Value
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta del saldo de la Orden", Me.ToString(), "TerminoConsultarSaldoOrden", Program.TituloSistema, Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub


    Public Sub validarBolsaParaEspecie()
        Try
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la bolsa", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'TabSeleccionadoGeneral = OrdenTabs.Otros
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la Bolsa para la Especie", Me.ToString(), "validarBolsaParaEspecie", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Me.IsBusy = False
        End Try
    End Sub

    Public Sub validarPrecioAPrecioInferior()
        Try
            If IsNothing(Me._OrdenSelected.Precio) Then
                _OrdenSelected.Precio = 0
            End If
            If Me._OrdenSelected.Precio < Me._OrdenSelected.PrecioInferior Then
                A2Utilidades.Mensajes.mostrarMensaje("El Precio debe ser mayor o igual al Precio Inferior", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el Precio y el Precio Inferior", Me.ToString(), "validarPrecioAPrecioInferior", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            If IsNothing(_OrdenSelected) Then
                Exit Sub
            End If
            ' Sebastian Londoño 
            If Me._OrdenSelected.Estado = "A" Then
                A2Utilidades.Mensajes.mostrarMensaje("La orden esta cancelada. No se puede duplicar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            'C1.Silverlight.C1MessageBox.Show("¿Esta seguro de Duplicar la Orden?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, _
            '                                 C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf validarDuplicarOrden)
            mostrarMensajePregunta("¿Esta seguro de Duplicar la Orden?",
                                   Program.TituloSistema,
                                   "DUPLICARORDEN",
                                   AddressOf validarDuplicarOrden, False)
            'configurarNuevaOrden(_OrdenSelected, True, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la duplicación de la orden", Me.ToString(), "duplicarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub enviarPorSAE()
        Dim strMsg As String = String.Empty

        Try
            If Me._OrdenSelected Is Nothing Then
                Exit Sub
            End If

            If Me.mlogSAEActivo And Me.mlogSAEActivoClaseOrden And Me.mlogPuedeEnrutarSAE Then
                If Me._OrdenSelected.Modificable = False Then
                    If Me._OrdenSelected.EstadoMakerChecker = EstadoMakerChecker.PA.ToString() Then
                        strMsg = "La orden está pendiente por aprobar y no puede ser enrutada a la Bolsa"
                    ElseIf Me._OrdenSelected.EstadoMakerChecker = EstadoMakerChecker.NA.ToString() Then
                        strMsg = "La orden fue rechazada y no puede ser enrutada a la Bolsa"
                    Else
                        strMsg = "Solamente las órdenes en estado pendiente pueden ser enrutadas a la Bolsa"
                    End If

                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    Me.IsBusy = True
                    dcProxy1.OrdenSAEs.Clear()
                    dcProxy1.Load(dcProxy1.enrutar_OrdenSAEQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, False, Program.Usuario, Program.HashConexion), AddressOf TerminoEnrutarOrdenSAE, "enrutarSAE")
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al enrutar la orden por SAE", Me.ToString(), "enviarPorSAE", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub RefrescarOrden()
        Try
            If Not Me.OrdenSelected Is Nothing Then
                cb = New CamposBusquedaOrden(Me)
                cb.NroOrden = Right(Me.OrdenSelected.NroOrden, 6)
                cb.AnoOrden = Left(Me.OrdenSelected.NroOrden, 4)
                cb.Version = Me.OrdenSelected.Version
                cb.Clase = Me.OrdenSelected.Clase

                If RegistroActivoPorAprobar Then
                    cb.EstadoMakerChecker = EstadoMakerChecker.PA.ToString()
                Else
                    cb.EstadoMakerChecker = EstadoMakerChecker.A.ToString()
                End If

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
            configurarNuevaOrden(OrdenPorDefecto, False, True)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la generación de una nueva orden de venta", Me.ToString(), "generarNuevaCompra", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub generarNuevaVenta()
        Try
            IsBusy = True
            configurarNuevaOrden(OrdenPorDefecto, False, False)
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
            If Not _OrdenSelected Is Nothing And mstrAccionOrden = String.Empty Then
                If Application.Current.Resources.Contains("A2VReporteOrdenesRS") = False Then
                    A2Utilidades.Mensajes.mostrarMensaje("El reporte para imprimir la orden no está configurado", Program.TituloSistema)
                    Exit Sub
                End If

                strReporte = Application.Current.Resources("A2VReporteOrdenesRS").ToString.Trim()
                strParametros = "&pstrClaseOrden=" & _OrdenSelected.Clase & "&pintIdOrden=" & _OrdenSelected.NroOrden & "&pintVersion=" & _OrdenSelected.Version & "&pdblSaldo=" & Me.SaldoOrden
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
            If Not Me.OrdenSelected Is Nothing Then
                If Not Me.ComitenteSeleccionado Is Nothing And pstrIdComitente.Equals(String.Empty) Then
                    strIdComitente = Me.ComitenteSeleccionado.IdComitente
                End If

                If Not strIdComitente.Equals(Me.OrdenSelected.IDComitente) Then
                    If pstrIdComitente.Trim.Equals(String.Empty) Then
                        strIdComitente = Me.OrdenSelected.IDComitente
                    Else
                        strIdComitente = pstrIdComitente
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
    Friend Sub buscarNemotecnico(Optional ByVal pstrNemotecnico As String = "", Optional ByVal pstrAccion As String = "")
        Dim strNemotecnico As String = String.Empty

        Try
            If Not Me.OrdenSelected Is Nothing Then
                If Not Me.NemotecnicoSeleccionado Is Nothing And pstrNemotecnico.Equals(String.Empty) Then
                    strNemotecnico = Me.NemotecnicoSeleccionado.Nemotecnico
                End If

                If Not strNemotecnico.Equals(Me.OrdenSelected.Nemotecnico) Then
                    If pstrNemotecnico.Trim.Equals(String.Empty) Then
                        strNemotecnico = Me.OrdenSelected.Nemotecnico
                    Else
                        strNemotecnico = pstrNemotecnico
                    End If
                    mdcProxyUtilidad01.BuscadorEspecies.Clear()
                    '-- CCM20120108: Incluir la clase para evitar que al digitar se traigan especies de una clase diferente a la de la orden
                    strNemotecnico = System.Web.HttpUtility.UrlEncode(strNemotecnico)
                    If pstrAccion = "buscar" Then
                        mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarEspeciesQuery(pstrNemotecnico, Me.ClaseOrden.ToString(), "A", "IdBolsa," + CStr(_OrdenSelected.IdBolsa), Program.Usuario, Program.HashConexion), AddressOf buscarNemotecnicoCompleted, "")
                    Else
                        mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarNemotecnicoEspecificoQuery(Me.ClaseOrden.ToString(), strNemotecnico, Program.Usuario, Program.HashConexion), AddressOf buscarNemotecnicoCompleted, "")
                    End If
                End If
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
            If Not Me.OrdenSelected Is Nothing Then
                Select Case pstrTipoItem.ToLower()
                    Case "receptores"
                        If Not Me.ReceptorTomaSeleccionado Is Nothing Then
                            strIdItem = Me.ReceptorTomaSeleccionado.IdItem
                        End If

                        pstrIdItem = pstrIdItem.Trim()
                        If Not strIdItem.Equals(Me.OrdenSelected.IdReceptorToma) Then
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.OrdenSelected.IdReceptorToma
                            Else
                                strIdItem = pstrIdItem
                            End If

                            If Not IsNothing(strIdItem) AndAlso Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                        End If
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
        Me.ComitenteSeleccionado = pobjComitente
    End Sub

    ''' <summary>
    ''' Actualizar elemento buscado con los datos recibidos como parámetro
    ''' </summary>
    ''' <param name="pstrTipoItem">Tipo de objeto que se recibe</param>
    ''' <param name="pobjItem">Item enviado como parámetro</param>
    Public Sub actualizarItemOrden(ByVal pstrTipoItem As String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrTipoItem.ToLower
                Case "idreceptortoma"
                    Me.ReceptorTomaSeleccionado = pobjItem
                    Me.OrdenSelected.IdReceptorToma = pobjItem.IdItem
                Case "idreceptor"
                    Me.ReceptoresOrdenSelected.IDReceptor = pobjItem.IdItem
                    Me.ReceptoresOrdenSelected.Nombre = pobjItem.Nombre
            End Select
        End If
    End Sub

    ''' <summary>
    ''' Actualizar el nemotécnico de la orden con los datos del nemotecnico recibido como parámetro
    ''' </summary>
    ''' <param name="pobjNemotecnico">Nemotécnico enviado como parámetro</param>
    Public Sub actualizarNemotecnicoOrden(ByVal pobjNemotecnico As OYDUtilidades.BuscadorEspecies)
        Me.NemotecnicoSeleccionado = pobjNemotecnico
        Me._OrdenSelected.Nemotecnico = pobjNemotecnico.Nemotecnico

        If Me._OrdenSelected.Objeto <> "IC" Then
            If Me.NemotecnicoSeleccionado.Mercado = "S" Then
                A2Utilidades.Mensajes.mostrarMensaje("La especie seleccionada es de Segundo Mercado, la clasificación debe ser Inversionista Calificado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        End If

        If Me.ClaseOrden.ToString() = ClasesOrden.C.ToString() Then
            Me._OrdenSelected.FechaEmision = pobjNemotecnico.Emision
            Me._OrdenSelected.FechaVencimiento = pobjNemotecnico.Vencimiento
            Me._OrdenSelected.Modalidad = pobjNemotecnico.CodModalidad
            Me._OrdenSelected.IndicadorEconomico = IIf(pobjNemotecnico.IdIndicador Is Nothing, Nothing, pobjNemotecnico.IdIndicador) ' Se hace por conversión de tipo de datos
            Me._OrdenSelected.PuntosIndicador = pobjNemotecnico.PuntosIndicador
            Me._OrdenSelected.TasaNominal = pobjNemotecnico.TasaFacial
        End If

    End Sub
#End Region

#Region "Tablas hijas"

#Region "Liquidaciones de la Orden"

    Private _ListaLiquidacionesOrden As EntitySet(Of OyDOrdenes.LiquidacionesOrden)
    Public Property LiquidacionesOrden() As EntitySet(Of OyDOrdenes.LiquidacionesOrden)
        Get
            Return (_ListaLiquidacionesOrden)
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.LiquidacionesOrden))
            _ListaLiquidacionesOrden = value
            MyBase.CambioItem("LiquidacionesOrden")
        End Set
    End Property

#End Region

#Region "Beneficarios"

    '******************************************************** BeneficiariosOrdenes 
    Private _ListaBeneficiariosOrdenes As EntitySet(Of OyDOrdenes.BeneficiariosOrden)
    Public Property ListaBeneficiariosOrdenes() As EntitySet(Of OyDOrdenes.BeneficiariosOrden)
        Get
            Return _ListaBeneficiariosOrdenes
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.BeneficiariosOrden))
            _ListaBeneficiariosOrdenes = value
            MyBase.CambioItem("ListaBeneficiariosOrdenes")
            MyBase.CambioItem("ListaBeneficiariosOrdenesPaged")
        End Set
    End Property

    Public ReadOnly Property BeneficiariosOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBeneficiariosOrdenes) Then
                Dim view = New PagedCollectionView(_ListaBeneficiariosOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _BeneficiariosOrdenSelected As OyDOrdenes.BeneficiariosOrden
    Public Property BeneficiariosOrdenSelected() As OyDOrdenes.BeneficiariosOrden
        Get
            Return _BeneficiariosOrdenSelected
        End Get
        Set(ByVal value As OyDOrdenes.BeneficiariosOrden)
            _BeneficiariosOrdenSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("BeneficiariosOrdenSelected")
            End If
        End Set
    End Property

#End Region

#Region "Receptores"

    '******************************************************** ReceptoresOrdenes 
    Private _ListaReceptoresOrdenes As EntitySet(Of OyDOrdenes.ReceptoresOrden)
    Public Property ListaReceptoresOrdenes() As EntitySet(Of OyDOrdenes.ReceptoresOrden)
        Get
            Return _ListaReceptoresOrdenes
        End Get
        Set(ByVal value As EntitySet(Of OyDOrdenes.ReceptoresOrden))
            _ListaReceptoresOrdenes = value
            If Not IsNothing(value) Then
                ReceptoresOrdenSelected = _ListaReceptoresOrdenes.FirstOrDefault
            End If
            MyBase.CambioItem("ListaReceptoresOrdenes")
            MyBase.CambioItem("ListaReceptoresOrdenesPaged")
        End Set
    End Property

    Public ReadOnly Property ReceptoresOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaReceptoresOrdenes) Then
                Dim view = New PagedCollectionView(_ListaReceptoresOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ReceptoresOrdenSelected As OyDOrdenes.ReceptoresOrden
    Public Property ReceptoresOrdenSelected() As OyDOrdenes.ReceptoresOrden
        Get
            Return _ReceptoresOrdenSelected
        End Get
        Set(ByVal value As OyDOrdenes.ReceptoresOrden)
            If Not IsNothing(value) Then
                _ReceptoresOrdenSelected = value
                MyBase.CambioItem("ReceptoresOrdenSelected")
            End If
        End Set
    End Property

#End Region

#Region "Adicionar Hijos"

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptoresOrden"
                Dim NewReceptoresOrden As New OyDOrdenes.ReceptoresOrden

                NewReceptoresOrden.ClaseOrden = Me._OrdenSelected.Clase
                NewReceptoresOrden.TipoOrden = Me._OrdenSelected.Tipo
                NewReceptoresOrden.NroOrden = Me._OrdenSelected.NroOrden
                NewReceptoresOrden.Version = Me._OrdenSelected.Version
                NewReceptoresOrden.IDComisionista = Me._OrdenSelected.IDComisionista
                NewReceptoresOrden.IDSucComisionista = Me._OrdenSelected.IdSucComisionista
                NewReceptoresOrden.FechaActualizacion = Now()
                NewReceptoresOrden.Usuario = Program.Usuario
                NewReceptoresOrden.Lider = False
                NewReceptoresOrden.Porcentaje = 0
                NewReceptoresOrden.Nombre = ""

                ListaReceptoresOrdenes.Add(NewReceptoresOrden)
                ReceptoresOrdenSelected = NewReceptoresOrden

                MyBase.CambioItem("ListaReceptoresOrdenes")
        End Select
    End Sub

#End Region

#Region "Borrar Hijos"

    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptoresOrden"
                If Not IsNothing(ListaReceptoresOrdenes) Then
                    If Not _ReceptoresOrdenSelected Is Nothing Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptoresOrdenSelected, ListaReceptoresOrdenes.ToList)

                        If ListaReceptoresOrdenes.Contains(_ReceptoresOrdenSelected) Then
                            ListaReceptoresOrdenes.Remove(_ReceptoresOrdenSelected)
                        End If
                        If ListaReceptoresOrdenes.Count > 0 Then
                            Program.PosicionarItemLista(ReceptoresOrdenSelected, ListaReceptoresOrdenes.ToList, intRegistroPosicionar)
                        Else
                            ReceptoresOrdenSelected = Nothing
                        End If
                        MyBase.CambioItem("ListaReceptoresOrdenes")
                    End If
                End If
        End Select
    End Sub

#End Region

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

#End Region

#Region "Consultar datos asociados a la orden o al comitente"

    ''' <summary>
    ''' Consultar las liquidaciones asociadas a la orden
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cargarLiquidaciones()
        If mstrAccionOrden <> MSTR_MC_ACCION_INGRESAR And mlogDuplicarOrden = False Then
            If Not IsNothing(OrdenSelected) Then
                IsBusyLiquidaciones = True
                dcProxyConsultas.LiquidacionesOrdens.Clear()
                dcProxyConsultas.Load(dcProxyConsultas.ConsultarLiquidacionesAsociadasOrden_MIQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarLiquidacionesOrden, "consultarLiquidaciones")
            End If
        Else
            dcProxyConsultas.LiquidacionesOrdens.Clear()
        End If
    End Sub


    ''<summary>
    ' Consultar los receptores asociados a la orden
    '</summary>
    'Private Sub consultarOrdenSAE()
    '    dcProxy1.OrdenSAEs.Clear()
    '    dcProxy1.Load(dcProxy1.ConsultarOrdenSAEQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarOrdenSAE, "consultarSAE")
    'End Sub

    ''' <summary>
    ''' Consultar los receptores asociados a la orden
    ''' </summary>
    Private Sub verificarReceptoresOrdenDuplicar()
        dcProxy1.ReceptoresOrdens.Clear()
        dcProxy1.Load(dcProxy1.Verificar_ReceptoresOrdenes_MI_OrdenQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, OrdenSelected.Version, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresOrdenes, "verificarReceptoresDuplicar")
    End Sub

    ''' <summary>
    ''' Consultar los receptores asociados a la orden
    ''' </summary>
    Private Sub consultarReceptoresOrden()
        dcProxy1.ReceptoresOrdens.Clear()
        dcProxy1.Load(dcProxy1.Traer_ReceptoresOrdenes_MI_OrdenQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, OrdenSelected.Version, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresOrdenes, "")
    End Sub

    ''' <summary>
    ''' Consultar los receptores del cliente para seleccionar los asociados a la orden
    ''' </summary>
    Private Sub consultarReceptoresComitente()
        If _mlogCargarReceptorClientes Then
            dcProxy1.ReceptoresOrdens.Clear()
            dcProxy1.Load(dcProxy1.Traer_ReceptoresOrdenes_MI_ClienteQuery(_OrdenSelected.Clase, _OrdenSelected.Tipo, _ComitenteSeleccionado.CodigoOYD, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresOrdenes, MSTR_ACC_CONSULTA_RECEPTORES_CLT)
        End If
    End Sub

    ''' <summary>
    ''' Consultar los beneficiarios de la cuenta depósito asignada a la orden
    ''' </summary>
    Private Sub consultarBeneficiariosOrden()
        dcProxy.BeneficiariosOrdens.Clear()
        dcProxy.Load(dcProxy.Traer_BeneficiariosOrdenes_MI_OrdenQuery(OrdenSelected.Clase, OrdenSelected.Tipo, OrdenSelected.NroOrden, OrdenSelected.Version, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosOrdenes, "")
    End Sub

    ''' <summary>
    ''' Consultar los beneficiarios de la cuenta depósito asignada al cliente
    ''' </summary>
    Private Sub consultarBeneficiariosCliente()
        dcProxy.BeneficiariosOrdens.Clear()
        If Not IsNothing(OrdenSelected) Then
            If Not IsNothing(OrdenSelected.CuentaDeposito) Then
                dcProxy.Load(dcProxy.Traer_BeneficiariosOrdenes_ClienteQuery(OrdenSelected.IDComitente, OrdenSelected.CuentaDeposito, OrdenSelected.UBICACIONTITULO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosOrdenes, "")
            End If
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
    ''' Seleccionar los datos del ordenante asociado a la orden de la lista de ordenantes
    ''' </summary>
    Private Sub buscarOrdenanteSeleccionado()
        If Not _Ordenantes Is Nothing And Not _OrdenSelected Is Nothing Then
            If Not Editando Then
                If Not _OrdenSelected.IDOrdenante Is Nothing Then
                    If (From obj In _Ordenantes Where obj.IdOrdenante = _OrdenSelected.IDOrdenante Select obj).ToList.Count > 0 Then
                        OrdenanteSeleccionado = (From cta In _Ordenantes Where cta.IdOrdenante.Trim() = _OrdenSelected.IDOrdenante.Trim() Select cta).ToList.ElementAt(0)
                    Else
                        'SLB20121205 Cuando el Ordenante se encuentra Inactivo se debe mostrar en el pantalla, a pesar de que en la lista no exista.
                        Dim strDescripcionOrdenante = LTrim(_OrdenSelected.IDOrdenante) & " - " & _OrdenSelected.NombreOrdenante.ToString
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
        If Not _OrdenSelected Is Nothing And Not CuentasDeposito Is Nothing Then
            If Not Editando Then
                If Not _OrdenSelected.CuentaDeposito Is Nothing Then
                    If (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _OrdenSelected.CuentaDeposito Select cta).ToList.Count > 0 Then
                        CtaDepositoSeleccionada = (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _OrdenSelected.CuentaDeposito Select cta).ToList.ElementAt(0)
                    Else
                        CtaDepositoSeleccionada = Nothing
                    End If
                Else
                    'AJUSTE20141017SV - Se adiciona lógica para que seleccione también el dato cuando solo venga la ubicación del título 
                    If _CuentasDeposito.Count = 1 Then
                        CtaDepositoSeleccionada = _CuentasDeposito.First
                    ElseIf (From cta In CuentasDeposito Where IsNothing(cta.NroCuentaDeposito) And cta.Deposito = _OrdenSelected.UBICACIONTITULO Select cta).ToList.Count > 0 Then
                        CtaDepositoSeleccionada = (From cta In CuentasDeposito Where IsNothing(cta.NroCuentaDeposito) And cta.Deposito = _OrdenSelected.UBICACIONTITULO Select cta).ToList.ElementAt(0)
                    End If
                End If

                If _CuentasDeposito.Count = 1 Then
                    CtaDepositoSeleccionada = _CuentasDeposito.First
                End If
            Else
                If (From cta In CuentasDeposito Select cta).ToList.Count = 1 Then
                    CtaDepositoSeleccionada = CuentasDeposito.First
                Else
                    CtaDepositoSeleccionada = Nothing
                End If
            End If
        Else
            CtaDepositoSeleccionada = Nothing
        End If
    End Sub

#End Region

    ''' <summary>
    ''' Este evento se dispara cuando alguna propiedad de la orden activa cambia
    ''' Se utiliza para calcular los días de vigencia y/o días de vencimiento cuando cambia alguna de las fechas involucradas en el cálculo
    ''' </summary>
    Private Sub _OrdenSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _OrdenSelected.PropertyChanged

        If Editando Then

            If mlogCambioBolsa = False Then
                mlogCambioBolsa = True
                Exit Sub
            End If

            'SLB20121116
            If e.PropertyName.ToLower.Equals("objeto") Then
                If Not _OrdenSelected.Objeto = String.Empty Then
                    _OrdenSelected.Ordinaria = False
                Else
                    _OrdenSelected.Ordinaria = True
                End If
            End If

            Try
                Select Case e.PropertyName.ToLower()
                    Case "idbolsa"
                        mlogCambioBolsa = False
                        NemotecnicoOrden = String.Empty
                        'Me.NemotecnicoOrden = Nothing
                        Me._OrdenSelected.Nemotecnico = Nothing
                End Select
            Catch ex As Exception
                mlogCambioBolsa = True 'SLB
            End Try

            If mlogRecalcularFechas = False Then 'CCM20120305
                mlogRecalcularFechas = True
                Exit Sub
            End If

            Try
                mlogRecalcularFechas = False 'CCM20120305

                Select Case e.PropertyName.ToLower()
                    'Case "fechaorden"
                    '    Me.calcularDiasOrden(OrdenesViewModel.MSTR_CALCULAR_DIAS_ORDEN, -1)
                    'VerificarFechaValida()
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

        End If

    End Sub

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaOrden
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
    Private mobjVM As OrdenesViewModel
#End Region

#Region "Propiedades"

    <Description("Renta fija o renta variable")>
    Public Property Clase As String = String.Empty
    <Description("Tipo de operación de la orden: compra, venta, ...")>
    Public Property Tipo As String = String.Empty
    Public Property AnoOrden As String = Nothing
    Public Property NroOrden As System.Nullable(Of Integer)
    Public Property Version As System.Nullable(Of Integer) '= 0
    'Public Property IDComitente As String = String.Empty
    'Public Property IDOrdenante As String = String.Empty
    Public Property FechaOrden As System.Nullable(Of Date)
    Public Property Nemotecnico As String = String.Empty
    Public Property FormaPago As String = String.Empty
    Public Property Objeto As String = Nothing
    Public Property CondicionesNegociacion As String
    Public Property TipoInversion As String = String.Empty
    Public Property TipoTransaccion As String = String.Empty
    Public Property CanalRecepcion As String = String.Empty
    Public Property MedioVerificable As String = String.Empty
    Public Property FechaHoraRecepcion As System.Nullable(Of Date)
    Public Property TipoLimite As String = String.Empty
    Public Property VigenciaHasta As System.Nullable(Of Date)
    Public Property Ejecucion As String = String.Empty
    Public Property Duracion As String = String.Empty
    Public Property Estado As String = String.Empty
    Public Property EstadoOrdenBus As String = String.Empty
    Public Property IdBolsa As System.Nullable(Of Integer)
    Public Property EstadoMakerChecker As String = String.Empty
    Public Property AccionMakerChecker As String = String.Empty

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

    Private _VisibleMakerAndCheker As Visibility = Visibility.Visible
    Public Property VisibleMakerAndCheker() As Visibility
        Get
            Return _VisibleMakerAndCheker
        End Get
        Set(ByVal value As Visibility)
            _VisibleMakerAndCheker = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VisibleMakerAndCheker"))
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

    Public Sub New(ByRef pobjVMPadre As OrdenesViewModel)
        mobjVM = pobjVMPadre
    End Sub

#End Region
End Class
