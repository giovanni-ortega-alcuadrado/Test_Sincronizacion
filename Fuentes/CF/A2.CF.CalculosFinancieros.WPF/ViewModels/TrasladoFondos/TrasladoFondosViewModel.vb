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
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Globalization
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports A2MCCoreWPF
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.Threading.Tasks

Public Class TrasladoFondosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            dcProxy = inicializarProxyCalculosFinancieros()
            dcProxy1 = inicializarProxyCalculosFinancieros()
            mdcProxyUtilidad = inicializarProxyUtilidadesOYD()

            If System.Diagnostics.Debugger.IsAttached Then

            End If

            '---------------------------------------------------------------------------------------------------------------------
            '-- Definir tipo de registro que manejará el control
            '---------------------------------------------------------------------------------------------------------------------
            If Not Program.IsDesignMode() Then
                IsBusy = True
                mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosCondicionalQuery("COMBOS_TRASLADOFONDOS", String.Empty, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosCondicional, "INICIO")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "OperacionesOtrosNegociosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

#Region "Variables"

    Private dcProxy As CalculosFinancierosDomainContext
    Private dcProxy1 As CalculosFinancierosDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Public viewTrasladoFondos As TrasladoFondosView

#End Region

#Region "Constantes"

#End Region

#Region "Propiedades para el Tipo de registro"

    Private _lngIDMoneda As Integer
    Public Property lngIDMoneda() As Integer
        Get
            Return _lngIDMoneda
        End Get
        Set(ByVal value As Integer)
            _lngIDMoneda = value
            MyBase.CambioItem("lngIDMoneda")
        End Set
    End Property

    Private _IDCompaniaFirma As Integer
    Public Property IDCompaniaFirma() As Integer
        Get
            Return _IDCompaniaFirma
        End Get
        Set(ByVal value As Integer)
            _IDCompaniaFirma = value
            MyBase.CambioItem("IDCompaniaFirma")
        End Set
    End Property

    Private _IDCompania As Integer
    Public Property IDCompania() As Integer
        Get
            Return _IDCompania
        End Get
        Set(ByVal value As Integer)
            _IDCompania = value
            LimpiarInformacionGrid()
            If Not IsNothing(mdcProxyUtilidad.ItemCombos) Then
                mdcProxyUtilidad.ItemCombos.Clear()
            End If

            If Not IsNothing(value) Then
                mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosCondicionalQuery("COMBOS_TRASLADOFONDOS", IDCompania.ToString, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosCondicional, "COMPANIA")
            Else
                mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosCondicionalQuery("COMBOS_TRASLADOFONDOS", "-1", 0, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosCondicional, "COMPANIA")
            End If
            MyBase.CambioItem("IDCompania")
        End Set
    End Property

    Private _NombreCompania As String
    Public Property NombreCompania() As String
        Get
            Return _NombreCompania
        End Get
        Set(ByVal value As String)
            _NombreCompania = value
            MyBase.CambioItem("NombreCompania")
        End Set
    End Property

    Private _FechaRegistro As DateTime = Now.Date
    Public Property FechaRegistro() As DateTime
        Get
            Return _FechaRegistro
        End Get
        Set(ByVal value As DateTime)
            _FechaRegistro = value
            LimpiarInformacionGrid()
            MyBase.CambioItem("FechaRegistro")
        End Set
    End Property

    Private _TipoRegistro As String
    Public Property TipoRegistro() As String
        Get
            Return _TipoRegistro
        End Get
        Set(ByVal value As String)
            _TipoRegistro = value
            LimpiarInformacionGrid()
            LimpiarVariables()
            MyBase.CambioItem("TipoRegistro")
        End Set
    End Property

    Private _SeleccionarTodos As Boolean
    Public Property SeleccionarTodos() As Boolean
        Get
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodos = value
            If _SeleccionarTodos Then
                If Not IsNothing(ListaInformacionMostrar) Then
                    For Each li In ListaInformacionMostrar
                        li.Seleccionar = True
                    Next
                End If
            Else
                If Not IsNothing(ListaInformacionMostrar) Then
                    For Each li In ListaInformacionMostrar
                        li.Seleccionar = False
                    Next
                End If
            End If
            CalcularTotalSeleccionado()
            MyBase.CambioItem("SeleccionarTodos")
        End Set
    End Property

    Private _TotalizarRegistros As Boolean
    Public Property TotalizarRegistros() As Boolean
        Get
            Return _TotalizarRegistros
        End Get
        Set(ByVal value As Boolean)
            _TotalizarRegistros = value
            MyBase.CambioItem("TotalizarRegistros")
        End Set
    End Property

    Private _HabilitarGenerar As Visibility = Visibility.Collapsed
    Public Property HabilitarGenerar() As Visibility
        Get
            Return _HabilitarGenerar
        End Get
        Set(ByVal value As Visibility)
            _HabilitarGenerar = value
            MyBase.CambioItem("HabilitarGenerar")
        End Set
    End Property

    Private _ListaInformacionMostrar As List(Of InformacionMostrarGridDinamica)
    Public Property ListaInformacionMostrar() As List(Of InformacionMostrarGridDinamica)
        Get
            Return _ListaInformacionMostrar
        End Get
        Set(ByVal value As List(Of InformacionMostrarGridDinamica))
            _ListaInformacionMostrar = value
            MyBase.CambioItem("ListaInformacionMostrar")
            MyBase.CambioItem("ListaInformacionMostrarPaged")
        End Set
    End Property

    Public ReadOnly Property ListaInformacionMostrarPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaInformacionMostrar) Then
                Dim view = New PagedCollectionView(_ListaInformacionMostrar)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _InformacionMostrarSeleccionado As InformacionMostrarGridDinamica
    Public Property InformacionMostrarSeleccionado() As InformacionMostrarGridDinamica
        Get
            Return _InformacionMostrarSeleccionado
        End Get
        Set(ByVal value As InformacionMostrarGridDinamica)
            _InformacionMostrarSeleccionado = value
            MyBase.CambioItem("InformacionMostrarSeleccionado")
        End Set
    End Property

    Private _ListaTiposRegistro As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTiposRegistro() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTiposRegistro
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTiposRegistro = value
            MyBase.CambioItem("ListaTiposRegistro")
        End Set
    End Property

    Private _TotalSeleccionado As Double
    Public Property TotalSeleccionado() As Double
        Get
            Return _TotalSeleccionado
        End Get
        Set(ByVal value As Double)
            _TotalSeleccionado = value
            MyBase.CambioItem("TotalSeleccionado")
        End Set
    End Property

#End Region

#Region "Propiedades para cargar Información de los Combos"

    Private _DiccionarioCombos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombos() As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombos
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombos = value
            MyBase.CambioItem("DiccionarioCombos")
        End Set
    End Property

    Private _DiccionarioHabilitarCampos As Dictionary(Of String, Visibility)
    Public Property DiccionarioHabilitarCampos() As Dictionary(Of String, Visibility)
        Get
            Return _DiccionarioHabilitarCampos
        End Get
        Set(ByVal value As Dictionary(Of String, Visibility))
            _DiccionarioHabilitarCampos = value
            MyBase.CambioItem("DiccionarioHabilitarCampos")
        End Set
    End Property

    Private _DiccionarioValoresDefecto As Dictionary(Of String, String)
    Public Property DiccionarioValoresDefecto() As Dictionary(Of String, String)
        Get
            Return _DiccionarioValoresDefecto
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _DiccionarioValoresDefecto = value
            MyBase.CambioItem("DiccionarioValoresDefecto")
        End Set
    End Property

    Private _DiccionarioDescripcionCampos As Dictionary(Of String, String)
    Public Property DiccionarioDescripcionCampos() As Dictionary(Of String, String)
        Get
            Return _DiccionarioDescripcionCampos
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _DiccionarioDescripcionCampos = value
            MyBase.CambioItem("DiccionarioDescripcionCampos")
        End Set
    End Property

    Private _DiccionarioOrdenCampos As Dictionary(Of String, Integer)
    Public Property DiccionarioOrdenCampos() As Dictionary(Of String, Integer)
        Get
            Return _DiccionarioOrdenCampos
        End Get
        Set(ByVal value As Dictionary(Of String, Integer))
            _DiccionarioOrdenCampos = value
            MyBase.CambioItem("DiccionarioOrdenCampos")
        End Set
    End Property

#End Region

