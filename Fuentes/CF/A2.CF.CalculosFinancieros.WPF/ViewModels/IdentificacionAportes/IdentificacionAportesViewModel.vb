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

Public Class IdentificacionAportesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicializaci�n"

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------            
            dcProxy = inicializarProxyCalculosFinancieros()
            dcProxy1 = inicializarProxyCalculosFinancieros()
            mdcProxyUtilidad = inicializarProxyUtilidadesOYD()

            'If System.Diagnostics.Debugger.IsAttached Then
            'End If
            '---------------------------------------------------------------------------------------------------------------------
            '-- Definir tipo de registro que manejar� el control
            '---------------------------------------------------------------------------------------------------------------------
            If Not Program.IsDesignMode() Then
                'IsBusy = True
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema en la creaci�n de los objetos", Me.ToString(), "OperacionesOtrosNegociosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Variables"
    Private dcProxy As CalculosFinancierosDomainContext
    Private dcProxy1 As CalculosFinancierosDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Public viewIdentificacionAportes As IdentificacionAportesView
    Public viewPendientesCancelar As IdentificacionAportes_CancelacionView
#End Region

#Region "Propiedades"

    Private WithEvents _IdentificacionAportes_ConsultadasSelected As New CFCalculosFinancieros.IdentificacionAportes_Consultadas
    Public Property IdentificacionAportes_ConsultadasSelected() As CFCalculosFinancieros.IdentificacionAportes_Consultadas
        Get
            Return _IdentificacionAportes_ConsultadasSelected
        End Get
        Set(ByVal value As CFCalculosFinancieros.IdentificacionAportes_Consultadas)
            _IdentificacionAportes_ConsultadasSelected = value
            MyBase.CambioItem("IdentificacionAportes_ConsultadasSelected")

            Try
                If Not IsNothing(_IdentificacionAportes_ConsultadasSelected) Then
                    MyBase.CambioItem("DiccionarioCombos")
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� al realizar las consultas.", Me.ToString(), "IdentificacionAportes_ConsultadasSelected", Application.Current.ToString(), Program.Maquina, ex)
                IsBusy = False
            End Try
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
            NombreConsecutivo = String.Empty

            If Not IsNothing(value) Then
                If IDCompania > 0 Then
                    If Not IsNothing(mdcProxyUtilidad.ItemCombos) Then
                        mdcProxyUtilidad.ItemCombos.Clear()
                    End If
                    mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosCondicionalQuery("COMBOS_TRASLADOFONDOS", IDCompania.ToString, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombos, "COMPANIA")
                Else
                    ListaConsecutivosNotas = Nothing
                    NombreConsecutivo = String.Empty
                    ListaInformacionMostrar = Nothing
                End If
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
                        li.logSeleccionado = True
                    Next
                End If
            Else
                If Not IsNothing(ListaInformacionMostrar) Then
                    For Each li In ListaInformacionMostrar
                        li.logSeleccionado = False
                    Next
                Else
                    mostrarMensaje("No existen registros para seleccionar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
            CalcularTotalSeleccionado()
            MyBase.CambioItem("SeleccionarTodos")
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

    Private _ListaInformacionMostrar As List(Of IdentificacionAportes_Consultadas)
    Public Property ListaInformacionMostrar() As List(Of IdentificacionAportes_Consultadas)
        Get
            Return _ListaInformacionMostrar
        End Get
        Set(ByVal value As List(Of IdentificacionAportes_Consultadas))
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

    Private WithEvents _InformacionMostrarSeleccionado As IdentificacionAportes_Consultadas
    Public Property InformacionMostrarSeleccionado() As IdentificacionAportes_Consultadas
        Get
            Return _InformacionMostrarSeleccionado
        End Get
        Set(ByVal value As IdentificacionAportes_Consultadas)
            _InformacionMostrarSeleccionado = value
            MyBase.CambioItem("InformacionMostrarSeleccionado")
        End Set
    End Property

    Private _ListaIdentificacionAportesCancelar As List(Of IdentificacionAportes_Consultadas_Pendientes)
    Public Property ListaIdentificacionAportesCancelar() As List(Of IdentificacionAportes_Consultadas_Pendientes)
        Get
            Return _ListaIdentificacionAportesCancelar
        End Get
        Set(ByVal value As List(Of IdentificacionAportes_Consultadas_Pendientes))
            _ListaIdentificacionAportesCancelar = value
            MyBase.CambioItem("ListaIdentificacionAportesCancelar")
            MyBase.CambioItem("ListaIdentificacionAportesCancelarPaged")
        End Set
    End Property

    Public ReadOnly Property ListaIdentificacionAportesCancelarPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaIdentificacionAportesCancelar) Then
                Dim view = New PagedCollectionView(_ListaIdentificacionAportesCancelar)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _IdentificacionAportesCancelar As IdentificacionAportes_Consultadas_Pendientes
    Public Property IdentificacionAportesCancelar() As IdentificacionAportes_Consultadas_Pendientes
        Get
            Return _IdentificacionAportesCancelar
        End Get
        Set(ByVal value As IdentificacionAportes_Consultadas_Pendientes)
            _IdentificacionAportesCancelar = value
            MyBase.CambioItem("IdentificacionAportesCancelar")
        End Set
    End Property

    Private _IDBanco As Integer
    Public Property IDBanco() As Integer
        Get
            Return _IDBanco
        End Get
        Set(ByVal value As Integer)
            _IDBanco = value
            MyBase.CambioItem("IDBanco")
        End Set
    End Property

    Private _NombreBanco As String
    Public Property NombreBanco() As String
        Get
            Return _NombreBanco
        End Get
        Set(ByVal value As String)
            _NombreBanco = value
            MyBase.CambioItem("NombreBanco")
        End Set
    End Property

    Private _NombreConsecutivo As String
    Public Property NombreConsecutivo() As String
        Get
            Return _NombreConsecutivo
        End Get
        Set(ByVal value As String)
            _NombreConsecutivo = value
            IDConcepto = Nothing
            DetalleConcepto = String.Empty
            DescripcionConcepto = String.Empty
            MyBase.CambioItem("NombreConsecutivo")
        End Set
    End Property

    Private _ListaConsecutivosNotas As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaConsecutivosNotas() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaConsecutivosNotas
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaConsecutivosNotas = value
            MyBase.CambioItem("ListaConsecutivosNotas")
        End Set
    End Property

    Private _IDConcepto As Integer
    Public Property IDConcepto() As Integer
        Get
            Return _IDConcepto
        End Get
        Set(ByVal value As Integer)
            _IDConcepto = value
            MyBase.CambioItem("IDConcepto")
        End Set
    End Property

    Private _DetalleConcepto As String
    Public Property DetalleConcepto() As String
        Get
            Return _DetalleConcepto
        End Get
        Set(ByVal value As String)
            _DetalleConcepto = value
            MyBase.CambioItem("DetalleConcepto")
        End Set
    End Property

    Private _DescripcionConcepto As String
    Public Property DescripcionConcepto() As String
        Get
            Return _DescripcionConcepto
        End Get
        Set(ByVal value As String)
            _DescripcionConcepto = value
            MyBase.CambioItem("DescripcionConcepto")
        End Set
    End Property

    Private _CentroCostos As String
    Public Property CentroCostos() As String
        Get
            Return _CentroCostos
        End Get
        Set(ByVal value As String)
            _CentroCostos = value
            MyBase.CambioItem("CentroCostos")
        End Set
    End Property

    Private _Valor As Double
    Public Property Valor() As Double
        Get
            Return _Valor
        End Get
        Set(ByVal value As Double)
            _Valor = value
            MyBase.CambioItem("Valor")
        End Set
    End Property

    Private _Fecha As Nullable(Of Date) = Nothing
    Public Property Fecha() As Nullable(Of Date)
        Get
            Return _Fecha
        End Get
        Set(ByVal value As Nullable(Of Date))
            _Fecha = value
            MyBase.CambioItem("Fecha")
        End Set
    End Property

    Private _IDComitente As String
    Public Property IDComitente() As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            MyBase.CambioItem("IDComitente")
        End Set
    End Property

    Private _NroEncargo As String
    Public Property NroEncargo() As String
        Get
            Return _NroEncargo
        End Get
        Set(ByVal value As String)
            _NroEncargo = value
            If _TipoGeneracion = "AD" Then
                If Not IsNothing(_ListaInformacionMostrar) Then
                    For Each li In _ListaInformacionMostrar
                        If li.logSeleccionado And li.strIDComitente = _IDComitente Then
                            If String.IsNullOrEmpty(li.strTipoRetiroFondos) Then
                                li.strTipoRetiroFondos = _TipoGeneracion
                            End If
                            If String.IsNullOrEmpty(li.strNroEncargo) Then
                                li.strNroEncargo = NroEncargo
                            End If
                        End If
                    Next
                End If
            End If
            MyBase.CambioItem("NroEncargo")
        End Set
    End Property

    Private _ListaTipoGeneracion As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTipoGeneracion() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTipoGeneracion
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTipoGeneracion = value
            MyBase.CambioItem("ListaTipoGeneracion")
        End Set
    End Property

    Private _TipoGeneracion As String
    Public Property TipoGeneracion() As String
        Get
            Return _TipoGeneracion
        End Get
        Set(ByVal value As String)
            _TipoGeneracion = value
            If _TipoGeneracion = "AD" Then
                HabilitarNroEncargo = True
            Else
                HabilitarNroEncargo = False
            End If
            MyBase.CambioItem("TipoGeneracion")
        End Set
    End Property

    Private _HabilitarNroEncargo As Boolean
    Public Property HabilitarNroEncargo() As Boolean
        Get
            Return _HabilitarNroEncargo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarNroEncargo = value
            MyBase.CambioItem("HabilitarNroEncargo")
            If _HabilitarNroEncargo = False Then
                NroEncargo = String.Empty

                If Not IsNothing(_ListaInformacionMostrar) Then
                    For Each li In _ListaInformacionMostrar
                        li.strNroEncargo = String.Empty
                    Next
                End If
            Else
                If Not IsNothing(_ListaInformacionMostrar) Then
                    For Each li In _ListaInformacionMostrar
                        If li.logSeleccionado And li.strIDComitente = _IDComitente Then
                            li.strTipoRetiroFondos = _TipoGeneracion
                            li.HabilitarNroEncargo = True
                            If String.IsNullOrEmpty(li.strNroEncargo) Then
                                li.strNroEncargo = NroEncargo
                            End If
                        End If
                    Next
                End If
            End If
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

