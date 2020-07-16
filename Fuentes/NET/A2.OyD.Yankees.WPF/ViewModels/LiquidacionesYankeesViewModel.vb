Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: LiquidacionesViewModel.vb
'Generado el : 05/30/2011 09:18:58
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDYankees
Imports System.Threading.Tasks
Imports A2ComunesControl
Imports System.Globalization

Public Class LiquidacionesYankeesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private ListaLiquidacionesYankeesAnterior As tblLiquidaciones_YANKEE
    Public Property cb As New CamposBusquedaLiquidacioneYankees
    Dim objProxy1 As UtilidadesDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim dcProxy As YankeesDomainContext
    Dim dcProxy1 As YankeesDomainContext
    Dim fechaCierre As DateTime
    Dim intBasecalculoInteres As Integer
    Dim stralianza As String
    Dim intSecuencia As Integer
    Dim strBusquedaGenerico As String

    ''' <summary>
    ''' Manejo del PropertyChanged de ListaLiquidacionesYankeesSelected
    ''' </summary>
    ''' <remarks>SLB20130910</remarks>
    Dim sw As Integer = 0

    Public _mlogBuscarCliente As Boolean = True
    Public _mlogBuscarOrdenante As Boolean = True
    Public _mlogBuscarClienteExterno As Boolean = True
    Public _mlogBuscarEspecie As Boolean = True
    Public valortotal As Boolean

    ''' <summary>
    ''' Manejo al guardar
    ''' </summary>
    ''' <remarks></remarks>
    Dim disparafocus As Boolean = False
    Dim disparaguardar As Boolean = False

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New YankeesDomainContext()
            dcProxy1 = New YankeesDomainContext()
            objProxy1 = New UtilidadesDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New YankeesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New YankeesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy1 = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.LiquidacionesFiltrarYankeesQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesYankees, "FiltroInicial")
                ' dcProxy1.Load(dcProxy1.TraerLiquidacionePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesPorDefecto_Completed, "Default")
                objProxy1.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
                objProxy1.Verificaparametro("SOLO_ALIANZAVALORES_LIQUIDACIONESYANKEES", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "SOLO_ALIANZAVALORES_LIQUIDACIONESYANKEES")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "LiquidacionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Constantes"

    Private Enum Tabs As Byte
        Negocio
        Receptores
        Contraparte
    End Enum

#End Region