#Region "Propiedades generacion fondos"

    Private _CONSECUTIVORC_FIRMA As String
    Public Property CONSECUTIVORC_FIRMA() As String
        Get
            Return _CONSECUTIVORC_FIRMA
        End Get
        Set(ByVal value As String)
            _CONSECUTIVORC_FIRMA = value
            MyBase.CambioItem("CONSECUTIVORC_FIRMA")
        End Set
    End Property

    Private _CONSECUTIVOCE_FIRMA As String
    Public Property CONSECUTIVOCE_FIRMA() As String
        Get
            Return _CONSECUTIVOCE_FIRMA
        End Get
        Set(ByVal value As String)
            _CONSECUTIVOCE_FIRMA = value
            MyBase.CambioItem("CONSECUTIVOCE_FIRMA")
        End Set
    End Property

    Private _CONSECUTIVOCE_COMPANIA As String
    Public Property CONSECUTIVOCE_COMPANIA() As String
        Get
            Return _CONSECUTIVOCE_COMPANIA
        End Get
        Set(ByVal value As String)
            _CONSECUTIVOCE_COMPANIA = value
            MyBase.CambioItem("CONSECUTIVOCE_COMPANIA")
        End Set
    End Property

    Private _CONSECUTIVORC_COMPANIA As String
    Public Property CONSECUTIVORC_COMPANIA() As String
        Get
            Return _CONSECUTIVORC_COMPANIA
        End Get
        Set(ByVal value As String)
            _CONSECUTIVORC_COMPANIA = value
            MyBase.CambioItem("CONSECUTIVORC_COMPANIA")
        End Set
    End Property

    Private _BANCORC_FIRMA As String
    Public Property BANCORC_FIRMA() As String
        Get
            Return _BANCORC_FIRMA
        End Get
        Set(ByVal value As String)
            _BANCORC_FIRMA = value
            MyBase.CambioItem("BANCORC_FIRMA")
        End Set
    End Property

    Private _DESCRIPCION_BANCORC_FIRMA As String
    Public Property DESCRIPCION_BANCORC_FIRMA() As String
        Get
            Return _DESCRIPCION_BANCORC_FIRMA
        End Get
        Set(ByVal value As String)
            _DESCRIPCION_BANCORC_FIRMA = value
            MyBase.CambioItem("DESCRIPCION_BANCORC_FIRMA")
        End Set
    End Property

    Private _BANCORC_COMPANIA As String
    Public Property BANCORC_COMPANIA() As String
        Get
            Return _BANCORC_COMPANIA
        End Get
        Set(ByVal value As String)
            _BANCORC_COMPANIA = value
            MyBase.CambioItem("BANCORC_COMPANIA")
        End Set
    End Property

    Private _DESCRIPCION_BANCORC_COMPANIA As String
    Public Property DESCRIPCION_BANCORC_COMPANIA() As String
        Get
            Return _DESCRIPCION_BANCORC_COMPANIA
        End Get
        Set(ByVal value As String)
            _DESCRIPCION_BANCORC_COMPANIA = value
            MyBase.CambioItem("DESCRIPCION_BANCORC_COMPANIA")
        End Set
    End Property

    Private _BANCOCE_FIRMA As String
    Public Property BANCOCE_FIRMA() As String
        Get
            Return _BANCOCE_FIRMA
        End Get
        Set(ByVal value As String)
            _BANCOCE_FIRMA = value
            MyBase.CambioItem("BANCOCE_FIRMA")
        End Set
    End Property

    Private _DESCRIPCION_BANCOCE_FIRMA As String
    Public Property DESCRIPCION_BANCOCE_FIRMA() As String
        Get
            Return _DESCRIPCION_BANCOCE_FIRMA
        End Get
        Set(ByVal value As String)
            _DESCRIPCION_BANCOCE_FIRMA = value
            MyBase.CambioItem("DESCRIPCION_BANCOCE_FIRMA")
        End Set
    End Property

    Private _BANCOCE_COMPANIA As String
    Public Property BANCOCE_COMPANIA() As String
        Get
            Return _BANCOCE_COMPANIA
        End Get
        Set(ByVal value As String)
            _BANCOCE_COMPANIA = value
            MyBase.CambioItem("BANCOCE_COMPANIA")
        End Set
    End Property

    Private _DESCRIPCION_BANCOCE_COMPANIA As String
    Public Property DESCRIPCION_BANCOCE_COMPANIA() As String
        Get
            Return _DESCRIPCION_BANCOCE_COMPANIA
        End Get
        Set(ByVal value As String)
            _DESCRIPCION_BANCOCE_COMPANIA = value
            MyBase.CambioItem("DESCRIPCION_BANCOCE_COMPANIA")
        End Set
    End Property

    Private _IDCONCEPTORC As String
    Public Property IDCONCEPTORC() As String
        Get
            Return _IDCONCEPTORC
        End Get
        Set(ByVal value As String)
            _IDCONCEPTORC = value
            MyBase.CambioItem("IDCONCEPTORC")
        End Set
    End Property

    Private _DESCRIPCION_IDCONCEPTORC As String
    Public Property DESCRIPCION_IDCONCEPTORC() As String
        Get
            Return _DESCRIPCION_IDCONCEPTORC
        End Get
        Set(ByVal value As String)
            _DESCRIPCION_IDCONCEPTORC = value
            MyBase.CambioItem("DESCRIPCION_IDCONCEPTORC")
        End Set
    End Property

    Private _IDCONCEPTOCE As String
    Public Property IDCONCEPTOCE() As String
        Get
            Return _IDCONCEPTOCE
        End Get
        Set(ByVal value As String)
            _IDCONCEPTOCE = value
            MyBase.CambioItem("IDCONCEPTOCE")
        End Set
    End Property

    Private _DESCRIPCION_IDCONCEPTOCE As String
    Public Property DESCRIPCION_IDCONCEPTOCE() As String
        Get
            Return _DESCRIPCION_IDCONCEPTOCE
        End Get
        Set(ByVal value As String)
            _DESCRIPCION_IDCONCEPTOCE = value
            MyBase.CambioItem("DESCRIPCION_IDCONCEPTOCE")
        End Set
    End Property

    Private _TIPOCHEQUECE As String
    Public Property TIPOCHEQUECE() As String
        Get
            Return _TIPOCHEQUECE
        End Get
        Set(ByVal value As String)
            _TIPOCHEQUECE = value
            MyBase.CambioItem("TIPOCHEQUECE")
        End Set
    End Property

    Private _CENTROCOSTO As String
    Public Property CENTROCOSTO() As String
        Get
            Return _CENTROCOSTO
        End Get
        Set(ByVal value As String)
            _CENTROCOSTO = value
            MyBase.CambioItem("CENTROCOSTO")
        End Set
    End Property

    Private _DESCRIPCION_CENTROCOSTO As String
    Public Property DESCRIPCION_CENTROCOSTO() As String
        Get
            Return _DESCRIPCION_CENTROCOSTO
        End Get
        Set(ByVal value As String)
            _DESCRIPCION_CENTROCOSTO = value
            MyBase.CambioItem("DESCRIPCION_CENTROCOSTO")
        End Set
    End Property

    Private _FORMAPAGO As String
    Public Property FORMAPAGO() As String
        Get
            Return _FORMAPAGO
        End Get
        Set(ByVal value As String)
            _FORMAPAGO = value
            If _FORMAPAGO = "C" Then
                HABILITARTIPOCHEQUE = True
            Else
                HABILITARTIPOCHEQUE = False
                TIPOCHEQUECE = String.Empty
            End If
            MyBase.CambioItem("FORMAPAGO")
        End Set
    End Property

    Private _CONSECUTIVONC_FIRMA As String
    Public Property CONSECUTIVONC_FIRMA() As String
        Get
            Return _CONSECUTIVONC_FIRMA
        End Get
        Set(ByVal value As String)
            _CONSECUTIVONC_FIRMA = value
            MyBase.CambioItem("CONSECUTIVONC_FIRMA")
        End Set
    End Property

    Private _CONSECUTIVONC_COMPANIA As String
    Public Property CONSECUTIVONC_COMPANIA() As String
        Get
            Return _CONSECUTIVONC_COMPANIA
        End Get
        Set(ByVal value As String)
            _CONSECUTIVONC_COMPANIA = value
            MyBase.CambioItem("CONSECUTIVONC_COMPANIA")
        End Set
    End Property

    Private _BANCONC_FIRMA As String
    Public Property BANCONC_FIRMA() As String
        Get
            Return _BANCONC_FIRMA
        End Get
        Set(ByVal value As String)
            _BANCONC_FIRMA = value
            MyBase.CambioItem("BANCONC_FIRMA")
        End Set
    End Property

    Private _DESCRIPCION_BANCONC_FIRMA As String
    Public Property DESCRIPCION_BANCONC_FIRMA() As String
        Get
            Return _DESCRIPCION_BANCONC_FIRMA
        End Get
        Set(ByVal value As String)
            _DESCRIPCION_BANCONC_FIRMA = value
            MyBase.CambioItem("DESCRIPCION_BANCONC_FIRMA")
        End Set
    End Property

    Private _BANCONC_COMPANIA As String
    Public Property BANCONC_COMPANIA() As String
        Get
            Return _BANCONC_COMPANIA
        End Get
        Set(ByVal value As String)
            _BANCONC_COMPANIA = value
            MyBase.CambioItem("BANCONC_COMPANIA")
        End Set
    End Property

    Private _DESCRIPCION_BANCONC_COMPANIA As String
    Public Property DESCRIPCION_BANCONC_COMPANIA() As String
        Get
            Return _DESCRIPCION_BANCONC_COMPANIA
        End Get
        Set(ByVal value As String)
            _DESCRIPCION_BANCONC_COMPANIA = value
            MyBase.CambioItem("DESCRIPCION_BANCONC_COMPANIA")
        End Set
    End Property

    Private _IDCONCEPTONC As String
    Public Property IDCONCEPTONC() As String
        Get
            Return _IDCONCEPTONC
        End Get
        Set(ByVal value As String)
            _IDCONCEPTONC = value
            MyBase.CambioItem("IDCONCEPTONC")
        End Set
    End Property

    Private _DESCRIPCION_IDCONCEPTONC As String
    Public Property DESCRIPCION_IDCONCEPTONC() As String
        Get
            Return _DESCRIPCION_IDCONCEPTONC
        End Get
        Set(ByVal value As String)
            _DESCRIPCION_IDCONCEPTONC = value
            MyBase.CambioItem("DESCRIPCION_IDCONCEPTONC")
        End Set
    End Property

    Private _BANCOGIRADORRC As String
    Public Property BANCOGIRADORRC() As String
        Get
            Return _BANCOGIRADORRC
        End Get
        Set(ByVal value As String)
            _BANCOGIRADORRC = value
            MyBase.CambioItem("BANCOGIRADORRC")
        End Set
    End Property

    Private _NROCHEQUERC As String
    Public Property NROCHEQUERC() As String
        Get
            Return _NROCHEQUERC
        End Get
        Set(ByVal value As String)
            _NROCHEQUERC = value
            MyBase.CambioItem("NROCHEQUERC")
        End Set
    End Property

    Private _FECHACONSIGNACIONRC As DateTime = Now.Date
    Public Property FECHACONSIGNACIONRC() As DateTime
        Get
            Return _FECHACONSIGNACIONRC
        End Get
        Set(ByVal value As DateTime)
            _FECHACONSIGNACIONRC = value
            MyBase.CambioItem("FECHACONSIGNACIONRC")
        End Set
    End Property

    Private _OBSERVACIONESRC As String
    Public Property OBSERVACIONESRC() As String
        Get
            Return _OBSERVACIONESRC
        End Get
        Set(ByVal value As String)
            _OBSERVACIONESRC = value
            MyBase.CambioItem("OBSERVACIONESRC")
        End Set
    End Property

    Private _TIPOPRODUCTORC As String
    Public Property TIPOPRODUCTORC() As String
        Get
            Return _TIPOPRODUCTORC
        End Get
        Set(ByVal value As String)
            _TIPOPRODUCTORC = value
            MyBase.CambioItem("TIPOPRODUCTORC")
        End Set
    End Property

    Private _HABILITARTIPOCHEQUE As Boolean
    Public Property HABILITARTIPOCHEQUE() As Boolean
        Get
            Return _HABILITARTIPOCHEQUE
        End Get
        Set(ByVal value As Boolean)
            _HABILITARTIPOCHEQUE = value
            MyBase.CambioItem("HABILITARTIPOCHEQUE")
        End Set
    End Property


