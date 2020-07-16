Imports Telerik.Windows.Controls

Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Text.RegularExpressions
Imports A2ControlMenu
Imports A2.OyD.OYDServer.RIA.Web.OyDOperaciones


Public Class OperacionesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As OyDOperaciones.Operaciones
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As OyDOperaciones.Operaciones
    Private mdcProxy As OperacionesDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private objProxyUtilidades As UtilidadesDomainContext
    Private OperacionesPorDefecto As OyDOperaciones.Operaciones
    Private objProxy As OperacionesDomainContext
    Dim LiquidacionesOrdenes As New LiquidacionesOrdenes
    Dim ListaLiquidacionesValidar As New List(Of LiquidacionesConsultar)
    Dim ConsultarCantidades As New ConsultarCantidad
    Dim aplazamientoserie As Aplazamiento_En_Serie
    Dim DicCamposTab As New Dictionary(Of String, Integer)

    'Boolean
    Private mlogTerminoSubmitChanges As Boolean = False
    Public logNuevoRegistro As Boolean = False
    Public logEditarRegistro As Boolean = False
    Public logEstadoRegistro, mlogCantidad As Boolean
    Dim logEsNuevo As Boolean
    Dim ValidaFiltro As Boolean
    Dim Selected As Boolean
    Dim logDisparaGuardar As Boolean
    Dim DisparaFocus As Boolean
    Dim DisparaGuardar As Boolean
    Dim ESTADO_EDICION As Boolean
    Dim Duplica As Boolean
    Dim mlogRepo As Boolean
    Private mlogEnPesos As Boolean = False
    Private _mlogDuplicando As Boolean = False
    Private mlogRecalculaValoresAlDuplicar As Boolean = False
    Private logNroOrdenEsNumerico As Boolean = False

    'Integer
    Public lngIDLiquidacionEdicion As Integer = 0
    Public ParcialEdicion As Integer = 0
    Private IdLiqAnterior As Nullable(Of Integer) = Nothing 'SV20140623 Consulta de registros despues de aplazar para identificar la liquidación aplazada
    Dim logCambiarSelected As Boolean = True
    Public logCambiarCollection As Boolean = True
    Public logConsultarDetalles As Boolean = True
    Dim logCambiarLista As Boolean = False
    Dim logBajarIsBusy As Boolean = False
    Dim NumeroLiq As Integer
    Dim strNroAplazamiento As Integer
    Const BOLSACOLOMBIA = 4


    'Date
    Public FechaLiquidacionEdicion As Date = Now
    Dim dtmCierre As Date

    'String
    Public strAccion As String
    Private strUserState As String = String.Empty
    Dim APLAZAMIENTO_LIQUIDACIONES_AMBOS As String = "NO"
    Dim Userstate As String = ""
    Dim mstrEstado As String = String.Empty
    Dim strTipoAplazamiento As String = ""
    Dim strAplazamiento As String = ""
    Dim strError As String
    Const strTitulo As String = "Titulo"
    Const strEfectivo As String = "Efectivo"
    Const STR_AMBOS As String = "Ambos"

    'Double
    Dim mdblValorTolerancia As Double = 0
    Dim mcursaldoOrden As Double
    Dim CANTIDADLIQ As Double
    Dim mdblCantidad As Double

    Public Event NotificacionCambiarForma(ByVal pobjSelected As OyDOperaciones.Operaciones, ByVal pstrLlamado As String)

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
    ''' <history>
    ''' ID caso de prueba: CP0001
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 22/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 22/2015 - Resultado Ok 
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

                ConsultarFechaCierreSistema()
                ConsultarParametros()
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
    ''' Lista de OyDOperaciones.Operaciones que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of OyDOperaciones.Operaciones)
    Public Property ListaEncabezado() As EntitySet(Of OyDOperaciones.Operaciones)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of OyDOperaciones.Operaciones))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

            If Not IsNothing(value) Then
                If logCambiarSelected Then
                    If IsNothing(mobjEncabezadoAnterior) Then
                        EncabezadoSeleccionado = ListaEncabezado.FirstOrDefault
                    Else
                        EncabezadoSeleccionado = mobjEncabezadoAnterior
                    End If
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de OyDOperaciones.Operaciones para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    Public ReadOnly Property ListaEncabezadoPaginada() As PagedCollectionView
        Get
            If logCambiarCollection Then
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
            Else
                Return _ListaEncabezadoPaginada
            End If
        End Get
    End Property

    ''' <summary>
    ''' Elemento de la lista de OyDOperaciones.Operaciones que se encuentra seleccionado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0002
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 22/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 22/2015 - Resultado Ok 
    ''' </history>
    Private WithEvents _EncabezadoSeleccionado As OyDOperaciones.Operaciones
    Public Property EncabezadoSeleccionado() As OyDOperaciones.Operaciones
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As OyDOperaciones.Operaciones)
            _EncabezadoSeleccionado = value

            If logConsultarDetalles Then
                If logBajarIsBusy Then
                    IsBusy = False
                    logBajarIsBusy = False
                End If

                If Selected = False Then

                    If Not value Is Nothing Then
                        If Not IsNothing(EncabezadoSeleccionado.lngIDOrden) And EncabezadoSeleccionado.lngIDOrden <> 0 Then
                            EncabezadoSeleccionado.intFechaOrden = CInt(Mid$(CStr(EncabezadoSeleccionado.lngIDOrden), 1, 4))
                            EncabezadoSeleccionado.strNroOrden = Mid$(CStr(EncabezadoSeleccionado.lngIDOrden), 5)
                        End If
                        NumeroLiq = 0

                        mdcProxy.OperacionesReceptores.Clear()
                            mdcProxy.OperacionesBeneficiarios.Clear()
                            mdcProxy.OperacionesEspecies.Clear()
                            mdcProxy.OperacionesAplazamientos.Clear()
                            mdcProxy.OperacionesCustodias.Clear()

                            If Not logEsNuevo Then
                                'Propiedades para ajustar despues de guardar la operación.
                                If ListaEncabezado.Count > 0 Then
                                    If Not IsNothing(ListaOperacionesReceptores) Then
                                        ListaOperacionesReceptores.Clear()
                                    End If

                                    mdcProxy.Load(mdcProxy.Traer_ReceptoresOrdenes_LiquidacionesQuery(EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CInt(EncabezadoSeleccionado.lngIDOrden), Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresOrdenes, "FiltroReceptoresOrdenes")
                                    mdcProxy.Load(mdcProxy.Traer_BeneficiariosOrdenes_LiquidacionesQuery(EncabezadoSeleccionado.intIDLiquidaciones, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosOrdenes, "FiltroBeneficiariosOrdenes")
                                    mdcProxy.Load(mdcProxy.Traer_EspeciesLiquidacionesQuery(CInt(_EncabezadoSeleccionado.lngID), CInt(_EncabezadoSeleccionado.lngParcial), CDate(_EncabezadoSeleccionado.dtmLiquidacion), Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesLiquidaciones, "FiltroEspeciesLiquidaciones")
                                    mdcProxy.Load(mdcProxy.Traer_AplazamientosLiquidacionesQuery(CInt(EncabezadoSeleccionado.lngID), CDate(EncabezadoSeleccionado.dtmLiquidacion), CInt(EncabezadoSeleccionado.lngParcial), EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAplazamientosLiquidaciones, "FiltroAplazamientosLiquidaciones")
                                    mdcProxy.Load(mdcProxy.Traer_CustodiasLiquidacionesQuery(CInt(EncabezadoSeleccionado.lngIDComisionista),
                                                                                           CInt(EncabezadoSeleccionado.lngIDSucComisionista),
                                                                                           EncabezadoSeleccionado.lngIDComitente,
                                                                                           EncabezadoSeleccionado.strIDEspecie,
                                                                                           EncabezadoSeleccionado.strTipo,
                                                                                           EncabezadoSeleccionado.strClaseOrden,
                                                                                           CInt(EncabezadoSeleccionado.lngID),
                                                                                           CInt(EncabezadoSeleccionado.lngParcial),
                                                                                           EncabezadoSeleccionado.dtmLiquidacion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodiasLiquidaciones, "FiltroCustodiasLiquidaciones")
                                End If
                            End If
                            logEsNuevo = False

                        End If

                    End If
                Selected = False
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        Select Case e.PropertyName
            Case "lngDiasVencimiento"
                If Not IsNothing(EncabezadoSeleccionado.lngPlazo) And Not IsNothing(EncabezadoSeleccionado.lngDiasVencimiento) _
                    And EncabezadoSeleccionado.lngDiasVencimiento > EncabezadoSeleccionado.lngPlazo Then
                    A2Utilidades.Mensajes.mostrarMensaje("Los días al vencimiento deben ser menores o iguales que el plazo del título ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If Not IsNothing(EncabezadoSeleccionado.lngDiasVencimiento) Then
                    If IsNothing(EncabezadoSeleccionado.dtmVencimiento) Then
                        EncabezadoSeleccionado.dtmVencimiento = DateAdd("d", CType(EncabezadoSeleccionado.lngDiasVencimiento, Double), EncabezadoSeleccionado.dtmLiquidacion)
                    End If
                    If Not IsNothing(EncabezadoSeleccionado.lngPlazo) Then
                        If IsNothing(EncabezadoSeleccionado.dtmEmision) Then
                            EncabezadoSeleccionado.dtmEmision = DateAdd("d", CType(EncabezadoSeleccionado.lngPlazo * (1 - 2), Double), EncabezadoSeleccionado.dtmVencimiento)
                        End If
                    End If
                End If
            Case "dtmVencimiento"
                If EncabezadoSeleccionado.dtmVencimiento < EncabezadoSeleccionado.dtmEmision Then
                    A2Utilidades.Mensajes.mostrarMensaje("La Fecha de vencimiento debe ser mayor que la de emisión ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    EncabezadoSeleccionado.dtmVencimiento = Now
                    Exit Sub
                End If
            Case "dblCantidad"
                If ListaEncabezado.Contains(EncabezadoSeleccionado) Then
                    mlogCantidad = True
                End If
            Case "strPlaza"
                If _EncabezadoSeleccionado.strPlaza = "LOC" Then
                    HabilitarComisionistaLocal = True
                    HabilitarComisionistaOtraPlaza = False
                    _EncabezadoSeleccionado.lngIDComisionistaOtraPlaza = 0
                    _EncabezadoSeleccionado.lngIDCiudadOtraPlaza = 0
                Else
                    HabilitarComisionistaLocal = False
                    HabilitarComisionistaOtraPlaza = True
                    _EncabezadoSeleccionado.lngIDComisionistaLocal = 0
                End If
            Case "strTipo"
                If Me._EncabezadoSeleccionado.lngIDOrden <> 0 And Not IsNothing(Me._EncabezadoSeleccionado.lngIDOrden) Then
                    ValidaOrden()
                End If
            Case "strClase"
                If Me._EncabezadoSeleccionado.lngIDOrden <> 0 And Not IsNothing(Me._EncabezadoSeleccionado.lngIDOrden) Then
                    ValidaOrden()
                End If
            Case "strNroOrden"
                If EncabezadoSeleccionado.lngIDOrden <> 0 And Not IsNothing(EncabezadoSeleccionado.lngIDOrden) Then
                    Exit Sub
                End If
                If e.PropertyName.Equals("strNroOrden") Then
                    If Not logNroOrdenEsNumerico Then
                        ValidaOrden()
                    End If
                End If
        End Select

    End Sub

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaOperaciones
    Public Property cb() As CamposBusquedaOperaciones
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaOperaciones)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _HabilitarCamposEspecificos As Boolean
    Public Property HabilitarCamposEspecificos() As Boolean
        Get
            Return _HabilitarCamposEspecificos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposEspecificos = value
            MyBase.CambioItem("HabilitarCamposEspecificos")
        End Set
    End Property

    Private _HabilitarParcial_y_DiasTramo As Boolean
    Public Property HabilitarParcial_y_DiasTramo() As Boolean
        Get
            Return _HabilitarParcial_y_DiasTramo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarParcial_y_DiasTramo = value
            MyBase.CambioItem("HabilitarParcial_y_DiasTramo")
        End Set
    End Property

    Private _HabilitarFechaLiquidacion As Boolean = False
    Public Property HabilitarFechaLiquidacion As Boolean
        Get
            Return _HabilitarFechaLiquidacion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaLiquidacion = value
            MyBase.CambioItem("HabilitarFechaLiquidacion")
        End Set
    End Property

    Private _HabilitarFechaVendido As Boolean = False
    Public Property HabilitarFechaVendido As Boolean
        Get
            Return _HabilitarFechaVendido
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaVendido = value
            MyBase.CambioItem("HabilitarFechaVendido")
        End Set
    End Property

    Private _HabilitarFechaTitulo As Boolean = False
    Public Property HabilitarFechaTitulo As Boolean
        Get
            Return _HabilitarFechaTitulo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaTitulo = value
            MyBase.CambioItem("HabilitarFechaTitulo")
        End Set
    End Property

    Private _HabilitarFechaEfectivo As Boolean = False
    Public Property HabilitarFechaEfectivo As Boolean
        Get
            Return _HabilitarFechaEfectivo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaEfectivo = value
            MyBase.CambioItem("HabilitarFechaEfectivo")
        End Set
    End Property

    Private _DatosTitulo As Boolean
    Public Property DatosTitulo As Boolean
        Get
            Return _DatosTitulo
        End Get
        Set(ByVal value As Boolean)
            _DatosTitulo = value
            MyBase.CambioItem("DatosTitulo")
        End Set
    End Property

    Private _HabilitarManualDias As Boolean
    Public Property HabilitarManualDias As Boolean
        Get
            Return _HabilitarManualDias
        End Get
        Set(ByVal value As Boolean)
            _HabilitarManualDias = value
            MyBase.CambioItem("HabilitarManualDias")
        End Set
    End Property

    Private _HabilitarEmision As Boolean
    Public Property HabilitarEmision As Boolean
        Get
            Return _HabilitarEmision
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEmision = value
            MyBase.CambioItem("HabilitarEmision")
        End Set
    End Property

    Private _HabilitarVencimiento As Boolean
    Public Property HabilitarVencimiento As Boolean
        Get
            Return _HabilitarVencimiento
        End Get
        Set(ByVal value As Boolean)
            _HabilitarVencimiento = value
            MyBase.CambioItem("HabilitarVencimiento")
        End Set
    End Property

    Private _HabilitarComisionistaLocal As Boolean = False
    Public Property HabilitarComisionistaLocal() As Boolean
        Get
            Return _HabilitarComisionistaLocal
        End Get
        Set(ByVal value As Boolean)
            _HabilitarComisionistaLocal = value
            MyBase.CambioItem("HabilitarComisionistaLocal")
        End Set
    End Property

    Private _HabilitarComisionistaOtraPlaza As Boolean = False
    Public Property HabilitarComisionistaOtraPlaza As Boolean
        Get
            Return _HabilitarComisionistaOtraPlaza
        End Get
        Set(ByVal value As Boolean)
            _HabilitarComisionistaOtraPlaza = value
            MyBase.CambioItem("HabilitarComisionistaOtraPlaza")
        End Set
    End Property

    Private _HabilitarRadioButton As Boolean
    Public Property HabilitarRadioButton As Boolean
        Get
            Return _HabilitarRadioButton
        End Get
        Set(ByVal value As Boolean)
            _HabilitarRadioButton = value
            MyBase.CambioItem("HabilitarRadioButton")
        End Set
    End Property

    Private _HabilitaBoton As Boolean = True
    Public Property HabilitaBoton As Boolean
        Get
            Return _HabilitaBoton
        End Get
        Set(ByVal value As Boolean)
            _HabilitaBoton = value
            MyBase.CambioItem("HabilitaBoton")
        End Set
    End Property

    Private _VmOperaciones As OperacionesView
    Public Property VmOperaciones As OperacionesView
        Get
            Return _VmOperaciones
        End Get
        Set(ByVal value As OperacionesView)
            _VmOperaciones = value
            MyBase.CambioItem("VmOperaciones")
        End Set
    End Property

    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero As Integer
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")
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

#End Region

#Region "Propiedades - Tablas hijas"

#Region "OperacionesReceptores"
    Private _ListaOperacionesReceptores As EntitySet(Of OperacionesReceptores)
    Public Property ListaOperacionesReceptores() As EntitySet(Of OperacionesReceptores)
        Get
            Return _ListaOperacionesReceptores
        End Get
        Set(ByVal value As EntitySet(Of OperacionesReceptores))
            _ListaOperacionesReceptores = value
            MyBase.CambioItem("ListaOperacionesReceptores")
        End Set
    End Property

    Private _ReceptoresOrdenesSelected As OperacionesReceptores
    Public Property ReceptoresOrdenesSelected() As OperacionesReceptores
        Get
            Return _ReceptoresOrdenesSelected
        End Get
        Set(ByVal value As OperacionesReceptores)
            'If Not value Is Nothing Then
            _ReceptoresOrdenesSelected = value
            MyBase.CambioItem("ReceptoresOrdenesSelected")
            'End If
        End Set
    End Property

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmReceptoresOrdenes"
                    Dim NewReceptoresOrdenes As New OyDOperaciones.OperacionesReceptores
                    NewReceptoresOrdenes.strTipoOrden = EncabezadoSeleccionado.strTipo
                    NewReceptoresOrdenes.strClaseOrden = EncabezadoSeleccionado.strClaseOrden
                    NewReceptoresOrdenes.lngIDOrden = CInt(EncabezadoSeleccionado.lngIDOrden)
                    NewReceptoresOrdenes.lngVersion = 0
                    NewReceptoresOrdenes.strUsuario = Program.Usuario
                    NewReceptoresOrdenes.logLider = False

                    ListaOperacionesReceptores.Add(NewReceptoresOrdenes)
                    ReceptoresOrdenesSelected = NewReceptoresOrdenes
                    MyBase.CambioItem("ReceptoresOrdenesSelected")
                    MyBase.CambioItem("ListaOperacionesReceptores")

            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro detalle", Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmReceptoresOrdenes"
                    If Not IsNothing(ListaOperacionesReceptores) Then
                        If Not IsNothing(ListaOperacionesReceptores) Then
                            If Not IsNothing(_ReceptoresOrdenesSelected) Then

                                If ListaOperacionesReceptores.Count > 0 Then
                                    Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptoresOrdenesSelected, ListaOperacionesReceptores.ToList)

                                    If ListaOperacionesReceptores.Where(Function(i) i.intIDReceptoresOrdenes = ReceptoresOrdenesSelected.intIDReceptoresOrdenes).Count > 0 Then
                                        ListaOperacionesReceptores.Remove(ListaOperacionesReceptores.Where(Function(i) i.intIDReceptoresOrdenes = ReceptoresOrdenesSelected.intIDReceptoresOrdenes).First)
                                    End If

                                    If ListaOperacionesReceptores.Count > 0 Then
                                        Program.PosicionarItemLista(ReceptoresOrdenesSelected, ListaOperacionesReceptores.ToList, intRegistroPosicionar)
                                    Else
                                        ReceptoresOrdenesSelected = Nothing
                                    End If
                                End If
                                MyBase.CambioItem("ReceptoresOrdenesSelected")
                                MyBase.CambioItem("ListaOperacionesReceptores")
                            End If
                        End If
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el registro detalle", Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "OperacionesEspecies"
    Private _ListaOperacionesEspecies As EntitySet(Of OperacionesEspecies)
    Public Property ListaOperacionesEspecies() As EntitySet(Of OperacionesEspecies)
        Get
            Return _ListaOperacionesEspecies
        End Get
        Set(ByVal value As EntitySet(Of OperacionesEspecies))
            _ListaOperacionesEspecies = value
            MyBase.CambioItem("ListaOperacionesEspecies")
        End Set
    End Property

    Private _EspeciesLiquidacionesSelected As OperacionesEspecies
    Public Property EspeciesLiquidacionesSelected() As OperacionesEspecies
        Get
            Return _EspeciesLiquidacionesSelected
        End Get
        Set(ByVal value As OperacionesEspecies)
            _EspeciesLiquidacionesSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("EspeciesLiquidacionesSelected")
            End If
        End Set
    End Property
#End Region

#Region "OperacionesAplazamientos"
    Private _ListaOperacionesAplazamientos As EntitySet(Of OperacionesAplazamientos)
    Public Property ListaOperacionesAplazamientos() As EntitySet(Of OperacionesAplazamientos)
        Get
            Return _ListaOperacionesAplazamientos
        End Get
        Set(ByVal value As EntitySet(Of OperacionesAplazamientos))
            _ListaOperacionesAplazamientos = value
            MyBase.CambioItem("ListaOperacionesAplazamientos")
        End Set
    End Property

    Private _AplazamientosLiquidacionesSelected As OperacionesAplazamientos
    Public Property AplazamientosLiquidacionesSelected() As OperacionesAplazamientos
        Get
            Return _AplazamientosLiquidacionesSelected
        End Get
        Set(ByVal value As OperacionesAplazamientos)
            _AplazamientosLiquidacionesSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("AplazamientosLiquidacionesSelected")
            End If
        End Set
    End Property
#End Region

#Region "OperacionesCustodias"
    Private _ListaOperacionesCustodias As EntitySet(Of OperacionesCustodias)
    Public Property ListaOperacionesCustodias() As EntitySet(Of OperacionesCustodias)
        Get
            Return _ListaOperacionesCustodias
        End Get
        Set(ByVal value As EntitySet(Of OperacionesCustodias))
            _ListaOperacionesCustodias = value
            MyBase.CambioItem("ListaOperacionesCustodias")
        End Set
    End Property
#End Region

#Region "OperacionesBeneficiarios"
    Private _ListaOperacionesBeneficiarios As EntitySet(Of OyDOperaciones.OperacionesBeneficiarios)
    Public Property ListaOperacionesBeneficiarios() As EntitySet(Of OyDOperaciones.OperacionesBeneficiarios)
        Get
            Return _ListaOperacionesBeneficiarios
        End Get
        Set(ByVal value As EntitySet(Of OyDOperaciones.OperacionesBeneficiarios))
            _ListaOperacionesBeneficiarios = value
            MyBase.CambioItem("ListaOperacionesBeneficiarios")
        End Set
    End Property

    Private _BeneficiariosOrdenesSelected As OperacionesBeneficiarios
    Public Property BeneficiariosOrdenesSelected() As OperacionesBeneficiarios
        Get
            Return _BeneficiariosOrdenesSelected
        End Get
        Set(ByVal value As OperacionesBeneficiarios)
            _BeneficiariosOrdenesSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("BeneficiariosOrdenesSelected")
            End If
        End Set
    End Property
#End Region

#End Region


#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0005, CP0006, CP0007, CP0008, CP0009, CP00010, CP00011, CP00012, CP00015, CP00016, CP00019, CP00020
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 22/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 22/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Sub NuevoRegistro()
        Dim objNvoEncabezado As New OyDOperaciones.Operaciones

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            logEstadoRegistro = True

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                mobjEncabezadoPorDefecto.lngParcial = 0
                Program.CopiarObjeto(Of OyDOperaciones.Operaciones)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intIDLiquidaciones = -1
                objNvoEncabezado.lngID = 0
                'objNvoEncabezado.lngParcial = -1
                objNvoEncabezado.lngParcial = 0
                objNvoEncabezado.strTipo = "C"
                objNvoEncabezado.strClaseOrden = "A"
                objNvoEncabezado.strIDEspecie = String.Empty
                objNvoEncabezado.lngIDOrden = 0
                objNvoEncabezado.strPrefijo = Nothing
                objNvoEncabezado.lngIDFactura = Nothing
                objNvoEncabezado.strFacturada = Nothing
                objNvoEncabezado.lngIDComitente = String.Empty
                objNvoEncabezado.lngIDOrdenante = String.Empty
                objNvoEncabezado.lngIDBolsa = 0
                objNvoEncabezado.dblValBolsa = 0
                objNvoEncabezado.bytIDRueda = 0
                objNvoEncabezado.dblTasaDescuento = 0
                objNvoEncabezado.dblTasaCompraVende = 0
                objNvoEncabezado.strModalidad = String.Empty
                objNvoEncabezado.strIndicadorEconomico = String.Empty
                objNvoEncabezado.dblPuntosIndicador = 0
                objNvoEncabezado.lngPlazo = 0
                objNvoEncabezado.dtmLiquidacion = Now.Date
                objNvoEncabezado.dtmCumplimiento = Now.Date
                objNvoEncabezado.dtmEmision = Nothing
                objNvoEncabezado.dtmVencimiento = Nothing
                objNvoEncabezado.logOtraPlaza = True
                objNvoEncabezado.strPlaza = String.Empty
                objNvoEncabezado.lngIDComisionistaLocal = 0
                objNvoEncabezado.lngIDComisionistaOtraPlaza = 0
                objNvoEncabezado.lngIDCiudadOtraPlaza = 0
                objNvoEncabezado.dblTasaEfectiva = 0
                objNvoEncabezado.dblCantidad = 0
                objNvoEncabezado.curPrecio = 0
                objNvoEncabezado.curTransaccion = 0
                objNvoEncabezado.curSubTotalLiq = 0
                objNvoEncabezado.curTotalLiq = 0
                objNvoEncabezado.curComision = 0
                objNvoEncabezado.curRetencion = 0
                objNvoEncabezado.curIntereses = 0
                objNvoEncabezado.curValorIva = 0
                objNvoEncabezado.lngDiasIntereses = 0
                objNvoEncabezado.dblFactorComisionPactada = 0
                objNvoEncabezado.strMercado = String.Empty
                objNvoEncabezado.strNroTitulo = String.Empty
                objNvoEncabezado.lngIDCiudadExpTitulo = 0
                objNvoEncabezado.lngPlazoOriginal = 0
                objNvoEncabezado.logAplazamiento = True
                objNvoEncabezado.bytVersionPapeleta = 0
                objNvoEncabezado.dtmEmisionOriginal = Nothing
                objNvoEncabezado.dtmVencimientoOriginal = Nothing
                objNvoEncabezado.lngImpresiones = 0
                objNvoEncabezado.strFormaPago = String.Empty
                objNvoEncabezado.lngCtrlImpPapeleta = 0
                objNvoEncabezado.lngDiasVencimiento = 0
                objNvoEncabezado.strPosicionPropia = String.Empty
                objNvoEncabezado.strTransaccion = String.Empty
                objNvoEncabezado.strTipoOperacion = String.Empty
                objNvoEncabezado.lngDiasContado = 0
                objNvoEncabezado.logOrdinaria = True
                objNvoEncabezado.strObjetoOrdenExtraordinaria = String.Empty
                objNvoEncabezado.lngNumPadre = 0
                objNvoEncabezado.lngParcialPadre = 0
                objNvoEncabezado.dtmOperacionPadre = Nothing
                objNvoEncabezado.lngDiasTramo = 0
                objNvoEncabezado.logVendido = True
                objNvoEncabezado.dtmVendido = Nothing
                objNvoEncabezado.logManual = True
                objNvoEncabezado.dblValorTraslado = 0
                objNvoEncabezado.dblValorBrutoCompraVencida = 0
                objNvoEncabezado.strAutoRetenedor = String.Empty
                objNvoEncabezado.strSujeto = String.Empty
                objNvoEncabezado.dblPcRenEfecCompraRet = 0
                objNvoEncabezado.dblPcRenEfecVendeRet = 0
                objNvoEncabezado.strReinversion = String.Empty
                objNvoEncabezado.strSwap = String.Empty
                objNvoEncabezado.lngNroSwap = 0
                objNvoEncabezado.strCertificacion = String.Empty
                objNvoEncabezado.dblDescuentoAcumula = 0
                objNvoEncabezado.dblPctRendimiento = 0
                objNvoEncabezado.dtmFechaCompraVencido = Nothing
                objNvoEncabezado.dblPrecioCompraVencido = 0
                objNvoEncabezado.strConstanciaEnajenacion = String.Empty
                objNvoEncabezado.strRepoTitulo = String.Empty
                objNvoEncabezado.dblServBolsaVble = 0
                objNvoEncabezado.dblServBolsaFijo = 0
                objNvoEncabezado.logTraslado = CType(0, Boolean?)
                objNvoEncabezado.strUBICACIONTITULO = String.Empty
                objNvoEncabezado.strHoraGrabacion = String.Empty
                objNvoEncabezado.strOrigenOperacion = String.Empty
                objNvoEncabezado.lngCodigoOperadorCompra = 0
                objNvoEncabezado.lngCodigoOperadorVende = 0
                objNvoEncabezado.strIdentificacionRemate = String.Empty
                objNvoEncabezado.strModalidaOperacion = String.Empty
                objNvoEncabezado.strIndicadorPrecio = String.Empty
                objNvoEncabezado.strPeriodoExdividendo = String.Empty
                objNvoEncabezado.lngPlazoOperacionRepo = 0
                objNvoEncabezado.dblValorCaptacionRepo = 0
                objNvoEncabezado.lngVolumenCompraRepo = 0
                objNvoEncabezado.dblPrecioNetoFraccion = 0
                objNvoEncabezado.lngVolumenNetoFraccion = 0
                objNvoEncabezado.lngCodigoContactoComercial = String.Empty
                objNvoEncabezado.lngNroFraccionOperacion = 0
                objNvoEncabezado.strIdentificacionPatrimonio1 = String.Empty
                objNvoEncabezado.strTipoidentificacionCliente2 = String.Empty
                objNvoEncabezado.strNitCliente2 = String.Empty
                objNvoEncabezado.strIdentificacionPatrimonio2 = String.Empty
                objNvoEncabezado.strTipoIdentificacionCliente3 = String.Empty
                objNvoEncabezado.strNitCliente3 = String.Empty
                objNvoEncabezado.strIdentificacionPatrimonio3 = String.Empty
                objNvoEncabezado.strIndicadorOperacion = String.Empty
                objNvoEncabezado.dblBaseRetencion = 0
                objNvoEncabezado.dblPorcRetencion = 0
                objNvoEncabezado.dblBaseRetencionTranslado = 0
                objNvoEncabezado.dblPorcRetencionTranslado = 0
                objNvoEncabezado.dblPorcIvaComision = 0
                objNvoEncabezado.strIndicadorAcciones = String.Empty
                objNvoEncabezado.strOperacionNegociada = String.Empty
                objNvoEncabezado.dtmFechaConstancia = Nothing
                objNvoEncabezado.dblValorConstancia = 0
                objNvoEncabezado.strGeneraConstancia = String.Empty
                objNvoEncabezado.logCargado = True
                objNvoEncabezado.dtmCumplimientoTitulo = Now.Date
                objNvoEncabezado.intNroLote = 0
                objNvoEncabezado.dblValorEntregaContraPago = 0
                objNvoEncabezado.strAquienSeEnviaRetencion = String.Empty
                objNvoEncabezado.strIDBaseDias = String.Empty
                objNvoEncabezado.strTipoDeOferta = String.Empty
                objNvoEncabezado.intNroLoteENC = 0
                objNvoEncabezado.dtmContabilidadENC = Nothing
                objNvoEncabezado.strCodigoIntermediario = String.Empty
                objNvoEncabezado.curValorExtemporaneo = 0
                objNvoEncabezado.strPosicionExtemporaneo = String.Empty
                objNvoEncabezado.dtmActualizacion = Date.Now
                objNvoEncabezado.strUsuario = Program.Usuario
                objNvoEncabezado.strInfoSesion = String.Empty
            End If

            HabilitaBoton = False
            HabilitarRadioButton = True
            logEsNuevo = True

            logNuevoRegistro = True
            logEditarRegistro = False
            ESTADO_EDICION = False

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = EncabezadoSeleccionado 'ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado

            'Propiedades para ajustar despues de guardar la operación.
            If EncabezadoSeleccionado.strClaseOrden.Equals("C") Then
                HabilitarManualDias = True
            End If

            mlogTerminoSubmitChanges = False

            Userstate = String.Empty

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
    ''' ID caso de prueba: CP0003
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 22/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 22/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Async Sub Filtrar()
        Try
            If Not ValidaFiltro Then
                Selected = True
                IsBusy = True
                mobjEncabezadoAnterior = Nothing
                Await ConsultarEncabezado(True, FiltroVM)
            End If
            ValidaFiltro = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la ejecución del filtro", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub QuitarFiltro()
        MyBase.QuitarFiltro()
        strUserState = ""
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0004
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 22/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 22/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Sub Buscar()
        Try
            PrepararNuevaBusqueda()
            mobjEncabezadoAnterior = Nothing
            'CType(VmOperaciones.dfBuscar.FindName("txtnOrden"), TextBox).Focus()
            MyBase.Buscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método buscar", Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            Selected = True
            If cb.lngID <> 0 Or cb.lngIDComitente <> String.Empty Or cb.strTipo <> String.Empty Or cb.strClaseOrden <> String.Empty Or
                cb.strNroOrden <> String.Empty Or Not IsNothing(cb.dtmLiquidacion) Or Not IsNothing(cb.dtmCumplimientoTitulo) Then 'Validar que ingresó algo en los campos de búsqueda

                ErrorForma = String.Empty

                Dim Orden As String

                If Not IsNumeric(cb.strNroOrden) And Not IsNothing(cb.strNroOrden) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El valor de La orden debe ser númerico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    cb.strNroOrden = Nothing
                    Exit Sub
                End If

                IsBusy = True
                If cb.strNroOrden Is Nothing Then
                    Orden = "0"
                Else
                    Orden = CStr(cb.intFechaOrden) + Format(CInt(cb.strNroOrden), "000000")
                End If

                'JABG20160309
                logCambiarSelected = True
                logCambiarLista = False
                logConsultarDetalles = True

                Await ConsultarEncabezado(False, String.Empty, CInt(cb.lngID), cb.lngIDComitente, cb.strTipo, cb.strClaseOrden, CDate(IIf(cb.dtmLiquidacion Is Nothing, "1799/01/01", cb.dtmLiquidacion)), CDate(IIf(cb.dtmCumplimientoTitulo Is Nothing, "1799/01/01", cb.dtmCumplimientoTitulo)), CInt(Orden), 0, cb.lngParcial)
                MyBase.ConfirmarBuscar()
                strUserState = "Busqueda"
            Else
                ErrorForma = ""
                IsBusy = True

                Await ConsultarEncabezado(False, String.Empty, CInt(cb.lngID), cb.lngIDComitente, cb.strTipo, cb.strClaseOrden, CDate(IIf(cb.dtmLiquidacion Is Nothing, "1799/01/01", cb.dtmLiquidacion)), CDate(IIf(cb.dtmCumplimientoTitulo Is Nothing, "1799/01/01", cb.dtmCumplimientoTitulo)), CInt(cb.intFechaOrden), CInt(cb.intFechaOrden), cb.lngParcial)
                MyBase.ConfirmarBuscar()
                strUserState = "Busqueda"
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        MyBase.CancelarBuscar()
        strUserState = ""
    End Sub


    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>


    Public Overrides Async Sub ActualizarRegistro()
        Try
            'CFMA20180115
            mstrEstado = "update"
            IsBusy = True
            Dim logContinuarGuardado As Boolean = True
            logContinuarGuardado = Await validarFechacierre("Guardar", _EncabezadoSeleccionado.dtmLiquidacion)

            IsBusy = False

            If logContinuarGuardado = False Then
                Exit Sub
            End If
            'CFMA20180115
            'ConsultarFechaCierreSistema("GUARDAR")
            ContinuarGuardadoDocumento()
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
    ''' ID caso de prueba: CP00013, CP00017, CP00018
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 22/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 22/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Async Sub EditarRegistro()
        Try
            'CFMA20180115
            IsBusy = True
            Dim logContinuarGuardado As Boolean = True
            logContinuarGuardado = Await validarFechacierre("Editar", _EncabezadoSeleccionado.dtmLiquidacion)

            IsBusy = False
            If logContinuarGuardado = False Then
                MyBase.RetornarValorEdicionNavegacion()
                Exit Sub
            End If
            'CFMA20180115
            mdcProxy.ModificacionValidars.Clear()
            mdcProxy.Load(mdcProxy.OperacionesModificacionValidarQuery(CInt(EncabezadoSeleccionado.lngID), CInt(EncabezadoSeleccionado.lngParcial), EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CDate(EncabezadoSeleccionado.dtmLiquidacion), EncabezadoSeleccionado.strTipoDeOferta, EncabezadoSeleccionado.lngIDComitente, EncabezadoSeleccionado.strIDEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminoModificacionValidar, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de editar el registro", Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub TerminoModificacionValidar(ByVal lo As LoadOperation(Of ModificacionValidar))
        Try
            Dim strMsg As String = String.Empty
            If Not lo.HasError Then
                If lo.Entities.FirstOrDefault.logExitoso Then

                    If mdcProxy.IsLoading Then
                        MyBase.RetornarValorEdicionNavegacion()
                        A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If

                    'Propiedad utilizada para habilitar o deshabilitar los botones de aplazar y duplicar
                    mobjEncabezadoAnterior = EncabezadoSeleccionado 'ObtenerRegistroAnterior()

                    logEstadoRegistro = False
                    If Not IsNothing(EncabezadoSeleccionado) Then
                        ' ConsultarFechaCierreSistema("EDITAR")
                        ContinuarEdicionDocumento()
                    Else
                        MyBase.RetornarValorEdicionNavegacion()
                    End If

                Else
                    MyBase.RetornarValorEdicionNavegacion()
                    Mensajes.mostrarMensaje(lo.Entities.FirstOrDefault.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                IsBusy = False
                MyBase.RetornarValorEdicionNavegacion()
                If lo.Error.ToString.Contains("ErrorPersonalizado") Then
                    Dim intPosIni As Integer = lo.Error.ToString.IndexOf("ErrorPersonalizado,") + 20
                    Dim intPosFin As Integer = lo.Error.ToString.IndexOf("|")
                    strMsg = lo.Error.ToString.Substring(intPosIni, intPosFin - intPosIni)
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    lo.MarkErrorAsHandled()
                    Exit Sub
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de operaciones",
                                                Me.ToString(), "TerminoModificacionValidar", Application.Current.ToString(), Program.Maquina, lo.Error)
                    lo.MarkErrorAsHandled()
                End If


            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la operación", Me.ToString(), "TerminoModificacionValidar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Async Sub TerminoEliminacionValidar(ByVal lo As LoadOperation(Of ModificacionValidar))
        Try
            Dim strMsg As String = String.Empty
            If Not lo.HasError Then
                If lo.Entities.FirstOrDefault.logExitoso Then

                    If Not IsNothing(_EncabezadoSeleccionado) Then
                        If mdcProxy.IsLoading Then
                            A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If

                        If EncabezadoSeleccionado.lngIDFactura > 0 And Not IsNothing(EncabezadoSeleccionado.lngIDFactura) Then
                            A2Utilidades.Mensajes.mostrarMensaje("La liquidación está facturada, no se puede borrar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If

                        If EncabezadoSeleccionado.logRetardoPadre Then
                            A2Utilidades.Mensajes.mostrarMensajePregunta("Desea guardar el historial de Retardos asociados a esta operación.", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf TerminaPreguntaHistorialretardos, True, "¿Desea guardarlos?")
                            IsBusy = False
                        Else
                            ContinuarBorradoDocumento()
                        End If

                    'ConsultarFechaCierreSistema("BORRAR")
                    'ContinuarBorradoDocumento()
                    'CFMA20180115
                End If

                Else
                    Mensajes.mostrarMensaje(lo.Entities.FirstOrDefault.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                IsBusy = False
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de operaciones", _
                '                                 Me.ToString(), "TerminoModificacionValidar", Application.Current.ToString(), Program.Maquina, lo.Error)
                'lo.MarkErrorAsHandled()
                If lo.Error.ToString.Contains("ErrorPersonalizado") Then
                    Dim intPosIni As Integer = lo.Error.ToString.IndexOf("ErrorPersonalizado,") + 20
                    Dim intPosFin As Integer = lo.Error.ToString.IndexOf("|")
                    strMsg = lo.Error.ToString.Substring(intPosIni, intPosFin - intPosIni)
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    lo.MarkErrorAsHandled()
                    Exit Sub
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de operaciones",
                                                Me.ToString(), "TerminoModificacionValidar", Application.Current.ToString(), Program.Maquina, lo.Error)
                    lo.MarkErrorAsHandled()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la operación", Me.ToString(), "TerminoModificacionValidar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty
            HabilitaBoton = True
            HabilitarCamposEspecificos = False
            HabilitarComisionistaLocal = False
            HabilitarComisionistaOtraPlaza = False
            HabilitarFechaLiquidacion = False
            HabilitarFechaVendido = False
            HabilitarFechaTitulo = False
            HabilitarFechaEfectivo = False
            HabilitarParcial_y_DiasTramo = False
            HabilitarVencimiento = False
            DatosTitulo = False
            HabilitarEmision = False
            logNuevoRegistro = False
            logEditarRegistro = False
            lngIDLiquidacionEdicion = 0
            ParcialEdicion = 0
            FechaLiquidacionEdicion = Now

            'Propiedades para ajustar despues de guardar la operación.
            HabilitarRadioButton = False
            HabilitarManualDias = False


            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxy.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
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
    ''' ID caso de prueba: CP0023
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 22/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 22/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Async Sub BorrarRegistro()
        Try
            'CFMA20180115
            IsBusy = True
            Dim logContinuarGuardado As Boolean = True
            logContinuarGuardado = Await validarFechacierre("Borrar", _EncabezadoSeleccionado.dtmLiquidacion)
            IsBusy = False
            If logContinuarGuardado = False Then
                Exit Sub
            End If
            'CFMA20180115
            mdcProxy.ModificacionValidars.Clear()
            mdcProxy.Load(mdcProxy.OperacionesModificacionValidarQuery(CInt(EncabezadoSeleccionado.lngID), CInt(EncabezadoSeleccionado.lngParcial), EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CDate(EncabezadoSeleccionado.dtmLiquidacion), EncabezadoSeleccionado.strTipoDeOferta, EncabezadoSeleccionado.lngIDComitente, EncabezadoSeleccionado.strIDEspecie, Program.Usuario, Program.HashConexion),
                          AddressOf TerminoEliminacionValidar, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ValidaLiq(Optional ByVal pstrUserState As String = "")
        Try
            If EncabezadoSeleccionado.lngID <> 0 Then
                If logNuevoRegistro Then
                    mdcProxy.ValidarLiquidacion(CInt(EncabezadoSeleccionado.lngID), EncabezadoSeleccionado.lngParcial, EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, EncabezadoSeleccionado.dtmLiquidacion, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerValidar, pstrUserState)
                ElseIf logEditarRegistro And (EncabezadoSeleccionado.lngID <> lngIDLiquidacionEdicion Or EncabezadoSeleccionado.lngParcial <> ParcialEdicion Or EncabezadoSeleccionado.dtmLiquidacion <> FechaLiquidacionEdicion) Then
                    mdcProxy.ValidarLiquidacion(CInt(EncabezadoSeleccionado.lngID), EncabezadoSeleccionado.lngParcial, EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, EncabezadoSeleccionado.dtmLiquidacion, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerValidar, pstrUserState)
                ElseIf pstrUserState.Equals("ACTUALIZAR") Then
                    GuardarLiq()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar de la operación", Me.ToString(), "ValidaLiq", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerValidar(ByVal lo As InvokeOperation(Of Integer))
        Try
            If Not lo.HasError Then
                NumeroLiq = lo.Value
                If NumeroLiq > 0 Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("La liquidación " + EncabezadoSeleccionado.lngID.ToString + " con el parcial " + EncabezadoSeleccionado.lngParcial.ToString + " y fecha " + CStr(EncabezadoSeleccionado.dtmLiquidacion) + " ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    If EncabezadoSeleccionado.strClaseOrden.Equals("C") Then
                        DatosTitulo = True
                        HabilitarEmision = True
                        HabilitarManualDias = True
                        HabilitarVencimiento = True
                    End If

                    If lo.UserState Is "ACTUALIZAR" Then
                        ActualizarRegistroDespuesValidadoDatos()
                    Else
                        IsBusy = False
                    End If
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones",
                                                 Me.ToString(), "Terminotraervalidar", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la operación", Me.ToString(), "TerminoTraerValidar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'llevarvalores
    Public Sub CalcularValoresVM()
        Try
            If Editando Then
                DisparaFocus = True
                EncabezadoSeleccionado.curComision = EncabezadoSeleccionado.curTransaccion * CType(IIf(IsNothing(EncabezadoSeleccionado.dblFactorComisionPactada), 0, EncabezadoSeleccionado.dblFactorComisionPactada), Double)
                EncabezadoSeleccionado.curTotalLiq = CType(IIf(IsNothing(EncabezadoSeleccionado.curTotalLiq), 0, EncabezadoSeleccionado.curTotalLiq), Double)
                EncabezadoSeleccionado.curTotalLiq = CType(IIf(IsNothing(EncabezadoSeleccionado.curSubTotalLiq), 0, EncabezadoSeleccionado.curSubTotalLiq), Double)
                If EncabezadoSeleccionado.curPrecio = 0 Then
                    EncabezadoSeleccionado.curTransaccion = EncabezadoSeleccionado.dblCantidad
                    EncabezadoSeleccionado.curSubTotalLiq = EncabezadoSeleccionado.curTransaccion
                    EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTransaccion
                    EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq + CType(IIf((EncabezadoSeleccionado.strTipo = "C" Or EncabezadoSeleccionado.strTipo = "R") And EncabezadoSeleccionado.strPosicionExtemporaneo = "S", EncabezadoSeleccionado.curValorExtemporaneo, -EncabezadoSeleccionado.curValorExtemporaneo), Double)
                    EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq + CType(IIf((EncabezadoSeleccionado.strTipo = "C" Or EncabezadoSeleccionado.strTipo = "R") And EncabezadoSeleccionado.strPosicionExtemporaneo = "A", -EncabezadoSeleccionado.curValorExtemporaneo, EncabezadoSeleccionado.curValorExtemporaneo), Double)
                Else
                    mdcProxy.Verilifavalor(EncabezadoSeleccionado.strIDEspecie, EncabezadoSeleccionado.dtmLiquidacion, Nothing, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerAplazamientosLiquidacionesvalor, Nothing)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular valores", Me.ToString(), "CalcularValoresVM", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub RefrescarForma()
        Try
            If Not IsNothing(EncabezadoSeleccionado) Then
                cb = New CamposBusquedaOperaciones

                cb.lngID = CInt(EncabezadoSeleccionado.lngID)
                cb.strTipo = EncabezadoSeleccionado.strTipo
                cb.strClaseOrden = EncabezadoSeleccionado.strClaseOrden
                cb.dtmLiquidacion = EncabezadoSeleccionado.dtmLiquidacion
                cb.dtmCumplimientoTitulo = EncabezadoSeleccionado.dtmCumplimiento
                cb.lngIDComitente = EncabezadoSeleccionado.lngIDComitente
                cb.lngParcial = CInt(EncabezadoSeleccionado.lngParcial)
                'ConfirmarBuscar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta para refrescar la forma", Me.ToString(), "RefrescarForma", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Function ValidarReceptoresOrdenes() As Boolean
        Dim retorno As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            Dim ExisteLider As Integer
            Dim SumaPorcentaje As Double

            If ListaOperacionesReceptores.Count <> 0 Then
                For Each DetalleReceptor In ListaOperacionesReceptores

                    If IsNothing(DetalleReceptor.dblPorcentaje) Or DetalleReceptor.dblPorcentaje = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("El porcentaje no puede ser vacío.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        retorno = False
                        Return retorno
                    End If

                    If DetalleReceptor.logLider Then
                        ExisteLider = ExisteLider + 1
                    End If

                    If IsNothing(DetalleReceptor.strIDReceptor) Then
                        A2Utilidades.Mensajes.mostrarMensaje("El código de receptor no puede ser vacío.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        retorno = False
                        Return retorno
                    End If

                    If IsNothing(DetalleReceptor.strNombre) Then
                        A2Utilidades.Mensajes.mostrarMensaje("El nombre del receptor no puede ser vacío, este se establece seleccionando el código del receptor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        retorno = False
                        Return retorno
                    End If

                    SumaPorcentaje = CDbl(SumaPorcentaje + DetalleReceptor.dblPorcentaje)
                Next

                If ExisteLider = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe haber un receptor líder.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    retorno = False
                    Return retorno
                End If

                If ExisteLider > 1 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Sólo debe existir un líder.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    retorno = False
                    Return retorno
                End If

                If SumaPorcentaje <> 100 Then
                    A2Utilidades.Mensajes.mostrarMensaje("El porcentaje es: " & SumaPorcentaje & ", el total debe ser 100.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    retorno = False
                    Return retorno
                End If

                Dim NumeroErrores = (From lr In ListaOperacionesReceptores Where lr.HasValidationErrors = True).Count
                If NumeroErrores <> 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    retorno = False
                    Return retorno
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe existir al menos un receptor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                retorno = False
                Return retorno
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ValidarReceptoresOrdenes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return retorno
    End Function

    Public Sub ActualizarRegistroDespuesValidadoDatos()
        Try
            If EncabezadoSeleccionado.logManual Then
                If EncabezadoSeleccionado.dblCantidad > mcursaldoOrden Then
                    logDisparaGuardar = False
                    A2Utilidades.Mensajes.mostrarMensajePregunta("La cantidad de la operación supera el saldo de la Orden.", Program.TituloSistema, ValoresUserState.Actualizar.ToString(), AddressOf TerminaPreguntaSobregiro, True, "¿Desea continuar?")
                    IsBusy = False
                    Exit Sub
                End If
            End If

            OrdenesLiq()
        Catch ex As Exception
            IsBusy = False
            DisparaGuardar = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistroDespuesValidadoDatos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ValidaOrden()
        Try
            If IsNothing(EncabezadoSeleccionado.intFechaOrden) Then
                Exit Sub
            End If

            If Not IsNumeric(EncabezadoSeleccionado.strNroOrden) Then
                A2Utilidades.Mensajes.mostrarMensaje("El número de la orden debe ser un valor numerico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logNroOrdenEsNumerico = True
                EncabezadoSeleccionado.strNroOrden = Nothing
                logNroOrdenEsNumerico = False
                Exit Sub
            End If

            Dim a = CStr(EncabezadoSeleccionado.intFechaOrden) + Format(CInt(EncabezadoSeleccionado.strNroOrden), "000000")
            EncabezadoSeleccionado.lngIDOrden = CInt(a)
            LiquidacionesConsultarValidar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la orden", Me.ToString(), "ValidaOrden", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se activa desde el botón Aplazar.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID caso de prueba: CP0021
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 22/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 22/2015 - Resultado Ok 
    ''' </history>
    Public Async Sub Aplazar()
        Try
            If IsNothing(EncabezadoSeleccionado) Then
                Exit Sub
            End If

            'CFMA20180115
            'If Not ValidaFechaCierre("aplazada") Then
            '    Exit Sub
            'End If
            IsBusy = True
            Dim logContinuarGuardado As Boolean = True
            logContinuarGuardado = Await validarFechacierre("Aplazar", _EncabezadoSeleccionado.dtmLiquidacion)
            If logContinuarGuardado = False Then
                IsBusy = False
                Exit Sub
            End If


            If EncabezadoSeleccionado.strTipo.Equals("R") Or EncabezadoSeleccionado.strTipo.Equals("S") Then
                strTipoAplazamiento = "I"
            Else
                strTipoAplazamiento = "S"
            End If
            aplazamientoserie = New Aplazamiento_En_Serie(strTipoAplazamiento, CDate(EncabezadoSeleccionado.dtmLiquidacion), CDate(EncabezadoSeleccionado.dtmCumplimientoTitulo), APLAZAMIENTO_LIQUIDACIONES_AMBOS)
            AddHandler aplazamientoserie.Closed, AddressOf CerroVentana
            Program.Modal_OwnerMainWindowsPrincipal(aplazamientoserie)
            aplazamientoserie.ShowDialog()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al aplazar la operación", Me.ToString(), "Aplazar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se activa desde el botón Duplicar.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID caso de prueba: CP0022
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 22/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 22/2015 - Resultado Ok 
    ''' </history>
    Public Sub Duplicar()
        Try
            If IsNothing(EncabezadoSeleccionado) Then
                Exit Sub
            End If
            IsBusy = True
            logEstadoRegistro = True
            _mlogDuplicando = True
            logNuevoRegistro = True
            logEditarRegistro = False
            ESTADO_EDICION = False
            LiquidacionesConsultarValidar()
            MyBase.CambioItem("Editando")
            Duplica = True
            Userstate = String.Empty
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al duplicar la operación", Me.ToString(), "Duplicar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SeleccionarCampoTab(ByVal pstrNombreCampo As String)
        Try
            If DicCamposTab.ContainsKey(pstrNombreCampo) Then
                Dim miTab = DicCamposTab(pstrNombreCampo)
                TabSeleccionadaFinanciero = miTab
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el tab", Me.ToString(), "SeleccionarCampoTab", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Función que valida si el número de orden es o no numérico.
    Public Function [IsNumeric](ByVal str As String) As Boolean
        Dim result As Decimal = 0
        Return Decimal.TryParse(str, result)
    End Function

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaOperaciones

            objCB.strNroOrden = Nothing
            objCB.lngID = 0
            objCB.strTipo = Nothing
            objCB.strClaseOrden = Nothing
            objCB.dtmLiquidacion = Nothing
            objCB.dtmCumplimientoTitulo = Nothing
            objCB.lngIDComitente = Nothing
            objCB.lngParcial = Nothing

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
    Private Function ObtenerRegistroAnterior() As OyDOperaciones.Operaciones
        Dim objEncabezado As OyDOperaciones.Operaciones = New OyDOperaciones.Operaciones

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of OyDOperaciones.Operaciones)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDLiquidaciones = _EncabezadoSeleccionado.intIDLiquidaciones
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then

                If (_EncabezadoSeleccionado.lngID) = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe Ingresar un número de  liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logResultado = False
                    Return logResultado
                End If

                If IsNothing(_EncabezadoSeleccionado.lngParcial) Or _EncabezadoSeleccionado.lngParcial = -1 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe Ingresar un número de  parcial.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logResultado = False
                    Return logResultado
                End If

                If (_EncabezadoSeleccionado.dtmCumplimientoTitulo < _EncabezadoSeleccionado.dtmLiquidacion) Or (_EncabezadoSeleccionado.dtmCumplimiento < _EncabezadoSeleccionado.dtmLiquidacion) Then
                    A2Utilidades.Mensajes.mostrarMensaje("La fecha de cumplimiento debe ser mayor o igual que la de liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    HabilitarFechaEfectivo = True
                    logResultado = False
                    Return logResultado
                End If

                If _EncabezadoSeleccionado.curPrecio = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("El precio no debe ser cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logResultado = False
                    Return logResultado
                End If

                If _EncabezadoSeleccionado.dblCantidad = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("La cantidad no debe ser cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logResultado = False
                    Return logResultado
                End If

                If NumeroLiq > 0 And logNuevoRegistro Then
                    A2Utilidades.Mensajes.mostrarMensaje("La liquidación " + _EncabezadoSeleccionado.lngID.ToString + " con el parcial " + _EncabezadoSeleccionado.lngParcial.ToString + " y fecha " + CStr(_EncabezadoSeleccionado.dtmLiquidacion) + " ya existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logResultado = False
                    Return logResultado
                End If

                If _EncabezadoSeleccionado.lngIDOrden = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("La Orden no debe ser cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logResultado = False
                    Return logResultado
                End If

                If _EncabezadoSeleccionado.curComision > _EncabezadoSeleccionado.curTransaccion Then
                    A2Utilidades.Mensajes.mostrarMensaje("La Comisión no debe ser mayor que el valor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logResultado = False
                    Return logResultado
                End If

                If _EncabezadoSeleccionado.strPlaza <> "LOC" And _EncabezadoSeleccionado.strPlaza <> String.Empty Then
                    If IsNothing(_EncabezadoSeleccionado.lngIDComisionistaOtraPlaza) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el comisionista de la otra plaza.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logResultado = False
                        Return logResultado
                    End If

                    If IsNothing(_EncabezadoSeleccionado.lngIDCiudadOtraPlaza) Or _EncabezadoSeleccionado.lngIDCiudadOtraPlaza = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la ciudad de la otra plaza.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logResultado = False
                        Return logResultado
                    End If
                ElseIf _EncabezadoSeleccionado.strPlaza = "LOC" Then
                    If IsNothing(_EncabezadoSeleccionado.lngIDComisionistaLocal) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el comisionista local.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logResultado = False
                        Return logResultado
                    End If

                End If

                If _EncabezadoSeleccionado.strClaseOrden = "C" Then

                    'CFMA20170211
                    'If IsNothing(_EncabezadoSeleccionado.lngPlazo) Or _EncabezadoSeleccionado.lngPlazo = 0 Then
                    '    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el plazo del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    '    DatosTitulo = True
                    '    logResultado = False
                    '    Return logResultado
                    'End If

                    If _EncabezadoSeleccionado.logManual = True Then
                        If IsNothing(_EncabezadoSeleccionado.lngPlazo) Or _EncabezadoSeleccionado.lngPlazo = 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el plazo del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            DatosTitulo = True
                            logResultado = False
                            Return logResultado
                        End If
                    End If
                    'CFMA20170211

                    If IsNothing(_EncabezadoSeleccionado.strModalidad) Or _EncabezadoSeleccionado.strModalidad = String.Empty Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la modalidad del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        DatosTitulo = True
                        logResultado = False
                        Return logResultado
                    End If
                    If IsNothing(_EncabezadoSeleccionado.dtmVencimiento) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la fecha de vencimiento del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        HabilitarVencimiento = True
                        logResultado = False
                        Return logResultado
                    End If
                End If

            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar un registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logResultado = False
                Return logResultado
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Private Sub ConsultarFechaCierreSistema(Optional ByVal pstrUserState As String = "")
        Try
            If objProxyUtilidades Is Nothing Then
                objProxyUtilidades = inicializarProxyUtilidades()
            End If

            objProxyUtilidades.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf ConfirmarConsultarFechaCierreSistema, pstrUserState)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cierre del sistema.", Me.ToString(), "ConsultarFechaCierreSistema", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'consultarFechaCierreCompleted
    Private Sub ConfirmarConsultarFechaCierreSistema(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        Try
            If obj.HasError Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "ConfirmarConsultarFechaCierreSistema", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            Else
                dtmCierre = CDate(obj.Value)
                If obj.UserState Is "EDITAR" Then
                    ContinuarEdicionDocumento()
                ElseIf obj.UserState Is "GUARDAR" Then
                    ContinuarGuardadoDocumento()
                ElseIf obj.UserState Is "BORRAR" Then
                    ContinuarBorradoDocumento()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "ConfirmarConsultarFechaCierreSistema", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ContinuarEdicionDocumento()
        Try
            'CFMA20180115
            'If EncabezadoSeleccionado.dtmLiquidacion <= dtmCierre Then
            '    IsBusy = False
            '    A2Utilidades.Mensajes.mostrarMensaje("La operación no se puede editar porque la fecha de liquidación es menor o igual a la fecha de cierre (" & dtmCierre.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If


            EncabezadoSeleccionado.strAutoRetenedor = "N"
            EncabezadoSeleccionado.strCertificacion = "N"
            EncabezadoSeleccionado.strConstanciaEnajenacion = "N"
            EncabezadoSeleccionado.strRepoTitulo = "N"
            EncabezadoSeleccionado.strSujeto = "N"
            EncabezadoSeleccionado.logTraslado = True
            EncabezadoSeleccionado.strSwap = "N"
            EncabezadoSeleccionado.strReinversion = "N"
            EncabezadoSeleccionado.strUsuario = Program.Usuario
            EncabezadoSeleccionado.dtmActualizacion = Now

            'Propiedad utilizada para habilitar o deshabilitar los botones de aplazar y duplicar
            Editando = True
            HabilitaBoton = False
            HabilitarRadioButton = False

            ESTADO_EDICION = True

            LiquidacionesConsultarValidar() 'SaldoOrden()

            If EncabezadoSeleccionado.strClaseOrden.Equals("C") Then
                HabilitarVencimiento = True
                HabilitarManualDias = True
            End If

            If EncabezadoSeleccionado.logManual Then
                If EncabezadoSeleccionado.lngIDFactura Is Nothing Or EncabezadoSeleccionado.lngIDFactura = 0 Then
                    HabilitarCamposEspecificos = True

                    If Not IsNothing(EncabezadoSeleccionado.lngParcialPadre) And EncabezadoSeleccionado.lngParcialPadre <> 0 Then
                        HabilitarParcial_y_DiasTramo = True
                    End If

                    HabilitarFechaLiquidacion = True
                    If EncabezadoSeleccionado.strClaseOrden.Equals("C") Then
                        DatosTitulo = True
                        HabilitarEmision = True
                    End If

                    If EncabezadoSeleccionado.strPlaza = "LOC" Then
                        HabilitarComisionistaLocal = True
                        HabilitarComisionistaOtraPlaza = False
                    Else
                        HabilitarComisionistaLocal = False
                        HabilitarComisionistaOtraPlaza = True
                    End If
                End If
            End If



            If EncabezadoSeleccionado.strTipo.Equals("C") Or EncabezadoSeleccionado.strTipo.Equals("R") Then
                If EncabezadoSeleccionado.logManual = False Then
                    HabilitarFechaVendido = False
                Else
                    HabilitarFechaVendido = True
                End If
            Else
                If EncabezadoSeleccionado.logManual = False Then
                    HabilitarFechaVendido = False
                Else
                    HabilitarFechaVendido = True
                End If
            End If


            logNuevoRegistro = False
            logEditarRegistro = True
            lngIDLiquidacionEdicion = CInt(EncabezadoSeleccionado.lngID)
            ParcialEdicion = CInt(EncabezadoSeleccionado.lngParcial)
            FechaLiquidacionEdicion = CDate(EncabezadoSeleccionado.dtmLiquidacion)

            'Propiedades para ajustar despues de guardar la operación.
            If EncabezadoSeleccionado.logManual = False Then
                HabilitarManualDias = False
                HabilitarFechaTitulo = False
                HabilitarVencimiento = False
            Else
                HabilitarManualDias = True
                HabilitarFechaTitulo = True
                HabilitarVencimiento = True
            End If

            Userstate = String.Empty

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar un registro",
             Me.ToString(), "ContinuarEdicionDocumento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ContinuarGuardadoDocumento()
        Try
            'CFMA20180115
            'If EncabezadoSeleccionado.dtmLiquidacion <= dtmCierre Then
            '    IsBusy = False
            '    A2Utilidades.Mensajes.mostrarMensaje("La fecha de liquidación es menor o igual a la fecha de cierre (" & dtmCierre.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If


            If ValidarReceptoresOrdenes() Then
                If ValidarRegistro() Then
                    ValidaLiq("ACTUALIZAR")
                Else
                    IsBusy = False
                End If
            Else
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            'disparaguardar = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ContinuarGuardadoDocumento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub LiquidacionesConsultarValidar()
        Try
            ListaLiquidacionesValidar.Clear()
            mdcProxy.LiquidacionesConsultars.Clear()
            mdcProxy.Load(mdcProxy.LiquidacionesConsultarValidarQuery(EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CInt(EncabezadoSeleccionado.lngIDOrden), Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesValidar, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la liquidación", Me.ToString(), "LiquidacionesConsultarValidar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    ''' Método para consultar los parámetros 
    ''' </summary>
    Private Sub ConsultarParametros()
        Try
            objProxyUtilidades.Verificaparametro("APLAZAMIENTO_LIQUIDACIONES_AMBOS", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "APLAZAMIENTO_LIQUIDACIONES_AMBOS")
            objProxyUtilidades.Verificaparametro("VALOR_TOLERANCIA_ORDENES_PESOS", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "VALOR_TOLERANCIA_ORDENES_PESOS")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los parametros de operaciones", Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de asignar el resultado de los parámetros consultados de Operaciones
    ''' </summary>
    ''' <param name="lo">Valor del parámetro</param>
    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        Try
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "APLAZAMIENTO_LIQUIDACIONES_AMBOS"
                        If Not String.IsNullOrEmpty(lo.Value) Then
                            APLAZAMIENTO_LIQUIDACIONES_AMBOS = lo.Value
                        End If
                    Case "VALOR_TOLERANCIA_ORDENES_PESOS"
                        If Not String.IsNullOrEmpty(lo.Value) Then
                            mdblValorTolerancia = CDbl(lo.Value)
                        End If
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub OrdenesLiq()
        Try
            mdcProxy.LiquidacionesOrdenes.Clear()
            mdcProxy.Load(mdcProxy.OrdenesLiqQuery(EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CInt(EncabezadoSeleccionado.lngIDOrden), Program.Usuario, Program.HashConexion), AddressOf TerminotraerOrdenesliq, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la orden", Me.ToString(), "OrdenesLiq", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ActualizartHistorialRetardos(ByVal logReconstruirRetardo As Boolean)
        Try
            mdcProxy.RetardoOperacionesActualizars.Clear()
            mdcProxy.Load(mdcProxy.OperacionesRetardosActualizarQuery(EncabezadoSeleccionado.lngID, EncabezadoSeleccionado.lngParcial, EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, EncabezadoSeleccionado.dtmLiquidacion, logReconstruirRetardo, Program.Usuario, Program.HashConexion), AddressOf Terminoactualizarretardos, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el historial de retardos", Me.ToString(), "ActualizartHistorialRetardos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminotraerOrdenesliq(ByVal lo As LoadOperation(Of LiquidacionesOrdenes))
        If Not lo.HasError Then
            Try
                LiquidacionesOrdenes = mdcProxy.LiquidacionesOrdenes.First
                If IsNothing(LiquidacionesOrdenes.logOrdinaria) Then
                    EncabezadoSeleccionado.logOrdinaria = False
                Else
                    EncabezadoSeleccionado.logOrdinaria = LiquidacionesOrdenes.logOrdinaria
                End If

                If IsNothing(LiquidacionesOrdenes.strObjeto) Then
                    EncabezadoSeleccionado.strObjetoOrdenExtraordinaria = Nothing
                Else
                    EncabezadoSeleccionado.strObjetoOrdenExtraordinaria = LiquidacionesOrdenes.strObjeto
                End If

                If IsNothing(LiquidacionesOrdenes.strObjeto) Then
                    EncabezadoSeleccionado.lngNumPadre = Nothing
                Else
                    If LiquidacionesOrdenes.strObjeto.Equals("CRR") Then
                        EncabezadoSeleccionado.lngNumPadre = EncabezadoSeleccionado.lngID
                    Else
                        EncabezadoSeleccionado.lngNumPadre = Nothing
                    End If
                End If
                GuardarLiq()
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarMensaje(ex.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End Try

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenes",
                                             Me.ToString(), "TerminotraerOrdenesliq", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub Terminoactualizarretardos(ByVal lo As LoadOperation(Of RetardoOperacionesActualizar))
        If Not lo.HasError Then
            Try
                ContinuarBorradoDocumento()
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarMensaje(ex.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End Try

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización de retardos",
                                             Me.ToString(), "Terminoactualizarretardos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub GuardarLiq()
        Try
            IsBusy = True
            Dim origen = "update"
            ErrorForma = ""
            mobjEncabezadoAnterior = EncabezadoSeleccionado

            mdcProxy.CumplimientoOrden_liq(EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CInt(EncabezadoSeleccionado.lngIDOrden),
                     EncabezadoSeleccionado.strIDEspecie, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion, AddressOf TerminoVerificarCumplimientoOrderliq, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "GuardarLiq", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub TerminoVerificarCumplimientoOrderliq(ByVal lo As InvokeOperation(Of String))
        Try
            If Not lo.HasError Then
                Dim EST_INGRESO As Boolean = False

                If Userstate <> String.Empty Then
                    Exit Sub
                End If
                Dim pdblCantidadLiq = Split(lo.Value, ",")
                If Not ListaEncabezado.Contains(EncabezadoSeleccionado) Then

                    If Not IsNothing(pdblCantidadLiq(0)) Then
                        If mlogEnPesos Then
                            CANTIDADLIQ = CDbl((CType(CDbl(pdblCantidadLiq(0)), Double?) + EncabezadoSeleccionado.curTotalLiq))
                        Else
                            CANTIDADLIQ = CDbl((CType(CDbl(pdblCantidadLiq(0)), Double?) + EncabezadoSeleccionado.dblCantidad))
                        End If
                    Else
                        If mlogEnPesos Then
                            CANTIDADLIQ = CDbl(EncabezadoSeleccionado.curTotalLiq)
                        Else
                            CANTIDADLIQ = CDbl(EncabezadoSeleccionado.dblCantidad)
                        End If
                    End If
                    EST_INGRESO = True
                    ListaEncabezado.Add(EncabezadoSeleccionado)
                    mstrEstado = "insert"
                Else
                    If mlogCantidad Then
                        If Not IsNothing(pdblCantidadLiq(0)) Then
                            If mlogEnPesos Then
                                CANTIDADLIQ = CDbl((CDbl(pdblCantidadLiq(0)) - mdblCantidad + EncabezadoSeleccionado.curTotalLiq))
                            Else
                                CANTIDADLIQ = CDbl((CDbl(pdblCantidadLiq(0)) - mdblCantidad + EncabezadoSeleccionado.dblCantidad))
                            End If
                        Else
                            If mlogEnPesos Then
                                CANTIDADLIQ = CDbl(EncabezadoSeleccionado.curTotalLiq)
                            Else
                                CANTIDADLIQ = CDbl(EncabezadoSeleccionado.dblCantidad)
                            End If
                        End If
                    End If
                End If

                'Si se crea o se edita una liquidación. 
                If EST_INGRESO Or mlogCantidad Then
                    If (CANTIDADLIQ = CDbl(pdblCantidadLiq(1)) And Not mlogEnPesos) Or
                        (mlogEnPesos And ((CANTIDADLIQ = CDbl(pdblCantidadLiq(1))) Or (CDbl(pdblCantidadLiq(1)) - CANTIDADLIQ <= mdblValorTolerancia))) Then
                        mdcProxy.ActualizaOrdenEstadoCumplida(EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CInt(EncabezadoSeleccionado.lngIDOrden), 0, "C", EncabezadoSeleccionado.dtmLiquidacion, Program.Usuario, Program.HashConexion, AddressOf TerminoActualizarOdenCumplida, Nothing)
                    End If
                End If

                If mlogCantidad And ((CANTIDADLIQ < CDbl(pdblCantidadLiq(1))) Or (CANTIDADLIQ > CDbl(pdblCantidadLiq(1)))) Then
                    mdcProxy.ActualizaOrdenEstadoCumplida(EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CInt(EncabezadoSeleccionado.lngIDOrden), 0, "P", EncabezadoSeleccionado.dtmLiquidacion, Program.Usuario, Program.HashConexion, AddressOf TerminoActualizarOdenCumplida, Nothing)
                End If

                If Not DisparaFocus Then
                    If EncabezadoSeleccionado.intIDLiquidaciones > 0 Then
                        IdLiqAnterior = EncabezadoSeleccionado.intIDLiquidaciones
                    End If
                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "insert")
                    Userstate = "insert"
                End If
                DisparaGuardar = True
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del Saldo de la Orden",
                         Me.ToString(), "TerminoVerificarCumplimientoOrderliq", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar el cumplimiento de la orden", Me.ToString(), "TerminoVerificarCumplimientoOrderliq", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminaPreguntaSobregiro(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                OrdenesLiq()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar la pregunta sobregiro", Me.ToString(), "TerminaPreguntaSobregiro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminaPreguntaHistorialretardos(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            ActualizartHistorialRetardos(objResultado.DialogResult)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar la pregunta historial retardos", Me.ToString(), "TerminaPreguntaHistorialretardos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ContinuarBorradoDocumento()
        Try
            'CFMA20180115
            'If EncabezadoSeleccionado.dtmLiquidacion <= dtmCierre Then
            '    IsBusy = False
            '    A2Utilidades.Mensajes.mostrarMensaje("La operación no se puede borrar porque la fecha de liquidación es menor o igual a la fecha de cierre (" & dtmCierre.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If


            Selected = True
            If Not IsNothing(EncabezadoSeleccionado) Then
                'Propiedades para ajustar despues de guardar la operación.
                If mdcProxy.Operaciones.Where(Function(i) i.intIDLiquidaciones = EncabezadoSeleccionado.intIDLiquidaciones).Count > 0 Then
                    mdcProxy.ActualizaOrdenEstado(EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CInt(EncabezadoSeleccionado.lngIDOrden), Program.Usuario, Program.HashConexion, AddressOf TerminoActualizarOrdenEstado,
                                                 EncabezadoSeleccionado.strTipo + "," + EncabezadoSeleccionado.strClaseOrden + "," + EncabezadoSeleccionado.lngIDOrden.ToString)
                    mdcProxy.Operaciones.Remove(EncabezadoSeleccionado)
                    Selected = True
                    EncabezadoSeleccionado = ListaEncabezado.LastOrDefault
                    IsBusy = True
                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                Else
                    IsBusy = False
                End If
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "ContinuarBorradoDocumento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoActualizarOrdenEstado(ByVal lo As InvokeOperation(Of String))
        Try
            If Not lo.HasError Then
                If Not lo.Value = String.Empty Then
                    Dim parametrosliq = Split(CStr(lo.UserState), ",")
                    mdcProxy.ActualizaOrdenEstadoCumplida(parametrosliq(0), parametrosliq(1), CInt(parametrosliq(2)), 0, "P", Now, Program.Usuario, Program.HashConexion, AddressOf TerminoActualizarOdenCumplida, Nothing)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones",
                                                 Me.ToString(), "TerminoActualizarOrdenEstado", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de actualizar el estado de la orden", Me.ToString(), "TerminoActualizarOrdenEstado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoActualizarOdenCumplida(ByVal lo As InvokeOperation(Of Integer))
        Try
            If Not lo.HasError Then
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones",
                                                 Me.ToString(), "TerminoActualizarOdenCumplida", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de actualizar la orden cumplida", Me.ToString(), "TerminoActualizarOdenCumplida", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerLiquidacionesValidar(ByVal lo As LoadOperation(Of LiquidacionesConsultar))
        Try
            If Not lo.HasError Then
                ListaLiquidacionesValidar = mdcProxy.LiquidacionesConsultars.ToList

                If ListaLiquidacionesValidar.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No existe la orden con esas características", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    EncabezadoSeleccionado.lngIDOrden = Nothing
                    Exit Sub
                End If

                mlogEnPesos = ListaLiquidacionesValidar.First.logEnPesos
                If ESTADO_EDICION Then
                    SaldoOrden()
                    Exit Sub
                End If

                If ListaLiquidacionesValidar.First.strEstado.Equals("A") Then
                    A2Utilidades.Mensajes.mostrarMensaje("La orden está cancelada. No es posible ingresar la liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    EncabezadoSeleccionado.lngIDOrden = Nothing
                    IsBusy = False
                    Exit Sub

                ElseIf ListaLiquidacionesValidar.First.strEstado.Equals("C") Then
                    A2Utilidades.Mensajes.mostrarMensaje("La orden está cumplida. No es posible ingresar la liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If Not Duplica Then
                        EncabezadoSeleccionado.lngIDOrden = Nothing
                        IsBusy = False
                    Else
                        IsBusy = False
                        Duplica = False
                    End If
                    Exit Sub
                End If

                HabilitarRadioButton = False

                If Not Duplica Then
                    mlogRepo = ListaLiquidacionesValidar.First.logRepo
                    EncabezadoSeleccionado.lngID = ListaLiquidacionesValidar.First.lngID
                    EncabezadoSeleccionado.lngIDComitente = ListaLiquidacionesValidar.First.lngIDComitente
                    EncabezadoSeleccionado.strNombreComitente = ListaLiquidacionesValidar.First.strComitente
                    EncabezadoSeleccionado.lngIDOrdenante = ListaLiquidacionesValidar.First.lngIDOrdenante
                    EncabezadoSeleccionado.strNombreOrdenante = ListaLiquidacionesValidar.First.strOrdenante
                    EncabezadoSeleccionado.strIDEspecie = ListaLiquidacionesValidar.First.strIDEspecie
                    EncabezadoSeleccionado.strNombreEspecie = ListaLiquidacionesValidar.First.strEspecie
                    EncabezadoSeleccionado.strUBICACIONTITULO = ListaLiquidacionesValidar.First.strUBICACIONTITULO
                    EncabezadoSeleccionado.dblFactorComisionPactada = CType(Format(ListaLiquidacionesValidar.First.dblComisionPactada / 100, "#0.0#####"), Double?)
                    EncabezadoSeleccionado.dtmEmision = ListaLiquidacionesValidar.First.dtmEmision
                    EncabezadoSeleccionado.dtmVencimiento = ListaLiquidacionesValidar.First.dtmVencimiento
                    EncabezadoSeleccionado.strModalidad = ListaLiquidacionesValidar.First.strModalidad
                    EncabezadoSeleccionado.dblTasaDescuento = CType(IIf(Not IsNothing(ListaLiquidacionesValidar.First.dblTasaInicial), ListaLiquidacionesValidar.First.dblTasaInicial, 0.0), Double?)
                    EncabezadoSeleccionado.dblTasaEfectiva = CType(IIf(Not IsNothing(ListaLiquidacionesValidar.First.dblTasaEfectiva), ListaLiquidacionesValidar.First.dblTasaEfectiva, 0.0), Double?)
                    EncabezadoSeleccionado.dblPuntosIndicador = CType(IIf(Not IsNothing(ListaLiquidacionesValidar.First.dblPuntosIndicador), ListaLiquidacionesValidar.First.dblPuntosIndicador, 0.0), Double?)
                    EncabezadoSeleccionado.lngPlazo = ListaLiquidacionesValidar.First.intPlazo
                    EncabezadoSeleccionado.strIndicadorEconomico = ListaLiquidacionesValidar.First.strIndicador

                    If EncabezadoSeleccionado.dtmCumplimientoTitulo.HasValue And EncabezadoSeleccionado.dtmVencimiento.HasValue Then
                        Dim lngDiasVcto As Long = DateDiff(DateInterval.Day, EncabezadoSeleccionado.dtmCumplimientoTitulo.Value.Date, EncabezadoSeleccionado.dtmVencimiento.Value.Date)
                        EncabezadoSeleccionado.lngDiasVencimiento = Convert.ToInt32(lngDiasVcto)
                    Else
                        EncabezadoSeleccionado.lngDiasVencimiento = 0
                    End If


                    If ListaLiquidacionesValidar.First.strTipo.Equals("C") Or ListaLiquidacionesValidar.First.strTipo.Equals("R") Then
                        EncabezadoSeleccionado.curPrecio = CType(IIf(Not IsNothing(ListaLiquidacionesValidar.First.dblPrecio), ListaLiquidacionesValidar.First.dblPrecio, 0.0), Double?) 'ListaLiquidacionesValidar.First.dblPrecio
                    Else
                        Dim curPrecio As Double? = 0

                        If Not IsNothing(ListaLiquidacionesValidar.First.dblSuperior) Then
                            If CDbl(ListaLiquidacionesValidar.First.dblSuperior) <> Double.Parse("0") Then
                                curPrecio = CType(IIf(Not IsNothing(ListaLiquidacionesValidar.First.dblSuperior), ListaLiquidacionesValidar.First.dblSuperior, 0.0), Double?) 'CDbl(ListaLiquidacionesValidar.First.dblSuperior)
                            Else
                                curPrecio = CType(IIf(Not IsNothing(ListaLiquidacionesValidar.First.dblPrecio), ListaLiquidacionesValidar.First.dblPrecio, 0.0), Double?) 'CDbl(ListaLiquidacionesValidar.First.dblPrecio)
                            End If
                        Else
                            curPrecio = CType(IIf(Not IsNothing(ListaLiquidacionesValidar.First.dblPrecio), ListaLiquidacionesValidar.First.dblPrecio, 0.0), Double?)
                        End If

                        EncabezadoSeleccionado.curPrecio = CType(IIf(Not IsNothing(ListaLiquidacionesValidar.First.dblPrecio), ListaLiquidacionesValidar.First.dblPrecio, 0.0), Double?) 'CType(curPrecio, Double?)

                    End If
                    EncabezadoSeleccionado.strSwap = "N"
                    EncabezadoSeleccionado.strReinversion = "N"
                    EncabezadoSeleccionado.strAutoRetenedor = "N"
                    EncabezadoSeleccionado.strCertificacion = "N"
                    EncabezadoSeleccionado.strConstanciaEnajenacion = "N"
                    EncabezadoSeleccionado.strRepoTitulo = "N"
                    EncabezadoSeleccionado.strSujeto = "N"
                    EncabezadoSeleccionado.strIDBaseDias = "N"
                    EncabezadoSeleccionado.lngIDBolsa = BOLSACOLOMBIA
                End If
                SaldoOrden()

                'Propiedades para ajustar despues de guardar la operación.
                If EncabezadoSeleccionado.strClaseOrden.Equals("C") Then
                    DatosTitulo = True
                    HabilitarEmision = True
                    HabilitarManualDias = True
                    HabilitarVencimiento = True
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Liquidaciones",
                                                 Me.ToString(), "TerminoTraerLiquidacionesValidar", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de validar la operación", Me.ToString(), "TerminoTraerLiquidacionesValidar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub SaldoOrden()
        Try
            mdcProxy.ConsultarCantidads.Clear()
            mdcProxy.Load(mdcProxy.LiquidacionesConsultarCantidadQuery(EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CInt(EncabezadoSeleccionado.lngIDOrden), EncabezadoSeleccionado.strIDEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminarTraerCantidad, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el saldo de la orden", Me.ToString(), "SaldoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminarTraerCantidad(ByVal lo As LoadOperation(Of ConsultarCantidad))
        Try
            If Not lo.HasError Then

                ConsultarCantidades = mdcProxy.ConsultarCantidads.First
                If Not Duplica Then
                    'Cuando es edicion es esta misma formula restandole liquidacionesselected.cantidad
                    If ESTADO_EDICION Then
                        If mlogEnPesos = True Then
                            mdblCantidad = CDbl(EncabezadoSeleccionado.curTotalLiq)
                        Else
                            mdblCantidad = CDbl(EncabezadoSeleccionado.dblCantidad)
                        End If
                        mcursaldoOrden = CDbl(ConsultarCantidades.dblCantidadOrden - (ConsultarCantidades.dblCantidadLiq - EncabezadoSeleccionado.dblCantidad))
                        ESTADO_EDICION = False
                        Exit Sub
                    Else
                        If mlogEnPesos Then
                            If EncabezadoSeleccionado.curPrecio = 0 Or IsNothing(EncabezadoSeleccionado.curPrecio) Then
                                EncabezadoSeleccionado.dblCantidad = ConsultarCantidades.dblCantidadOrden - ConsultarCantidades.dblCantidadLiq
                            Else
                                EncabezadoSeleccionado.dblCantidad = (ConsultarCantidades.dblCantidadOrden - ConsultarCantidades.dblCantidadLiq) / EncabezadoSeleccionado.curPrecio
                            End If
                            mcursaldoOrden = CDbl(EncabezadoSeleccionado.dblCantidad)
                        Else
                            EncabezadoSeleccionado.dblCantidad = ConsultarCantidades.dblCantidadOrden - ConsultarCantidades.dblCantidadLiq
                            mcursaldoOrden = CDbl(EncabezadoSeleccionado.dblCantidad)
                        End If
                    End If

                    If EncabezadoSeleccionado.curPrecio = 0 Then
                        EncabezadoSeleccionado.curTransaccion = ListaLiquidacionesValidar.First.dblCantidad
                        EncabezadoSeleccionado.curSubTotalLiq = EncabezadoSeleccionado.curTransaccion
                        EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTransaccion
                    Else
                        If EncabezadoSeleccionado.strClaseOrden.Equals("A") Then
                            EncabezadoSeleccionado.curTransaccion = EncabezadoSeleccionado.curPrecio * EncabezadoSeleccionado.dblCantidad
                        Else
                            EncabezadoSeleccionado.curTransaccion = EncabezadoSeleccionado.curPrecio * EncabezadoSeleccionado.dblCantidad / 100
                        End If
                        EncabezadoSeleccionado.curSubTotalLiq = EncabezadoSeleccionado.curTransaccion
                        EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTransaccion
                    End If

                    If Not IsNothing(ListaLiquidacionesValidar.First.dblComisionPactada) Then
                        If Not IsNothing(EncabezadoSeleccionado.curTransaccion) Then
                            EncabezadoSeleccionado.curComision = EncabezadoSeleccionado.curTransaccion * EncabezadoSeleccionado.dblFactorComisionPactada
                            If ListaLiquidacionesValidar.First.strTipo.Equals("C") Or ListaLiquidacionesValidar.First.strTipo.Equals("R") Then
                                EncabezadoSeleccionado.curSubTotalLiq = EncabezadoSeleccionado.curSubTotalLiq + EncabezadoSeleccionado.curComision
                                EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq + EncabezadoSeleccionado.curComision
                            Else
                                EncabezadoSeleccionado.curSubTotalLiq = EncabezadoSeleccionado.curSubTotalLiq - EncabezadoSeleccionado.curComision
                                EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq - EncabezadoSeleccionado.curComision
                            End If
                        End If
                    Else
                        EncabezadoSeleccionado.curComision = 0
                    End If
                    If Not IsNothing(ListaLiquidacionesValidar.First.strObjeto) Then
                        If Not ListaLiquidacionesValidar.First.logOrdinaria And ListaLiquidacionesValidar.First.strObjeto.Equals("CRR") Then
                            'HABILITAR ESTAS DOS  PROPIEDADES
                            HabilitarParcial_y_DiasTramo = True
                        End If
                    End If
                End If

                NuevoReceptorLiq()
                HabilitarCamposEspecificos = True

                If _mlogDuplicando Then
                    If EncabezadoSeleccionado.strPlaza = "LOC" Then
                        HabilitarComisionistaLocal = True
                        HabilitarComisionistaOtraPlaza = False
                    Else
                        HabilitarComisionistaLocal = False
                        HabilitarComisionistaOtraPlaza = True
                    End If
                    _mlogDuplicando = False
                Else
                    HabilitarComisionistaLocal = False
                    HabilitarComisionistaOtraPlaza = False
                End If
                HabilitarFechaLiquidacion = True
                HabilitarFechaVendido = False
                HabilitarFechaTitulo = True
                HabilitarFechaEfectivo = True

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Liquidacione ",
                                                 Me.ToString(), "TerminartraerCantidad", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer la cantidad", Me.ToString(), "TerminarTraerCantidad", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub NuevoReceptorLiq()
        Try
            If mlogRecalculaValoresAlDuplicar Then
                mlogRecalculaValoresAlDuplicar = False
                Exit Sub
            End If

            mdcProxy.OperacionesReceptores.Clear()
            If Not IsNothing(ListaOperacionesReceptores) Then
                ListaOperacionesReceptores.Clear()
            End If
            mdcProxy.Load(mdcProxy.ReceptoresOrdenesliqQuery(EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CInt(EncabezadoSeleccionado.lngIDOrden), Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresLiq, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear el receptor", Me.ToString(), "NuevoReceptorLiq", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerReceptoresLiq(ByVal lo As LoadOperation(Of OperacionesReceptores))
        Try
            If Not lo.HasError Then
                ListaOperacionesReceptores = mdcProxy.OperacionesReceptores
                BeneficiariosOrdenes()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Liquidacione ",
                                                 Me.ToString(), "Terminotraerreceptoresliq", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer los receptores", Me.ToString(), "TerminoTraerReceptoresLiq", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BeneficiariosOrdenes()
        Try
            mdcProxy.OperacionesBeneficiarios.Clear()
            mdcProxy.Load(mdcProxy.BeneficiariosOrdenesliqQuery(EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CInt(EncabezadoSeleccionado.lngIDOrden), Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosLiq, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los beneficiarios", Me.ToString(), "BeneficiariosOrdenes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerBeneficiariosLiq(ByVal lo As LoadOperation(Of OperacionesBeneficiarios))
        Try
            If Not lo.HasError Then
                ListaOperacionesBeneficiarios = mdcProxy.OperacionesBeneficiarios
                EspecieLiq()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BeneficiariosOrdenes",
                                                 Me.ToString(), "TerminoTraerBeneficiariosliq", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer los beneficiarios", Me.ToString(), "TerminoTraerBeneficiariosLiq", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub EspecieLiq()
        Try
            mdcProxy.OperacionesEspecies.Clear()
            mdcProxy.Load(mdcProxy.EspeciesOrdenesLiqQuery(EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.strClaseOrden, CInt(EncabezadoSeleccionado.lngIDOrden), Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesliq, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las especies", Me.ToString(), "EspecieLiq", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerEspeciesliq(ByVal lo As LoadOperation(Of OperacionesEspecies))
        Try
            If Not lo.HasError Then
                ListaOperacionesEspecies = mdcProxy.OperacionesEspecies
                If Duplica Then
                    NuevoDuplicado()
                    Duplica = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesLiquidaciones",
                                                 Me.ToString(), "TerminoTraerEspeciesliq", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las especies", Me.ToString(), "TerminoTraerEspeciesliq", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub NuevoDuplicado()
        Try
            Dim NewLiquidacione As New OyDOperaciones.Operaciones

            NewLiquidacione.lngIDComisionista = EncabezadoSeleccionado.lngIDComisionista
            NewLiquidacione.lngIDSucComisionista = EncabezadoSeleccionado.lngIDSucComisionista
            NewLiquidacione.lngID = EncabezadoSeleccionado.lngID
            'NewLiquidacione.lngParcial = -1
            NewLiquidacione.lngParcial = 0
            NewLiquidacione.strTipo = EncabezadoSeleccionado.strTipo
            NewLiquidacione.strClaseOrden = EncabezadoSeleccionado.strClaseOrden
            NewLiquidacione.strIDEspecie = EncabezadoSeleccionado.strIDEspecie
            NewLiquidacione.lngIDOrden = EncabezadoSeleccionado.lngIDOrden

            NewLiquidacione.strPrefijo = Nothing
            NewLiquidacione.lngIDFactura = Nothing
            NewLiquidacione.strFacturada = Nothing

            NewLiquidacione.intFechaOrden = Now.Year

            NewLiquidacione.lngIDComitente = EncabezadoSeleccionado.lngIDComitente
            NewLiquidacione.lngIDOrdenante = EncabezadoSeleccionado.lngIDOrdenante
            NewLiquidacione.lngIDBolsa = EncabezadoSeleccionado.lngIDBolsa
            NewLiquidacione.dblValBolsa = EncabezadoSeleccionado.dblValBolsa
            NewLiquidacione.bytIDRueda = EncabezadoSeleccionado.bytIDRueda
            NewLiquidacione.dblTasaDescuento = EncabezadoSeleccionado.dblTasaDescuento
            NewLiquidacione.dblTasaCompraVende = EncabezadoSeleccionado.dblTasaCompraVende
            NewLiquidacione.strModalidad = EncabezadoSeleccionado.strModalidad
            NewLiquidacione.strIndicadorEconomico = EncabezadoSeleccionado.strIndicadorEconomico
            NewLiquidacione.dblPuntosIndicador = EncabezadoSeleccionado.dblPuntosIndicador
            NewLiquidacione.lngPlazo = EncabezadoSeleccionado.lngPlazo
            NewLiquidacione.dtmLiquidacion = EncabezadoSeleccionado.dtmLiquidacion
            NewLiquidacione.dtmCumplimiento = EncabezadoSeleccionado.dtmCumplimiento
            NewLiquidacione.dtmEmision = EncabezadoSeleccionado.dtmEmision
            NewLiquidacione.dtmVencimiento = EncabezadoSeleccionado.dtmVencimiento
            NewLiquidacione.logOtraPlaza = EncabezadoSeleccionado.logOtraPlaza
            NewLiquidacione.strPlaza = EncabezadoSeleccionado.strPlaza
            NewLiquidacione.lngIDComisionistaLocal = EncabezadoSeleccionado.lngIDComisionistaLocal
            NewLiquidacione.lngIDComisionistaOtraPlaza = EncabezadoSeleccionado.lngIDComisionistaOtraPlaza
            NewLiquidacione.lngIDCiudadOtraPlaza = EncabezadoSeleccionado.lngIDCiudadOtraPlaza
            NewLiquidacione.dblTasaEfectiva = EncabezadoSeleccionado.dblTasaEfectiva

            If mlogEnPesos Then
                If _EncabezadoSeleccionado.curPrecio = 0 Or IsNothing(_EncabezadoSeleccionado.curPrecio) Then
                    NewLiquidacione.dblCantidad = ConsultarCantidades.dblCantidadOrden - ConsultarCantidades.dblCantidadLiq
                Else
                    NewLiquidacione.dblCantidad = (ConsultarCantidades.dblCantidadOrden - ConsultarCantidades.dblCantidadLiq) / _EncabezadoSeleccionado.curPrecio
                End If
            Else
                NewLiquidacione.dblCantidad = ConsultarCantidades.dblCantidadOrden - ConsultarCantidades.dblCantidadLiq
            End If
            mcursaldoOrden = CDbl(NewLiquidacione.dblCantidad)
            NewLiquidacione.curPrecio = 0
            NewLiquidacione.dblFactorComisionPactada = 0

            If NewLiquidacione.curPrecio = 0 Then
                NewLiquidacione.curTransaccion = ListaLiquidacionesValidar.First.dblCantidad
                NewLiquidacione.curSubTotalLiq = NewLiquidacione.curTransaccion
                NewLiquidacione.curTotalLiq = NewLiquidacione.curTransaccion
            Else
                If NewLiquidacione.strClaseOrden.Equals("A") Then
                    NewLiquidacione.curTransaccion = NewLiquidacione.curPrecio * NewLiquidacione.dblCantidad
                Else
                    NewLiquidacione.curTransaccion = NewLiquidacione.curPrecio * NewLiquidacione.dblCantidad / 100
                End If
                NewLiquidacione.curSubTotalLiq = NewLiquidacione.curTransaccion
                NewLiquidacione.curTotalLiq = NewLiquidacione.curTransaccion

            End If

            If Not IsNothing(ListaLiquidacionesValidar.First.dblComisionPactada) Then
                If Not IsNothing(NewLiquidacione.curTransaccion) Then
                    NewLiquidacione.curComision = NewLiquidacione.curTransaccion * NewLiquidacione.dblFactorComisionPactada
                    If ListaLiquidacionesValidar.First.strTipo.Equals("C") Or ListaLiquidacionesValidar.First.strTipo.Equals("R") Then
                        NewLiquidacione.curSubTotalLiq = NewLiquidacione.curSubTotalLiq + NewLiquidacione.curComision
                        NewLiquidacione.curTotalLiq = NewLiquidacione.curTotalLiq + NewLiquidacione.curComision
                    Else
                        NewLiquidacione.curSubTotalLiq = NewLiquidacione.curSubTotalLiq - NewLiquidacione.curComision
                        NewLiquidacione.curTotalLiq = NewLiquidacione.curTotalLiq - NewLiquidacione.curComision
                    End If
                End If
            Else
                NewLiquidacione.curComision = 0
            End If
            If Not IsNothing(ListaLiquidacionesValidar.First.strObjeto) Then
                If Not ListaLiquidacionesValidar.First.logOrdinaria And ListaLiquidacionesValidar.First.strObjeto.Equals("CRR") Then
                    'HABILITAR ESTAS DOS  PROPIEDADES
                    HabilitarParcial_y_DiasTramo = True
                End If
            End If
            NewLiquidacione.strTransaccion = EncabezadoSeleccionado.strTransaccion
            NewLiquidacione.curRetencion = EncabezadoSeleccionado.curRetencion
            NewLiquidacione.curIntereses = EncabezadoSeleccionado.curIntereses
            NewLiquidacione.curValorIva = EncabezadoSeleccionado.curValorIva
            NewLiquidacione.lngDiasIntereses = EncabezadoSeleccionado.lngDiasIntereses
            NewLiquidacione.strMercado = EncabezadoSeleccionado.strMercado
            NewLiquidacione.strNroTitulo = EncabezadoSeleccionado.strNroTitulo
            NewLiquidacione.lngIDCiudadExpTitulo = EncabezadoSeleccionado.lngIDCiudadExpTitulo
            NewLiquidacione.lngPlazoOriginal = EncabezadoSeleccionado.lngPlazoOriginal
            NewLiquidacione.logAplazamiento = EncabezadoSeleccionado.logAplazamiento
            NewLiquidacione.bytVersionPapeleta = EncabezadoSeleccionado.bytVersionPapeleta
            NewLiquidacione.dtmEmisionOriginal = EncabezadoSeleccionado.dtmEmisionOriginal
            NewLiquidacione.dtmVencimientoOriginal = EncabezadoSeleccionado.dtmVencimientoOriginal
            NewLiquidacione.lngImpresiones = EncabezadoSeleccionado.lngImpresiones
            NewLiquidacione.strFormaPago = EncabezadoSeleccionado.strFormaPago
            NewLiquidacione.lngCtrlImpPapeleta = EncabezadoSeleccionado.lngCtrlImpPapeleta
            NewLiquidacione.lngDiasVencimiento = EncabezadoSeleccionado.lngDiasVencimiento
            NewLiquidacione.strPosicionPropia = EncabezadoSeleccionado.strPosicionPropia
            NewLiquidacione.strTransaccion = EncabezadoSeleccionado.strTransaccion
            NewLiquidacione.strTipoOperacion = EncabezadoSeleccionado.strTipoOperacion
            NewLiquidacione.lngDiasContado = EncabezadoSeleccionado.lngDiasContado
            NewLiquidacione.logOrdinaria = EncabezadoSeleccionado.logOrdinaria
            NewLiquidacione.strObjetoOrdenExtraordinaria = EncabezadoSeleccionado.strObjetoOrdenExtraordinaria
            NewLiquidacione.lngNumPadre = EncabezadoSeleccionado.lngNumPadre
            NewLiquidacione.lngParcialPadre = EncabezadoSeleccionado.lngParcialPadre
            NewLiquidacione.dtmOperacionPadre = EncabezadoSeleccionado.dtmOperacionPadre
            NewLiquidacione.lngDiasTramo = EncabezadoSeleccionado.lngDiasTramo
            NewLiquidacione.logVendido = EncabezadoSeleccionado.logVendido
            NewLiquidacione.dtmVendido = EncabezadoSeleccionado.dtmVendido
            NewLiquidacione.logManual = EncabezadoSeleccionado.logManual
            NewLiquidacione.dblValorTraslado = EncabezadoSeleccionado.dblValorTraslado
            NewLiquidacione.dblValorBrutoCompraVencida = EncabezadoSeleccionado.dblValorBrutoCompraVencida
            NewLiquidacione.strAutoRetenedor = EncabezadoSeleccionado.strAutoRetenedor
            NewLiquidacione.strSujeto = EncabezadoSeleccionado.strSujeto
            NewLiquidacione.dblPcRenEfecCompraRet = EncabezadoSeleccionado.dblPcRenEfecCompraRet
            NewLiquidacione.dblPcRenEfecVendeRet = EncabezadoSeleccionado.dblPcRenEfecVendeRet
            NewLiquidacione.strReinversion = EncabezadoSeleccionado.strReinversion
            NewLiquidacione.strSwap = EncabezadoSeleccionado.strSwap
            NewLiquidacione.lngNroSwap = EncabezadoSeleccionado.lngNroSwap
            NewLiquidacione.strCertificacion = EncabezadoSeleccionado.strCertificacion
            NewLiquidacione.dblDescuentoAcumula = EncabezadoSeleccionado.dblDescuentoAcumula
            NewLiquidacione.dblPctRendimiento = EncabezadoSeleccionado.dblPctRendimiento
            NewLiquidacione.dtmFechaCompraVencido = EncabezadoSeleccionado.dtmFechaCompraVencido
            NewLiquidacione.dblPrecioCompraVencido = EncabezadoSeleccionado.dblPrecioCompraVencido
            NewLiquidacione.strConstanciaEnajenacion = EncabezadoSeleccionado.strConstanciaEnajenacion
            NewLiquidacione.strRepoTitulo = EncabezadoSeleccionado.strRepoTitulo
            NewLiquidacione.dblServBolsaVble = EncabezadoSeleccionado.dblServBolsaVble
            NewLiquidacione.dblServBolsaFijo = EncabezadoSeleccionado.dblServBolsaFijo
            NewLiquidacione.logTraslado = EncabezadoSeleccionado.logTraslado
            NewLiquidacione.strUBICACIONTITULO = EncabezadoSeleccionado.strUBICACIONTITULO
            NewLiquidacione.strHoraGrabacion = EncabezadoSeleccionado.strHoraGrabacion
            NewLiquidacione.strOrigenOperacion = EncabezadoSeleccionado.strOrigenOperacion
            NewLiquidacione.lngCodigoOperadorCompra = EncabezadoSeleccionado.lngCodigoOperadorCompra
            NewLiquidacione.lngCodigoOperadorVende = EncabezadoSeleccionado.lngCodigoOperadorVende
            NewLiquidacione.strIdentificacionRemate = EncabezadoSeleccionado.strIdentificacionRemate
            NewLiquidacione.strModalidaOperacion = EncabezadoSeleccionado.strModalidaOperacion
            NewLiquidacione.strIndicadorPrecio = EncabezadoSeleccionado.strIndicadorPrecio
            NewLiquidacione.strPeriodoExdividendo = EncabezadoSeleccionado.strPeriodoExdividendo
            NewLiquidacione.lngPlazoOperacionRepo = EncabezadoSeleccionado.lngPlazoOperacionRepo
            NewLiquidacione.dblValorCaptacionRepo = EncabezadoSeleccionado.dblValorCaptacionRepo
            NewLiquidacione.lngVolumenCompraRepo = EncabezadoSeleccionado.lngVolumenCompraRepo
            NewLiquidacione.dblPrecioNetoFraccion = EncabezadoSeleccionado.dblPrecioNetoFraccion
            NewLiquidacione.lngVolumenNetoFraccion = EncabezadoSeleccionado.lngVolumenNetoFraccion
            NewLiquidacione.lngCodigoContactoComercial = EncabezadoSeleccionado.lngCodigoContactoComercial
            NewLiquidacione.lngNroFraccionOperacion = EncabezadoSeleccionado.lngNroFraccionOperacion
            NewLiquidacione.strIdentificacionPatrimonio1 = EncabezadoSeleccionado.strIdentificacionPatrimonio1
            NewLiquidacione.strTipoidentificacionCliente2 = EncabezadoSeleccionado.strTipoidentificacionCliente2
            NewLiquidacione.strNitCliente2 = EncabezadoSeleccionado.strNitCliente2
            NewLiquidacione.strIdentificacionPatrimonio2 = EncabezadoSeleccionado.strIdentificacionPatrimonio2
            NewLiquidacione.strTipoIdentificacionCliente3 = EncabezadoSeleccionado.strTipoIdentificacionCliente3
            NewLiquidacione.strNitCliente3 = EncabezadoSeleccionado.strNitCliente3
            NewLiquidacione.strIdentificacionPatrimonio3 = EncabezadoSeleccionado.strIdentificacionPatrimonio3
            NewLiquidacione.strIndicadorOperacion = EncabezadoSeleccionado.strIndicadorOperacion
            NewLiquidacione.dblBaseRetencion = EncabezadoSeleccionado.dblBaseRetencion
            NewLiquidacione.dblPorcRetencion = EncabezadoSeleccionado.dblPorcRetencion
            NewLiquidacione.dblBaseRetencionTranslado = EncabezadoSeleccionado.dblBaseRetencionTranslado
            NewLiquidacione.dblPorcRetencionTranslado = EncabezadoSeleccionado.dblPorcRetencionTranslado
            NewLiquidacione.dblPorcIvaComision = EncabezadoSeleccionado.dblPorcIvaComision
            NewLiquidacione.strIndicadorAcciones = EncabezadoSeleccionado.strIndicadorAcciones
            NewLiquidacione.strOperacionNegociada = EncabezadoSeleccionado.strOperacionNegociada
            NewLiquidacione.dtmFechaConstancia = EncabezadoSeleccionado.dtmFechaConstancia
            NewLiquidacione.dblValorConstancia = EncabezadoSeleccionado.dblValorConstancia
            NewLiquidacione.strGeneraConstancia = EncabezadoSeleccionado.strGeneraConstancia
            NewLiquidacione.logCargado = EncabezadoSeleccionado.logCargado
            NewLiquidacione.dtmActualizacion = Now
            NewLiquidacione.strUsuario = Program.Usuario
            NewLiquidacione.dtmCumplimientoTitulo = EncabezadoSeleccionado.dtmCumplimientoTitulo

            NewLiquidacione.intNroLote = 0

            NewLiquidacione.dblValorEntregaContraPago = EncabezadoSeleccionado.dblValorEntregaContraPago
            NewLiquidacione.strAquienSeEnviaRetencion = EncabezadoSeleccionado.strAquienSeEnviaRetencion
            NewLiquidacione.strIDBaseDias = EncabezadoSeleccionado.strIDBaseDias
            NewLiquidacione.strTipoDeOferta = EncabezadoSeleccionado.strTipoDeOferta
            NewLiquidacione.intNroLoteENC = EncabezadoSeleccionado.intNroLoteENC
            NewLiquidacione.dtmContabilidadENC = EncabezadoSeleccionado.dtmContabilidadENC
            NewLiquidacione.intIDLiquidaciones = -1
            NewLiquidacione.strCodigoIntermediario = EncabezadoSeleccionado.strCodigoIntermediario

            mobjEncabezadoAnterior = EncabezadoSeleccionado
            HabilitaBoton = False
            HabilitarRadioButton = False
            EncabezadoSeleccionado = NewLiquidacione
            HabilitarManualDias = True

            MyBase.CambioItem("EncabezadoSeleccionado")
            Editando = True
            MyBase.CambioItem("Editando")
            IsBusy = False
            mlogRecalculaValoresAlDuplicar = True
            ValidaOrden()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    'CFMA20180115
    'Private Function ValidaFechaCierre(ByVal opcion As String) As Boolean
    '    ValidaFechaCierre = True
    '    If Format(EncabezadoSeleccionado.dtmLiquidacion, "yyyy/MM/dd") <= Format(dtmCierre, "yyyy/MM/dd") Then
    '        If Format(dtmCierre, "yyyy/MM/dd") <> "1900/01/01" Then
    '            A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & EncabezadoSeleccionado.dtmLiquidacion & "). no puede ser " & opcion & " porque su fecha no es igual a la fecha abierta para el usuario ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '            ValidaFechaCierre = False
    '        Else
    '            A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & EncabezadoSeleccionado.dtmLiquidacion & "). no puede ser " & opcion & " porque su fecha es inferior a la fecha de cierre", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '            ValidaFechaCierre = False
    '        End If
    '    End If
    '    Return ValidaFechaCierre
    'End Function

    Private Async Function validarFechacierre(ByVal opcion As String, ByVal pdtmFechaValidacion As Nullable(Of DateTime)) As Task(Of Boolean)
        Try
            Dim logRetorno As Boolean = Await Program.ValidarFechaCierre("O", opcion, "Operaciones", pdtmFechaValidacion, Program.Usuario, True)
            Return logRetorno
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la validación de la fecha de cierre.",
                                                         Me.ToString(), "validarFechacierre", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function
    'CFMA20180115

    Private Sub CerroVentana(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If aplazamientoserie.DialogResult = True Then
                Select Case aplazamientoserie.TipoSelected.Descripcion
                    Case strTitulo
                        strAplazamiento = "T"
                    Case strEfectivo
                        strAplazamiento = "A"
                    Case STR_AMBOS
                        strAplazamiento = "B"
                End Select

                If strTipoAplazamiento.Equals("S") Then
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Se aplazarán todos los parciales de compra o venta de la operación: " & EncabezadoSeleccionado.lngID _
                                                                 & vbCr & " con la siguiente Fecha: " & aplazamientoserie.TipoSelected.FechaAplazamiento & " de " & aplazamientoserie.TipoSelected.Descripcion,
                                                                 Program.TituloSistema, "CERROVENTANA", AddressOf TerminaPregunta, True, "¿Desea continuar?")
                Else
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Se aplazará la operación: " & EncabezadoSeleccionado.lngID _
                                                                 & vbCr & " con la siguiente Fecha: " & aplazamientoserie.TipoSelected.FechaAplazamiento & " de " & aplazamientoserie.TipoSelected.Descripcion,
                                                                 Program.TituloSistema, "CERROVENTANA", AddressOf TerminaPregunta, True, "¿Desea continuar?")
                End If
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la ventana emergente", Me.ToString(), "CerroVentana", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                If objProxy Is Nothing Then
                    objProxy = inicializarProxyOperaciones()
                End If

                IsBusy = True
                If Not IsNothing(EncabezadoSeleccionado) Then
                    objProxy.Aplazamiento(strTipoAplazamiento, strAplazamiento, EncabezadoSeleccionado.strClaseOrden, EncabezadoSeleccionado.strTipo, CInt(EncabezadoSeleccionado.lngID),
                    CInt(EncabezadoSeleccionado.lngParcial), CDate(EncabezadoSeleccionado.dtmLiquidacion), aplazamientoserie.TipoSelected.FechaAplazamiento, Program.Usuario, strError, strNroAplazamiento, Program.HashConexion, AddressOf TerminoActualizarAplazamiento, "")
                End If
            Else
                IsBusy = False
                Exit Sub

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar pregunta", Me.ToString(), "TerminaPregunta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Function TerminoActualizarAplazamiento(ByVal obj As InvokeOperation(Of String)) As Task
        IsBusy = False

        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "TerminoactualizarAplazamiento", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else

            If Not IsNothing(obj.Value) Then
                If (IsNumeric(obj.Value)) Then
                    If CInt(obj.Value) = -5 Then
                        A2Utilidades.Mensajes.mostrarMensaje("La operación tiene titulos descargados" & vbNewLine & " No puede ser aplazada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Se aplazaron " & obj.Value & " Operaciones", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IdLiqAnterior = EncabezadoSeleccionado.intIDLiquidaciones
                        mdcProxy.Operaciones.Clear()
                        If strUserState = "Busqueda" Then
                            ConfirmarBuscar()
                        Else
                            Await ConsultarEncabezado(True, String.Empty) ' Recarga la lista para que carguen los include
                        End If
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(obj.Value, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            Else

            End If
        End If
    End Function

    Private Sub TerminoTraerAplazamientosLiquidacionesvalor(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        Try
            If Not lo.HasError Then
                EncabezadoSeleccionado.curTransaccion = EncabezadoSeleccionado.curPrecio * EncabezadoSeleccionado.dblCantidad * lo.Value
                EncabezadoSeleccionado.curSubTotalLiq = EncabezadoSeleccionado.curTransaccion
                EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTransaccion
                EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq + CType(CDbl(IIf((EncabezadoSeleccionado.strTipo = "C" Or EncabezadoSeleccionado.strTipo = "R") And EncabezadoSeleccionado.strPosicionExtemporaneo = "S", EncabezadoSeleccionado.curValorExtemporaneo, -EncabezadoSeleccionado.curValorExtemporaneo)), Double?)
                EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq + CType(CDbl(IIf((EncabezadoSeleccionado.strTipo = "C" Or EncabezadoSeleccionado.strTipo = "R") And EncabezadoSeleccionado.strPosicionExtemporaneo = "A", -EncabezadoSeleccionado.curValorExtemporaneo, -EncabezadoSeleccionado.curValorExtemporaneo)), Double?)

                Denominacionespecie()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones",
                                                 Me.ToString(), "TerminoTraerAplazamientosLiquidacionesvalor", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer aplazamientos", Me.ToString(), "TerminoTraerAplazamientosLiquidacionesvalor", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Denominacionespecie()
        Try
            mdcProxy.VerificaNombreTarifa(EncabezadoSeleccionado.strIDEspecie, "", Program.Usuario, Program.HashConexion, AddressOf TerminoTraerNombre, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer la denominación de la especie", Me.ToString(), "Denominacionespecie", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerNombre(ByVal lo As InvokeOperation(Of String))
        Try
            If Not lo.HasError Then
                If lo.Value.Equals("PESOS") Then
                    EncabezadoSeleccionado.curComision = CType(IIf(IsNothing(EncabezadoSeleccionado.dblCantidad), 0, EncabezadoSeleccionado.dblCantidad), Double) * CType(IIf(IsNothing(EncabezadoSeleccionado.dblFactorComisionPactada), 0, EncabezadoSeleccionado.dblFactorComisionPactada), Double)
                Else
                    EncabezadoSeleccionado.curComision = CType(IIf(IsNothing(EncabezadoSeleccionado.curTransaccion), 0, EncabezadoSeleccionado.curTransaccion), Double) * CType(IIf(IsNothing(EncabezadoSeleccionado.dblFactorComisionPactada), 0, EncabezadoSeleccionado.dblFactorComisionPactada), Double)
                End If
                mdcProxy.VerificadblIvaComision(Nothing, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerIva, Nothing)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones",
                                                 Me.ToString(), "TerminoTraernombre", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer el nombre", Me.ToString(), "TerminoTraerNombre", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerIva(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        Try
            If Not lo.HasError Then

                If Userstate <> String.Empty Then
                    Exit Sub
                End If

                If EncabezadoSeleccionado.curComision > 0 And lo.Value > 0 Then
                    EncabezadoSeleccionado.curValorIva = ((EncabezadoSeleccionado.curComision) * (lo.Value / 100) - 0.005)
                Else
                    EncabezadoSeleccionado.curValorIva = 0
                End If

                If EncabezadoSeleccionado.strTipo.Equals("C") Or EncabezadoSeleccionado.strTipo.Equals("R") Then
                    EncabezadoSeleccionado.curSubTotalLiq = EncabezadoSeleccionado.curSubTotalLiq + EncabezadoSeleccionado.curComision + EncabezadoSeleccionado.curValorIva
                    EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq + EncabezadoSeleccionado.curComision + EncabezadoSeleccionado.curValorIva
                Else
                    EncabezadoSeleccionado.curSubTotalLiq = EncabezadoSeleccionado.curSubTotalLiq - EncabezadoSeleccionado.curComision - EncabezadoSeleccionado.curValorIva
                    EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq - EncabezadoSeleccionado.curComision - EncabezadoSeleccionado.curValorIva
                End If
                If Not IsNothing(EncabezadoSeleccionado.dblServBolsaFijo) And EncabezadoSeleccionado.logTraslado Then
                    If EncabezadoSeleccionado.strTipo.Equals("C") Or EncabezadoSeleccionado.strTipo.Equals("R") Then
                        EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq + EncabezadoSeleccionado.dblServBolsaFijo
                    Else
                        EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq - EncabezadoSeleccionado.dblServBolsaFijo
                    End If
                End If

                If Not IsNothing(EncabezadoSeleccionado.curRetencion) Then
                    EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq + EncabezadoSeleccionado.curRetencion
                End If

                If Not IsNothing(EncabezadoSeleccionado.curIntereses) Then
                    EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq + EncabezadoSeleccionado.curIntereses
                End If

                If Not IsNothing(EncabezadoSeleccionado.dblValorTraslado) Then
                    EncabezadoSeleccionado.curTotalLiq = EncabezadoSeleccionado.curTotalLiq + EncabezadoSeleccionado.dblValorTraslado
                End If

                If DisparaGuardar And DisparaFocus Then
                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, mstrEstado)
                    Userstate = "insert"
                End If
                'disparaguardar = False
                DisparaFocus = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones",
                                                 Me.ToString(), "Terminotraeriva", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer el iva", Me.ToString(), "TerminoTraerIva", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            'IsBusy = False

            If So.HasError Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                If So.UserState Is "BorrarRegistro" Then
                    mdcProxy.RejectChanges()
                End If

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

                So.MarkErrorAsHandled()
                Exit Try
            End If


            HabilitarCamposEspecificos = False
            HabilitarFechaEfectivo = False
            HabilitaBoton = True
            HabilitarRadioButton = False
            mlogCantidad = False
            DisparaFocus = False
            DisparaGuardar = False
            'Userstate = ""

            logNuevoRegistro = False
            logEditarRegistro = False
            lngIDLiquidacionEdicion = 0
            ParcialEdicion = 0
            FechaLiquidacionEdicion = Now

            'Propiedades para ajustar despues de guardar la operación.
            HabilitarFechaLiquidacion = False
            HabilitarFechaTitulo = False
            DatosTitulo = False
            HabilitarManualDias = False
            HabilitarEmision = False
            HabilitarVencimiento = False
            HabilitarFechaVendido = False
            HabilitarComisionistaOtraPlaza = False

            MyBase.TerminoSubmitChanges(So)
            IsBusy = False

            If (So.UserState Is "insert" Or So.UserState Is "BorrarRegistro") And strUserState = "Busqueda" Then
                ConfirmarBuscar()
            ElseIf So.UserState Is "insert" Or So.UserState Is "BorrarRegistro" Then
                MyBase.QuitarFiltroDespuesGuardar()
                IsBusy = True
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty, 0, "", "", "", Nothing, Nothing, 0, 0, 0, True)
                IsBusy = False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del resultado de la actualización de los datos", Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            If Not So Is Nothing Then
                If So.HasError Then
                    So.MarkErrorAsHandled()
                End If
            End If
        Finally
            IsBusy = False
        End Try
    End Sub

#Region "Tablas Hijas"
    Private Sub TerminoTraerReceptoresOrdenes(ByVal lo As LoadOperation(Of OperacionesReceptores))
        Try
            If Not lo.HasError Then
                ListaOperacionesReceptores = mdcProxy.OperacionesReceptores
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresOrdenes",
                                                 Me.ToString(), "TerminoTraerReceptoresOrdenes", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer los receptores", Me.ToString(), "TerminoTraerReceptoresOrdenes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerBeneficiariosOrdenes(ByVal lo As LoadOperation(Of OperacionesBeneficiarios))
        Try
            If Not lo.HasError Then
                ListaOperacionesBeneficiarios = mdcProxy.OperacionesBeneficiarios
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BeneficiariosOrdenes",
                                                 Me.ToString(), "TerminoTraerBeneficiariosOrdenes", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer los beneficiarios", Me.ToString(), "TerminoTraerBeneficiariosOrdenes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerEspeciesLiquidaciones(ByVal lo As LoadOperation(Of OperacionesEspecies))
        Try
            If Not lo.HasError Then
                ListaOperacionesEspecies = mdcProxy.OperacionesEspecies
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesLiquidaciones",
                                                 Me.ToString(), "TerminoTraerEspeciesLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las especies", Me.ToString(), "TerminoTraerEspeciesLiquidaciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerAplazamientosLiquidaciones(ByVal lo As LoadOperation(Of OperacionesAplazamientos))
        Try
            If Not lo.HasError Then
                ListaOperacionesAplazamientos = mdcProxy.OperacionesAplazamientos
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de AplazamientosLiquidaciones",
                                                 Me.ToString(), "TerminoTraerAplazamientosLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer aplazamientos", Me.ToString(), "TerminoTraerAplazamientosLiquidaciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerCustodiasLiquidaciones(ByVal lo As LoadOperation(Of OperacionesCustodias))
        Try
            If Not lo.HasError Then
                ListaOperacionesCustodias = mdcProxy.OperacionesCustodias
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CustodiasLiquidaciones",
                                                 Me.ToString(), "TerminoTraerCustodiasLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las custodias", Me.ToString(), "TerminoTraerCustodiasLiquidaciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of OyDOperaciones.Operaciones)
        Dim dcProxy As OperacionesDomainContext

        Try
            dcProxy = inicializarProxyOperaciones()

            objRet = Await dcProxy.Load(dcProxy.ConsultarOperacionesPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
    ''' Consultar de forma sincrónica los datos de OyDOperaciones.Operaciones
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal plngID As Integer = 0,
                                               Optional ByVal plngIDComitente As String = "",
                                               Optional ByVal pstrTipo As String = "",
                                               Optional ByVal pstrClaseOrden As String = "",
                                               Optional ByVal pdtmLiquidacion As Date = Nothing,
                                               Optional ByVal pdtmCumplimiento As Date = Nothing,
                                               Optional ByVal plngIDOrden As Integer = 0,
                                               Optional ByVal plngIDano As Integer = 0,
                                               Optional ByVal plngParcial As Integer = 0,
                                               Optional ByVal plogDespuesGuardado As Boolean = False) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of OyDOperaciones.Operaciones)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyOperaciones()
            End If

            mdcProxy.Operaciones.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarOperacionesSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarOperacionesSyncQuery(plngID:=plngID,
                                                                                    plngIDComitente:=plngIDComitente,
                                                                                    pstrTipo:=pstrTipo,
                                                                                    pstrClaseOrden:=pstrClaseOrden,
                                                                                    pdtmLiquidacion:=pdtmLiquidacion,
                                                                                    pdtmCumplimiento:=pdtmCumplimiento,
                                                                                    plngIDOrden:=plngIDOrden,
                                                                                    plngIDano:=plngIDano,
                                                                                    plngParcial:=plngParcial,
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

                    If Not IsNothing((From c In mdcProxy.Operaciones.ToList Where c.intIDLiquidaciones = IdLiqAnterior Select c).FirstOrDefault) Then
                        logCambiarSelected = False
                        logConsultarDetalles = False
                        logCambiarLista = True
                    End If

                    ListaEncabezado = mdcProxy.Operaciones
                    If Not IsNothing(EncabezadoSeleccionado) Then
                        If EncabezadoSeleccionado.intIDLiquidaciones = 0 Then
                            EncabezadoSeleccionado = ListaEncabezado.Last
                        End If
                    End If

                    If logCambiarSelected = False Then
                            logCambiarSelected = True
                        End If
                        If logCambiarLista Then
                            logConsultarDetalles = True
                        End If

                        If objRet.Entities.Count > 0 Then
                            If Not IsNothing(IdLiqAnterior) AndAlso IdLiqAnterior > 0 Then
                                If logCambiarLista Then
                                    logConsultarDetalles = False
                                End If

                                EncabezadoSeleccionado = (From c In ListaEncabezado Where c.intIDLiquidaciones = IdLiqAnterior Select c).FirstOrDefault
                                If logCambiarLista Then
                                    logConsultarDetalles = True
                                End If
                                IdLiqAnterior = Nothing

                                If IsNothing(EncabezadoSeleccionado) Then
                                    EncabezadoSeleccionado = ListaEncabezado.Last
                                End If

                                If Not plogFiltrar Then
                                    MyBase.ConfirmarBuscar()
                                End If
                            End If
                        Else
                            If plogDespuesGuardado = False Then
                                If Not plogFiltrar Or (plogFiltrar And Not pstrFiltro.Equals(String.Empty)) Then
                                    ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                                    A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos que concuerden con los criterios de búsqueda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                End If
                            End If
                        End If

                        If logCambiarLista Then
                            PosicionarItemEnLista(_EncabezadoSeleccionado, "TERMINOGUARDAR")
                        Else
                            IsBusy = False
                        End If


                    End If
                    Else
                ListaEncabezado.Clear()
            End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de OyDOperaciones.Operaciones ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Sub PosicionarItemEnLista(ByVal pobjSelectedPosicionar As OyDOperaciones.Operaciones, ByVal pstrLlamado As String)
        Try
            If Not IsNothing(pobjSelectedPosicionar) Then
                If pstrLlamado = "TERMINOGUARDAR" Then
                    logCambiarCollection = False

                    If Not IsNothing(ListaEncabezadoPaginada) And Not IsNothing(pobjSelectedPosicionar) Then
                        Dim logItemDiferente As Boolean = True
                        Dim intPosicionItem As Integer = ListaEncabezadoPaginada.CurrentPosition
                        logConsultarDetalles = False

                        If intPosicionItem >= 0 Then
                            Dim objGridViewRegistro As PagedCollectionView = ListaEncabezadoPaginada
                            Dim intPosicionObjeto As Integer = 0
                            Dim intPagina As Integer = 0
                            Dim logContieneRegistro As Boolean = False

                            objGridViewRegistro.MoveToFirstPage()

                            While logContieneRegistro = False
                                intPosicionObjeto = 0
                                For Each li In objGridViewRegistro
                                    If li.Equals(pobjSelectedPosicionar) Then
                                        logContieneRegistro = True
                                        Exit For
                                    End If
                                    intPosicionObjeto += 1
                                Next
                                If logContieneRegistro = False Then
                                    intPagina += 1
                                    objGridViewRegistro.MoveToNextPage()
                                End If
                            End While

                            If logContieneRegistro Then
                                If intPosicionItem <> intPosicionObjeto Then
                                    ListaEncabezadoPaginada.MoveToFirstPage()

                                    For index = 1 To intPagina
                                        ListaEncabezadoPaginada.MoveToNextPage()
                                    Next

                                    ListaEncabezadoPaginada.MoveCurrentToPosition(intPosicionObjeto)
                                End If
                            End If
                        End If
                    End If

                    MyBase.CambioItem("ListaLiquidacionesPaged")

                    MyBase.ModoMVVM = String.Empty
                    MyBase.MensajeMVVM = Enumeradores.Mensajes.FormaBoton
                    logCambiarCollection = True
                    logConsultarDetalles = True
                    logBajarIsBusy = True
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista del encabezado",
                                             Me.ToString(), "PosicionarItemEnLista", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
Public Class CamposBusquedaOperaciones
    Implements INotifyPropertyChanged

    Private _intFechaOrden As System.Nullable(Of Integer) = Now.Year
    Public Property intFechaOrden() As System.Nullable(Of Integer)
        Get
            Return _intFechaOrden
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intFechaOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intFechaOrden"))
        End Set
    End Property

    Private _strNroOrden As String
    Public Property strNroOrden() As String
        Get
            Return _strNroOrden
        End Get
        Set(ByVal value As String)
            _strNroOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNroOrden"))
        End Set
    End Property

    Private _lngID As Integer
    Public Property lngID() As Integer
        Get
            Return _lngID
        End Get
        Set(ByVal value As Integer)
            _lngID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngID"))
        End Set
    End Property

    Private _strTipo As String
    Public Property strTipo() As String
        Get
            Return _strTipo
        End Get
        Set(ByVal value As String)
            _strTipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipo"))
        End Set
    End Property

    Private _strClaseOrden As String
    Public Property strClaseOrden() As String
        Get
            Return _strClaseOrden
        End Get
        Set(ByVal value As String)
            _strClaseOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strClaseOrden"))
        End Set
    End Property

    Private _dtmLiquidacion As System.Nullable(Of Date)
    Public Property dtmLiquidacion() As System.Nullable(Of Date)
        Get
            Return _dtmLiquidacion
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _dtmLiquidacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmLiquidacion"))
        End Set
    End Property

    Private _dtmCumplimientoTitulo As System.Nullable(Of Date)
    Public Property dtmCumplimientoTitulo() As System.Nullable(Of Date)
        Get
            Return _dtmCumplimientoTitulo
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _dtmCumplimientoTitulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmCumplimientoTitulo"))
        End Set
    End Property

    Private _lngIDComitente As String
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDComitente"))
        End Set
    End Property

    Private _ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            _ComitenteSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ComitenteSeleccionado"))
        End Set
    End Property

    Public Property lngParcial As Integer


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class




