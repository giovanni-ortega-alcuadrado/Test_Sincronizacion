Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports A2Utilidades.Mensajes

Public Class ConfiguracionFacturasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private ConfiguracionAnterior As ConfiguracionFacturas
    Dim dcProxy As BolsaDomainContext
    Dim IdItemActualizar As Integer
    Dim logActualizarSelected As Boolean
    Dim logCambiarPropiedades As Boolean = True

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New BolsaDomainContext()
            Else
                dcProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ConfiguracionFacturasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfiguracionFacturas, " ")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                Me.ToString(), "ConfiguracionFacturasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados asincronicos"
    Public Sub TerminoTraerConfiguracionFacturas(ByVal lo As LoadOperation(Of ConfiguracionFacturas))
        Try
            If Not lo.HasError Then

                If lo.UserState = "TerminoGuardar" Then
                    logActualizarSelected = False
                End If

                ListaConfiguracion = dcProxy.ConfiguracionFacturas

                logActualizarSelected = True

                If dcProxy.ConfiguracionFacturas.Count > 0 Then
                    If lo.UserState = "TerminoGuardar" Then
                        If ListaConfiguracion.Where(Function(i) i.IDConfiguracionfacturas = IdItemActualizar).Count > 0 Then
                            ConfiguracionSelected = ListaConfiguracion.Where(Function(i) i.IDConfiguracionfacturas = IdItemActualizar).FirstOrDefault
                        Else
                            ConfiguracionSelected = ListaConfiguracion.First
                        End If
                    End If
                Else

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de clientes", _
                                                 Me.ToString(), "TerminoTraerClientes", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "SSe presentó un problema en la obtención de la lista de Configuracion Facturas", _
                                Me.ToString(), "TerminoTraerConfiguracionFacturas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Propiedades"
    Private _ListaConfiguracion As EntitySet(Of ConfiguracionFacturas)
    Public Property ListaConfiguracion() As EntitySet(Of ConfiguracionFacturas)
        Get
            Return _ListaConfiguracion
        End Get
        Set(ByVal value As EntitySet(Of ConfiguracionFacturas))
            _ListaConfiguracion = value

            MyBase.CambioItem("ListaConfiguracion")
            MyBase.CambioItem("ListaConfiguracionPaged")
            If Not IsNothing(_ListaConfiguracion) Then
                If ListaConfiguracion.Count > 0 And logActualizarSelected Then
                    _ConfiguracionSelected = ListaConfiguracion.FirstOrDefault
                Else
                    _ConfiguracionSelected = Nothing
                End If
            Else
                _ConfiguracionSelected = Nothing
            End If
        End Set
    End Property
    Public ReadOnly Property ListaConfiguracionPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConfiguracion) Then
                Dim view = New PagedCollectionView(_ListaConfiguracion)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ConfiguracionSelected As ConfiguracionFacturas
    Public Property ConfiguracionSelected() As ConfiguracionFacturas
        Get
            Return _ConfiguracionSelected
        End Get
        Set(ByVal value As ConfiguracionFacturas)
            _ConfiguracionSelected = value
            MyBase.CambioItem("ConfiguracionSelected")
        End Set
    End Property

    Private _UsarFechade As Boolean = False
    Public Property UsarFechade() As Boolean
        Get
            Return _UsarFechade
        End Get
        Set(ByVal value As Boolean)
            _UsarFechade = value
            If _UsarFechade = True Then
                HabilitarAU = True
            Else
                HabilitarAU = False
                FechaLiqAgrup = False
                FechaCumplAgrup = False
            End If
            MyBase.CambioItem("UsarFechade")
        End Set
    End Property

    Private _FechaLiqAgrup As Boolean = False
    Public Property FechaLiqAgrup() As Boolean
        Get
            Return _FechaLiqAgrup
        End Get
        Set(ByVal value As Boolean)
            _FechaLiqAgrup = value
            MyBase.CambioItem("FechaLiqAgrup")
        End Set
    End Property

    Private _FechaCumplAgrup As Boolean = False
    Public Property FechaCumplAgrup() As Boolean
        Get
            Return _FechaCumplAgrup
        End Get
        Set(ByVal value As Boolean)
            _FechaCumplAgrup = value
            MyBase.CambioItem("FechaCumplAgrup")
        End Set
    End Property

    Private _UsarFechaDeOrd As Boolean = False
    Public Property UsarFechaDeOrd() As Boolean
        Get
            Return _UsarFechaDeOrd
        End Get
        Set(ByVal value As Boolean)
            _UsarFechaDeOrd = value
            If _UsarFechaDeOrd = True Then
                HabilitarOU = True
                HabilitarOULiq = True
                HabilitarOUCumpl = True
            Else
                HabilitarOU = False
                HabilitarOULiq = False
                intFechaLiq = 0
                HabilitarOUCumpl = False
                intFechaCumpl = 0
            End If
            MyBase.CambioItem("UsarFechaDeOrd")
        End Set
    End Property

    Private _FechaLiqOrd As Boolean = False
    Public Property FechaLiqOrd() As Boolean
        Get
            Return _FechaLiqOrd
        End Get
        Set(ByVal value As Boolean)
            _FechaLiqOrd = value
            If _FechaLiqOrd = True Then
                HabilitarOULiq = True
                HabilitarOUCumpl = False
            Else
                HabilitarOULiq = False
                HabilitarOUCumpl = True
            End If
            MyBase.CambioItem("FechaLiqOrd")
        End Set
    End Property

    Private _intFechaLiq As Integer = 0
    Public Property intFechaLiq() As Integer
        Get
            Return _intFechaLiq
        End Get
        Set(ByVal value As Integer)
            _intFechaLiq = value
            MyBase.CambioItem("intFechaLiq")
        End Set
    End Property

    Private _FechaCumplOrd As Boolean = False
    Public Property FechaCumplOrd() As Boolean
        Get
            Return _FechaCumplOrd
        End Get
        Set(ByVal value As Boolean)
            _FechaCumplOrd = value
            If _FechaCumplOrd = True Then
                HabilitarOUCumpl = True
                HabilitarOULiq = False
            Else
                HabilitarOUCumpl = False
                HabilitarOULiq = True
            End If
            MyBase.CambioItem("FechaCumplOrd")
        End Set
    End Property

    Private _intFechaCumpl As Integer = 0
    Public Property intFechaCumpl() As Integer
        Get
            Return _intFechaCumpl
        End Get
        Set(ByVal value As Integer)
            _intFechaCumpl = value
            MyBase.CambioItem("intFechaCumpl")
        End Set
    End Property


    Private _TipoOperacionAgrup As Boolean = False
    Public Property TipoOperacionAgrup() As Boolean
        Get
            Return _TipoOperacionAgrup
        End Get
        Set(ByVal value As Boolean)
            _TipoOperacionAgrup = value
            If _TipoOperacionAgrup = True Then
                HabilitarATVenta = False
                HabilitarATCompra = False
            Else
                HabilitarATVenta = True
                HabilitarATCompra = True
            End If
            MyBase.CambioItem("TipoOperacionAgrup")
        End Set
    End Property

    Private _VentaAgrup As Boolean = False
    Public Property VentaAgrup() As Boolean
        Get
            Return _VentaAgrup
        End Get
        Set(ByVal value As Boolean)
            _VentaAgrup = value
            If _VentaAgrup = True Then
                HabTipoOperacionAgrup = False
                HabilitarATCompra = False
            Else
                HabTipoOperacionAgrup = True
                HabilitarATCompra = True
            End If
            MyBase.CambioItem("VentaAgrup")
        End Set
    End Property

    Private _CompraAgrup As Boolean = False
    Public Property CompraAgrup() As Boolean
        Get
            Return _CompraAgrup
        End Get
        Set(ByVal value As Boolean)
            _CompraAgrup = value
            If _CompraAgrup = True Then
                HabTipoOperacionAgrup = False
                HabilitarATVenta = False
            Else
                HabTipoOperacionAgrup = True
                HabilitarATVenta = True
            End If
            MyBase.CambioItem("CompraAgrup")
        End Set
    End Property


    Private _HabTipoOperacionAgrup As Boolean = False
    Public Property HabTipoOperacionAgrup() As Boolean
        Get
            Return _HabTipoOperacionAgrup
        End Get
        Set(ByVal value As Boolean)
            _HabTipoOperacionAgrup = value
            MyBase.CambioItem("HabTipoOperacionAgrup")
        End Set
    End Property

    Private _HabilitarAU As Boolean = False
    Public Property HabilitarAU() As Boolean
        Get
            Return _HabilitarAU
        End Get
        Set(ByVal value As Boolean)
            _HabilitarAU = value
            MyBase.CambioItem("HabilitarAU")
        End Set
    End Property

    Private _HabilitarATVenta As Boolean = False
    Public Property HabilitarATVenta() As Boolean
        Get
            Return _HabilitarATVenta
        End Get
        Set(ByVal value As Boolean)
            _HabilitarATVenta = value
            MyBase.CambioItem("HabilitarATVenta")
        End Set
    End Property

    Private _HabilitarATCompra As Boolean = False
    Public Property HabilitarATCompra() As Boolean
        Get
            Return _HabilitarATCompra
        End Get
        Set(ByVal value As Boolean)
            _HabilitarATCompra = value
            MyBase.CambioItem("HabilitarATCompra")
        End Set
    End Property

    Private _HabilitarAC As Boolean = False
    Public Property HabilitarAC() As Boolean
        Get
            Return _HabilitarAC
        End Get
        Set(ByVal value As Boolean)
            _HabilitarAC = value
            MyBase.CambioItem("HabilitarAC")
        End Set
    End Property

    Private _HabilitarOU As Boolean = False
    Public Property HabilitarOU() As Boolean
        Get
            Return _HabilitarOU
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOU = value
            MyBase.CambioItem("HabilitarOU")
        End Set
    End Property

    Private _HabilitarOULiq As Boolean = False
    Public Property HabilitarOULiq() As Boolean
        Get
            Return _HabilitarOULiq
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOULiq = value
            MyBase.CambioItem("HabilitarOULiq")
        End Set
    End Property

    Private _HabilitarOUCumpl As Boolean = False
    Public Property HabilitarOUCumpl() As Boolean
        Get
            Return _HabilitarOUCumpl
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOUCumpl = value
            MyBase.CambioItem("HabilitarOUCumpl")
        End Set
    End Property

    Private _CompraOrd As Boolean = False
    Public Property CompraOrd() As Boolean
        Get
            Return _CompraOrd
        End Get
        Set(ByVal value As Boolean)
            _CompraOrd = value
            If _CompraOrd = True Then
                HabilitarOTcompra = True
            Else
                HabilitarOTcompra = False
                intCompraOrd = 0
            End If
            MyBase.CambioItem("CompraOrd")
        End Set
    End Property

    Private _VentaOrd As Boolean = False
    Public Property VentaOrd() As Boolean
        Get
            Return _VentaOrd
        End Get
        Set(ByVal value As Boolean)
            _VentaOrd = value
            If _VentaOrd = True Then
                HabilitarOTVenta = True
            Else
                HabilitarOTVenta = False
                intVentaOrd = 0
            End If
            MyBase.CambioItem("VentaOrd")
        End Set
    End Property

    Private _intCompraOrd As Integer = 0
    Public Property intCompraOrd() As Integer
        Get
            Return _intCompraOrd
        End Get
        Set(ByVal value As Integer)
            _intCompraOrd = value
            MyBase.CambioItem("intCompraOrd")
        End Set
    End Property

    Private _intVentaOrd As Integer = 0
    Public Property intVentaOrd() As Integer
        Get
            Return _intVentaOrd
        End Get
        Set(ByVal value As Integer)
            _intVentaOrd = value
            MyBase.CambioItem("intVentaOrd")
        End Set
    End Property


    Private _HabilitarOT As Boolean = False
    Public Property HabilitarOT() As Boolean
        Get
            Return _HabilitarOT
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOT = value
            MyBase.CambioItem("HabilitarOT")
        End Set
    End Property

    Private _AcciAgrup As Boolean = False
    Public Property AcciAgrup() As Boolean
        Get
            Return _AcciAgrup
        End Get
        Set(ByVal value As Boolean)
            _AcciAgrup = value
            If _AcciAgrup = True Then
                HabilitarOCAcci = True
            Else
                HabilitarOCAcci = False
                intAcciAgrup = 0
            End If
            MyBase.CambioItem("AcciAgrup")
        End Set
    End Property


    Private _intAcciAgrup As Integer = 0
    Public Property intAcciAgrup() As Integer
        Get
            Return _intAcciAgrup
        End Get
        Set(ByVal value As Integer)
            _intAcciAgrup = value
            MyBase.CambioItem("intAcciAgrup")
        End Set
    End Property

    Private _SimultaneaAgrup As Boolean = False
    Public Property SimultaneaAgrup() As Boolean
        Get
            Return _SimultaneaAgrup
        End Get
        Set(ByVal value As Boolean)
            _SimultaneaAgrup = value
            If _SimultaneaAgrup = True Then
                HabilitarOCSimul = True
            Else
                HabilitarOCSimul = False
                intSimulAgrup = 0
            End If
            MyBase.CambioItem("SimultaneaAgrup")
        End Set
    End Property


    Private _intSimulAgrup As Integer = 0
    Public Property intSimulAgrup() As Integer
        Get
            Return _intSimulAgrup
        End Get
        Set(ByVal value As Integer)
            _intSimulAgrup = value
            MyBase.CambioItem("intSimulAgrup")
        End Set
    End Property

    Private _HabilitarOTVenta As Boolean = False
    Public Property HabilitarOTVenta() As Boolean
        Get
            Return _HabilitarOTVenta
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOTVenta = value
            MyBase.CambioItem("HabilitarOTVenta")
        End Set
    End Property

    Private _RFAgrup As Boolean = False
    Public Property RFAgrup() As Boolean
        Get
            Return _RFAgrup
        End Get
        Set(ByVal value As Boolean)
            _RFAgrup = value
            If _RFAgrup = True Then
                HabilitarOCRF = True
            Else
                HabilitarOCRF = False
                intRFAgrup = 0
            End If
            MyBase.CambioItem("RFAgrup")
        End Set
    End Property

    Private _intRFAgrup As Integer = 0
    Public Property intRFAgrup() As Integer
        Get
            Return _intRFAgrup
        End Get
        Set(ByVal value As Integer)
            _intRFAgrup = value
            MyBase.CambioItem("intRFAgrup")
        End Set
    End Property


    Private _ReposAgrup As Boolean = False
    Public Property ReposAgrup() As Boolean
        Get
            Return _ReposAgrup
        End Get
        Set(ByVal value As Boolean)
            _ReposAgrup = value
            If _ReposAgrup = True Then
                HabilitarOCRepos = True
            Else
                HabilitarOCRepos = False
                intReposAgrup = 0
            End If
            MyBase.CambioItem("RFAgrup")
        End Set
    End Property

    Private _intReposAgrup As Integer = 0
    Public Property intReposAgrup() As Integer
        Get
            Return _intReposAgrup
        End Get
        Set(ByVal value As Integer)
            _intReposAgrup = value
            MyBase.CambioItem("intReposAgrup")
        End Set
    End Property

    Private _HabilitarOTcompra As Boolean = False
    Public Property HabilitarOTcompra() As Boolean
        Get
            Return _HabilitarOTcompra
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOTcompra = value
            MyBase.CambioItem("HabilitarOTcompra")
        End Set
    End Property

    Private _HabilitarOC As Boolean = False
    Public Property HabilitarOC() As Boolean
        Get
            Return _HabilitarOC
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOC = value
            MyBase.CambioItem("HabilitarOC")
        End Set
    End Property

    Private _HabilitarOCAcci As Boolean = False
    Public Property HabilitarOCAcci() As Boolean
        Get
            Return _HabilitarOCAcci
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOCAcci = value
            MyBase.CambioItem("HabilitarOCAcci")
        End Set
    End Property

    Private _HabilitarOCSimul As Boolean = False
    Public Property HabilitarOCSimul() As Boolean
        Get
            Return _HabilitarOCSimul
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOCSimul = value
            MyBase.CambioItem("HabilitarOCSimul")
        End Set
    End Property

    Private _HabilitarOCRF As Boolean = False
    Public Property HabilitarOCRF() As Boolean
        Get
            Return _HabilitarOCRF
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOCRF = value
            MyBase.CambioItem("HabilitarOCRF")
        End Set
    End Property

    Private _HabilitarOCRepos As Boolean = False
    Public Property HabilitarOCRepos() As Boolean
        Get
            Return _HabilitarOCRepos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOCRepos = value
            MyBase.CambioItem("HabilitarOCRepos")
        End Set
    End Property