#End Region

#End Region

#Region "Metodos"

    Public Sub LimpiarCompania()
        Try
            IDCompania = Nothing
            NombreCompania = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar la compañia.", Me.ToString(), "LimpiarCompania", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarInformacionGrid()
        Try
            ListaInformacionMostrar = Nothing
            HabilitarGenerar = Visibility.Collapsed
            SeleccionarTodos = False

            Dim objDiccionarioCamposEditables As New Dictionary(Of String, Visibility)

            If Not IsNothing(DiccionarioHabilitarCampos) Then
                For Each li In DiccionarioHabilitarCampos
                    If Not objDiccionarioCamposEditables.ContainsKey(li.Key) Then
                        objDiccionarioCamposEditables.Add(li.Key, Visibility.Collapsed)
                    End If
                Next
            End If

            DiccionarioHabilitarCampos = objDiccionarioCamposEditables
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar la información del grid.", Me.ToString(), "LimpiarInformacionGrid", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarVariables()
        Try
            CONSECUTIVORC_FIRMA = String.Empty
            CONSECUTIVOCE_FIRMA = String.Empty
            CONSECUTIVOCE_COMPANIA = String.Empty
            CONSECUTIVORC_COMPANIA = String.Empty
            BANCORC_FIRMA = String.Empty
            BANCOCE_FIRMA = String.Empty
            BANCORC_COMPANIA = String.Empty
            BANCOCE_COMPANIA = String.Empty
            IDCONCEPTORC = String.Empty
            IDCONCEPTOCE = String.Empty
            TIPOCHEQUECE = String.Empty
            CENTROCOSTO = String.Empty
            FORMAPAGO = String.Empty
            DESCRIPCION_BANCOCE_COMPANIA = String.Empty
            DESCRIPCION_BANCOCE_FIRMA = String.Empty
            DESCRIPCION_BANCORC_COMPANIA = String.Empty
            DESCRIPCION_BANCORC_FIRMA = String.Empty
            DESCRIPCION_CENTROCOSTO = String.Empty
            DESCRIPCION_IDCONCEPTOCE = String.Empty
            DESCRIPCION_IDCONCEPTORC = String.Empty
            CONSECUTIVONC_COMPANIA = String.Empty
            BANCONC_COMPANIA = String.Empty
            DESCRIPCION_BANCONC_COMPANIA = String.Empty
            IDCONCEPTONC = String.Empty
            DESCRIPCION_IDCONCEPTONC = String.Empty
            BANCOGIRADORRC = Nothing
            NROCHEQUERC = Nothing
            FECHACONSIGNACIONRC = Now.Date
            OBSERVACIONESRC = String.Empty
            TIPOPRODUCTORC = Nothing
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar la información del grid.", Me.ToString(), "LimpiarInformacionGrid", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosCompania(ByVal pstrIDCompania As String)
        Try
            If Not IsNothing(mdcProxyUtilidad.BuscadorGenericos) Then
                mdcProxyUtilidad.BuscadorGenericos.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarItemsQuery(pstrIDCompania, "compania", "A", "incluircompaniasclasespadres", "", "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCompania, pstrIDCompania)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosCompania. ", Me.ToString(), "ConsultarDatosCompania", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub ConsultarTraslados()
        Try
            Await ConsultarInformacion()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar las operaciones.", Me.ToString(), "ConsultarTraslados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function ConsultarInformacion(Optional ByVal plogMostrarMensaje As Boolean = True) As Task
        Try
            If IsNothing(IDCompania) Or IDCompania = 0 Then
                mostrarMensaje("Debe de seleccionar la compañia.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            If String.IsNullOrEmpty(TipoRegistro) Then
                mostrarMensaje("Debe de seleccionar el tipo de operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            IsBusy = True
            LimpiarInformacionGrid()

            Dim objRet As LoadOperation(Of CFCalculosFinancieros.TrasladosFondosInformacion)

            If Not IsNothing(dcProxy.TrasladosFondosInformacions) Then
                dcProxy.TrasladosFondosInformacions.Clear()
            End If

            objRet = Await dcProxy.Load(dcProxy.TrasladoFondos_ConsultarInformacionSyncQuery(IDCompania, TipoRegistro, FechaRegistro, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consultar los combos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar los combos.", Me.ToString(), "CargarCombos", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.ToList.Count > 0 Then
                        Dim objListaRetornoInformacion As New List(Of TrasladosFondosInformacion)
                        Dim strTitulos As String = String.Empty
                        Dim objListaInformacionMostrarGrid As New List(Of InformacionMostrarGridDinamica)

                        If objRet.Entities.Where(Function(i) CBool(i.logEsTitulo) = True).Count() > 0 Then
                            strTitulos = objRet.Entities.Where(Function(i) CBool(i.logEsTitulo) = True).First.strInformacionRegistro
                        End If

                        If objRet.Entities.Where(Function(i) CBool(i.logEsTitulo) = False).Count > 0 Then
                            objListaRetornoInformacion = objRet.Entities.Where(Function(i) CBool(i.logEsTitulo) = False).ToList
                        End If

                        If plogMostrarMensaje Then
                            If IsNothing(objListaRetornoInformacion) Then
                                mostrarMensaje("No se encontraron registros para mostrar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                LimpiarInformacionGrid()
                                IsBusy = False
                                Exit Function
                            End If

                            If objListaRetornoInformacion.Count = 0 Then
                                mostrarMensaje("No se encontraron registros para mostrar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                LimpiarInformacionGrid()
                                IsBusy = False
                                Exit Function
                            End If
                        End If

                        Dim intContadorRegistro As Integer = 1

                        For Each li In objListaRetornoInformacion
                            Dim objFilaGrid As New InformacionMostrarGridDinamica

                            objFilaGrid.Seleccionar = False
                            objFilaGrid.ID = li.intIDRegistro
                            objFilaGrid.Valor = li.dblValorRegistro
                            objFilaGrid.lngIDMoneda = li.lngIDMoneda
                            objFilaGrid.strTipoRegistro = li.strTipoRegistro

                            intContadorRegistro = 1

                            For Each objDato In li.strInformacionRegistro.Split(CChar("|"))
                                objFilaGrid.LlevarValorColumna(objDato, intContadorRegistro)
                                intContadorRegistro += 1
                            Next

                            objListaInformacionMostrarGrid.Add(objFilaGrid)
                        Next

                        ListaInformacionMostrar = objListaInformacionMostrarGrid

                        'ACTUALIZAR TITULOS COLUMNAS GRID
                        If Not String.IsNullOrEmpty(strTitulos) Then
                            Dim intContador1 As Integer = 1
                            Dim intContador2 As Integer = 1
                            Dim intPrimerasColumnas As Integer = 1
                            Dim logSalir As Boolean = False

                            For Each li In viewTrasladoFondos.dgtInformacion.Columns
                                If intPrimerasColumnas > 3 Then
                                    intContador2 = 1

                                    For Each liTitulo In strTitulos.Split(CChar("|"))
                                        If intContador2 = intContador1 Then
                                            li.Header = liTitulo
                                            Exit For
                                        Else
                                            intContador2 += 1
                                        End If
                                    Next

                                    intContador1 += 1
                                Else
                                    intPrimerasColumnas += 1
                                End If
                            Next
                        End If

                        HabilitarGenerar = Visibility.Visible
                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar las operaciones.", Me.ToString(), "ConsultarTraslados", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusy = False
    End Function

    Public Sub GenerarTraslados()
        Try

            If IsNothing(IDCompania) Or IDCompania = 0 Then
                mostrarMensaje("Debe de seleccionar la compañia.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(TipoRegistro) Then
                mostrarMensaje("Debe de seleccionar el tipo de operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(ListaInformacionMostrar) Then
                mostrarMensaje("Debe de seleccionar al menos un item para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaInformacionMostrar.Where(Function(i) i.Seleccionar).Count = 0 Then
                mostrarMensaje("Debe de seleccionar al menos un item para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If


            If Not IsNothing(ListaInformacionMostrar) And (TipoRegistro = "OMC" Or TipoRegistro = "OMV") Then
                lngIDMoneda = ListaInformacionMostrar.FirstOrDefault.lngIDMoneda.Value

                If ListaInformacionMostrar.Count > 1 Then
                    For Each li In ListaInformacionMostrar
                        If li.lngIDMoneda <> lngIDMoneda Then
                            mostrarMensaje("No se puede generar el proceso porque todos los registros seleccionados no pertenecen a la misma moneda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                    Next
                End If
            End If

            If Not IsNothing(mdcProxyUtilidad.ItemCombos) Then
                mdcProxyUtilidad.ItemCombos.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosCondicionalQuery("COMBOS_TRASLADOFONDOS", IDCompania.ToString, lngIDMoneda, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosCondicional, "GENERAR_TRASLADOS")


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar las operaciones.", Me.ToString(), "GenerarTraslados", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Async Function ConfirmarGeneracionTraslados() As Task(Of Boolean)
        Try
            If IsNothing(IDCompania) Or IDCompania = 0 Then
                mostrarMensaje("Debe de seleccionar la compañia.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            If String.IsNullOrEmpty(TipoRegistro) Then
                mostrarMensaje("Debe de seleccionar el tipo de operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            If IsNothing(ListaInformacionMostrar) Then
                mostrarMensaje("Debe de seleccionar al menos un item para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            If ListaInformacionMostrar.Where(Function(i) i.Seleccionar).Count = 0 Then
                mostrarMensaje("Debe de seleccionar al menos un item para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            If ValidarDatosRequeridosGeneracion() = False Then
                Return False
            End If


            IsBusy = True

            Dim objRet As LoadOperation(Of CFCalculosFinancieros.TrasladosFondosRespuestaGeneracion)
            Dim strRegistrosGenerar As String = String.Empty
            Dim strDatosParaGeneracion As String = String.Empty

            For Each li In ListaInformacionMostrar
                If li.Seleccionar Then
                    If String.IsNullOrEmpty(strRegistrosGenerar) Then
                        strRegistrosGenerar = String.Format("{0}={1}={2}", li.ID, li.Valor, li.strTipoRegistro)
                    Else
                        strRegistrosGenerar = String.Format("{0}|{1}={2}={3}", strRegistrosGenerar, li.ID, li.Valor, li.strTipoRegistro)
                    End If
                End If
            Next

            strDatosParaGeneracion = RetornarValoresParaGeneracion()

            If Not IsNothing(dcProxy.TrasladosFondosRespuestaGeneracions) Then
                dcProxy.TrasladosFondosRespuestaGeneracions.Clear()
            End If


            objRet = Await dcProxy.Load(dcProxy.TrasladoFondos_GenerarSyncQuery(IDCompania, TipoRegistro, FechaRegistro, strRegistrosGenerar, TotalizarRegistros, strDatosParaGeneracion, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la ejecutar la generación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar al ejecutar la generación.", Me.ToString(), "ConfirmarGeneracionTraslados", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                    IsBusy = False
                    Return False
                Else
                    If objRet.Entities.ToList.Count > 0 Then
                        Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                        Dim objListaMensajes As New List(Of String)
                        Dim logExisteError As Boolean = False

                        For Each li In objRet.Entities.ToList
                            If li.logExitoso = False Then
                                logExisteError = True
                            End If
                            objListaMensajes.Add(li.strMensaje)
                        Next

                        objViewImportarArchivo.ListaMensajes = objListaMensajes
                        objViewImportarArchivo.Title = "Traslado fondos"
                        Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                        objViewImportarArchivo.ShowDialog()
                        IsBusy = False

                        If logExisteError Then
                            Return False
                        Else
                            LimpiarInformacionGrid()
                            Await ConsultarInformacion(False)
                            Return True
                        End If
                    Else
                        mostrarMensaje("El sistema no retorno información de retorno por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Return False
                    End If

                End If
            Else
                IsBusy = False
                Return False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación.", Me.ToString(), "ConfirmarGeneracionTraslados", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return False
        End Try
    End Function

    Public Function ValidarDatosRequeridosGeneracion() As Boolean
        Dim plogRetorno As Boolean = True
        Try
            If Not IsNothing(DiccionarioHabilitarCampos) Then
                Dim strCamposRequeridos As String = String.Empty
                For Each li In DiccionarioHabilitarCampos
                    If li.Value = Visibility.Visible Then
                        Select Case li.Key
                            Case "CONSECUTIVORC_FIRMA"
                                If String.IsNullOrEmpty(CONSECUTIVORC_FIRMA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("CONSECUTIVORC_FIRMA"))
                                    plogRetorno = False
                                End If
                            Case "CONSECUTIVOCE_FIRMA"
                                If String.IsNullOrEmpty(CONSECUTIVOCE_FIRMA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("CONSECUTIVOCE_FIRMA"))
                                    plogRetorno = False
                                End If
                            Case "CONSECUTIVONC_FIRMA"
                                If String.IsNullOrEmpty(CONSECUTIVONC_FIRMA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("CONSECUTIVONC_FIRMA"))
                                    plogRetorno = False
                                End If
                            Case "CONSECUTIVOCE_COMPANIA"
                                If String.IsNullOrEmpty(CONSECUTIVOCE_COMPANIA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("CONSECUTIVOCE_COMPANIA"))
                                    plogRetorno = False
                                End If
                            Case "CONSECUTIVORC_COMPANIA"
                                If String.IsNullOrEmpty(CONSECUTIVORC_COMPANIA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("CONSECUTIVORC_COMPANIA"))
                                    plogRetorno = False
                                End If
                            Case "CONSECUTIVONC_COMPANIA"
                                If String.IsNullOrEmpty(CONSECUTIVONC_COMPANIA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("CONSECUTIVONC_COMPANIA"))
                                    plogRetorno = False
                                End If
                            Case "BANCORC_FIRMA"
                                If String.IsNullOrEmpty(BANCORC_FIRMA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("BANCORC_FIRMA"))
                                    plogRetorno = False
                                End If
                            Case "BANCOCE_FIRMA"
                                If String.IsNullOrEmpty(BANCOCE_FIRMA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("BANCOCE_FIRMA"))
                                    plogRetorno = False
                                End If
                            Case "BANCONC_FIRMA"
                                If String.IsNullOrEmpty(BANCONC_FIRMA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("BANCONC_FIRMA"))
                                    plogRetorno = False
                                End If
                            Case "BANCORC_COMPANIA"
                                If String.IsNullOrEmpty(BANCORC_COMPANIA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("BANCORC_COMPANIA"))
                                    plogRetorno = False
                                End If
                            Case "BANCOCE_COMPANIA"
                                If String.IsNullOrEmpty(BANCOCE_COMPANIA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("BANCOCE_COMPANIA"))
                                    plogRetorno = False
                                End If
                            Case "BANCONC_COMPANIA"
                                If String.IsNullOrEmpty(BANCONC_COMPANIA) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("BANCONC_COMPANIA"))
                                    plogRetorno = False
                                End If
                            Case "IDCONCEPTORC"
                                If String.IsNullOrEmpty(IDCONCEPTORC) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("IDCONCEPTORC"))
                                    plogRetorno = False
                                End If
                            Case "IDCONCEPTOCE"
                                If String.IsNullOrEmpty(IDCONCEPTOCE) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("IDCONCEPTOCE"))
                                    plogRetorno = False
                                End If
                            Case "IDCONCEPTONC"
                                If String.IsNullOrEmpty(IDCONCEPTONC) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("IDCONCEPTONC"))
                                    plogRetorno = False
                                End If
                            Case "TIPOCHEQUECE"
                                If HABILITARTIPOCHEQUE Then
                                    If String.IsNullOrEmpty(TIPOCHEQUECE) Then
                                        strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("TIPOCHEQUECE"))
                                        plogRetorno = False
                                    End If
                                End If
                            Case "FORMAPAGO"
                                If String.IsNullOrEmpty(FORMAPAGO) Then
                                    strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("FORMAPAGO"))
                                    plogRetorno = False
                                End If
                            Case "BANCOGIRADORRC"
                                If String.IsNullOrEmpty(BANCOGIRADORRC) Then
                                    If Not IsNothing(ListaCamposObligatorios) Then
                                        If ListaCamposObligatorios.Where(Function(i) i.lngID = FORMAPAGO And i.strBancoGirador).Count > 0 Then
                                            strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("BANCOGIRADORRC"))
                                            plogRetorno = False
                                        End If
                                    End If
                                End If
                            Case "NROCHEQUERC"
                                If String.IsNullOrEmpty(NROCHEQUERC) Then
                                    If Not IsNothing(ListaCamposObligatorios) Then
                                        If ListaCamposObligatorios.Where(Function(i) i.lngID = FORMAPAGO And i.lngNumCheque).Count > 0 Then
                                            strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("NROCHEQUERC"))
                                            plogRetorno = False
                                        End If
                                    End If
                                End If
                            Case "FECHACONSIGNACIONRC"
                                If IsNothing(FECHACONSIGNACIONRC) Then
                                    If Not IsNothing(ListaCamposObligatorios) Then
                                        If ListaCamposObligatorios.Where(Function(i) i.lngID = FORMAPAGO And i.dtmConsignacion).Count > 0 Then
                                            strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("FECHACONSIGNACIONRC"))
                                            plogRetorno = False
                                        End If
                                    End If
                                End If
                            Case "OBSERVACIONESRC"
                                If String.IsNullOrEmpty(OBSERVACIONESRC) Then
                                    If Not IsNothing(ListaCamposObligatorios) Then
                                        If ListaCamposObligatorios.Where(Function(i) i.lngID = FORMAPAGO And i.strComentario).Count > 0 Then
                                            strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("OBSERVACIONESRC"))
                                            plogRetorno = False
                                        End If
                                    End If
                                End If
                            Case "TIPOPRODUCTORC"
                                If String.IsNullOrEmpty(TIPOPRODUCTORC) Then
                                    If Not IsNothing(ListaCamposObligatorios) Then
                                        If ListaCamposObligatorios.Where(Function(i) i.lngID = FORMAPAGO And i.lngidproducto).Count > 0 Then
                                            strCamposRequeridos = String.Format("{0}{1} -{2}", strCamposRequeridos, vbCrLf, RetornarTituloValor("TIPOPRODUCTORC"))
                                            plogRetorno = False
                                        End If
                                    End If
                                End If
                        End Select
                    End If
                Next

                If plogRetorno = False Then
                    mostrarMensaje(String.Format("Los siguientes campos son requeridos:{0}", strCamposRequeridos), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar las operaciones.", Me.ToString(), "ValidarDatosRequeridosGeneracion", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            plogRetorno = False
        End Try
        Return plogRetorno
    End Function

    Public Function RetornarValoresParaGeneracion() As String
        Dim pstrRetorno As String = String.Empty
        Try
            If Not IsNothing(DiccionarioHabilitarCampos) Then
                For Each li In DiccionarioHabilitarCampos
                    If li.Value = Visibility.Visible Then
                        Select Case li.Key
                            Case "CONSECUTIVORC_FIRMA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "CONSECUTIVORC_FIRMA", CONSECUTIVORC_FIRMA)
                            Case "CONSECUTIVOCE_FIRMA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "CONSECUTIVOCE_FIRMA", CONSECUTIVOCE_FIRMA)
                            Case "CONSECUTIVONC_FIRMA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "CONSECUTIVONC_FIRMA", CONSECUTIVONC_FIRMA)
                            Case "CONSECUTIVOCE_COMPANIA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "CONSECUTIVOCE_COMPANIA", CONSECUTIVOCE_COMPANIA)
                            Case "CONSECUTIVORC_COMPANIA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "CONSECUTIVORC_COMPANIA", CONSECUTIVORC_COMPANIA)
                            Case "CONSECUTIVONC_COMPANIA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "CONSECUTIVONC_COMPANIA", CONSECUTIVONC_COMPANIA)
                            Case "BANCORC_FIRMA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "BANCORC_FIRMA", BANCORC_FIRMA)
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "DESCRIPCIONBANCORC_FIRMA", DESCRIPCION_BANCORC_FIRMA)
                            Case "BANCOCE_FIRMA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "BANCOCE_FIRMA", BANCOCE_FIRMA)
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "DESCRIPCIONBANCOCE_FIRMA", DESCRIPCION_BANCOCE_FIRMA)
                            Case "BANCONC_FIRMA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "BANCONC_FIRMA", BANCONC_FIRMA)
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "DESCRIPCIONBANCONC_FIRMA", DESCRIPCION_BANCONC_FIRMA)
                            Case "BANCORC_COMPANIA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "BANCORC_COMPANIA", BANCORC_COMPANIA)
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "DESCRIPCIONBANCORC_COMPANIA", DESCRIPCION_BANCORC_COMPANIA)
                            Case "BANCOCE_COMPANIA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "BANCOCE_COMPANIA", BANCOCE_COMPANIA)
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "DESCRIPCIONBANCOCE_COMPANIA", DESCRIPCION_BANCOCE_COMPANIA)
                            Case "BANCONC_COMPANIA"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "BANCONC_COMPANIA", BANCONC_COMPANIA)
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "DESCRIPCIONBANCONC_COMPANIA", DESCRIPCION_BANCONC_COMPANIA)
                            Case "IDCONCEPTORC"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "IDCONCEPTORC", IDCONCEPTORC)
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "DETALLEIDCONCEPTORC", DESCRIPCION_IDCONCEPTORC)
                            Case "IDCONCEPTOCE"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "IDCONCEPTOCE", IDCONCEPTOCE)
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "DETALLEIDCONCEPTOCE", DESCRIPCION_IDCONCEPTOCE)
                            Case "IDCONCEPTONC"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "IDCONCEPTONC", IDCONCEPTONC)
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "DETALLEIDCONCEPTONC", DESCRIPCION_IDCONCEPTONC)
                            Case "TIPOCHEQUECE"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "TIPOCHEQUECE", TIPOCHEQUECE)
                            Case "CENTROCOSTO"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "CENTROCOSTO", CENTROCOSTO)
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "DESCRIPCIONCENTROCOSTO", DESCRIPCION_CENTROCOSTO)
                            Case "FORMAPAGO"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "FORMAPAGO", FORMAPAGO)
                            Case "BANCOGIRADORRC"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "BANCOGIRADORRC", BANCOGIRADORRC)
                            Case "NROCHEQUERC"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "NROCHEQUERC", NROCHEQUERC)
                            Case "FECHACONSIGNACIONRC"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "FECHACONSIGNACIONRC", FECHACONSIGNACIONRC.ToString("yyyy-MM-dd"))
                            Case "OBSERVACIONESRC"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "OBSERVACIONESRC", OBSERVACIONESRC)
                            Case "TIPOPRODUCTORC"
                                pstrRetorno = RetornarValorConcatenado(pstrRetorno, "TIPOPRODUCTORC", TIPOPRODUCTORC)
                        End Select
                    End If
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar las operaciones.", Me.ToString(), "ValidarDatosRequeridosGeneracion", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            pstrRetorno = String.Empty
        End Try
        Return pstrRetorno
    End Function

    Private Function RetornarTituloValor(ByVal pstrItem As String) As String
        Dim strDescripcion As String = String.Empty

        If Not IsNothing(DiccionarioDescripcionCampos) Then
            If DiccionarioDescripcionCampos.ContainsKey(pstrItem) Then
                strDescripcion = DiccionarioDescripcionCampos(pstrItem)
            End If
        End If

        Return strDescripcion
    End Function

    Private Function RetornarValorConcatenado(ByVal pstrRegistroCompleto As String, ByVal pstrItem As String, ByVal pstrValor As String) As String
        If String.IsNullOrEmpty(pstrRegistroCompleto) Then
            pstrRegistroCompleto = String.Format("{0}={1}", pstrItem, pstrValor)
        Else
            pstrRegistroCompleto = String.Format("{0}|{1}={2}", pstrRegistroCompleto, pstrItem, pstrValor)
        End If

        Return pstrRegistroCompleto
    End Function

    Public Async Function ConsultarFechaDefectoCompania() As Task
        Try
            If IDCompania > 0 Then
                IsBusy = True

                Dim objRet As InvokeOperation(Of Nullable(Of Date))
                Dim objProxy As UtilidadesDomainContext

                If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                    objProxy = New UtilidadesDomainContext()
                Else
                    objProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                End If

                'Se realiza para aumentar el tiempo de consulta de ria y evitar el timeup en algunas consultas
                DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

                ErrorForma = String.Empty

                objRet = Await objProxy.consultarFechaHabilPosteriorCierreCompaniaSync(IDCompania, Program.Usuario, Program.HashConexion).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al calcular los valores.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los valores.", Me.ToString(), "ObtenerCalculosMotor", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        FechaRegistro = CDate(objRet.Value)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cierre de la compania.", _
                                                Me.ToString(), "ConsultarFechaDefectoCompania", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Function

    Public Sub CalcularTotalSeleccionado()
        Try
            TotalSeleccionado = 0

            If Not IsNothing(ListaInformacionMostrar) Then
                For Each li In ListaInformacionMostrar
                    If li.Seleccionar Then
                        TotalSeleccionado += li.Valor
                    End If
                Next
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular el valor.", _
                                                Me.ToString(), "CalcularTotalSeleccionado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _InformacionMostrarSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _InformacionMostrarSeleccionado.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "seleccionar"
                    CalcularTotalSeleccionado()
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_InformacionMostrarSeleccionado_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