#Region "ResultadosAsincronicos"

    Private Sub TerminoTraerLiquidacionesYankees(ByVal lo As LoadOperation(Of tblLiquidaciones_YANKEE))
        If Not lo.HasError Then
            ListaLiquidacionesYankees = dcProxy.tblLiquidaciones_YANKEEs
            If dcProxy.tblLiquidaciones_YANKEEs.Count > 0 Then
                If lo.UserState = "insert" Then
                    ListaLiquidacionesYankeesSelected = ListaLiquidacionesYankees.Last
                End If
                MyBase.CambiarFormulario_Forma_Manual()
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroInicial" Or lo.UserState = "FiltroVM" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    visNavegando = "Collapsed"
                    MyBase.CambioItem("visNavegando")
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Liquidaciones Yankees", _
                                             Me.ToString(), "TerminoTraerLiquidacionesYankees", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Private Sub TerminotraerContraparte(ByVal lo As LoadOperation(Of tblDetalleLiquida_YANKEE))
        If Not lo.HasError Then
            ListaContraparte = dcProxy.tblDetalleLiquida_YANKEEs
            Total_Comision_Detalle()
            If dcProxy.tblDetalleLiquida_YANKEEs.Count > 0 Then
                'ListaLiquidacionesYankeesSelected = ListaLiquidacionesYankees.Last
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista Contraparte", _
                                             Me.ToString(), "TerminotraerContraparte", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Private Sub Terminotrearreceptores(ByVal lo As LoadOperation(Of ReceptoresYankees))
        If Not lo.HasError Then
            ListaReceptores = dcProxy.ReceptoresYankees
            If dcProxy.ReceptoresYankees.Count > 0 Then

                ReceptoresSelected = ListaReceptores.First

            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de receptores", _
                                             Me.ToString(), "Terminotrearreceptores", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Private Sub terminotraercalculodias(ByVal lo As InvokeOperation(Of String))
        Try
            If Not lo.HasError Then
                If lo.Value = String.Empty Then
                    intBasecalculoInteres = 1
                Else
                    intBasecalculoInteres = CInt(lo.Value)
                End If
                ListaLiquidacionesYankeesSelected.dblValorIntAcumulados = ListaLiquidacionesYankeesSelected.intDiasIntereses * ListaLiquidacionesYankeesSelected.dblValorNominal * ListaLiquidacionesYankeesSelected.dblTasaCupon / 100 / intBasecalculoInteres
                ListaLiquidacionesYankeesSelected.dblValorTotal = valortotalotramoneda()
                ListaLiquidacionesYankeesSelected.dblPrecioSucio = valorpreciosucio()
                ListaLiquidacionesYankeesSelected.dblAcumuladoInteres = Math.Abs(Porcentajeinteresacumulados)
                ListaLiquidacionesYankeesSelected.dblValorUSD = valortotalusd()
                ListaLiquidacionesYankeesSelected.dblValorPesos = valortotalpesos()

                If disparaguardar Then
                    Program.VerificarCambiosProxyServidor(dcProxy)
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "insert")
                End If
                disparafocus = False

            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los calculos", _
                                                 Me.ToString(), "terminotraercalculodias", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los calculos", _
                                                         Me.ToString(), "terminotraercalculodias", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se adiciona la función para el cálculo cuando se modifica el Valor Total.
    ''' </summary>
    ''' <remarks>SLB20130911</remarks>
    Public Sub txtTotal_LostFocus()
        ListaLiquidacionesYankeesSelected.dblPrecioSucio = valorpreciosucio()
        ListaLiquidacionesYankeesSelected.dblAcumuladoInteres = Math.Abs(Porcentajeinteresacumulados)
        ListaLiquidacionesYankeesSelected.dblValorUSD = valortotalusd()
        ListaLiquidacionesYankeesSelected.dblValorPesos = valortotalpesos()
    End Sub
    Public Sub dblTasaConversion_LostFocus()
        ListaLiquidacionesYankeesSelected.dblValorUSD = valortotalusd()
        For Each detallecontaparte In ListaContraparte
            detallecontaparte.dblTasaConversion = ListaLiquidacionesYankeesSelected.dblTasaConversion
        Next
    End Sub

    Public Sub dtmFechaUltPagoCupon_LostFocus()
        If ListaLiquidacionesYankeesSelected.dtmCumplimiento > ListaLiquidacionesYankeesSelected.dtmFechaUltPagoCupon Then
            ListaLiquidacionesYankeesSelected.intDiasIntereses = CType(Math.Abs(DateDiff("d", ListaLiquidacionesYankeesSelected.dtmCumplimiento, ListaLiquidacionesYankeesSelected.dtmFechaUltPagoCupon)), Integer)
        Else
            ListaLiquidacionesYankeesSelected.intDiasIntereses = Nothing
        End If
    End Sub

    Public Sub dblTasaSpot_LostFocus()
        ListaLiquidacionesYankeesSelected.dblValorPesos = valortotalpesos()
    End Sub

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            fechaCierre = obj.Value
        End If
    End Sub
    Private Sub Terminotraerespecieitem(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not IsNothing(ListaComboIsin) Then
                ListaComboIsin.Clear()
            End If
            If lo.Entities.ToList.Count > 0 Then
                For Each li In lo.Entities.ToList
                    ListaComboIsin.Add(New CamposComboIsin With {.ID = li.IdItem, .Descripcion = li.Descripcion})
                Next
                If Editando Then
                    If ListaComboIsin.Count = 1 Then
                        ListaLiquidacionesYankeesSelected.strIsin = ListaComboIsin(0).ID
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie", Me.ToString(), "Terminotraerespecieitem", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub Terminotraerisinitem(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If Editando Then
                    'Dim Format = "d"
                    'Dim provider As CultureInfo = CultureInfo.InvariantCulture
                    'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
                    'System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("en-US")
                    ListaLiquidacionesYankeesSelected.strModalidadTitulo = lo.Entities.ToList(0).IdItem
                    ListaLiquidacionesYankeesSelected.dblTasaCupon = lo.Entities.ToList(0).Nombre
                    If Not String.IsNullOrEmpty(lo.Entities.ToList(0).Descripcion) Then
                        'ListaLiquidacionesYankeesSelected.dtmVencimiento = Date.Parse(lo.Entities.ToList(0).Descripcion, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces) 'Date.ParseExact(lo.Entities.ToList(0).Descripcion, Format, provider)
                        ListaLiquidacionesYankeesSelected.dtmVencimiento = DateTime.ParseExact(lo.Entities.ToList(0).Descripcion, "yyyy-MM-dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None)
                    End If
                    If Not String.IsNullOrEmpty(lo.Entities.ToList(0).EtiquetaIdItem) Then
                        'ListaLiquidacionesYankeesSelected.dtmEmision = Date.Parse(lo.Entities.ToList(0).EtiquetaIdItem, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces) ' Date.ParseExact(lo.Entities.ToList(0).EtiquetaIdItem, Format, provider)
                        ListaLiquidacionesYankeesSelected.dtmEmision = DateTime.ParseExact(lo.Entities.ToList(0).EtiquetaIdItem, "yyyy-MM-dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie", Me.ToString(), "Terminotraerisinitem", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub Terminotraerparametro(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el parametro", Me.ToString(), "Terminotraerparametro", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            Select Case obj.UserState
                Case "SOLO_ALIANZAVALORES_LIQUIDACIONESYANKEES"
                    stralianza = obj.Value
            End Select
        End If
    End Sub
#End Region

#Region "Propiedades"

    Private _ListaLiquidacionesYankees As EntitySet(Of tblLiquidaciones_YANKEE)
    Public Property ListaLiquidacionesYankees() As EntitySet(Of tblLiquidaciones_YANKEE)
        Get
            Return _ListaLiquidacionesYankees
        End Get
        Set(ByVal value As EntitySet(Of tblLiquidaciones_YANKEE))
            _ListaLiquidacionesYankees = value

            MyBase.CambioItem("ListaLiquidacionesYankees")
            MyBase.CambioItem("ListaLiquidacionesYankeesPaged")
            If Not IsNothing(value) Then
                If IsNothing(ListaLiquidacionesYankeesAnterior) Then
                    ListaLiquidacionesYankeesSelected = _ListaLiquidacionesYankees.FirstOrDefault
                Else
                    ListaLiquidacionesYankeesSelected = ListaLiquidacionesYankeesAnterior
                End If
            End If
        End Set
    End Property
    Public ReadOnly Property ListaLiquidacionesYankeesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLiquidacionesYankees) Then
                Dim view = New PagedCollectionView(_ListaLiquidacionesYankees)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Dim loRC As LoadOperation(Of ReceptoresYankees)
    Dim loDY As LoadOperation(Of tblDetalleLiquida_YANKEE)

    Private WithEvents _ListaLiquidacionesYankeesSelected As tblLiquidaciones_YANKEE
    Public Property ListaLiquidacionesYankeesSelected() As tblLiquidaciones_YANKEE
        Get
            Return _ListaLiquidacionesYankeesSelected
        End Get
        Set(ByVal value As tblLiquidaciones_YANKEE)
            Try
                _ListaLiquidacionesYankeesSelected = value
                If Not IsNothing(value) Then
                    buscarisin()
                    If value.strTipoCliente = "E" Then
                        VisibilityClientes = Visibility.Collapsed
                        VisibilityClientesExterior = Visibility.Visible
                    Else
                        VisibilityClientes = Visibility.Visible
                        VisibilityClientesExterior = Visibility.Collapsed
                    End If

                    If Not IsNothing(loRC) Then
                        If Not loRC.IsComplete Then
                            loRC.Cancel()
                        End If
                    End If

                    If Not IsNothing(loDY) Then
                        If Not loDY.IsComplete Then
                            loDY.Cancel()
                        End If
                    End If

                    dcProxy.tblDetalleLiquida_YANKEEs.Clear()
                    loDY = dcProxy.Load(dcProxy.LiquidacionesConsultarYankeesContraparteQuery(value.IDY, Program.Usuario, Program.HashConexion), LoadBehavior.KeepCurrent, AddressOf TerminotraerContraparte, Nothing)

                    dcProxy.ReceptoresYankees.Clear()
                    loRC = dcProxy.Load(dcProxy.ReceptoresYankeesConsultarQuery(value.IDY, Program.Usuario, Program.HashConexion), LoadBehavior.MergeIntoCurrent, AddressOf Terminotrearreceptores, Nothing)

                End If
                MyBase.CambioItem("ListaLiquidacionesYankeesSelected")
            Catch ex As Exception
            End Try
        End Set
    End Property

    Private _ListaComboIsin As New ObservableCollection(Of CamposComboIsin)
    Public Property ListaComboIsin() As ObservableCollection(Of CamposComboIsin)
        Get
            Return _ListaComboIsin
        End Get
        Set(ByVal value As ObservableCollection(Of CamposComboIsin))
            _ListaComboIsin = value
            MyBase.CambioItem("ListaComboIsin")
        End Set
    End Property
    Private _VisibilityClientes As Visibility
    Public Property VisibilityClientes() As Visibility
        Get
            Return _VisibilityClientes
        End Get
        Set(ByVal value As Visibility)
            _VisibilityClientes = value
            MyBase.CambioItem("VisibilityClientes")
        End Set
    End Property
    Private _VisibilityClientesExterior As Visibility = Visibility.Collapsed
    Public Property VisibilityClientesExterior() As Visibility
        Get
            Return _VisibilityClientesExterior
        End Get
        Set(ByVal value As Visibility)
            _VisibilityClientesExterior = value
            MyBase.CambioItem("VisibilityClientesExterior")
        End Set
    End Property
    Private _ListaContraparte As EntitySet(Of tblDetalleLiquida_YANKEE)
    Public Property ListaContraparte() As EntitySet(Of tblDetalleLiquida_YANKEE)
        Get
            Return _ListaContraparte
        End Get
        Set(ByVal value As EntitySet(Of tblDetalleLiquida_YANKEE))
            _ListaContraparte = value
            MyBase.CambioItem("ListaContraparte")
        End Set
    End Property
    Private _ListaReceptores As EntitySet(Of ReceptoresYankees)
    Public Property ListaReceptores() As EntitySet(Of ReceptoresYankees)
        Get
            Return _ListaReceptores
        End Get
        Set(ByVal value As EntitySet(Of ReceptoresYankees))
            _ListaReceptores = value
            MyBase.CambioItem("ListaReceptores")
        End Set
    End Property
    Private WithEvents _ContraparteSelected As tblDetalleLiquida_YANKEE
    Public Property ContraparteSelected() As tblDetalleLiquida_YANKEE
        Get
            Return _ContraparteSelected
        End Get
        Set(ByVal value As tblDetalleLiquida_YANKEE)
            Try
                _ContraparteSelected = value
                MyBase.CambioItem("ContraparteSelected")
            Catch ex As Exception

            End Try
        End Set
    End Property
    Private WithEvents _ReceptoresSelected As ReceptoresYankees
    Public Property ReceptoresSelected() As ReceptoresYankees
        Get
            Return _ReceptoresSelected
        End Get
        Set(ByVal value As ReceptoresYankees)
            Try
                _ReceptoresSelected = value
                MyBase.CambioItem("ReceptoresSelected")
            Catch ex As Exception

            End Try
        End Set
    End Property
    Private _EditarItem As Boolean
    Public Property EditarItem As Boolean
        Get
            Return _EditarItem
        End Get
        Set(value As Boolean)
            _EditarItem = value
            MyBase.CambioItem("EditarItem")
        End Set
    End Property
    Private _Read As Boolean = True
    Public Property Read As Boolean
        Get
            Return _Read
        End Get
        Set(value As Boolean)
            _Read = value
            MyBase.CambioItem("Read")
        End Set
    End Property
    Private _IndexTab As Integer = 0
    Public Property IndexTab As Boolean
        Get
            Return _IndexTab
        End Get
        Set(value As Boolean)
            _IndexTab = value
            MyBase.CambioItem("IndexTab")
        End Set
    End Property
    Private _ListaLiquidacionesYankeesSelectedcl As New OyDYankees.tblLiquidaciones_YANKEE
    Public Property ListaLiquidacionesYankeesSelectedcl() As OyDYankees.tblLiquidaciones_YANKEE
        Get
            Return _ListaLiquidacionesYankeesSelectedcl
        End Get
        Set(ByVal value As OyDYankees.tblLiquidaciones_YANKEE)
            _ListaLiquidacionesYankeesSelectedcl = value
            MyBase.CambioItem("ListaLiquidacionesYankeesSelectedcl")
        End Set
    End Property

    Private _TotalComisiones As Double?
    Public Property TotalComisiones As Double?
        Get
            Return _TotalComisiones
        End Get
        Set(ByVal value As Double?)
            _TotalComisiones = value
            MyBase.CambioItem("TotalComisiones")
        End Set
    End Property

    Private _TabSeleccionado As Integer = 0
    ''' <summary>
    ''' Propiedad para controlar el tab activo del tab control que contiene los datos generales de la orden
    ''' </summary>
    Public Property TabSeleccionado As Integer
        Get
            Return _TabSeleccionado
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionado = value
            MyBase.CambioItem("TabSeleccionado")
        End Set
    End Property
#End Region

#Region "Metodos"

    Public Overrides Sub NuevoRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim NewLiquidacione As New tblLiquidaciones_YANKEE
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewLiquidacione.IDLiquidaciones_Yankees = -1
            NewLiquidacione.strUsuario = Program.Usuario
            NewLiquidacione.strTipoCliente = "O"
            NewLiquidacione.dblPrecioSucio = 0
            NewLiquidacione.dblTasaConversion = 0
            NewLiquidacione.dblTasaCupon = 0
            NewLiquidacione.dblPrecioLimpio = 0
            NewLiquidacione.dblValorIntAcumulados = 0
            NewLiquidacione.dblValorNominal = 0
            NewLiquidacione.dblAcumuladoInteres = 0
            NewLiquidacione.dblValorIntAcumulados = 0
            NewLiquidacione.dblValorUSD = 0
            NewLiquidacione.dblTasaSpot = 0
            NewLiquidacione.intDiasIntereses = 0
            NewLiquidacione.strTipoOperacion = "C"
            NewLiquidacione.strEstado = "P"
            NewLiquidacione.dtmEstado = Now.Date
            NewLiquidacione.dtmRegistro = Now.Date
            NewLiquidacione.dtmCumplimiento = Now.Date
            NewLiquidacione.dtmEmision = Nothing
            NewLiquidacione.dtmVencimiento = Nothing
            ListaLiquidacionesYankeesAnterior = ListaLiquidacionesYankeesSelected
            ListaLiquidacionesYankeesSelected = NewLiquidacione
            intSecuencia = 0
            MyBase.CambioItem("ListaLiquidacionesYankeesSelected")
            Editando = True
            EditarItem = True
            Read = False
            MyBase.CambioItem("Editando")
            If Not IsNothing(ListaComboIsin) Then
                ListaComboIsin.Clear()
            End If
            NombreColeccionDetalle = "cmContraparte"
            NuevoRegistroDetalle()

            If visNavegando = "Collapsed" Then
                MyBase.CambiarFormulario_Forma_Manual()
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.lngID <> 0 Or cb.IDComitente <> String.Empty Or cb.Tipo <> String.Empty Or Not IsNothing(cb.Fregistro) Or Not IsNothing(cb.CumplimientoTitulo) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.tblLiquidaciones_YANKEEs.Clear()
                ListaLiquidacionesYankeesAnterior = Nothing

                'Dim orden As String
                'If Not IsNumeric(cb.NumeroOrden1) And Not IsNothing(cb.NumeroOrden1) Then
                '    A2Utilidades.Mensajes.mostrarMensaje("La orden debe ser un valor númerico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    cb.NumeroOrden1 = Nothing
                '    Exit Sub
                'End If
                IsBusy = True
                'If cb.NumeroOrden1 Is Nothing Then
                '    orden = "0"
                'Else
                '    orden = CStr(cb.NumeroOrden) + Format(CInt(cb.NumeroOrden1), "000000")
                'End If
                'DescripcionFiltroVM = " ID = " & cb.ID.ToString() & " IDComitente = " & cb.IDComitente.ToString()
                'dcProxy.Load(dcProxy.TesoreriaConsultarQuery(Filtro, CamposBusquedaTesoreria.Tipo, CamposBusquedaTesoreria.NombreConsecutivo, CamposBusquedaTesoreria.IDDocumento, IIf(CamposBusquedaTesoreria.Documento.Year < 1900, Nothing, CamposBusquedaTesoreria.Documento), estadoMC,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "Busqueda")
                'dcProxy.Load(dcProxy.LiquidacionesConsultarQuery(cb.ID, cb.IDComitente, cb.Tipo, cb.ClaseOrden, IIf(cb.Liquidacion.Date.Year < 1800, "1799/01/01", cb.Liquidacion), IIf(cb.CumplimientoTitulo.Date.Year < 1800, "1799/01/01", cb.CumplimientoTitulo), CInt(orden), 0,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Busqueda")
                dcProxy.Load(dcProxy.LiquidacionesConsultarYankeesQuery(cb.lngID, cb.Fregistro, cb.CumplimientoTitulo, cb.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesYankees, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaLiquidacioneYankees
                CambioItem("cb")
                'Else
                '    ErrorForma = ""
                '    dcProxy.tblLiquidaciones_YANKEEs.Clear()
                '    ListaLiquidacionesYankeesAnterior = Nothing
                '    IsBusy = True
                '    'dcProxy.Load(dcProxy.LiquidacionesConsultarQuery(cb.ID, cb.IDComitente, cb.Tipo, cb.ClaseOrden, IIf(cb.Liquidacion.Date.Year < 1800, "1799/01/01", cb.Liquidacion), IIf(cb.CumplimientoTitulo.Date.Year < 1800, "1799/01/01", cb.CumplimientoTitulo), cb.NumeroOrden, cb.NumeroOrden,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Busqueda")
                '    dcProxy.Load(dcProxy.LiquidacionesConsultarYankeesQuery(cb.lngID, cb.Fregistro, cb.CumplimientoTitulo, cb.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesYankees, "Busqueda")
                '    MyBase.ConfirmarBuscar()
                '    cb = New CamposBusquedaLiquidacioneYankees
                '    CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Filtrar()
        Try
            'If Not validafiltro Then
            'selected = True
            dcProxy.tblLiquidaciones_YANKEEs.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.LiquidacionesFiltrarYankeesQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesYankees, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.LiquidacionesFiltrarYankeesQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesYankees, "FiltroVM")
            End If
            'End If
            ' validafiltro = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Metodo encargado de lanzar la edicion del formulario.
    ''' </summary>
    Public Overrides Sub EditarRegistro()

        If dcProxy.IsLoading Then
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If

        If Not IsNothing(_ListaLiquidacionesYankeesSelected) Then
            If ListaLiquidacionesYankeesSelected.strEstado = "A" Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("La Liquidación se encuentra anulada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            Else
                If ListaLiquidacionesYankeesSelected.lngIDFactura Is Nothing Or ListaLiquidacionesYankeesSelected.lngIDFactura = 0 Then
                    Editando = True
                    EditarItem = True
                    Read = False
                Else
                    Editando = True
                    EditarItem = False
                    Read = True
                End If
                If Not IsNothing(ListaLiquidacionesYankees) Then
                    intSecuencia = ListaContraparte.Count
                End If
            End If
        End If
    End Sub
    Public Overrides Sub CancelarEditarRegistro()
        Try
            'habilitaboton = True
            'habilitamanualsi = False
            'HabilitarComisionistaLocal = False
            'HabilitarComisionistaOtraPlaza = False
            'HabilitarFechaLiquidacion = False
            'HabilitarFechaVendido = False
            'HabilitarFechaTitulo = False
            'HabilitarFechaEfectivo = False
            'habilitaparciald = False
            'habilitavencimiento = False
            'Datostitulo = False
            'habilitaemision = False
            ErrorForma = ""
            If Not IsNothing(_ListaLiquidacionesYankeesSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditarItem = False
                Read = True
                If _ListaLiquidacionesYankeesSelected.EntityState = EntityState.Detached Then
                    ListaLiquidacionesYankeesSelected = ListaLiquidacionesYankeesAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaLiquidacionesYankeesSelected.strEstado = "A" Then
                A2Utilidades.Mensajes.mostrarMensaje("La liquidación ya se encuentra anulada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            ListaLiquidacionesYankeesSelected.strEstado = "A"
            ListaLiquidacionesYankeesSelected.strUsuario = Program.Usuario
            ListaLiquidacionesYankeesSelected.dtmEstado = Now.Date
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub buscarisin()
        objProxy1.BuscadorGenericos.Clear()
        objProxy1.Load(objProxy1.buscarItemEspecificoQuery("ESPECIEISIN", ListaLiquidacionesYankeesSelected.strIDEspecie, Program.Usuario, Program.HashConexion), AddressOf Terminotraerespecieitem, Nothing)
    End Sub
    Public Overrides Sub ActualizarRegistro()
        Try
            IsBusy = True
            If validaciones() Then
                If ValidarReceptores() Then
                    If ValidarContraparte() Then
                        If ValidarContraparte_OyD_Externo() Then
                            If Not ListaLiquidacionesYankees.Contains(ListaLiquidacionesYankeesSelected) Then
                                For Each receptor In ListaReceptores
                                    If receptor.Porcentaje > 0 Then
                                        receptor.Usuario = Program.Usuario
                                    End If
                                    ListaLiquidacionesYankeesSelected.ReceptoresYankees.Add(receptor)
                                Next
                                For Each contraparte In ListaContraparte
                                    ListaLiquidacionesYankeesSelected.tblDetalleLiquida_YANKEEs.Add(contraparte)
                                Next
                                ListaLiquidacionesYankees.Add(ListaLiquidacionesYankeesSelected)
                            End If

                            'IsBusy = True
                            'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "insert")

                            If Not disparafocus Then
                                Program.VerificarCambiosProxyServidor(dcProxy)
                                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "insert")
                            End If
                            disparaguardar = True
                        Else
                            IsBusy = False
                            TabSeleccionado = Tabs.Contraparte
                        End If
                    Else
                        IsBusy = False
                        TabSeleccionado = Tabs.Contraparte
                    End If
                Else
                    IsBusy = False
                    TabSeleccionado = Tabs.Receptores
                End If
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            disparaguardar = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                'Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)


                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                Dim strMsg As String = String.Empty
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                    Exit Sub
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            EditarItem = False
            Read = True
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            disparafocus = False
            disparaguardar = False
        End Try
    End Sub

    Public Function [IsNumeric](ByVal str As String) As Boolean
        Dim result As Decimal = 0
        Return Decimal.TryParse(str, result)
    End Function

    Public Sub traercalculosinteresespecies()
        disparafocus = True
        dcProxy.ConsultaCalculoEspecie(ListaLiquidacionesYankeesSelected.strIDEspecie, Program.Usuario, Program.HashConexion, AddressOf terminotraercalculodias, Nothing)
    End Sub
    Public Sub Valor_PrecioSucio_DetalleContraparte()
        'disparafocus = True
        'dcProxy.ConsultaCalculoEspecie(ListaLiquidacionesYankeesSelected.strIDEspecie,Program.Usuario, Program.HashConexion, AddressOf terminotraercalculodias, Nothing)
        For Each detallecontaparte In ListaContraparte
            detallecontaparte.dblValorTotal = Valor_TotalOtraMoneda_Detalle()
            detallecontaparte.dblPrecioSucio = detallecontaparte.dblValorTotal / detallecontaparte.dblValorNominal * 100
            detallecontaparte.dblValorUSD = Valor_TotalUSD_Detalle()
            detallecontaparte.dblValorPesos = Valor_TotalPesos_Detalle()
            detallecontaparte.dblComision = Valor_Comision_Detalle()
        Next
    End Sub

    Public Sub ValorNominal_DetalleContraparte()
        For Each detallecontraparte In ListaContraparte
            detallecontraparte.dblValorTotal = Valor_TotalOtraMoneda_Detalle()
            detallecontraparte.dblPrecioSucio = detallecontraparte.dblValorTotal / detallecontraparte.dblValorNominal * 100
            detallecontraparte.dblValorUSD = Valor_TotalUSD_Detalle()
            detallecontraparte.dblValorPesos = Valor_TotalPesos_Detalle()
            detallecontraparte.dblComision = Valor_Comision_Detalle()
        Next
    End Sub

    Public Sub TasaSpotTRM_DetalleContraparte()
        For Each detallecontraparte In ListaContraparte
            detallecontraparte.dblValorPesos = Valor_TotalPesos_Detalle()
            detallecontraparte.dblComision = Valor_Comision_Detalle()
        Next
    End Sub
    Public Sub SubVlrTraslComision_DetalleContraparte()
        For Each detallecontraparte In ListaContraparte
            detallecontraparte.dblComision = Valor_Comision_Detalle()
        Next
    End Sub
    Public Sub TasaConversion_DetalleContraparte()
        For Each detallecontraparte In ListaContraparte
            detallecontraparte.dblValorUSD = Valor_TotalUSD_Detalle()
        Next
    End Sub


    Public Function valortotalotramoneda() As Double
        If stralianza = "SI" Then
            valortotalotramoneda = (ListaLiquidacionesYankeesSelected.dblValorNominal * ListaLiquidacionesYankeesSelected.dblPrecioLimpio / 100) + ListaLiquidacionesYankeesSelected.dblValorIntAcumulados
        Else
            valortotalotramoneda = ListaLiquidacionesYankeesSelected.dblPrecioSucio * ListaLiquidacionesYankeesSelected.dblValorNominal / 100
        End If
    End Function

    Public Function Valor_TotalOtraMoneda_Detalle() As Double
        Dim dblRetorno As Double
        Dim Valor_Intereses_Acum_Detalle As Double
        For Each detallecontraparte In ListaContraparte
            If Not IsNothing(detallecontraparte.dblValorNominal) Then
                If Not IsNothing(ListaLiquidacionesYankeesSelected.dblTasaCupon) Then
                    If Not IsNothing(intBasecalculoInteres) Then
                        Valor_Intereses_Acum_Detalle = ListaLiquidacionesYankeesSelected.intDiasIntereses * detallecontraparte.dblValorNominal * ListaLiquidacionesYankeesSelected.dblTasaCupon / 100 / intBasecalculoInteres
                        dblRetorno = Valor_Intereses_Acum_Detalle
                        valortotal = False
                        Exit For
                    End If
                End If
            End If

            If Not IsNothing(detallecontraparte.dblPrecioLimpio) And detallecontraparte.dblPrecioLimpio <> 0 Then
                Valor_TotalOtraMoneda_Detalle = (detallecontraparte.dblValorNominal * detallecontraparte.dblPrecioLimpio / 100) + Valor_Intereses_Acum_Detalle
                dblRetorno = Valor_TotalOtraMoneda_Detalle
                valortotal = True
                'Exit For
            End If
        Next
        Return dblRetorno
    End Function

    Public Function Valor_TotalUSD_Detalle() As Double
        For Each detallecontraparte In ListaContraparte
            If Not IsNothing(ListaLiquidacionesYankeesSelected.dblTasaConversion) Then
                If Not IsNothing(detallecontraparte.dblValorTotal) Then
                    Valor_TotalUSD_Detalle = detallecontraparte.dblTasaConversion * detallecontraparte.dblValorTotal
                    Exit For
                End If
            End If
        Next
        Return Valor_TotalUSD_Detalle
    End Function

    Public Function Valor_TotalPesos_Detalle() As Double
        For Each detallecontraparte In ListaContraparte
            If Not IsNothing(detallecontraparte.dblTasaSpot) Then
                If Not IsNothing(detallecontraparte.dblValorUSD) Then
                    Valor_TotalPesos_Detalle = detallecontraparte.dblValorUSD * detallecontraparte.dblTasaSpot
                    Exit For
                End If
            End If
        Next
        Return Valor_TotalPesos_Detalle
    End Function

    Public Function Valor_Comision_Detalle() As Double
        Dim dblPrecioSucio As Double
        For Each detallecontraparte In ListaContraparte
            If Not IsNothing(detallecontraparte.dblValorNominal) Then
                dblPrecioSucio = detallecontraparte.dblPrecioSucio
            End If
            Valor_Comision_Detalle = ListaLiquidacionesYankeesSelected.dblPrecioSucio - dblPrecioSucio
            Valor_Comision_Detalle = Math.Abs(Valor_Comision_Detalle) / 100
            If Not IsNothing(detallecontraparte.dblValorNominal) Then
                Valor_Comision_Detalle = Valor_Comision_Detalle * detallecontraparte.dblValorNominal
            End If

            If Valor_Comision_Detalle > 0 Then
                Valor_Comision_Detalle = Valor_Comision_Detalle - detallecontraparte.dblTrasladoComision
            End If
        Next
        Return Valor_Comision_Detalle
    End Function

    Public Function valorpreciosucio() As Double
        If stralianza = "SI" Then
            If ListaLiquidacionesYankeesSelected.dblValorNominal <> 0 Then
                valorpreciosucio = ListaLiquidacionesYankeesSelected.dblValorTotal / ListaLiquidacionesYankeesSelected.dblValorNominal * 100
            Else
                Return Nothing
                Exit Function
            End If
        Else
            valorpreciosucio = ListaLiquidacionesYankeesSelected.dblPrecioLimpio + ListaLiquidacionesYankeesSelected.dblAcumuladoInteres
        End If
    End Function
    Public Function Porcentajeinteresacumulados() As Double
        Porcentajeinteresacumulados = ListaLiquidacionesYankeesSelected.dblPrecioSucio - ListaLiquidacionesYankeesSelected.dblPrecioLimpio
    End Function
    Public Function valortotalusd() As Double
        valortotalusd = ListaLiquidacionesYankeesSelected.dblTasaConversion * ListaLiquidacionesYankeesSelected.dblValorTotal
    End Function
    Public Function valortotalpesos() As Double
        valortotalpesos = ListaLiquidacionesYankeesSelected.dblValorUSD * ListaLiquidacionesYankeesSelected.dblTasaSpot
    End Function
    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptores"
                Dim NewReceptores As New ReceptoresYankees
                NewReceptores.IDY = ListaLiquidacionesYankeesSelected.IDY
                NewReceptores.Usuario = Program.Usuario
                ListaReceptores.Add(NewReceptores)
                ReceptoresSelected = NewReceptores
                MyBase.CambioItem("ReceptoresSelected")
                MyBase.CambioItem("ListaReceptores")
            Case "cmContraparte"
                Dim NewContraparte As New tblDetalleLiquida_YANKEE
                NewContraparte.IDY = ListaLiquidacionesYankeesSelected.IDY
                NewContraparte.strUsuario = Program.Usuario
                NewContraparte.logTrasladoComision = False
                NewContraparte.lngIDSecuencia = intSecuencia + 1
                intSecuencia = intSecuencia + 1
                If ListaLiquidacionesYankeesSelected.strTipoOperacion = "C" Then
                    NewContraparte.strTipoOperacion = "V"
                Else
                    NewContraparte.strTipoOperacion = "C"
                End If
                If ListaLiquidacionesYankeesSelected.strTipoCliente = "O" Then
                    NewContraparte.TipoClienteOyD = True
                    NewContraparte.strTipoCliente = "O"
                Else
                    NewContraparte.TipoClienteExterno = True
                    NewContraparte.strTipoCliente = "E"
                End If

                If IsNothing(_ListaContraparte) Then
                    ListaContraparte = dcProxy.tblDetalleLiquida_YANKEEs
                End If

                ListaContraparte.Add(NewContraparte)
                ContraparteSelected = NewContraparte
                MyBase.CambioItem("ContraparteSelected")
                MyBase.CambioItem("ListaContraparte")
        End Select
    End Sub
    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptores"
                If Not IsNothing(ListaReceptores) Then
                    If Not IsNothing(ListaReceptores) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptoresSelected, ListaReceptores.ToList)

                        ListaReceptores.Remove(_ReceptoresSelected)
                        If ListaReceptores.Count > 0 Then
                            Program.PosicionarItemLista(ReceptoresSelected, ListaReceptores.ToList, intRegistroPosicionar)
                        Else
                            ReceptoresSelected = Nothing
                        End If
                        MyBase.CambioItem("ReceptoresSelected")
                        MyBase.CambioItem("ListaReceptores")
                    End If
                End If
            Case "cmContraparte"
                If Not IsNothing(ListaContraparte) Then
                    If Not IsNothing(ListaContraparte) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ContraparteSelected, ListaContraparte.ToList)

                        ListaContraparte.Remove(_ContraparteSelected)
                        If ListaContraparte.Count > 0 Then
                            Program.PosicionarItemLista(ContraparteSelected, ListaContraparte.ToList, intRegistroPosicionar)
                        Else
                            ContraparteSelected = Nothing
                        End If
                        MyBase.CambioItem("ContraparteSelected")
                        MyBase.CambioItem("ListaContraparte")
                    End If
                End If
        End Select
    End Sub
    Private Function validaciones() As Boolean
        Dim a As Boolean
        a = True
        If ListaLiquidacionesYankeesSelected.strTipoCliente = "O" Then
            If ListaLiquidacionesYankeesSelected.lngIDComitente = String.Empty Or ListaLiquidacionesYankeesSelected.lngIDComitente Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe Ingresar el código del comitente ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                a = False
                Return a
            End If
        Else
            If ListaLiquidacionesYankeesSelected.lngIDClienteExterno = String.Empty Or ListaLiquidacionesYankeesSelected.lngIDClienteExterno Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe Ingresar el código del cliente externo ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                a = False
                Return a
            End If
        End If

        If ListaLiquidacionesYankeesSelected.strIDEspecie = String.Empty Or ListaLiquidacionesYankeesSelected.strIDEspecie Is Nothing Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe Ingresar el nemotécnico de la especie ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If ListaLiquidacionesYankeesSelected.strIsin = String.Empty Or ListaLiquidacionesYankeesSelected.strIsin Is Nothing Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe Ingresar el Isin de la especie ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If ListaLiquidacionesYankeesSelected.dtmEmision Is Nothing Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la fecha de emisión del titulo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If ListaLiquidacionesYankeesSelected.dtmVencimiento Is Nothing Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la fecha de vencimiento del titulo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If (ListaLiquidacionesYankeesSelected.dtmVencimiento < ListaLiquidacionesYankeesSelected.dtmEmision) Or (ListaLiquidacionesYankeesSelected.dtmVencimiento = ListaLiquidacionesYankeesSelected.dtmEmision) Then
            A2Utilidades.Mensajes.mostrarMensaje("La Fecha de vencimiento debe ser mayor que la de emisión ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If ListaLiquidacionesYankeesSelected.strModalidadTitulo = String.Empty Or ListaLiquidacionesYankeesSelected.strModalidadTitulo Is Nothing Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la modalidad del titulo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If ListaLiquidacionesYankeesSelected.dblPrecioLimpio Is Nothing Or ListaLiquidacionesYankeesSelected.dblPrecioLimpio = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el precio limpio", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            TabSeleccionado = Tabs.Negocio
            a = False
            Return a
        ElseIf ListaLiquidacionesYankeesSelected.dblPrecioLimpio > 200 Or ListaLiquidacionesYankeesSelected.dblPrecioLimpio <= 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("El precio limpio debe estar entre 0 y 200", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            TabSeleccionado = Tabs.Negocio
            a = False
            Return a
        End If
        If ListaLiquidacionesYankeesSelected.dblTasaCupon Is Nothing Or ListaLiquidacionesYankeesSelected.dblTasaCupon = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la tasa cupón", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            TabSeleccionado = Tabs.Negocio
            a = False
            Return a
        ElseIf ListaLiquidacionesYankeesSelected.dblTasaCupon > 100.0 Or ListaLiquidacionesYankeesSelected.dblTasaCupon < 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("La tasa cupón debe estar entre 0 y 100", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            TabSeleccionado = Tabs.Negocio
            a = False
            Return a
        End If
        If ListaLiquidacionesYankeesSelected.dblTasaConversion Is Nothing Or ListaLiquidacionesYankeesSelected.dblTasaConversion = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la tasa de conversión", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            TabSeleccionado = Tabs.Negocio
            a = False
            Return a
        End If
        If ListaLiquidacionesYankeesSelected.dtmFechaUltPagoCupon Is Nothing Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la fecha de último pago de cupón", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            TabSeleccionado = Tabs.Negocio
            a = False
            Return a
        End If
        If ListaLiquidacionesYankeesSelected.dblValorNominal Is Nothing Or ListaLiquidacionesYankeesSelected.dblValorNominal = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el valor nominal", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            TabSeleccionado = Tabs.Negocio
            a = False
            Return a
        End If
        If ListaLiquidacionesYankeesSelected.dblAcumuladoInteres Is Nothing Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el Acumulado Intereses", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            TabSeleccionado = Tabs.Negocio
            a = False
            Return a
        ElseIf ListaLiquidacionesYankeesSelected.dblAcumuladoInteres > ListaLiquidacionesYankeesSelected.dblTasaCupon Or ListaLiquidacionesYankeesSelected.dblValorIntAcumulados < 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("El Acumulado Intereses debe estar entre 0 y la tasa cupón", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            TabSeleccionado = Tabs.Negocio
            a = False
            Return a
        End If
        If IsNothing(ListaLiquidacionesYankeesSelected.dblTasaSpot) Or ListaLiquidacionesYankeesSelected.dblTasaSpot = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un valor diferente de cero en la tasa spot", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            TabSeleccionado = Tabs.Negocio
            a = False
            Return a
        End If
        Return a
    End Function

    Public Function ValidarReceptores() As Boolean
        Dim retorno As Boolean = False
        Try
            Dim intexisteLider As Integer
            Dim intsumaPorcentaje As Integer

            If ListaReceptores.Count <> 0 Then
                For Each detalleReceptor In ListaReceptores
                    'If IsNothing(detalleReceptor.Porcentaje) Or detalleReceptor.Porcentaje = 0 Then
                    '    Throw New ValidationException("El porcentaje del receptor no puede ser cero.")
                    'End If
                    If detalleReceptor.Lider Then
                        intexisteLider = intexisteLider + 1
                    End If
                    If IsNothing(detalleReceptor.IDReceptor) Then
                        Throw New ValidationException("El código de receptor no puede ser vacío.")
                    End If
                    If IsNothing(detalleReceptor.Nombre) Then
                        Throw New ValidationException("El nombre del receptor no puede ser vacío, este se establece seleccionando el código del receptor.")
                    End If
                    intsumaPorcentaje = intsumaPorcentaje + detalleReceptor.Porcentaje
                    Dim idreceptor = detalleReceptor.IDReceptor
                    Dim receptores = ListaReceptores.Where(Function(li) IIf(IsNothing(li.IDReceptor), String.Empty, li.IDReceptor).Equals(idreceptor)).ToList
                    If receptores.Count = 2 Then
                        Throw New ValidationException("No puede existir dos receptores iguales.")
                    End If
                Next

                If intexisteLider = 0 Then
                    Throw New ValidationException("Debe haber un receptor líder.")
                End If
                If intexisteLider > 1 Then
                    Throw New ValidationException("Sólo debe existir un líder.")
                End If
                If intsumaPorcentaje <> 100 Then
                    Throw New ValidationException("El porcentaje es: " & intsumaPorcentaje & ", el total debe ser 100.")
                End If

                Dim numeroErrores = (From lr In ListaReceptores Where lr.HasValidationErrors = True).Count
                If numeroErrores <> 0 Then
                    Throw New ValidationException("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.")
                End If
            Else
                Throw New ValidationException("Debe existir al menos un receptor.")
            End If
            retorno = True
        Catch ex As ValidationException
            IsBusy = False
            A2Utilidades.Mensajes.mostrarMensaje(ex.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return retorno
    End Function

    Public Function ValidarContraparte() As Boolean
        Dim retorno As Boolean = False
        Dim dblvalornominalas As Double
        Dim dblcomision As Double
        Try
            If ListaContraparte.Count <> 0 Then
                For Each detallecontaparte In ListaContraparte
                    If detallecontaparte.TipoClienteExterno = False And detallecontaparte.TipoClienteOyD = False Then
                        Throw New ValidationException("Debe Seleccionar un tipo de cliente para esta operación.")
                    End If
                    If detallecontaparte.TipoClienteOyD = True And String.IsNullOrEmpty(detallecontaparte.lngIDComitente) Then
                        Throw New ValidationException("Debe Ingresar el código del comitente en la contraparte.")
                    End If
                    If detallecontaparte.TipoClienteExterno = True And String.IsNullOrEmpty(detallecontaparte.lngIDClienteExterno) Then
                        Throw New ValidationException("Debe Ingresar el código del cliente externo en la contraparte.")
                    End If
                    If ListaLiquidacionesYankeesSelected.strTipoCliente = "O" Then
                        If detallecontaparte.TipoClienteOyD = True And (LTrim(RTrim(detallecontaparte.lngIDComitente)) = LTrim(RTrim(ListaLiquidacionesYankeesSelected.lngIDComitente))) Then
                            Throw New ValidationException("Comitente no puede ser ingresado. existe en el encabezado.")
                        End If
                    End If
                    If ListaLiquidacionesYankeesSelected.strTipoCliente = "E" Then
                        If detallecontaparte.TipoClienteExterno = True And (detallecontaparte.lngIDClienteExterno = ListaLiquidacionesYankeesSelected.lngIDClienteExterno) Then
                            Throw New ValidationException("Cliente externo no puede ser ingresado. existe en el encabezado.")
                        End If
                    End If
                    If detallecontaparte.dblValorNominal = 0 Then
                        Throw New ValidationException("Debe Ingresar el valor nominal en la contraparte.")
                    End If
                    If detallecontaparte.dblPrecioLimpio = 0 Then
                        Throw New ValidationException("El precio limpio no debe ser cero en la contraparte.")
                    End If
                    If detallecontaparte.dblPrecioLimpio >= 200 Or detallecontaparte.dblPrecioLimpio <= 0 Then
                        Throw New ValidationException("El precio limpio  debe estar entre 0 y 200 en la contraparte.")
                    End If
                    If detallecontaparte.dblPrecioSucio = 0 Then
                        Throw New ValidationException("El precio sucio no debe ser cero en la contraparte.")
                    End If
                    If detallecontaparte.dblPrecioSucio > 200 Or detallecontaparte.dblPrecioSucio <= 0 Then
                        Throw New ValidationException("El precio sucio  debe estar entre 0 y 200 en la contraparte.")
                    End If
                    If IsNothing(detallecontaparte.dblValorTotal) Then
                        detallecontaparte.dblValorTotal = Valor_TotalOtraMoneda_Detalle(detallecontaparte)
                    End If
                    If detallecontaparte.dblTIR <= 0 Or IsNothing(detallecontaparte.dblTIR) Then
                        Throw New ValidationException("El Tir no debe ser menor que cero en la contraparte.")
                    End If
                    If detallecontaparte.dblTasaSpot = 0 Or IsNothing(detallecontaparte.dblTasaSpot) Then
                        Throw New ValidationException("La tasa spot no debe ser cero en la contraparte.")
                    Else
                        detallecontaparte.dblValorPesos = Valor_TotalPesos_Detalle(detallecontaparte)
                    End If
                    If detallecontaparte.logTrasladoComision And detallecontaparte.dblTrasladoComision = Nothing Then
                        Throw New ValidationException("El Valor traslado comisión debe ser ingresado en la contraparte.")
                    End If
                    If detallecontaparte.dblComision = Nothing Then
                        detallecontaparte.dblComision = Valor_TotalOtraMoneda_Detalle(detallecontaparte)
                    End If
                    dblvalornominalas = dblvalornominalas + detallecontaparte.dblValorNominal
                    dblcomision = dblcomision + detallecontaparte.dblValorTotal
                    If ListaLiquidacionesYankeesSelected.strTipoOperacion = "C" Then
                        ListaLiquidacionesYankeesSelected.dblComision = ListaLiquidacionesYankeesSelected.dblValorTotal - dblcomision
                    Else
                        ListaLiquidacionesYankeesSelected.dblComision = dblcomision - ListaLiquidacionesYankeesSelected.dblValorTotal
                    End If
                Next
                If ListaLiquidacionesYankeesSelected.dblValorNominal <> dblvalornominalas Then
                    Throw New ValidationException("El Valor nominal del encabezado debe ser igual a la sumatoria de los valores nominales de la contraparte.")
                End If
            Else
                Throw New ValidationException("Debe existir al menos una contraparte.")
            End If
            retorno = True
        Catch ex As ValidationException
            IsBusy = False
            A2Utilidades.Mensajes.mostrarMensaje(ex.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ValidarContraparte", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return retorno
    End Function

    ''' <summary>
    ''' Valida que en la contraparte si es OyD o Externo tenga los datos correctos seleccionados
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>SLB20130911</remarks>
    Public Function ValidarContraparte_OyD_Externo() As Boolean
        Dim retorno As Boolean = False
        Try
            For Each objLista In ListaContraparte

                If objLista.TipoClienteOyD Then 'OyD
                    If Not objLista.lngIDClienteExterno = String.Empty Then
                        objLista.lngIDClienteExterno = String.Empty
                        objLista.strClienteExterno = String.Empty
                        objLista.strVendedor = String.Empty
                        objLista.lngIDDepositoExtranjero = Nothing
                        objLista.DescripcionDeposito = String.Empty
                        objLista.strNumeroCuenta = String.Empty
                        objLista.strNombreTitular = String.Empty
                        Throw New ValidationException("Cuando en la contraparte esta seleccionado cliente OyD no puede seleccionarse un cliente externo.")
                    End If
                Else 'Externo
                    If Not objLista.lngIDComitente = String.Empty Or Not objLista.lngIDOrdenante = String.Empty Then
                        objLista.lngIDComitente = String.Empty
                        objLista.strComitente = String.Empty
                        objLista.lngIDOrdenante = String.Empty
                        objLista.strOrdenante = String.Empty
                        Throw New ValidationException("Cuando en la contraparte esta seleccionado cliente Externo no puede seleccionarse ni el comitente ni el ordenante.")
                    End If
                End If

            Next
            retorno = True
        Catch ex As ValidationException
            IsBusy = False
            A2Utilidades.Mensajes.mostrarMensaje(ex.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ValidarContraparte", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return retorno
    End Function

    Private Function Valor_TotalOtraMoneda_Detalle(ByVal li As Object) As Double
        Dim valor_intereses_acum_detalle As Double
        Valor_TotalOtraMoneda_Detalle = 0
        valor_intereses_acum_detalle = ListaLiquidacionesYankeesSelected.intDiasIntereses * li.dblvalornominal * ListaLiquidacionesYankeesSelected.dblTasaCupon / 100 / intBasecalculoInteres
        Valor_TotalOtraMoneda_Detalle = (li.dblvalornominal * li.dblpreciolimpio / 100) + valor_intereses_acum_detalle
    End Function
    Private Function Valor_TotalPesos_Detalle(ByVal li As Object) As Double
        Valor_TotalPesos_Detalle = 0
        Valor_TotalPesos_Detalle = li.dblValorUSD * li.dblTasaSpot
    End Function
    Private Function Valor_Comision_Detalle(ByVal li As Object) As Double
        Valor_Comision_Detalle = 0
        Dim dblpreciosucio As Double
        dblpreciosucio = li.dblpreciosucio
        Valor_Comision_Detalle = (ListaLiquidacionesYankeesSelected.dblPrecioSucio - dblpreciosucio) / 100
        Valor_Comision_Detalle = Valor_Comision_Detalle * li.dblvalornominal
        If Valor_Comision_Detalle > 0 Then
            Valor_Comision_Detalle = Valor_Comision_Detalle - (li.dblTrasladoComision)
        End If
    End Function
    Public Sub buscarordenante(ByVal strllamado As String)
        sw = 1
        If strllamado = "E" Then
            objProxy1.BuscadorOrdenantes.Clear()
            objProxy1.Load(objProxy1.buscarOrdenantesComitenteQuery(ListaLiquidacionesYankeesSelected.lngIDComitente, Program.Usuario, Program.HashConexion), AddressOf buscarOrdenanteCompleted, strllamado)
        Else
            objProxy1.BuscadorOrdenantes.Clear()
            objProxy1.Load(objProxy1.buscarOrdenantesComitenteQuery(ContraparteSelected.lngIDComitente, Program.Usuario, Program.HashConexion), AddressOf buscarOrdenanteCompleted, strllamado)
        End If
    End Sub
    Private Sub buscarOrdenanteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorOrdenantes))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If
            If strTipoItem = "E" Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.ToList.Where(Function(l) l.Lider = True).Count > 0 Then
                        Dim ordenante = lo.Entities.ToList.Where(Function(l) l.Lider = True).First
                        Me.ListaLiquidacionesYankeesSelected.lngIDOrdenante = ordenante.IdOrdenante
                        Me.ListaLiquidacionesYankeesSelected.strOrdenante = ordenante.Nombre
                    End If
                End If
                sw = 0
            Else
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.ToList.Where(Function(l) l.Lider = True).Count > 0 Then
                        Dim ordenante = lo.Entities.ToList.Where(Function(l) l.Lider = True).First
                        Me.ContraparteSelected.lngIDOrdenante = ordenante.IdOrdenante
                        Me.ContraparteSelected.strOrdenante = ordenante.Nombre
                    End If
                End If
                sw = 0
            End If

        Catch ex As Exception
            sw = 0
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items", Me.ToString(), "buscarOrdenanteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Buscar()
        cb.lngID = Nothing
        cb.IDComitente = ""
        cb.Tipo = ""
        cb.Fregistro = Nothing
        cb.CumplimientoTitulo = Nothing
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Metodo para calcular la comisión total
    ''' </summary>
    ''' <remarks>SLB20130906</remarks>
    Private Sub Total_Comision_Detalle()
        TotalComisiones = 0
        For Each objLista In ListaContraparte
            TotalComisiones = TotalComisiones + IIf(IsNothing(objLista.dblComision), "0", objLista.dblComision)
        Next
    End Sub

    ''' <summary>
    ''' Consulta los repectores del comitente seleccionado.
    ''' </summary>
    ''' <remarks>SLB20130910</remarks>
    Public Sub ReceptoresPorClientes_Consultar()
        dcProxy.ReceptoresYankees.Clear()
        dcProxy.Load(dcProxy.ReceptoresPorClientes_ConsultarQuery(_ListaLiquidacionesYankeesSelected.IDY, _ListaLiquidacionesYankeesSelected.lngIDComitente, Program.Usuario, Program.HashConexion), AddressOf Terminotrearreceptores, "Consultar")
    End Sub

#End Region

#Region "Property Changed"

    Private Sub _ListaLiquidacionesYankeesSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ListaLiquidacionesYankeesSelected.PropertyChanged
        Try
            If Editando And sw = 0 Then
                Select Case e.PropertyName
                    Case "strTipoCliente"
                        If ListaLiquidacionesYankeesSelected.strTipoCliente = "E" Then
                            VisibilityClientes = Visibility.Collapsed
                            VisibilityClientesExterior = Visibility.Visible
                            sw = 1
                            _ListaLiquidacionesYankeesSelected.lngIDComitente = String.Empty
                            _ListaLiquidacionesYankeesSelected.strComitente = String.Empty
                            _ListaLiquidacionesYankeesSelected.lngIDOrdenante = String.Empty
                            _ListaLiquidacionesYankeesSelected.strOrdenante = String.Empty
                            sw = 0
                        Else
                            VisibilityClientes = Visibility.Visible
                            VisibilityClientesExterior = Visibility.Collapsed
                            sw = 1
                            _ListaLiquidacionesYankeesSelected.lngIDClienteExterno = String.Empty
                            _ListaLiquidacionesYankeesSelected.strClienteExterno = String.Empty
                            _ListaLiquidacionesYankeesSelected.strVendedor = String.Empty
                            _ListaLiquidacionesYankeesSelected.lngIDDepositoExtranjero = Nothing
                            _ListaLiquidacionesYankeesSelected.strNumeroCuenta = String.Empty
                            _ListaLiquidacionesYankeesSelected.strNombreTitular = String.Empty
                            sw = 0
                        End If

                    Case "strTipoOperacion"
                        If Not IsNothing(ListaContraparte) Then
                            For Each li In _ListaContraparte
                                If ListaLiquidacionesYankeesSelected.strTipoOperacion = "C" Then
                                    li.strTipoOperacion = "V"
                                Else
                                    li.strTipoOperacion = "C"
                                End If
                            Next
                        End If

                    Case "lngIDComitente"
                        ListaLiquidacionesYankeesSelected.lngIDOrdenante = String.Empty
                        ListaLiquidacionesYankeesSelected.strOrdenante = String.Empty

                        If Not String.IsNullOrEmpty(_ListaLiquidacionesYankeesSelected.lngIDComitente) And _mlogBuscarCliente Then
                            buscarComitente(_ListaLiquidacionesYankeesSelected.lngIDComitente, "encabezado")
                        End If

                    Case "lngIDOrdenante"
                        If Not String.IsNullOrEmpty(_ListaLiquidacionesYankeesSelected.lngIDOrdenante) And _mlogBuscarOrdenante Then
                            If Not String.IsNullOrEmpty(_ListaLiquidacionesYankeesSelected.lngIDComitente) Then
                                Dim strFiltro = _ListaLiquidacionesYankeesSelected.lngIDComitente & "," & _ListaLiquidacionesYankeesSelected.lngIDOrdenante & "."
                                buscarGenerico(strFiltro, "ClienteOrdenante")
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("Seleccione primero el comitente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                _ListaLiquidacionesYankeesSelected.lngIDOrdenante = String.Empty
                            End If
                        End If

                    Case "lngIDClienteExterno"
                        If Not String.IsNullOrEmpty(_ListaLiquidacionesYankeesSelected.lngIDClienteExterno) And _mlogBuscarClienteExterno Then
                            buscarGenerico(_ListaLiquidacionesYankeesSelected.lngIDClienteExterno, "ClienteExerno")
                        End If

                    Case "strIDEspecie"
                        If Not String.IsNullOrEmpty(_ListaLiquidacionesYankeesSelected.strIDEspecie) And _mlogBuscarEspecie Then
                            buscarGenerico(_ListaLiquidacionesYankeesSelected.strIDEspecie, "especiesRF")
                        End If
                    Case "strIsin"
                        objProxy1.BuscadorGenericos.Clear()
                        objProxy1.Load(objProxy1.buscarItemsQuery(ListaLiquidacionesYankeesSelected.strIDEspecie, "ESPECIEISINFUNGIBLE", "A", ListaLiquidacionesYankeesSelected.strIsin, "", "", Program.Usuario, Program.HashConexion), AddressOf Terminotraerisinitem, Nothing)
                End Select
            End If
        Catch ex As Exception
            sw = 0
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_ListaLiquidacionesYankeesSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _ContraparteSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ContraparteSelected.PropertyChanged
        If Editando And sw = 0 Then
            Select Case e.PropertyName
                Case "TipoClienteOyD"
                    If ContraparteSelected.TipoClienteOyD Then
                        _ContraparteSelected.strTipoCliente = "O"
                        _ContraparteSelected.lngIDClienteExterno = String.Empty
                        _ContraparteSelected.strClienteExterno = String.Empty
                        _ContraparteSelected.strVendedor = String.Empty
                        _ContraparteSelected.lngIDDepositoExtranjero = Nothing
                        _ContraparteSelected.DescripcionDeposito = String.Empty
                        _ContraparteSelected.strNumeroCuenta = String.Empty
                        _ContraparteSelected.strNombreTitular = String.Empty
                        _ContraparteSelected.TipoClienteExterno = False
                        '        Case 1 'CLIENTE OYD
                        'With grdContraparte
                        '    .Columns("lngIDClienteExterno").Locked = True 'codigo cliente externo
                        '    .Columns("Externo").Value = False   'cliente externo
                        '    .Columns("lngIDComitente").Locked = False  'codigo comitente
                        '    .Columns("lngIDOrdenante").Locked = False    'codigo ordenante
                        '    .Columns("lngIDClienteExterno").Value = ""  'CODIGO CLIENTE EXTERNO
                        '    .Columns("strClienteExterno").Value = ""  'NOMBRE CLIENTE EXTERNO
                        '    .Columns("strVendedor").Value = ""  'VENDEDOR
                        '    .Columns("strNombreDeposito").Value = "" 'DEPOSITO EXTERNO
                        '    .Columns("strNumeroCuenta").Value = "" 'CUENTA
                        '    .Columns("strNombreTitular").Value = "" 'TITULAR
                        'End With
                    End If
                Case "TipoClienteExterno"
                    If ContraparteSelected.TipoClienteExterno Then
                        _ContraparteSelected.strTipoCliente = "E"
                        _ContraparteSelected.lngIDComitente = String.Empty
                        _ContraparteSelected.strComitente = String.Empty
                        _ContraparteSelected.lngIDOrdenante = String.Empty
                        _ContraparteSelected.strOrdenante = String.Empty
                        _ContraparteSelected.TipoClienteOyD = False
                    End If
                    'Case 2 'CLIENTE EXTERNO

                    '    With grdContraparte
                    '        .Columns("OyD").Value = False  'cliente oyd
                    '        .Columns("lngIDComitente").Locked = True  'codigo comitente
                    '        .Columns("lngIDOrdenante").Locked = True  'codigo ordenante
                    '        .Columns("lngIDClienteExterno").Locked = False 'codigo cliente externo
                    '        .Columns("lngIDComitente").Value = ""  'CODIGO CLIENTE
                    '        .Columns("strComitente").Value = ""  'NOMBRE CLIENTE
                    '        .Columns("lngIDOrdenante").Value = ""  'CODIGO ORDENANTE
                    '        .Columns("strOrdenante").Value = ""  'NOMBRE ORDENANTE
                    '    End With
                Case "lngIDComitente"
                    _ContraparteSelected.lngIDOrdenante = String.Empty
                    _ContraparteSelected.strOrdenante = String.Empty

                    If Not String.IsNullOrEmpty(_ContraparteSelected.lngIDComitente) And _mlogBuscarCliente Then
                        buscarComitente(_ContraparteSelected.lngIDComitente, "detalle")
                    End If
                Case "lngIDOrdenante"
                    If Not String.IsNullOrEmpty(_ContraparteSelected.lngIDOrdenante) And _mlogBuscarOrdenante Then
                        If Not String.IsNullOrEmpty(_ContraparteSelected.lngIDComitente) Then
                            Dim strFiltro = _ContraparteSelected.lngIDComitente & "," & _ContraparteSelected.lngIDOrdenante & "."
                            buscarGenerico(strFiltro, "ClienteOrdenante", "Detalle")
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Seleccione primero el comitente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _ContraparteSelected.lngIDOrdenante = String.Empty
                        End If
                    End If

                Case "lngIDClienteExterno"
                    If Not String.IsNullOrEmpty(_ContraparteSelected.lngIDClienteExterno) And _mlogBuscarClienteExterno Then
                        buscarGenerico(_ContraparteSelected.lngIDClienteExterno, "ClienteExerno", "Detalle")
                    End If

            End Select

        End If
    End Sub

#End Region

#Region "Busqueda de Comitente desde el control de la vista"

    ''' <summary>
    ''' Buscar los datos del comitente que tiene asignada la Tesoreria.
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks>SLB20130122</remarks>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")
        sw = 1
        Dim strIdComitente As String = String.Empty

        Try
            If Not Me.ListaLiquidacionesYankeesSelected Is Nothing Then
                If Not strIdComitente.Equals(Me.ListaLiquidacionesYankeesSelected.lngIDComitente) Then
                    If pstrIdComitente.Trim.Equals(String.Empty) Then
                        strIdComitente = Me.ListaLiquidacionesYankeesSelected.lngIDComitente
                    Else
                        strIdComitente = pstrIdComitente
                    End If

                    If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                        objProxy.BuscadorClientes.Clear()
                        objProxy.Load(objProxy.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            sw = 0
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130122</remarks>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Select Case lo.UserState.ToString
                        Case "encabezado"
                            _ListaLiquidacionesYankeesSelected.lngIDComitente = String.Empty
                            _ListaLiquidacionesYankeesSelected.strComitente = String.Empty
                            sw = 0
                        Case "detalle"
                            _ContraparteSelected.lngIDComitente = String.Empty
                            _ContraparteSelected.strComitente = String.Empty
                            sw = 0
                            'Me.ComitenteSeleccionadoDetalle(Nothing)
                    End Select
                Else
                    Select Case lo.UserState.ToString
                        Case "encabezado"
                            _ListaLiquidacionesYankeesSelected.lngIDComitente = lo.Entities.ToList.First.CodigoOYD
                            _ListaLiquidacionesYankeesSelected.strComitente = lo.Entities.ToList.First.Nombre
                            buscarordenante("E")
                            ReceptoresPorClientes_Consultar()
                        Case "detalle"
                            _ContraparteSelected.lngIDComitente = lo.Entities.ToList.First.IdComitente
                            _ContraparteSelected.strComitente = lo.Entities.ToList.First.Nombre
                            buscarordenante("D")
                            'Me.ComitenteSeleccionadoDetalle(lo.Entities.ToList.FirstOrDefault)
                    End Select
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Select Case lo.UserState.ToString
                    Case "encabezado"
                        _ListaLiquidacionesYankeesSelected.lngIDComitente = String.Empty
                        _ListaLiquidacionesYankeesSelected.strComitente = String.Empty
                        sw = 0
                    Case "detalle"
                        _ContraparteSelected.lngIDComitente = String.Empty
                        _ContraparteSelected.strComitente = String.Empty
                        sw = 0
                        'Me.ComitenteSeleccionadoDetalle(Nothing)
                End Select
            End If
        Catch ex As Exception
            sw = 0
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ' ''' <summary>
    ' ''' Método para asignar los valores correspodientes del comitente seleccionado en el detalle
    ' ''' </summary>
    ' ''' <param name="pobjComitente"></param>
    ' ''' <remarks>20130312</remarks>
    'Private Sub ComitenteSeleccionadoDetalle(ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
    '    If Not IsNothing(pobjComitente) Then
    '        _DetalleTesoreriSelected.IDComitente = pobjComitente.IdComitente
    '        _DetalleTesoreriSelected.Nombre = pobjComitente.Nombre
    '        _mlogBuscarNIT = False
    '        _DetalleTesoreriSelected.NIT = pobjComitente.NroDocumento
    '        _mlogBuscarNIT = True
    '        ValidarSaldoCliente()
    '    Else
    '        _DetalleTesoreriSelected.IDComitente = String.Empty
    '        _DetalleTesoreriSelected.Nombre = String.Empty
    '        _DetalleTesoreriSelected.NIT = String.Empty
    '    End If
    '    If moduloTesoreria = ClasesTesoreria.N.ToString Then _DetalleTesoreriSelected.IDBanco = Nothing
    'End Sub

#End Region

#Region "Buscador Generico"

    ''' <summary>
    ''' Buscar el ordenante, cliente exterior.
    ''' </summary>
    ''' <param name="pstrFiltro"></param>
    ''' <param name="pstrBusqueda"></param>
    ''' <remarks>SLB20130710</remarks>
    Friend Sub buscarGenerico(Optional ByVal pstrFiltro As String = "", Optional ByVal pstrBusqueda As String = "", Optional ByVal pstrProviene As String = "")
        Try
            sw = 1
            strBusquedaGenerico = pstrBusqueda + pstrProviene
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemEspecificoQuery(pstrBusqueda, pstrFiltro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBuscadorGenerico, strBusquedaGenerico)
        Catch ex As Exception
            sw = 1
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método recibe del buscador generico.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130710</remarks>
    Private Sub TerminoTraerBuscadorGenerico(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "ClienteExerno"
                        If lo.Entities.ToList.Count > 0 Then
                            _ListaLiquidacionesYankeesSelected.lngIDClienteExterno = lo.Entities.First.IdItem
                            _ListaLiquidacionesYankeesSelected.strClienteExterno = lo.Entities.First.Nombre
                            _ListaLiquidacionesYankeesSelected.strVendedor = lo.Entities.First.Descripcion
                            _ListaLiquidacionesYankeesSelected.lngIDDepositoExtranjero = lo.Entities.First.EtiquetaIdItem
                            _ListaLiquidacionesYankeesSelected.strNumeroCuenta = lo.Entities.First.InfoAdicional01
                            _ListaLiquidacionesYankeesSelected.strNombreTitular = lo.Entities.First.InfoAdicional02
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El cliente externo ingresado no existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _ListaLiquidacionesYankeesSelected.lngIDClienteExterno = String.Empty
                            _ListaLiquidacionesYankeesSelected.strClienteExterno = String.Empty
                            _ListaLiquidacionesYankeesSelected.strVendedor = String.Empty
                            _ListaLiquidacionesYankeesSelected.lngIDDepositoExtranjero = Nothing
                            _ListaLiquidacionesYankeesSelected.strNumeroCuenta = String.Empty
                            _ListaLiquidacionesYankeesSelected.strNombreTitular = String.Empty
                        End If
                        sw = 0
                    Case "ClienteExernoDetalle"
                        If lo.Entities.ToList.Count > 0 Then
                            _ContraparteSelected.lngIDClienteExterno = lo.Entities.First.IdItem
                            _ContraparteSelected.strClienteExterno = lo.Entities.First.Nombre
                            _ContraparteSelected.strVendedor = lo.Entities.First.Descripcion
                            _ContraparteSelected.lngIDDepositoExtranjero = lo.Entities.First.EtiquetaIdItem
                            _ContraparteSelected.DescripcionDeposito = lo.Entities.First.CodigoAuxiliar
                            _ContraparteSelected.strNumeroCuenta = lo.Entities.First.InfoAdicional01
                            _ContraparteSelected.strNombreTitular = lo.Entities.First.InfoAdicional02
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El cliente externo ingresado no existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _ContraparteSelected.lngIDClienteExterno = String.Empty
                            _ContraparteSelected.strClienteExterno = String.Empty
                            _ContraparteSelected.strVendedor = String.Empty
                            _ContraparteSelected.lngIDDepositoExtranjero = Nothing
                            _ContraparteSelected.DescripcionDeposito = String.Empty
                            _ContraparteSelected.strNumeroCuenta = String.Empty
                            _ContraparteSelected.strNombreTitular = String.Empty
                        End If
                        sw = 0
                    Case "ClienteOrdenante"
                        If lo.Entities.ToList.Count > 0 Then
                            _ListaLiquidacionesYankeesSelected.lngIDOrdenante = lo.Entities.First.IdItem
                            _ListaLiquidacionesYankeesSelected.strOrdenante = lo.Entities.First.Nombre
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El ordenante ingresada no existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _ListaLiquidacionesYankeesSelected.lngIDOrdenante = String.Empty
                            _ListaLiquidacionesYankeesSelected.strOrdenante = String.Empty
                        End If
                        sw = 0
                    Case "ClienteOrdenanteDetalle"
                        If lo.Entities.ToList.Count > 0 Then
                            _ContraparteSelected.lngIDOrdenante = lo.Entities.First.IdItem
                            _ContraparteSelected.strOrdenante = lo.Entities.First.Nombre
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El ordenante ingresada no existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _ContraparteSelected.lngIDOrdenante = String.Empty
                            _ContraparteSelected.strOrdenante = String.Empty
                        End If
                        sw = 0
                    Case "especiesRF"
                        If lo.Entities.ToList.Count > 0 Then
                            _ListaLiquidacionesYankeesSelected.strIDEspecie = lo.Entities.First.IdItem
                            _ListaLiquidacionesYankeesSelected.strNombreEspecie = lo.Entities.First.Nombre
                            buscarisin()
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("La especie ingresada no existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _ListaLiquidacionesYankeesSelected.strIDEspecie = String.Empty
                            _ListaLiquidacionesYankeesSelected.strNombreEspecie = String.Empty
                        End If
                        sw = 0
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos", _
                                             Me.ToString(), "TerminoTraerBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            sw = 0
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el banco", Me.ToString(), _
                                                             "TerminoTraerCuentasBancarias", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region


End Class
Public Class CamposBusquedaLiquidacioneYankees

    Implements INotifyPropertyChanged
    Private _lngID As Integer
    <Display(Name:="Nro Operación")> _
    Public Property lngID As Integer
        Get
            Return _lngID
        End Get
        Set(ByVal value As Integer)
            _lngID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngID"))
        End Set
    End Property

    Private _IDComitente As String
    <Display(Name:="Comitente")> _
    Public Property IDComitente As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitente"))
        End Set
    End Property
    Private _Tipo As String
    <Display(Name:="Tipo")> _
    Public Property Tipo As String
        Get
            Return _Tipo
        End Get
        Set(ByVal value As String)
            _Tipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Tipo"))
        End Set
    End Property


    Private _Fregistro As System.Nullable(Of DateTime)
    <Display(Name:="Fecha Registro")> _
    Public Property Fregistro As System.Nullable(Of DateTime)
        Get
            Return _Fregistro
        End Get
        Set(value As System.Nullable(Of DateTime))
            _Fregistro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Fregistro"))
        End Set
    End Property

    Private _CumplimientoTitulo As System.Nullable(Of DateTime)
    <Display(Name:="Fecha Cumplimiento")> _
    Public Property CumplimientoTitulo As System.Nullable(Of DateTime)
        Get
            Return _CumplimientoTitulo
        End Get
        Set(value As System.Nullable(Of DateTime))
            _CumplimientoTitulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CumplimientoTitulo"))
        End Set
    End Property

    <Display(Name:="Fecha Cumplimiento")> _
    Public Property DisplayDate As DateTime = Now.Date

    <Display(Name:="Fecha Registro")> _
    Public Property DisplayDate2 As DateTime = Now.Date
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


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class CamposComboIsin
    Implements INotifyPropertyChanged

    Private _ID As String
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property


    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