#Region "Propiedades para cargar Informaci�n de los Combos"

#End Region

#Region "Metodos"

    Public Sub LimpiarInformacionGrid()
        Try
            ListaInformacionMostrar = Nothing
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al limpiar la informaci�n del grid.", Me.ToString(), "LimpiarInformacionGrid", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarCompania()
        Try
            Me.IDCompania = Nothing
            Me.IDCompaniaFirma = Nothing
            Me.NombreCompania = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al limpiar la informaci�n. ", Me.ToString(), "LimpiarCompania", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarBanco()
        Try
            Me.IDBanco = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al limpiar la informaci�n. ", Me.ToString(), "LimpiarBanco", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosCompania(ByVal pstrIDCompania As String)
        Try
            IsBusy = True
            If Not IsNothing(mdcProxyUtilidad.BuscadorGenericos) Then
                mdcProxyUtilidad.BuscadorGenericos.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarItemsQuery(pstrIDCompania, "compania", "A", "incluircompaniasclaseshijas", "FIC", "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCompania, pstrIDCompania)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo ConsultarDatosCompania. ", Me.ToString(), "ConsultarDatosCompania", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosBanco(ByVal pstrIDBanco As String)
        Try
            IsBusy = True
            Dim strIDCompania As String = String.Empty

            If Not IsNothing(IDCompania) Then
                strIDCompania = IDCompania.ToString
            End If

            If Not IsNothing(mdcProxyUtilidad.BuscadorGenericos) Then
                mdcProxyUtilidad.BuscadorGenericos.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarItemsQuery(pstrIDBanco, "CuentasBancarias", "A", String.Empty, strIDCompania, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarBanco, pstrIDBanco)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo ConsultarDatosBanco. ", Me.ToString(), "ConsultarDatosBanco", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosComitente(ByVal pstrIDComitente As String)
        Try
            IsBusy = True
            If Not IsNothing(mdcProxyUtilidad.BuscadorClientes) Then
                mdcProxyUtilidad.BuscadorClientes.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarClientesQuery(pstrIDComitente, "A", "C", "todoslosreceptores", Program.Usuario, False, Nothing, Program.HashConexion), AddressOf TerminoConsultarComitente, pstrIDComitente)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo ConsultarDatosComitente. ", Me.ToString(), "ConsultarDatosComitente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosComitente(ByVal pstrIDComitente As String, ByVal pintIDDetalle As Integer)
        Try
            IsBusy = True
            If Not IsNothing(mdcProxyUtilidad.BuscadorClientes) Then
                mdcProxyUtilidad.BuscadorClientes.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarClientesQuery(pstrIDComitente, "A", "C", "todoslosreceptores", Program.Usuario, False, Nothing, Program.HashConexion), AddressOf TerminoConsultarComitenteDetalle, String.Format("{0}|{1}", pintIDDetalle, pstrIDComitente))
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo ConsultarDatosComitente. ", Me.ToString(), "ConsultarDatosComitente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosEncargo(ByVal pstrNroEncargo As String)
        Try
            IsBusy = True
            If Not IsNothing(mdcProxyUtilidad.BuscadorGenericos) Then
                mdcProxyUtilidad.BuscadorGenericos.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarItemsQuery(pstrNroEncargo, "ENCARGOSCOMITENTE", "T", String.Empty, IDComitente, IDCompania.ToString, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarNroEncargo, pstrNroEncargo)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo ConsultarDatosComitente. ", Me.ToString(), "ConsultarDatosComitente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosEncargo(ByVal pstrNroEncargo As String, ByVal pstrIDComitente As String, ByVal pintIDDetalle As Integer)
        Try
            IsBusy = True
            If Not IsNothing(mdcProxyUtilidad.BuscadorGenericos) Then
                mdcProxyUtilidad.BuscadorGenericos.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarItemsQuery(pstrNroEncargo, "ENCARGOSCOMITENTE", "T", String.Empty, pstrIDComitente, IDCompania.ToString, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarNroEncargoDetalle, String.Format("{0}|{1}", pintIDDetalle, pstrNroEncargo))
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo ConsultarDatosComitente. ", Me.ToString(), "ConsultarDatosComitente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosCentroCosto(ByVal pstrCentroCosto As String)
        Try
            IsBusy = True
            If Not IsNothing(mdcProxyUtilidad.BuscadorGenericos) Then
                mdcProxyUtilidad.BuscadorGenericos.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarItemEspecificoQuery("CentrosCosto", pstrCentroCosto, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCentroCosto, pstrCentroCosto)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo ConsultarDatosComitente. ", Me.ToString(), "ConsultarDatosComitente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatos()
        Try
            If IsNothing(IDCompania) Or IDCompania = 0 Then
                mostrarMensaje("Debe de seleccionar la compa�ia para poder consultar la informaci�n.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            If Not IsNothing(dcProxy.IdentificacionAportes_Consultadas) Then
                dcProxy.IdentificacionAportes_Consultadas.Clear()
            End If

            dcProxy.Load(dcProxy.IdentificacionAportes_ConsultarQuery(IDCompania, Fecha, Valor, IDBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarDatos, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo ConsultarDatos. ", Me.ToString(), "ConsultarDatos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarPendientesGenerar()
        Try
            If IsNothing(IDCompania) Or IDCompania = 0 Then
                mostrarMensaje("Debe de seleccionar la compa�ia para poder consultar la informaci�n.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            viewPendientesCancelar = New IdentificacionAportes_CancelacionView(Me)
            AddHandler viewPendientesCancelar.Closed, AddressOf TerminoMostrarPendientesCancelar
            Program.Modal_OwnerMainWindowsPrincipal(viewPendientesCancelar)
            viewPendientesCancelar.ShowDialog()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo ConsultarPendientesGenerar. ", Me.ToString(), "ConsultarPendientesGenerar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoMostrarPendientesCancelar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ConsultarDatos()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo TerminoMostrarPendientesCancelar. ", Me.ToString(), "TerminoMostrarPendientesCancelar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosPendientes()
        Try
            IsBusy = True

            If Not IsNothing(dcProxy.IdentificacionAportes_Consultadas_Pendientes) Then
                dcProxy.IdentificacionAportes_Consultadas_Pendientes.Clear()
            End If

            dcProxy.Load(dcProxy.IdentificacionAportes_ConsultarPendientesQuery(IDCompania, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarDatosPendientes, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo ConsultarDatosPendientes. ", Me.ToString(), "ConsultarDatosPendientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Generar()
        Try
            Dim registro As String = String.Empty

            If IsNothing(ListaInformacionMostrar) Then
                mostrarMensaje("Debe existir al menos un registro seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaInformacionMostrar.Where(Function(i) i.logSeleccionado).Count = 0 Then
                mostrarMensaje("Debe existir al menos un registro seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaInformacionMostrar.Where(Function(i) i.logSeleccionado And String.IsNullOrEmpty(i.strIDComitente)).Count > 0 Then
                mostrarMensaje("Todos los registros deben de contener un Suscriptor seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(IDCompania) Or IDCompania = 0 Then
                mostrarMensaje("Debe de seleccionar la compa�ia para poder generar la informaci�n.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaInformacionMostrar.Where(Function(i) i.logSeleccionado And String.IsNullOrEmpty(i.strTipoRetiroFondos)).Count > 0 Then
                mostrarMensaje("Todos los registros deben de contener un Tipo de generaci�n seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaInformacionMostrar.Where(Function(i) i.logSeleccionado And i.strTipoRetiroFondos = "AD" And String.IsNullOrEmpty(i.strNroEncargo)).Count > 0 Then
                mostrarMensaje("Todos los registros deben de contener un Nro encargo seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            For Each objDato In ListaInformacionMostrar
                If objDato.logSeleccionado Then
                    If String.IsNullOrEmpty(registro) Then
                        registro = objDato.intID.ToString + "=" + objDato.strIDComitente + "=" + objDato.strTipoRetiroFondos + "=" + objDato.strNroEncargo
                    Else
                        registro += "|" + objDato.intID.ToString + "=" + objDato.strIDComitente + "=" + objDato.strTipoRetiroFondos + "=" + objDato.strNroEncargo
                    End If
                End If
            Next

            If Not IsNothing(dcProxy.IdentificacionAportes_Procesadas) Then
                dcProxy.IdentificacionAportes_Procesadas.Clear()
            End If
            dcProxy.Load(dcProxy.IdentificacionAportes_GenerarQuery(IDCompania, registro, Program.Usuario, Program.HashConexion), AddressOf TerminoGenerar, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo Generar. ", Me.ToString(), "Generar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CancelarPendientes()
        Try
            Dim registro As String = String.Empty

            If IsNothing(ListaIdentificacionAportesCancelar) Then
                mostrarMensaje("Debe existir al menos un registro seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaIdentificacionAportesCancelar.Where(Function(i) i.logSeleccionado).Count = 0 Then
                mostrarMensaje("Debe existir al menos un registro seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            For Each objDato In ListaIdentificacionAportesCancelar
                If objDato.logSeleccionado Then
                    If String.IsNullOrEmpty(registro) Then
                        registro = objDato.intID.ToString
                    Else
                        registro += "," + objDato.intID.ToString
                    End If
                End If
            Next

            If Not IsNothing(dcProxy.IdentificacionAportes_Procesadas) Then
                dcProxy.IdentificacionAportes_Procesadas.Clear()
            End If
            dcProxy.Load(dcProxy.IdentificacionAportes_CancelarPendientesQuery(IDCompania, registro, Program.Usuario, Program.HashConexion), AddressOf TerminoCancelarPendientes, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al consultar el m�todo CancelarPendientes. ", Me.ToString(), "CancelarPendientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function RetornarTituloValor(ByVal pstrItem As String) As String
        Dim strDescripcion As String = String.Empty

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

    Public Sub CalcularTotalSeleccionado()
        Try
            TotalSeleccionado = 0

            If Not IsNothing(ListaInformacionMostrar) Then
                For Each li In ListaInformacionMostrar
                    If li.logSeleccionado Then
                        TotalSeleccionado += CDbl(li.curValor)
                    End If
                Next
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se present� un problema al calcular el valor.", _
                                                Me.ToString(), "CalcularTotalSeleccionado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _InformacionMostrarSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _InformacionMostrarSeleccionado.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "logseleccionado"
                    CalcularTotalSeleccionado()
                Case "strtiporetirofondos"
                    If _InformacionMostrarSeleccionado.strTipoRetiroFondos = "AD" Then
                        _InformacionMostrarSeleccionado.HabilitarNroEncargo = True
                    Else
                        _InformacionMostrarSeleccionado.HabilitarNroEncargo = False
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_InformacionMostrarSeleccionado_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

#End Region

#Region "Resultados Asincr�nicos"

    Private Sub TerminoConsultarCompania(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).Count > 0 Then
                        IDCompania = CInt(lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.IdItem)
                        NombreCompania = lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.Nombre
                    Else
                        mostrarMensaje("La compa�ia no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IDCompania = Nothing
                        NombreCompania = String.Empty
                    End If
                Else
                    mostrarMensaje("La compa�ia no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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

    Private Sub TerminoConsultarBanco(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).Count > 0 Then
                        IDBanco = CInt(lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.IdItem)
                    Else
                        mostrarMensaje("El banco no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IDBanco = Nothing
                    End If
                Else
                    mostrarMensaje("El banco no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IDBanco = Nothing
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarBanco", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarBanco", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoConsultarComitente(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count = 0 Then
                    mostrarMensaje("El comitente no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IDComitente = String.Empty
                Else
                    If lo.Entities.Where(Function(i) i.IdComitente = lo.UserState.ToString).Count = 0 Then
                        mostrarMensaje("El comitente no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IDComitente = String.Empty
                    Else
                        IDComitente = lo.Entities.Where(Function(i) i.IdComitente = lo.UserState.ToString).First.IdComitente
                        If Not IsNothing(ListaInformacionMostrar) Then
                            For Each li In ListaInformacionMostrar
                                If li.logSeleccionado And String.IsNullOrEmpty(li.strIDComitente) Then
                                    li.strIDComitente = IDComitente
                                End If
                            Next
                        End If
                    End If

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarComitente", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoConsultarComitenteDetalle(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count = 0 Then
                    mostrarMensaje("El comitente no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If Not IsNothing(ListaInformacionMostrar) Then
                        For Each li In ListaInformacionMostrar
                            If li.intID = CInt(lo.UserState) Then
                                li.strIDComitente = String.Empty
                                Exit For
                            End If
                        Next
                    End If
                Else
                    Dim intDetalleEncargo As Integer = CInt(lo.UserState.ToString.Split(CChar("|"))(0))
                    Dim strCodigo As String = lo.UserState.ToString.Split(CChar("|"))(1)
                    If lo.Entities.Where(Function(i) i.IdComitente = strCodigo).Count = 0 Then
                        mostrarMensaje("El comitente no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        If Not IsNothing(ListaInformacionMostrar) Then
                            For Each li In ListaInformacionMostrar
                                If li.intID = intDetalleEncargo Then
                                    li.strIDComitente = String.Empty
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarComitenteDetalle", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarComitenteDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoConsultarNroEncargo(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count = 0 Then
                    mostrarMensaje("El Nro encargo no existe o no pertenece al Comitente seleccionado, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    NroEncargo = String.Empty
                Else
                    If lo.Entities.Where(Function(i) i.CodItem = lo.UserState.ToString).Count = 0 Then
                        mostrarMensaje("El Nro encargo no existe o no pertenece al Comitente seleccionado, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        NroEncargo = String.Empty
                    Else
                        NroEncargo = lo.Entities.First.CodItem
                        If Not IsNothing(ListaInformacionMostrar) Then
                            For Each li In ListaInformacionMostrar
                                If li.logSeleccionado And IDComitente = li.strIDComitente And String.IsNullOrEmpty(NroEncargo) Then
                                    li.strNroEncargo = lo.Entities.First.CodItem
                                End If
                            Next
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarNroEncargo", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarNroEncargo", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoConsultarNroEncargoDetalle(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count = 0 Then
                    mostrarMensaje("El Nro encargo no existe o no pertenece al Comitente seleccionado, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If Not IsNothing(ListaInformacionMostrar) Then
                        For Each li In ListaInformacionMostrar
                            If li.intID = CInt(lo.UserState) Then
                                li.strNroEncargo = String.Empty
                                Exit For
                            End If
                        Next
                    End If
                Else
                    Dim intDetalleEncargo As Integer = CInt(lo.UserState.ToString.Split(CChar("|"))(0))
                    Dim strCodigo As String = lo.UserState.ToString.Split(CChar("|"))(1)
                    If lo.Entities.Where(Function(i) i.CodItem = strCodigo).Count = 0 Then
                        mostrarMensaje("El Nro encargo no existe o no pertenece al Comitente seleccionado, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        If Not IsNothing(ListaInformacionMostrar) Then
                            For Each li In ListaInformacionMostrar
                                If li.intID = intDetalleEncargo Then
                                    li.strNroEncargo = String.Empty
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarComitenteDetalle", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarComitenteDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoConsultarCentroCosto(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).Count > 0 Then
                        CentroCostos = lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.IdItem
                    Else
                        mostrarMensaje("El Centro de costo no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        CentroCostos = Nothing
                    End If
                Else
                    mostrarMensaje("El Centro de costo no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    CentroCostos = Nothing
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarBanco", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarBanco", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoConsultarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then

                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDUtilidades.ItemCombo) = Nothing
                    Dim objDiccionario As New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    Dim objListaNotas As New List(Of OYDUtilidades.ItemCombo)
                    Dim objListaTipoGeneracion As New List(Of OYDUtilidades.ItemCombo)

                    If objDiccionario.ContainsKey("NOTAS_COMPANIA") Then
                        objListaNotas = objDiccionario("NOTAS_COMPANIA")
                    End If
                    If objDiccionario.ContainsKey("TIPOGENERACION") Then
                        objListaTipoGeneracion = objDiccionario("TIPOGENERACION")
                    End If

                    ListaConsecutivosNotas = objListaNotas
                    ListaTipoGeneracion = objListaTipoGeneracion

                    If ListaConsecutivosNotas.Count > 0 Then
                        If ListaConsecutivosNotas.Count = 1 Then
                            NombreConsecutivo = ListaConsecutivosNotas.First.ID
                        End If
                    End If

                    'APERTURA POR DEFECTO
                    TipoGeneracion = "AP"

                    '-----------------------------------------------------
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Private Sub TerminoConsultarDatos(ByVal lo As LoadOperation(Of CFCalculosFinancieros.IdentificacionAportes_Consultadas))
        Try
            If lo.HasError = False Then
                ListaInformacionMostrar = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la Consulta de los Datos.", Me.ToString(), "TerminoConsultarDatos", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la Consulta de los Datos.", Me.ToString(), "TerminoConsultarDatos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarDatosPendientes(ByVal lo As LoadOperation(Of CFCalculosFinancieros.IdentificacionAportes_Consultadas_Pendientes))
        Try
            If lo.HasError = False Then
                ListaIdentificacionAportesCancelar = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la Consulta de los Datos.", Me.ToString(), "TerminoConsultarDatos", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la Consulta de los Datos.", Me.ToString(), "TerminoConsultarDatos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoGenerar(ByVal lo As LoadOperation(Of CFCalculosFinancieros.IdentificacionAportes_Procesadas))
        Try
            If lo.HasError = False Then
                If lo.Entities.ToList.Count > 0 Then
                    Dim objListaMensajes As New List(Of String)
                    
                    For Each li In lo.Entities.ToList
                        objListaMensajes.Add(li.strMensaje)
                    Next

                    Dim objControlResultados As New ResultadoGenericoImportaciones()
                    objControlResultados.ListaMensajes = objListaMensajes
                    Program.Modal_OwnerMainWindowsPrincipal(objControlResultados)
                    objControlResultados.ShowDialog()

                    ConsultarDatos()
                Else
                    mostrarMensaje("Se presento un problema al traer la informaci�n de la base de datos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al Generar los Datos.", Me.ToString(), "TerminoGenerar", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al Generar los Datos.", Me.ToString(), "TerminoGenerar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoCancelarPendientes(ByVal lo As LoadOperation(Of CFCalculosFinancieros.IdentificacionAportes_Procesadas))
        Try
            If lo.HasError = False Then
                If lo.Entities.ToList.Count > 0 Then
                    Dim objListaMensajes As New List(Of String)

                    For Each li In lo.Entities.ToList
                        objListaMensajes.Add(li.strMensaje)
                    Next

                    Dim objControlResultados As New ResultadoGenericoImportaciones()
                    objControlResultados.ListaMensajes = objListaMensajes
                    Program.Modal_OwnerMainWindowsPrincipal(objControlResultados)
                    objControlResultados.ShowDialog()

                    viewPendientesCancelar.Close()

                    ConsultarDatos()
                Else
                    mostrarMensaje("Se presento un problema al traer la informaci�n de la base de datos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al Generar los Datos.", Me.ToString(), "TerminoGenerar", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al Generar los Datos.", Me.ToString(), "TerminoGenerar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

#End Region


End Class

Public Class InformacionGridIdentificacionAporte
    Implements INotifyPropertyChanged

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

    Private _Suscriptor As Integer
    Public Property Suscriptor() As Integer
        Get
            Return _Suscriptor
        End Get
        Set(ByVal value As Integer)
            _Suscriptor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Suscriptor"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class