#End Region

#Region "Resultados Asincrónicos"

    Private Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    Select Case objResultado.CodigoLlamado.ToUpper

                    End Select
                End If
            Else
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta del mensaje pregunta.", Me.ToString(), "TerminoMensajePregunta", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub TerminoConsultarCompania(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).Count > 0 Then
                        IDCompania = CInt(lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.IdItem)
                        NombreCompania = lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.Nombre
                        Await ConsultarFechaDefectoCompania()
                    Else
                        mostrarMensaje("La compañia no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IDCompania = Nothing
                        NombreCompania = String.Empty
                    End If
                Else
                    mostrarMensaje("La compañia no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IDCompania = Nothing
                    NombreCompania = String.Empty
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoConsultarCombosCondicional(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then

                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDUtilidades.ItemCombo) = Nothing
                    Dim objDiccionario As New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
                    Dim objDiccionarioCamposEditables As New Dictionary(Of String, Visibility)

                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    If lo.UserState.ToString = "INICIO" Then
                        If objDiccionario.ContainsKey("TIPOREGISTRO") Then
                            ListaTiposRegistro = objDiccionario("TIPOREGISTRO")
                        End If

                        If objDiccionario.ContainsKey("COMPANIAFIRMA") Then
                            If objDiccionario("COMPANIAFIRMA").Count > 0 Then
                                IDCompaniaFirma = CInt(objDiccionario("COMPANIAFIRMA").First.intID)
                            End If
                        End If

                        If objDiccionario.ContainsKey("CAMPOSFILTROCOMPLETOS") Then
                            For Each li In objDiccionario("CAMPOSFILTROCOMPLETOS")
                                If Not objDiccionarioCamposEditables.ContainsKey(li.ID) Then
                                    objDiccionarioCamposEditables.Add(li.ID, Visibility.Collapsed)
                                End If
                            Next
                        End If

                        DiccionarioHabilitarCampos = objDiccionarioCamposEditables
                    ElseIf lo.UserState.ToString = "COMPANIA" Then
                        DiccionarioCombos = Nothing
                        DiccionarioCombos = objDiccionario

                        mdcProxyUtilidad.CamposObligatorios.Clear()
                        mdcProxyUtilidad.Load(mdcProxyUtilidad.ConsularCamposObligatoriosQuery("tblCheques", "(Todos)", "strFormaPagoRC", "(Todos)", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCamposObligatorios, "")
                    ElseIf lo.UserState.ToString = "GENERAR_TRASLADOS" Then
                        DiccionarioCombos = Nothing
                        DiccionarioCombos = objDiccionario

                        mdcProxyUtilidad.CamposObligatorios.Clear()
                        mdcProxyUtilidad.Load(mdcProxyUtilidad.ConsularCamposObligatoriosQuery("tblCheques", "(Todos)", "strFormaPagoRC", "(Todos)", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCamposObligatorios, "GENERAR_TRASLADOS")


                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Private Async Sub TerminoConsultarCamposObligatorios(ByVal lo As LoadOperation(Of OYDUtilidades.CamposObligatorios))
        Try
            If Not lo.HasError Then
                'Dim ListaFormaPago = CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_COMBOSESPECIFICOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("FormaPagoRC")
                Dim ListaFormaPago = DiccionarioCombos("FORMAPAGO").ToList

                For Each Lista In ListaFormaPago
                    Dim CamposObligatorioSelected = New CamposObligatorios

                    CamposObligatorioSelected.lngID = Lista.ID
                    CamposObligatorioSelected.strFormadePago = Lista.Descripcion

                    For Each campo In (From ld In lo.Entities.ToList Where ld.ValorCampoCondicionante = Lista.ID)
                        If campo.NombreCampoObligado.Equals("strBancoGirador") Then CamposObligatorioSelected.strBancoGirador = True

                        If campo.NombreCampoObligado.Equals("lngNumCheque") Then CamposObligatorioSelected.lngNumCheque = True

                        If campo.NombreCampoObligado.Equals("lngBancoConsignacion") Then CamposObligatorioSelected.lngBancoConsignacion = True

                        If campo.NombreCampoObligado.Equals("dtmConsignacion") Then CamposObligatorioSelected.dtmConsignacion = True

                        If campo.NombreCampoObligado.Equals("strComentario") Then CamposObligatorioSelected.strComentario = True

                        If campo.NombreCampoObligado.Equals("lngidproducto") Then CamposObligatorioSelected.lngidproducto = True
                    Next
                    ListaCamposObligatorios.Add(CamposObligatorioSelected)
                Next

                If lo.UserState.ToString = "GENERAR_TRASLADOS" Then

                    IsBusy = True
                    Dim objRet As LoadOperation(Of CFCalculosFinancieros.TrasladosFondosConfiguracion)

                    If Not IsNothing(dcProxy.TrasladosFondosConfiguracions) Then
                        dcProxy.TrasladosFondosConfiguracions.Clear()
                    End If

                    objRet = Await dcProxy.Load(dcProxy.TrasladoFondos_ConsultarConfiguracionSyncQuery(IDCompania, TipoRegistro, Program.Usuario, Program.HashConexion)).AsTask()

                    If Not objRet Is Nothing Then
                        If objRet.HasError Then
                            If objRet.Error Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consultar los combos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            Else
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar los combos.", Me.ToString(), "GenerarTraslados", Program.TituloSistema, Program.Maquina, objRet.Error)
                            End If

                            objRet.MarkErrorAsHandled()
                        Else
                            If objRet.Entities.ToList.Count > 0 Then
                                Dim objDiccionarioOrdenCampos As New Dictionary(Of String, Integer)
                                Dim objDiccionarioTituloCampos As New Dictionary(Of String, String)
                                Dim objDiccionarioCamposDefecto As New Dictionary(Of String, String)
                                Dim objListaKeysVisible As New List(Of String)

                                For Each li In DiccionarioHabilitarCampos.Keys
                                    objListaKeysVisible.Add(li)
                                Next

                                For Each li In objListaKeysVisible
                                    DiccionarioHabilitarCampos(li) = Visibility.Collapsed
                                Next

                                For Each li In objRet.Entities.ToList
                                    If DiccionarioHabilitarCampos.ContainsKey(li.strCampoFiltro) Then
                                        DiccionarioHabilitarCampos(li.strCampoFiltro) = Visibility.Visible
                                    End If
                                    If Not objDiccionarioOrdenCampos.ContainsKey(li.strCampoFiltro) Then
                                        objDiccionarioOrdenCampos.Add(li.strCampoFiltro, CInt(li.intOrden))
                                    End If
                                    If Not objDiccionarioTituloCampos.ContainsKey(li.strCampoFiltro) Then
                                        objDiccionarioTituloCampos.Add(li.strCampoFiltro, li.strTituloCampoFiltro)
                                    End If
                                    If Not objDiccionarioCamposDefecto.ContainsKey(li.strCampoFiltro) Then
                                        objDiccionarioCamposDefecto.Add(li.strCampoFiltro, li.strValorDefecto)
                                    End If
                                Next



                                DiccionarioDescripcionCampos = objDiccionarioTituloCampos
                                DiccionarioOrdenCampos = objDiccionarioOrdenCampos
                                DiccionarioValoresDefecto = objDiccionarioCamposDefecto

                                If DiccionarioCombos.ContainsKey("EGRESOS") Then
                                    If DiccionarioCombos("EGRESOS").Count = 1 Then
                                        CONSECUTIVOCE_FIRMA = DiccionarioCombos("EGRESOS").First.ID
                                    End If
                                End If

                                If DiccionarioCombos.ContainsKey("EGRESOS_COMPANIA") Then
                                    If DiccionarioCombos("EGRESOS_COMPANIA").Count = 1 Then
                                        CONSECUTIVOCE_COMPANIA = DiccionarioCombos("EGRESOS_COMPANIA").First.ID
                                    End If
                                End If

                                If DiccionarioCombos.ContainsKey("CAJA") Then
                                    If DiccionarioCombos("CAJA").Count = 1 Then
                                        CONSECUTIVORC_FIRMA = DiccionarioCombos("CAJA").First.ID
                                    End If
                                End If

                                If DiccionarioCombos.ContainsKey("CAJA_COMPANIA") Then
                                    If DiccionarioCombos("CAJA_COMPANIA").Count = 1 Then
                                        CONSECUTIVORC_COMPANIA = DiccionarioCombos("CAJA_COMPANIA").First.ID
                                    End If
                                End If

                                If DiccionarioCombos.ContainsKey("FORMAPAGO") Then
                                    If DiccionarioCombos("FORMAPAGO").Count = 1 Then
                                        FORMAPAGO = DiccionarioCombos("FORMAPAGO").First.ID
                                    End If
                                End If

                                If DiccionarioCombos.ContainsKey("NOTAS") Then
                                    If DiccionarioCombos("NOTAS").Count = 1 Then
                                        CONSECUTIVONC_FIRMA = DiccionarioCombos("NOTAS").First.ID
                                    End If
                                End If

                                If DiccionarioCombos.ContainsKey("NOTAS_COMPANIA") Then
                                    If DiccionarioCombos("NOTAS_COMPANIA").Count = 1 Then
                                        CONSECUTIVONC_COMPANIA = DiccionarioCombos("NOTAS_COMPANIA").First.ID
                                    End If
                                End If

                                'LLEVA LOS VALORES POR DEFECTO PARA LA PANTALLA DEPENDIENDO DE LA ULTIMA GENERACIÓN DEL USUARIO
                                For Each li In DiccionarioHabilitarCampos
                                    If li.Value = Visibility.Visible Then
                                        Select Case li.Key
                                            Case "CONSECUTIVORC_FIRMA"
                                                If DiccionarioValoresDefecto.ContainsKey("CONSECUTIVORC_FIRMA") Then
                                                    If DiccionarioCombos.ContainsKey("CAJA") Then
                                                        If DiccionarioCombos("CAJA").Where(Function(i) i.ID = DiccionarioValoresDefecto("CONSECUTIVORC_FIRMA")).Count > 0 Then
                                                            CONSECUTIVORC_FIRMA = DiccionarioValoresDefecto("CONSECUTIVORC_FIRMA")
                                                        End If
                                                    End If
                                                End If
                                            Case "CONSECUTIVOCE_FIRMA"
                                                If DiccionarioValoresDefecto.ContainsKey("CONSECUTIVOCE_FIRMA") Then
                                                    If DiccionarioCombos.ContainsKey("EGRESOS") Then
                                                        If DiccionarioCombos("EGRESOS").Where(Function(i) i.ID = DiccionarioValoresDefecto("CONSECUTIVOCE_FIRMA")).Count > 0 Then
                                                            CONSECUTIVOCE_FIRMA = DiccionarioValoresDefecto("CONSECUTIVOCE_FIRMA")
                                                        End If
                                                    End If
                                                End If
                                            Case "CONSECUTIVONC_FIRMA"
                                                If DiccionarioValoresDefecto.ContainsKey("CONSECUTIVONC_FIRMA") Then
                                                    If DiccionarioCombos.ContainsKey("NOTAS") Then
                                                        If DiccionarioCombos("NOTAS").Where(Function(i) i.ID = DiccionarioValoresDefecto("CONSECUTIVONC_FIRMA")).Count > 0 Then
                                                            CONSECUTIVONC_FIRMA = DiccionarioValoresDefecto("CONSECUTIVONC_FIRMA")
                                                        End If
                                                    End If
                                                End If
                                            Case "CONSECUTIVOCE_COMPANIA"
                                                If DiccionarioValoresDefecto.ContainsKey("CONSECUTIVOCE_COMPANIA") Then
                                                    If DiccionarioCombos.ContainsKey("EGRESOS_COMPANIA") Then
                                                        If DiccionarioCombos("EGRESOS_COMPANIA").Where(Function(i) i.ID = DiccionarioValoresDefecto("CONSECUTIVOCE_COMPANIA")).Count > 0 Then
                                                            CONSECUTIVOCE_COMPANIA = DiccionarioValoresDefecto("CONSECUTIVOCE_COMPANIA")
                                                        End If
                                                    End If
                                                End If
                                            Case "CONSECUTIVORC_COMPANIA"
                                                If DiccionarioValoresDefecto.ContainsKey("CONSECUTIVORC_COMPANIA") Then
                                                    If DiccionarioCombos.ContainsKey("CAJA_COMPANIA") Then
                                                        If DiccionarioCombos("CAJA_COMPANIA").Where(Function(i) i.ID = DiccionarioValoresDefecto("CONSECUTIVORC_COMPANIA")).Count > 0 Then
                                                            CONSECUTIVORC_COMPANIA = DiccionarioValoresDefecto("CONSECUTIVORC_COMPANIA")
                                                        End If
                                                    End If
                                                End If
                                            Case "CONSECUTIVONC_COMPANIA"
                                                If DiccionarioValoresDefecto.ContainsKey("CONSECUTIVONC_COMPANIA") Then
                                                    If DiccionarioCombos.ContainsKey("NOTAS_COMPANIA") Then
                                                        If DiccionarioCombos("NOTAS_COMPANIA").Where(Function(i) i.ID = DiccionarioValoresDefecto("CONSECUTIVONC_COMPANIA")).Count > 0 Then
                                                            CONSECUTIVONC_COMPANIA = DiccionarioValoresDefecto("CONSECUTIVONC_COMPANIA")
                                                        End If
                                                    End If
                                                End If
                                            Case "BANCORC_FIRMA"
                                                If DiccionarioValoresDefecto.ContainsKey("BANCORC_FIRMA") Then
                                                    BANCORC_FIRMA = DiccionarioValoresDefecto("BANCORC_FIRMA")
                                                End If
                                                If DiccionarioValoresDefecto.ContainsKey("DESCRIPCIONBANCORC_FIRMA") Then
                                                    DESCRIPCION_BANCORC_FIRMA = DiccionarioValoresDefecto("DESCRIPCIONBANCORC_FIRMA")
                                                End If
                                            Case "BANCOCE_FIRMA"
                                                If DiccionarioValoresDefecto.ContainsKey("BANCOCE_FIRMA") Then
                                                    BANCOCE_FIRMA = DiccionarioValoresDefecto("BANCOCE_FIRMA")
                                                End If
                                                If DiccionarioValoresDefecto.ContainsKey("DESCRIPCIONBANCOCE_FIRMA") Then
                                                    DESCRIPCION_BANCOCE_FIRMA = DiccionarioValoresDefecto("DESCRIPCIONBANCOCE_FIRMA")
                                                End If
                                            Case "BANCONC_FIRMA"
                                                If DiccionarioValoresDefecto.ContainsKey("BANCONC_FIRMA") Then
                                                    BANCONC_FIRMA = DiccionarioValoresDefecto("BANCONC_FIRMA")
                                                End If
                                                If DiccionarioValoresDefecto.ContainsKey("DESCRIPCIONBANCONC_FIRMA") Then
                                                    DESCRIPCION_BANCONC_FIRMA = DiccionarioValoresDefecto("DESCRIPCIONBANCONC_FIRMA")
                                                End If
                                            Case "BANCORC_COMPANIA"
                                                If DiccionarioValoresDefecto.ContainsKey("BANCORC_COMPANIA") Then
                                                    BANCORC_COMPANIA = DiccionarioValoresDefecto("BANCORC_COMPANIA")
                                                End If
                                                If DiccionarioValoresDefecto.ContainsKey("DESCRIPCIONBANCORC_COMPANIA") Then
                                                    DESCRIPCION_BANCORC_COMPANIA = DiccionarioValoresDefecto("DESCRIPCIONBANCORC_COMPANIA")
                                                End If
                                            Case "BANCOCE_COMPANIA"
                                                If DiccionarioValoresDefecto.ContainsKey("BANCOCE_COMPANIA") Then
                                                    BANCOCE_COMPANIA = DiccionarioValoresDefecto("BANCOCE_COMPANIA")
                                                End If
                                                If DiccionarioValoresDefecto.ContainsKey("DESCRIPCIONBANCOCE_COMPANIA") Then
                                                    DESCRIPCION_BANCOCE_COMPANIA = DiccionarioValoresDefecto("DESCRIPCIONBANCOCE_COMPANIA")
                                                End If
                                            Case "BANCONC_COMPANIA"
                                                If DiccionarioValoresDefecto.ContainsKey("BANCONC_COMPANIA") Then
                                                    BANCONC_COMPANIA = DiccionarioValoresDefecto("BANCONC_COMPANIA")
                                                End If
                                                If DiccionarioValoresDefecto.ContainsKey("DESCRIPCIONBANCONC_COMPANIA") Then
                                                    DESCRIPCION_BANCONC_COMPANIA = DiccionarioValoresDefecto("DESCRIPCIONBANCONC_COMPANIA")
                                                End If
                                            Case "IDCONCEPTORC"
                                                If DiccionarioValoresDefecto.ContainsKey("IDCONCEPTORC") Then
                                                    IDCONCEPTORC = DiccionarioValoresDefecto("IDCONCEPTORC")
                                                End If
                                                If DiccionarioValoresDefecto.ContainsKey("DESCRIPCIONIDCONCEPTORC") Then
                                                    DESCRIPCION_IDCONCEPTORC = DiccionarioValoresDefecto("DESCRIPCIONIDCONCEPTORC")
                                                End If
                                            Case "IDCONCEPTOCE"
                                                If DiccionarioValoresDefecto.ContainsKey("IDCONCEPTOCE") Then
                                                    IDCONCEPTOCE = DiccionarioValoresDefecto("IDCONCEPTOCE")
                                                End If
                                                If DiccionarioValoresDefecto.ContainsKey("DETALLEIDCONCEPTOCE") Then
                                                    DESCRIPCION_IDCONCEPTOCE = DiccionarioValoresDefecto("DETALLEIDCONCEPTOCE")
                                                End If
                                            Case "IDCONCEPTONC"
                                                If DiccionarioValoresDefecto.ContainsKey("IDCONCEPTONC") Then
                                                    IDCONCEPTONC = DiccionarioValoresDefecto("IDCONCEPTONC")
                                                End If
                                                If DiccionarioValoresDefecto.ContainsKey("DETALLEIDCONCEPTONC") Then
                                                    DESCRIPCION_IDCONCEPTONC = DiccionarioValoresDefecto("DETALLEIDCONCEPTONC")
                                                End If
                                            Case "CENTROCOSTO"
                                                If DiccionarioValoresDefecto.ContainsKey("CENTROCOSTO") Then
                                                    CENTROCOSTO = DiccionarioValoresDefecto("CENTROCOSTO")
                                                End If
                                                If DiccionarioValoresDefecto.ContainsKey("DESCRIPCIONCENTROCOSTO") Then
                                                    DESCRIPCION_CENTROCOSTO = DiccionarioValoresDefecto("DESCRIPCIONCENTROCOSTO")
                                                End If
                                            Case "FORMAPAGO"
                                                If DiccionarioValoresDefecto.ContainsKey("FORMAPAGO") Then
                                                    If DiccionarioCombos.ContainsKey("FORMAPAGO") Then
                                                        If DiccionarioCombos("FORMAPAGO").Where(Function(i) i.ID = DiccionarioValoresDefecto("FORMAPAGO")).Count > 0 Then
                                                            FORMAPAGO = DiccionarioValoresDefecto("FORMAPAGO")
                                                        End If
                                                    End If
                                                End If
                                            Case "TIPOCHEQUECE"
                                                If DiccionarioValoresDefecto.ContainsKey("TIPOCHEQUECE") Then
                                                    If DiccionarioCombos.ContainsKey("TIPOSELLO") Then
                                                        If DiccionarioCombos("TIPOSELLO").Where(Function(i) i.ID = DiccionarioValoresDefecto("TIPOCHEQUECE")).Count > 0 Then
                                                            TIPOCHEQUECE = DiccionarioValoresDefecto("TIPOCHEQUECE")
                                                        End If
                                                    End If
                                                End If
                                        End Select
                                    End If
                                Next

                                Dim objViewTrasladoFondosGenerar As New TrasladoFondosGeneracionView(Me)
                                Program.Modal_OwnerMainWindowsPrincipal(objViewTrasladoFondosGenerar)
                                IsBusy = False
                                objViewTrasladoFondosGenerar.ShowDialog()

                            Else
                                mostrarMensaje("El tipo de operación no tiene configuración, por favor comuniquese con el administrador.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                            End If
                        End If
                    End If

                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los campos obligatorios",
                                                     Me.ToString(), "TerminoConsultarCamposObligatorios", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar al terminar de consultar los campos obligatorios",
                                 Me.ToString(), "TerminoConsultarCamposObligatorios", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Funciones Generales"

    Property ListaCamposObligatorios As New List(Of CamposObligatorios)

#End Region

#Region "Eventos"

#End Region

End Class

Public Class InformacionMostrarGridDinamica
    Implements INotifyPropertyChanged

    Public Sub LlevarValorColumna(ByVal pstrValor As String, ByVal pintColumna As Integer)
        If pintColumna = 1 Then
            ColumnaDinamicaInfoAdicional1 = pstrValor
        ElseIf pintColumna = 2 Then
            ColumnaDinamicaInfoAdicional2 = pstrValor
        ElseIf pintColumna = 3 Then
            ColumnaDinamicaInfoAdicional3 = pstrValor
        ElseIf pintColumna = 4 Then
            ColumnaDinamicaInfoAdicional4 = pstrValor
        ElseIf pintColumna = 5 Then
            ColumnaDinamicaInfoAdicional5 = pstrValor
        ElseIf pintColumna = 6 Then
            ColumnaDinamicaInfoAdicional6 = pstrValor
        ElseIf pintColumna = 7 Then
            ColumnaDinamicaInfoAdicional7 = pstrValor
        ElseIf pintColumna = 8 Then
            ColumnaDinamicaInfoAdicional8 = pstrValor
        ElseIf pintColumna = 9 Then
            ColumnaDinamicaInfoAdicional9 = pstrValor
        ElseIf pintColumna = 10 Then
            ColumnaDinamicaInfoAdicional10 = pstrValor
        ElseIf pintColumna = 11 Then
            ColumnaDinamicaInfoAdicional11 = pstrValor
        ElseIf pintColumna = 12 Then
            ColumnaDinamicaInfoAdicional12 = pstrValor
        ElseIf pintColumna = 13 Then
            ColumnaDinamicaInfoAdicional13 = pstrValor
        ElseIf pintColumna = 14 Then
            ColumnaDinamicaInfoAdicional14 = pstrValor
        ElseIf pintColumna = 15 Then
            ColumnaDinamicaInfoAdicional15 = pstrValor
        ElseIf pintColumna = 16 Then
            ColumnaDinamicaInfoAdicional16 = pstrValor
        ElseIf pintColumna = 17 Then
            ColumnaDinamicaInfoAdicional17 = pstrValor
        ElseIf pintColumna = 18 Then
            ColumnaDinamicaInfoAdicional18 = pstrValor
        ElseIf pintColumna = 19 Then
            ColumnaDinamicaInfoAdicional19 = pstrValor
        ElseIf pintColumna = 20 Then
            ColumnaDinamicaInfoAdicional20 = pstrValor
        End If
    End Sub

    Private _Seleccionar As Boolean
    Public Property Seleccionar() As Boolean
        Get
            Return _Seleccionar
        End Get
        Set(ByVal value As Boolean)
            _Seleccionar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Seleccionar"))
        End Set
    End Property

    Private _ID As Integer
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _Valor As Double
    Public Property Valor() As Double
        Get
            Return _Valor
        End Get
        Set(ByVal value As Double)
            _Valor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Valor"))
        End Set
    End Property

    Private _strTipoRegistro As String
    Public Property strTipoRegistro() As String
        Get
            Return _strTipoRegistro
        End Get
        Set(ByVal value As String)
            _strTipoRegistro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoRegistro"))
        End Set
    End Property

    Private _lngIDMoneda As System.Nullable(Of Integer)
    Public Property lngIDMoneda() As System.Nullable(Of Integer)
        Get
            Return _lngIDMoneda
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _lngIDMoneda = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDMoneda"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional1 As String
    Public Property ColumnaDinamicaInfoAdicional1() As String
        Get
            Return _ColumnaDinamicaInfoAdicional1
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional1 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional1"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional2 As String
    Public Property ColumnaDinamicaInfoAdicional2() As String
        Get
            Return _ColumnaDinamicaInfoAdicional2
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional2 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional2"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional3 As String
    Public Property ColumnaDinamicaInfoAdicional3() As String
        Get
            Return _ColumnaDinamicaInfoAdicional3
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional3 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional3"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional4 As String
    Public Property ColumnaDinamicaInfoAdicional4() As String
        Get
            Return _ColumnaDinamicaInfoAdicional4
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional4 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional4"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional5 As String
    Public Property ColumnaDinamicaInfoAdicional5() As String
        Get
            Return _ColumnaDinamicaInfoAdicional5
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional5 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional5"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional6 As String
    Public Property ColumnaDinamicaInfoAdicional6() As String
        Get
            Return _ColumnaDinamicaInfoAdicional6
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional6 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional6"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional7 As String
    Public Property ColumnaDinamicaInfoAdicional7() As String
        Get
            Return _ColumnaDinamicaInfoAdicional7
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional7 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional7"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional8 As String
    Public Property ColumnaDinamicaInfoAdicional8() As String
        Get
            Return _ColumnaDinamicaInfoAdicional8
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional8 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional8"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional9 As String
    Public Property ColumnaDinamicaInfoAdicional9() As String
        Get
            Return _ColumnaDinamicaInfoAdicional9
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional9 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional9"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional10 As String
    Public Property ColumnaDinamicaInfoAdicional10() As String
        Get
            Return _ColumnaDinamicaInfoAdicional10
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional10 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional10"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional11 As String
    Public Property ColumnaDinamicaInfoAdicional11() As String
        Get
            Return _ColumnaDinamicaInfoAdicional11
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional11 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional11"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional12 As String
    Public Property ColumnaDinamicaInfoAdicional12() As String
        Get
            Return _ColumnaDinamicaInfoAdicional12
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional12 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional12"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional13 As String
    Public Property ColumnaDinamicaInfoAdicional13() As String
        Get
            Return _ColumnaDinamicaInfoAdicional13
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional13 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional13"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional14 As String
    Public Property ColumnaDinamicaInfoAdicional14() As String
        Get
            Return _ColumnaDinamicaInfoAdicional14
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional14 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional14"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional15 As String
    Public Property ColumnaDinamicaInfoAdicional15() As String
        Get
            Return _ColumnaDinamicaInfoAdicional15
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional15 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional15"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional16 As String
    Public Property ColumnaDinamicaInfoAdicional16() As String
        Get
            Return _ColumnaDinamicaInfoAdicional16
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional16 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional16"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional17 As String
    Public Property ColumnaDinamicaInfoAdicional17() As String
        Get
            Return _ColumnaDinamicaInfoAdicional17
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional17 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional17"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional18 As String
    Public Property ColumnaDinamicaInfoAdicional18() As String
        Get
            Return _ColumnaDinamicaInfoAdicional18
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional18 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional18"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional19 As String
    Public Property ColumnaDinamicaInfoAdicional19() As String
        Get
            Return _ColumnaDinamicaInfoAdicional19
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional19 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional19"))
        End Set
    End Property

    Private _ColumnaDinamicaInfoAdicional20 As String
    Public Property ColumnaDinamicaInfoAdicional20() As String
        Get
            Return _ColumnaDinamicaInfoAdicional20
        End Get
        Set(ByVal value As String)
            _ColumnaDinamicaInfoAdicional20 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ColumnaDinamicaInfoAdicional20"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class

Public Class CamposObligatorios
    Public Property lngID As String = String.Empty
    Public Property strFormadePago As String = String.Empty
    Public Property strBancoGirador As Boolean = False
    Public Property lngNumCheque As Boolean = False
    Public Property lngBancoConsignacion As Boolean = False
    Public Property dtmConsignacion As Boolean = False
    Public Property strComentario As Boolean = False
    Public Property lngidproducto As Boolean = False
End Class