#End Region

#Region "Metodos"

    Public Overrides Sub Buscar()
        A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        'MessageBox.Show("Esta funcionalidad no está disponible para este maestro.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        'MyBase.Buscar()
    End Sub


    Private Function Validaciones() As Boolean
        Try
            Validaciones = True
            If ConfiguracionSelected.UsarFechade And ConfiguracionSelected.FechaCumplimiento = False And ConfiguracionSelected.FechaLiquidacion = False Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar fecha de cumplimiento o fecha de liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Validaciones = True
                Exit Function
            End If

            If (ConfiguracionSelected.FechaLiquidacion_ORD = True And ConfiguracionSelected.intFechaLiquidacion = 0) Or (ConfiguracionSelected.FechaCumplimiento_ORD = True And ConfiguracionSelected.intFechaCumplimiento = 0) Or (ConfiguracionSelected.VentaOrd = True And ConfiguracionSelected.intTipoOperacion = 0) Or (ConfiguracionSelected.CompraOrd = True And ConfiguracionSelected.TipoOperacionC = 0) Or (ConfiguracionSelected.AccionesORD = True And ConfiguracionSelected.intClaseOperacion = 0) Or (ConfiguracionSelected.RentafijaORD = True And ConfiguracionSelected.NroOrdenRF = 0) Or (ConfiguracionSelected.SimultaneasORD = True And ConfiguracionSelected.NroOrdenSimultaneas = 0) Or (ConfiguracionSelected.reposORD = True And ConfiguracionSelected.NroOrdenrepos = 0) Then
                A2Utilidades.Mensajes.mostrarMensaje("Los numeros deben ser mayor a 0", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Validaciones = True
                Exit Function
            End If

            If ConfiguracionSelected.UsarFechade_ORD And ConfiguracionSelected.FechaCumplimiento_ORD = False And ConfiguracionSelected.FechaLiquidacion_ORD = False Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar fecha de cumplimiento o fecha de liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Validaciones = True
                Exit Function
            End If

            If ConfiguracionSelected.intFechaLiquidacion <> 0 Or Not IsNothing(ConfiguracionSelected.intFechaLiquidacion) Or
               ConfiguracionSelected.intFechaCumplimiento <> 0 Or Not IsNothing(ConfiguracionSelected.intFechaCumplimiento) Or
               ConfiguracionSelected.intTipoOperacion <> 0 Or Not IsNothing(ConfiguracionSelected.intTipoOperacion) Or
               ConfiguracionSelected.TipoOperacionC <> 0 Or Not IsNothing(ConfiguracionSelected.TipoOperacionC) Or
               ConfiguracionSelected.intClaseOperacion <> 0 Or Not IsNothing(ConfiguracionSelected.intClaseOperacion) Or
               ConfiguracionSelected.NroOrdenRF <> 0 Or Not IsNothing(ConfiguracionSelected.NroOrdenRF) Or
               ConfiguracionSelected.NroOrdenSimultaneas <> 0 Or Not IsNothing(ConfiguracionSelected.NroOrdenSimultaneas) Or
               ConfiguracionSelected.NroOrdenrepos <> 0 Or Not IsNothing(ConfiguracionSelected.NroOrdenrepos) Then

                If (ConfiguracionSelected.intFechaLiquidacion = ConfiguracionSelected.intFechaCumplimiento) Then
                    If ConfiguracionSelected.intFechaLiquidacion = 0 And ConfiguracionSelected.intFechaCumplimiento = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intFechaLiquidacion = ConfiguracionSelected.intTipoOperacion) Then
                    If ConfiguracionSelected.intFechaLiquidacion = 0 And ConfiguracionSelected.intTipoOperacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intFechaLiquidacion = ConfiguracionSelected.TipoOperacionC) Then
                    If ConfiguracionSelected.intFechaLiquidacion = 0 And ConfiguracionSelected.TipoOperacionC = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intFechaLiquidacion = ConfiguracionSelected.intClaseOperacion) Then
                    If ConfiguracionSelected.intFechaLiquidacion = 0 And ConfiguracionSelected.intClaseOperacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intFechaCumplimiento = ConfiguracionSelected.intFechaLiquidacion) Then
                    If ConfiguracionSelected.intFechaCumplimiento = 0 And ConfiguracionSelected.intFechaLiquidacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intFechaCumplimiento = ConfiguracionSelected.intTipoOperacion) Then
                    If ((ConfiguracionSelected.intFechaCumplimiento = 0 Or IsNothing(ConfiguracionSelected.intFechaCumplimiento)) And ConfiguracionSelected.intTipoOperacion = 0 Or IsNothing(ConfiguracionSelected.intTipoOperacion)) Then

                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intFechaCumplimiento = ConfiguracionSelected.TipoOperacionC) Then
                    If ConfiguracionSelected.intFechaCumplimiento = 0 And ConfiguracionSelected.TipoOperacionC = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intTipoOperacion = ConfiguracionSelected.intFechaCumplimiento) Then
                    If ConfiguracionSelected.intTipoOperacion = 0 And ConfiguracionSelected.intFechaCumplimiento = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intTipoOperacion = ConfiguracionSelected.intFechaLiquidacion) Then
                    If ConfiguracionSelected.intTipoOperacion = 0 And ConfiguracionSelected.intFechaLiquidacion = 0 Then

                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intTipoOperacion = ConfiguracionSelected.intClaseOperacion) Then
                    If ConfiguracionSelected.intTipoOperacion = 0 And ConfiguracionSelected.intClaseOperacion = 0 Then

                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intTipoOperacion = ConfiguracionSelected.TipoOperacionC) Then
                    If ConfiguracionSelected.intTipoOperacion = 0 And ConfiguracionSelected.TipoOperacionC = 0 Then

                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intClaseOperacion = ConfiguracionSelected.intTipoOperacion) Then
                    If ConfiguracionSelected.intClaseOperacion = 0 And ConfiguracionSelected.intTipoOperacion = 0 Then

                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intClaseOperacion = ConfiguracionSelected.intFechaLiquidacion) Then
                    If ConfiguracionSelected.intClaseOperacion = 0 And ConfiguracionSelected.intFechaLiquidacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intClaseOperacion = ConfiguracionSelected.intFechaCumplimiento) Then
                    If ConfiguracionSelected.intClaseOperacion = 0 And ConfiguracionSelected.intFechaCumplimiento = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenRF = ConfiguracionSelected.intFechaCumplimiento) Then
                    If ConfiguracionSelected.NroOrdenRF = 0 And ConfiguracionSelected.intFechaCumplimiento = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenRF = ConfiguracionSelected.intFechaLiquidacion) Then
                    If ConfiguracionSelected.NroOrdenRF = 0 And ConfiguracionSelected.intFechaLiquidacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenSimultaneas = ConfiguracionSelected.intFechaCumplimiento) Then
                    If ConfiguracionSelected.NroOrdenSimultaneas = 0 And ConfiguracionSelected.intFechaCumplimiento = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenSimultaneas = ConfiguracionSelected.intFechaLiquidacion) Then
                    If ConfiguracionSelected.NroOrdenSimultaneas = 0 And ConfiguracionSelected.intFechaLiquidacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenrepos = ConfiguracionSelected.intFechaCumplimiento) Then
                    If ConfiguracionSelected.NroOrdenrepos = 0 And ConfiguracionSelected.intFechaCumplimiento = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenrepos = ConfiguracionSelected.intFechaLiquidacion) Then
                    If ConfiguracionSelected.NroOrdenrepos = 0 And ConfiguracionSelected.intFechaLiquidacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intClaseOperacion = ConfiguracionSelected.NroOrdenRF) Then
                    If ConfiguracionSelected.intClaseOperacion = 0 And ConfiguracionSelected.NroOrdenRF = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intClaseOperacion = ConfiguracionSelected.NroOrdenSimultaneas) Then
                    If ConfiguracionSelected.intClaseOperacion = 0 And ConfiguracionSelected.NroOrdenSimultaneas = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intClaseOperacion = ConfiguracionSelected.NroOrdenrepos) Then
                    If ConfiguracionSelected.intClaseOperacion = 0 And ConfiguracionSelected.NroOrdenrepos = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenRF = ConfiguracionSelected.intClaseOperacion) Then
                    If ConfiguracionSelected.NroOrdenRF = 0 And ConfiguracionSelected.intClaseOperacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenRF = ConfiguracionSelected.NroOrdenSimultaneas) Then
                    If ConfiguracionSelected.NroOrdenRF = 0 And ConfiguracionSelected.NroOrdenSimultaneas = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenRF = ConfiguracionSelected.NroOrdenrepos) Then
                    If ConfiguracionSelected.NroOrdenRF = 0 And ConfiguracionSelected.NroOrdenrepos = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenSimultaneas = ConfiguracionSelected.intClaseOperacion) Then
                    If ConfiguracionSelected.NroOrdenSimultaneas = 0 And ConfiguracionSelected.intClaseOperacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenSimultaneas = ConfiguracionSelected.NroOrdenRF) Then
                    If ConfiguracionSelected.NroOrdenSimultaneas = 0 And ConfiguracionSelected.NroOrdenRF = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenSimultaneas = ConfiguracionSelected.NroOrdenrepos) Then
                    If ConfiguracionSelected.NroOrdenSimultaneas = 0 And ConfiguracionSelected.NroOrdenrepos = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenrepos = ConfiguracionSelected.intClaseOperacion) Then
                    If ConfiguracionSelected.NroOrdenrepos = 0 And ConfiguracionSelected.intClaseOperacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenrepos = ConfiguracionSelected.NroOrdenRF) Then
                    If ConfiguracionSelected.NroOrdenrepos = 0 And ConfiguracionSelected.NroOrdenRF = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.NroOrdenrepos = ConfiguracionSelected.NroOrdenSimultaneas) Then
                    If ConfiguracionSelected.NroOrdenrepos = 0 And ConfiguracionSelected.NroOrdenSimultaneas = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intTipoOperacion = ConfiguracionSelected.NroOrdenRF) Then
                    If ConfiguracionSelected.intTipoOperacion = 0 And ConfiguracionSelected.NroOrdenRF = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intTipoOperacion = ConfiguracionSelected.NroOrdenSimultaneas) Then
                    If ConfiguracionSelected.intTipoOperacion = 0 And ConfiguracionSelected.NroOrdenSimultaneas = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intTipoOperacion = ConfiguracionSelected.intClaseOperacion) Then
                    If ConfiguracionSelected.intTipoOperacion = 0 And ConfiguracionSelected.intClaseOperacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.intTipoOperacion = ConfiguracionSelected.NroOrdenrepos) Then
                    If ConfiguracionSelected.intTipoOperacion = 0 And ConfiguracionSelected.NroOrdenrepos = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.TipoOperacionC = ConfiguracionSelected.intClaseOperacion) Then
                    If ConfiguracionSelected.TipoOperacionC = 0 And ConfiguracionSelected.intClaseOperacion = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.TipoOperacionC = ConfiguracionSelected.NroOrdenRF) Then
                    If ConfiguracionSelected.TipoOperacionC = 0 And ConfiguracionSelected.NroOrdenRF = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.TipoOperacionC = ConfiguracionSelected.NroOrdenSimultaneas) Then
                    If ConfiguracionSelected.TipoOperacionC = 0 And ConfiguracionSelected.NroOrdenSimultaneas = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If

                If (ConfiguracionSelected.TipoOperacionC = ConfiguracionSelected.NroOrdenrepos) Then
                    If ConfiguracionSelected.TipoOperacionC = 0 And ConfiguracionSelected.NroOrdenrepos = 0 Then
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Los campos de ordenamiento NO pueden quedar con el mismo número.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = True
                        Exit Function
                    End If
                End If
            End If

            'If (ConfiguracionSelected.FechaLiquidacion_ORD = False) And ConfiguracionSelected.intFechaLiquidacion <> 0 Or Not IsNothing(ConfiguracionSelected.intFechaLiquidacion) Then
            '    A2Utilidades.Mensajes.mostrarMensaje("No se puede poner el número de ordenamiento ya que no esta seleccionado la fecha de Liquidación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Validaciones = True
            '    Exit Function
            'End If

            'If (ConfiguracionSelected.FechaCumplimiento_ORD = False) And ConfiguracionSelected.intFechaCumplimiento <> 0 Or Not IsNothing(ConfiguracionSelected.intFechaCumplimiento) Then
            '    A2Utilidades.Mensajes.mostrarMensaje("No se puede poner el número de ordenamiento ya que no esta seleccionado la fecha de Cumplimiento", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Validaciones = True
            '    Exit Function
            'End If

            'If (ConfiguracionSelected.FechaCumplimiento_ORD = False) And ConfiguracionSelected.intFechaCumplimiento <> 0 Or Not IsNothing(ConfiguracionSelected.intFechaCumplimiento) Then
            '    A2Utilidades.Mensajes.mostrarMensaje("No se puede poner el número de ordenamiento ya que no esta seleccionado la fecha de Cumplimiento", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Validaciones = True
            '    Exit Function
            'End If

            'If (ConfiguracionSelected.VentaOrd = False) And ConfiguracionSelected.intTipoOperacion <> 0 Or Not IsNothing(ConfiguracionSelected.intTipoOperacion) Then
            '    A2Utilidades.Mensajes.mostrarMensaje("No se puede poner el número de ordenamiento ya que no esta seleccionado la opción de Venta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Validaciones = True
            '    Exit Function
            'End If

            'If (ConfiguracionSelected.CompraOrd = False) And ConfiguracionSelected.TipoOperacionC <> 0 Or Not IsNothing(ConfiguracionSelected.TipoOperacionC) Then
            '    A2Utilidades.Mensajes.mostrarMensaje("No se puede poner el número de ordenamiento ya que no esta seleccionado la opción de Compra", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Validaciones = True
            '    Exit Function
            'End If
            Validaciones = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición de un registro", _
                                                         Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            Validaciones = True
        End Try
    End Function
    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_ConfiguracionSelected) Then
                Editando = True
                If ConfiguracionSelected.UsarFechade Then
                    HabilitarAU = True
                Else
                    HabilitarAU = False
                End If

                If ConfiguracionSelected.TipoOperacion Then
                    HabilitarATVenta = False
                    HabilitarATCompra = False
                    HabTipoOperacionAgrup = True
                Else
                    HabTipoOperacionAgrup = True
                    HabilitarATVenta = True
                    HabilitarATCompra = True
                End If

                If ConfiguracionSelected.TipoOperacionVenta Then
                    HabTipoOperacionAgrup = False
                    HabilitarATCompra = False
                    HabilitarATVenta = True
                    'Else
                    '    HabTipoOperacionAgrup = True
                    '    HabilitarATCompra = True
                End If

                If ConfiguracionSelected.TipoOperacionCompra Then
                    HabTipoOperacionAgrup = False
                    HabilitarATVenta = False
                    HabilitarATCompra = True
                    'Else
                    '    HabTipoOperacionAgrup = True
                    '    HabilitarATVenta = True
                End If

                HabilitarAC = True

                If ConfiguracionSelected.UsarFechade_ORD Then
                    HabilitarOU = True
                Else
                    HabilitarOU = False

                End If

                If ConfiguracionSelected.FechaLiquidacion_ORD Then
                    HabilitarOULiq = True
                Else
                    HabilitarOULiq = False
                End If

                If ConfiguracionSelected.FechaCumplimiento_ORD Then
                    HabilitarOUCumpl = True
                Else
                    HabilitarOUCumpl = False
                End If
                HabilitarOT = True

                If ConfiguracionSelected.VentaOrd Then
                    HabilitarOTVenta = True

                Else
                    HabilitarOTVenta = False
                End If

                If ConfiguracionSelected.CompraOrd Then
                    HabilitarOTcompra = True
                Else
                    HabilitarOTcompra = False
                End If

                If ConfiguracionSelected.AccionesORD Then
                    HabilitarOCAcci = True
                Else
                    HabilitarOCAcci = False
                End If

                If ConfiguracionSelected.SimultaneasORD Then
                    HabilitarOCSimul = True
                Else
                    HabilitarOCSimul = False
                End If

                If ConfiguracionSelected.RentafijaORD Then
                    HabilitarOCRF = True
                Else
                    HabilitarOCRF = False
                End If

                If ConfiguracionSelected.reposORD Then
                    HabilitarOCRepos = True
                Else
                    HabilitarOCRepos = False
                End If
                HabilitarOC = True

                MyBase.CambioItem("Editando")
                ObtenerRegistroAnterior()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición de un registro", _
                                                         Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ConfiguracionSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                HabilitarAU = False
                HabTipoOperacionAgrup = False
                HabilitarATVenta = False
                HabilitarATCompra = False
                HabilitarAC = False
                HabilitarOU = False
                HabilitarOULiq = False
                HabilitarOUCumpl = False
                HabilitarOT = False
                HabilitarOTVenta = False
                HabilitarOTcompra = False
                HabilitarOC = False
                HabilitarOCAcci = False
                HabilitarOCSimul = False
                HabilitarOCRF = False
                HabilitarOCRepos = False
                ConfiguracionSelected = ConfiguracionAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la cancelacion de un registro", _
                                                         Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If Validaciones() = False Then
                Dim origen = "ActualizarRegistro"
                ErrorForma = ""
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                IsBusy = True
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objConfiguracion As New ConfiguracionFacturas
            If Not IsNothing(_ConfiguracionSelected) Then
                objConfiguracion.AccionesORD = _ConfiguracionSelected.AccionesORD
                objConfiguracion.ClaseOperacion = _ConfiguracionSelected.ClaseOperacion
                objConfiguracion.ClaseOperacion_ORD = _ConfiguracionSelected.ClaseOperacion_ORD
                objConfiguracion.ClaseOperacionAcciones = _ConfiguracionSelected.ClaseOperacionAcciones
                objConfiguracion.ClaseOperacionRentaFija = _ConfiguracionSelected.ClaseOperacionRentaFija
                objConfiguracion.ClaseOperacionRentaRepos = _ConfiguracionSelected.ClaseOperacionRentaRepos
                objConfiguracion.ClaseOperacionRentaSimultaneas = _ConfiguracionSelected.ClaseOperacionRentaSimultaneas
                objConfiguracion.CompraOrd = _ConfiguracionSelected.CompraOrd
                objConfiguracion.FechaCumplimiento = _ConfiguracionSelected.FechaCumplimiento
                objConfiguracion.FechaCumplimiento_ORD = _ConfiguracionSelected.FechaCumplimiento_ORD
                objConfiguracion.FechaLiquidacion = _ConfiguracionSelected.FechaLiquidacion
                objConfiguracion.FechaLiquidacion_ORD = _ConfiguracionSelected.FechaLiquidacion_ORD
                objConfiguracion.IDConfiguracionfacturas = _ConfiguracionSelected.IDConfiguracionfacturas
                objConfiguracion.InfoSesion = _ConfiguracionSelected.InfoSesion
                objConfiguracion.intClaseOperacion = _ConfiguracionSelected.intClaseOperacion
                objConfiguracion.intFechaCumplimiento = _ConfiguracionSelected.intFechaCumplimiento
                objConfiguracion.intFechaLiquidacion = _ConfiguracionSelected.intFechaLiquidacion
                objConfiguracion.intTipoOperacion = _ConfiguracionSelected.intTipoOperacion
                objConfiguracion.NroOrdenrepos = _ConfiguracionSelected.NroOrdenrepos
                objConfiguracion.NroOrdenRF = _ConfiguracionSelected.NroOrdenRF
                objConfiguracion.NroOrdenSimultaneas = _ConfiguracionSelected.NroOrdenSimultaneas
                objConfiguracion.RentafijaORD = _ConfiguracionSelected.RentafijaORD
                objConfiguracion.reposORD = _ConfiguracionSelected.reposORD
                objConfiguracion.SimultaneasORD = _ConfiguracionSelected.SimultaneasORD
                objConfiguracion.TipoOperacion = _ConfiguracionSelected.TipoOperacion
                objConfiguracion.TipoOperacion_ORD = _ConfiguracionSelected.TipoOperacion_ORD
                objConfiguracion.TipoOperacionC = _ConfiguracionSelected.TipoOperacionC
                objConfiguracion.TipoOperacionCompra = _ConfiguracionSelected.TipoOperacionCompra
                objConfiguracion.TipoOperacionVenta = _ConfiguracionSelected.TipoOperacionVenta
                objConfiguracion.UsarFechade = _ConfiguracionSelected.UsarFechade
                objConfiguracion.UsarFechade_ORD = _ConfiguracionSelected.UsarFechade_ORD
                objConfiguracion.VentaOrd = _ConfiguracionSelected.VentaOrd
            End If

            ConfiguracionAnterior = Nothing
            ConfiguracionAnterior = objConfiguracion
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la configuracion anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                Dim strMsg As String = String.Empty
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If (So.Error.Message.Contains("Errorpersonalizado,") = True) Then
                        Dim Mensaje1 = Split(So.Error.Message, "Errorpersonalizado,")
                        Dim Mensaje = Split(Mensaje1(1), vbCr)
                        A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                        Exit Sub
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                    End If
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)
            IdItemActualizar = 0
            Editando = False
            HabilitarAU = False
            HabTipoOperacionAgrup = False
            HabilitarATVenta = False
            HabilitarATCompra = False
            HabilitarAC = False
            HabilitarOU = False
            HabilitarOULiq = False
            HabilitarOUCumpl = False
            HabilitarOT = False
            HabilitarOTVenta = False
            HabilitarOTcompra = False
            HabilitarOC = False
            HabilitarOCAcci = False
            HabilitarOCSimul = False
            HabilitarOCRF = False
            HabilitarOCRepos = False

            If So.UserState = "ActualizarRegistro" Then
                If Not IsNothing(_ConfiguracionSelected) Then
                    IdItemActualizar = _ConfiguracionSelected.IDConfiguracionfacturas
                End If
            End If

            dcProxy.ConfiguracionFacturas.Clear()
            MyBase.QuitarFiltroDespuesGuardar()
            dcProxy.Load(dcProxy.ConfiguracionFacturasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfiguracionFacturas, "TerminoGuardar")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Private Sub __ConfiguracionSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ConfiguracionSelected.PropertyChanged
        If Editando And logCambiarPropiedades Then
            If e.PropertyName.Equals("UsarFechade") Then
                If _ConfiguracionSelected.UsarFechade Then
                    HabilitarAU = True
                Else
                    HabilitarAU = False
                    ConfiguracionSelected.FechaLiquidacion = False
                    ConfiguracionSelected.FechaCumplimiento = False
                End If
            End If
            If e.PropertyName.Equals("TipoOperacion") Then
                If _ConfiguracionSelected.TipoOperacion Then
                    HabilitarATVenta = False
                    HabilitarATCompra = False
                    ConfiguracionSelected.TipoOperacionVenta = False
                    ConfiguracionSelected.TipoOperacionCompra = False
                Else
                    HabilitarATVenta = True
                    HabilitarATCompra = True
                End If
            End If
            If e.PropertyName.Equals("TipoOperacionVenta") Then
                If _ConfiguracionSelected.TipoOperacionVenta Then
                    HabilitarATCompra = False
                    HabTipoOperacionAgrup = False
                    ConfiguracionSelected.TipoOperacion = False
                    ConfiguracionSelected.TipoOperacionCompra = False
                Else
                    HabilitarATCompra = True
                    HabTipoOperacionAgrup = True
                End If
            End If

            If e.PropertyName.Equals("TipoOperacionCompra") Then
                If _ConfiguracionSelected.TipoOperacionCompra Then
                    HabilitarATVenta = False
                    HabTipoOperacionAgrup = False
                    ConfiguracionSelected.TipoOperacion = False
                    ConfiguracionSelected.TipoOperacionVenta = False
                Else
                    HabilitarATVenta = True
                    HabTipoOperacionAgrup = True
                End If
            End If

            If e.PropertyName.Equals("UsarFechade_ORD") Then
                logCambiarPropiedades = False
                If _ConfiguracionSelected.UsarFechade_ORD Then
                    HabilitarOU = True
                    HabilitarOULiq = False
                    HabilitarOUCumpl = False
                    ConfiguracionSelected.FechaLiquidacion_ORD = False
                    ConfiguracionSelected.FechaCumplimiento_ORD = False
                Else
                    HabilitarOU = False
                    HabilitarOULiq = False
                    HabilitarOUCumpl = False
                    ConfiguracionSelected.FechaLiquidacion_ORD = False
                    ConfiguracionSelected.FechaCumplimiento_ORD = False
                    ConfiguracionSelected.intFechaCumplimiento = 0
                    ConfiguracionSelected.intFechaLiquidacion = 0
                End If
                logCambiarPropiedades = True
            End If

            If e.PropertyName.Equals("FechaLiquidacion_ORD") Then
                If _ConfiguracionSelected.FechaLiquidacion_ORD Then
                    HabilitarOULiq = True
                    HabilitarOUCumpl = False
                    ConfiguracionSelected.intFechaCumplimiento = 0
                Else
                    HabilitarOULiq = False
                    HabilitarOUCumpl = True
                    ConfiguracionSelected.intFechaLiquidacion = 0
                End If
            End If

            If e.PropertyName.Equals("FechaCumplimiento_ORD") Then
                If _ConfiguracionSelected.FechaCumplimiento_ORD Then
                    HabilitarOULiq = False
                    HabilitarOUCumpl = True
                    ConfiguracionSelected.intFechaLiquidacion = 0
                Else
                    HabilitarOULiq = True
                    HabilitarOUCumpl = False
                    ConfiguracionSelected.intFechaCumplimiento = 0
                End If
            End If

            If e.PropertyName.Equals("VentaOrd") Then
                If _ConfiguracionSelected.VentaOrd Then
                    HabilitarOTVenta = True
                Else
                    HabilitarOTVenta = False
                    ConfiguracionSelected.intTipoOperacion = 0
                End If
            End If

            If e.PropertyName.Equals("CompraOrd") Then
                If _ConfiguracionSelected.CompraOrd Then
                    HabilitarOTcompra = True
                Else
                    HabilitarOTcompra = False
                    ConfiguracionSelected.TipoOperacionC = 0
                End If
            End If

            If e.PropertyName.Equals("AccionesORD") Then
                If _ConfiguracionSelected.AccionesORD Then
                    HabilitarOCAcci = True
                Else
                    HabilitarOCAcci = False
                    ConfiguracionSelected.intClaseOperacion = 0
                End If
            End If

            If e.PropertyName.Equals("SimultaneasORD") Then
                If _ConfiguracionSelected.SimultaneasORD Then
                    HabilitarOCSimul = True
                Else
                    HabilitarOCSimul = False
                    ConfiguracionSelected.NroOrdenSimultaneas = 0
                End If
            End If

            If e.PropertyName.Equals("RentafijaORD") Then
                If _ConfiguracionSelected.RentafijaORD Then
                    HabilitarOCRF = True
                Else
                    HabilitarOCRF = False
                    ConfiguracionSelected.NroOrdenRF = 0
                End If
            End If


            If e.PropertyName.Equals("reposORD") Then
                If _ConfiguracionSelected.reposORD Then
                    HabilitarOCRepos = True
                Else
                    HabilitarOCRepos = False
                    ConfiguracionSelected.NroOrdenrepos = 0
                End If
            End If
        End If

    End Sub
#End Region
End Class
