Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: Liquidaciones_OTCViewModel.vb
'Generado el : 09/21/2012 11:57:23
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
Imports A2Utilidades.Mensajes

Public Class Liquidaciones_OTCViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim logValidacion As Boolean = False
    Private ReceptorOTC_PorDefecto As New ReceptoresOT
    Private Liquidaciones_OTPorDefecto As Liquidaciones_OT
    Private Liquidaciones_OTAnterior As Liquidaciones_OT
    Dim dcProxy As OTCDomainContext
    Dim dcProxy1 As OTCDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Dim A2UtilsViewModel As A2UtilsViewModel
    Dim FechaCierre As DateTime
    'Public mstrEsAccion As String = ""
    Public ConsultodesdeBoton As Boolean = True 'SLB20140605 No lanzar eventos en el PropertyChanged de forma inadecuada.
    Dim intIDRegistro As Integer = 0
    Dim logRecargarDetalle As Boolean = True

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OTCDomainContext
                dcProxy1 = New OTCDomainContext
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
            Else
                dcProxy = New OTCDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OTCDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.Liquidaciones_OTCFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones_OTC, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerLiquidaciones_OTPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones_OTCPorDefecto_Completed, "Default")
                mdcProxyUtilidad01.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Liquidaciones_OTCViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerLiquidaciones_OTCPorDefecto_Completed(ByVal lo As LoadOperation(Of Liquidaciones_OT))
        If Not lo.HasError Then
            Liquidaciones_OTPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Liquidaciones_OT por defecto", _
                                             Me.ToString(), "TerminoTraerLiquidaciones_OTPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerLiquidaciones_OTC(ByVal lo As LoadOperation(Of Liquidaciones_OT))

        If Not lo.HasError Then
            If lo.UserState = "TERMINOANULAR" Or lo.UserState = "TERMINAGUARDAR" Then
                logRecargarDetalle = False
            End If
            ListaLiquidaciones_OTC = Nothing
            ListaLiquidaciones_OTC = dcProxy.Liquidaciones_OTs
            If lo.UserState = "TERMINOANULAR" Or lo.UserState = "TERMINAGUARDAR" Then
                logRecargarDetalle = True
            End If
            If dcProxy.Liquidaciones_OTs.Count > 0 Then
                If lo.UserState = "insert" Then
                    Liquidaciones_OTSelected = Nothing
                    Liquidaciones_OTSelected = ListaLiquidaciones_OTC.Last
                    Liquidaciones_OTSelected = (From o In ListaLiquidaciones_OTC _
                                               Where o.ID = Liquidaciones_OTSelected.ID And o.VERSION = 1 _
                                               Select o).ToList.FirstOrDefault
                ElseIf lo.UserState = "TERMINOANULAR" Or lo.UserState = "TERMINAGUARDAR" Then
                    If ListaLiquidaciones_OTC.Where(Function(i) i.ID = intIDRegistro).Count > 0 Then
                        Liquidaciones_OTSelected = ListaLiquidaciones_OTC.Where(Function(i) i.ID = intIDRegistro).First
                    Else
                        If ListaLiquidaciones_OTC.Count > 0 Then
                            Liquidaciones_OTSelected = ListaLiquidaciones_OTC.First
                        End If
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Liquidaciones_OTC", _
                                             Me.ToString(), "TerminoTraerLiquidaciones_OT", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Public Sub TerminoAnulariquidaciones_OTC(ByVal lo As InvokeOperation(Of Integer))
        If Not lo.HasError Then

            intIDRegistro = _Liquidaciones_OTSelected.ID

            MyBase.QuitarFiltroDespuesGuardar()
            IsBusy = True
            dcProxy.Liquidaciones_OTs.Clear()
            dcProxy.Load(dcProxy.Liquidaciones_OTCFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones_OTC, "TERMINOANULAR")

            '_Liquidaciones_OTSelected.ESTADO = "A"
            'logEstadoAnulada = True
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la anulación de Liquidaciones_OTC", _
                                             Me.ToString(), "TerminoAnulariquidaciones_OTC", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False

    End Sub

    Private Sub TerminoTraerReceptores(ByVal lo As LoadOperation(Of ReceptoresOT))
        If Not lo.HasError Then
            ListaReceptores = Nothing
            ListaReceptores = dcProxy.ReceptoresOTs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Receptores", _
                                             Me.ToString(), "TerminoTraerReceptores", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            FechaCierre = obj.Value
        End If
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaLiquidaciones_OTC As EntitySet(Of Liquidaciones_OT)
    Public Property ListaLiquidaciones_OTC() As EntitySet(Of Liquidaciones_OT)
        Get
            Return _ListaLiquidaciones_OTC
        End Get
        Set(ByVal value As EntitySet(Of Liquidaciones_OT))
            _ListaLiquidaciones_OTC = value

            MyBase.CambioItem("ListaLiquidaciones_OTC")
            MyBase.CambioItem("ListaLiquidaciones_OTCPaged")
            If Not IsNothing(value) Then
                If logRecargarDetalle Then
                    If IsNothing(Liquidaciones_OTAnterior) Then
                        Liquidaciones_OTSelected = _ListaLiquidaciones_OTC.FirstOrDefault
                    Else
                        Liquidaciones_OTSelected = Liquidaciones_OTAnterior
                    End If
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaLiquidaciones_OTCPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLiquidaciones_OTC) Then
                Dim view = New PagedCollectionView(_ListaLiquidaciones_OTC)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _Liquidaciones_OTSelected As Liquidaciones_OT
    Public Property Liquidaciones_OTSelected() As Liquidaciones_OT
        Get
            Return _Liquidaciones_OTSelected
        End Get
        Set(ByVal value As Liquidaciones_OT)
            _Liquidaciones_OTSelected = value

            If Not IsNothing(_Liquidaciones_OTSelected) Then
                'ConvertirDatos()
                If _Liquidaciones_OTSelected.ID <> -1 Then
                    If logRecargarDetalle Then
                        dcProxy.ReceptoresOTs.Clear()
                        dcProxy.Load(dcProxy.Receptores_OTCConsultarQuery(_Liquidaciones_OTSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptores, Nothing)
                    End If
                End If

                If _Liquidaciones_OTSelected.RENTAFIJA Then
                    logTasaFija = True
                Else
                    logTasaFija = False
                End If

            End If

            MyBase.CambioItem("Liquidaciones_OTSelected")
        End Set
    End Property

    'Private _logTipoOperacionCompra As Boolean
    'Public Property logTipoOperacionCompra() As Boolean
    '    Get
    '        Return _logTipoOperacionCompra
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _logTipoOperacionCompra = value
    '        If _logTipoOperacionCompra = True Then
    '            _Liquidaciones_OTSelected.TIPOOPERACION = "C"
    '        End If
    '        MyBase.CambioItem("logTipoOperacionCompra")
    '    End Set
    'End Property

    'Private _logTipoOperacionVenta As Boolean
    'Public Property logTipoOperacionVenta() As Boolean
    '    Get
    '        Return _logTipoOperacionVenta
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _logTipoOperacionVenta = value
    '        If _logTipoOperacionVenta = True Then
    '            _Liquidaciones_OTSelected.TIPOOPERACION = "V"
    '        End If
    '        MyBase.CambioItem("logTipoOperacionVenta")
    '    End Set
    'End Property

    Private _logTasaFija As Boolean
    Public Property logTasaFija As Boolean
        Get
            Return _logTasaFija
        End Get
        Set(ByVal value As Boolean)
            _logTasaFija = value

            'If _logTasaFija = True Then
            '    _Liquidaciones_OTSelected.RENTAFIJA = True
            '    _Liquidaciones_OTSelected.INDICADOR = Nothing
            '    _Liquidaciones_OTSelected.PUNTOSINDICADOR = Nothing
            'End If

            MyBase.CambioItem("logTasaFija")
        End Set
    End Property

    'Private _logTasaVariable As Boolean
    'Public Property logTasaVariable() As Boolean
    '    Get
    '        Return _logTasaVariable
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _logTasaVariable = value
    '        'If _logTasaVariable = True Then
    '        '    _Liquidaciones_OTSelected.RENTAFIJA = False
    '        '    _Liquidaciones_OTSelected.TASAEFECTIVAANUAL = Nothing
    '        '    _Liquidaciones_OTSelected.INDICADOR = "0"
    '        'End If
    '        MyBase.CambioItem("logTasaVariable")
    '    End Set
    'End Property

    'Private _logEstadoActiva As Boolean
    'Public Property logEstadoActiva() As Boolean
    '    Get
    '        Return _logEstadoActiva
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _logEstadoActiva = value
    '        MyBase.CambioItem("logEstadoActiva")
    '    End Set
    'End Property

    'Private _logEstadoAnulada As Boolean
    'Public Property logEstadoAnulada() As Boolean
    '    Get
    '        Return _logEstadoAnulada
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _logEstadoAnulada = value
    '        If _logEstadoAnulada = True Then
    '            _Liquidaciones_OTSelected.ESTADO = "A"
    '        End If
    '        MyBase.CambioItem("logEstadoAnulada")
    '    End Set
    'End Property

    'Private _logEstadoImpresa As Boolean
    'Public Property logEstadoImpresa() As Boolean
    '    Get
    '        Return _logEstadoImpresa
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _logEstadoImpresa = value
    '        MyBase.CambioItem("logEstadoImpresa")
    '    End Set
    'End Property


    Private WithEvents _cb As CamposBusquedaLiquidaciones_OT = New CamposBusquedaLiquidaciones_OT
    Public Property cb() As CamposBusquedaLiquidaciones_OT
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaLiquidaciones_OT)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property
    Private _logHabilitaAccion As Boolean
    Public Property logHabilitaAccion As Boolean
        Get
            Return _logHabilitaAccion
        End Get
        Set(ByVal value As Boolean)
            _logHabilitaAccion = value
            MyBase.CambioItem("logHabilitaAccion")
        End Set
    End Property

#End Region

#Region "Métodos"

    'Public Sub ConvertirDatos()
    '    Try
    '        'Marcado de los radio buton de Tipo de Operación
    '        If _Liquidaciones_OTSelected.TIPOOPERACION <> String.Empty Then

    '            logTipoOperacionVenta = False
    '            logTipoOperacionCompra = False

    '            If _Liquidaciones_OTSelected.TIPOOPERACION = "C" Then
    '                logTipoOperacionCompra = True
    '            ElseIf _Liquidaciones_OTSelected.TIPOOPERACION = "V" Then
    '                logTipoOperacionVenta = True
    '            End If

    '        End If

    '        'Marcado de los radio buton segun el estado de la liquidacion
    '        If _Liquidaciones_OTSelected.ESTADO <> String.Empty Then

    '            logEstadoActiva = False
    '            logEstadoAnulada = False
    '            logEstadoImpresa = False

    '            If _Liquidaciones_OTSelected.ESTADO = "I" Then
    '                logEstadoImpresa = True
    '            ElseIf _Liquidaciones_OTSelected.ESTADO = "A" Then
    '                logEstadoAnulada = True
    '            ElseIf _Liquidaciones_OTSelected.ESTADO = "P" Then
    '                logEstadoActiva = True
    '            End If
    '        End If

    '        'Marcado de los radiobuton segun el tipo de tasa de la liquidación
    '        If Not IsNothing(_Liquidaciones_OTSelected.RENTAFIJA) Then

    '            logTasaFija = False
    '            logTasaVariable = False

    '            If _Liquidaciones_OTSelected.RENTAFIJA = True Then
    '                logTasaFija = True
    '            Else
    '                logTasaVariable = True
    '            End If

    '        End If

    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de hacer las conversiones de los valores", _
    '                                                     Me.ToString(), "ConvertirDatos", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    Public Sub CalcularPrecio()

        If Not IsNothing(Liquidaciones_OTSelected.MONTO) And Not IsNothing(Liquidaciones_OTSelected.CANTIDADNEGOCIADA) Then
            Liquidaciones_OTSelected.PRECIO = (Liquidaciones_OTSelected.MONTO / Liquidaciones_OTSelected.CANTIDADNEGOCIADA)
        End If

    End Sub

    Public Sub CalcularDiasVencimiento(ByVal strTipoCalculo As String)

        If Not IsNothing(Liquidaciones_OTSelected.OPERACION) And Not IsNothing(Liquidaciones_OTSelected.VENCIMIENTO) And strTipoCalculo = "fechas" Then
            Liquidaciones_OTSelected.DIASALVENCIMIENTOTITULO = DateDiff(DateInterval.Day, CType(Liquidaciones_OTSelected.OPERACION, Date).Date, CType(Liquidaciones_OTSelected.VENCIMIENTO, Date).Date)
        End If

        If Not IsNothing(Liquidaciones_OTSelected.OPERACION) And strTipoCalculo = "dias" And _Liquidaciones_OTSelected.DIASALVENCIMIENTOTITULO <> 0 Then
            Liquidaciones_OTSelected.VENCIMIENTO = DateAdd(DateInterval.Day, CDbl(_Liquidaciones_OTSelected.DIASALVENCIMIENTOTITULO), CDate(_Liquidaciones_OTSelected.OPERACION))
        End If

    End Sub

    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewLiquidaciones_OT As New Liquidaciones_OT
            'TODO: Verificar cuales son los campos que deben inicializarse
            'Dim ASD As Int16 = Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))



            NewLiquidaciones_OT.ID = 0
            NewLiquidaciones_OT.VERSION = Liquidaciones_OTPorDefecto.VERSION
            'NewLiquidaciones_OT.NOMBRESISTEMA = Liquidaciones_OTPorDefecto.NOMBRESISTEMA


            If (Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS)) Then
                If Not IsNothing(CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))) Then
                    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("SISTEMA_OTC") Then
                        NewLiquidaciones_OT.NOMBRESISTEMA = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("SISTEMA_OTC").FirstOrDefault.Descripcion
                    End If
                End If
            End If


            'If Not IsNothing(CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("SISTEMA_OTC")) Then
            '    NewLiquidaciones_OT.NOMBRESISTEMA = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("SISTEMA_OTC").FirstOrDefault.Descripcion
            'End If

            NewLiquidaciones_OT.NUMEROOPERACION = Liquidaciones_OTPorDefecto.NUMEROOPERACION
            ' SLB20140605 En el campo dtmOperacion se debe grabar sola la Fecha sin Hora 
            NewLiquidaciones_OT.OPERACION = Now.Date 'Liquidaciones_OTPorDefecto.OPERACION
            NewLiquidaciones_OT.TIPOOPERACION = Liquidaciones_OTPorDefecto.TIPOOPERACION
            NewLiquidaciones_OT.Mercado = Liquidaciones_OTPorDefecto.Mercado
            NewLiquidaciones_OT.TIPONEGOCIACION = Liquidaciones_OTPorDefecto.TIPONEGOCIACION
            NewLiquidaciones_OT.REGISTROOPERACION = Liquidaciones_OTPorDefecto.REGISTROOPERACION
            NewLiquidaciones_OT.TIPOPAGOOPERACION = Liquidaciones_OTPorDefecto.TIPOPAGOOPERACION
            NewLiquidaciones_OT.IDESPECIE = Liquidaciones_OTPorDefecto.IDESPECIE
            NewLiquidaciones_OT.CANTIDADNEGOCIADA = 0
            NewLiquidaciones_OT.EMISION = Liquidaciones_OTPorDefecto.EMISION
            NewLiquidaciones_OT.CUMPLIMIENTO = Liquidaciones_OTPorDefecto.CUMPLIMIENTO.Date
            NewLiquidaciones_OT.VENCIMIENTO = Liquidaciones_OTPorDefecto.VENCIMIENTO
            NewLiquidaciones_OT.DIASALVENCIMIENTOTITULO = Liquidaciones_OTPorDefecto.DIASALVENCIMIENTOTITULO
            NewLiquidaciones_OT.TASAINTERESNOMINAL = 0
            NewLiquidaciones_OT.MODALIDADTASANOMINAL = Liquidaciones_OTPorDefecto.MODALIDADTASANOMINAL
            NewLiquidaciones_OT.TASAEFECTIVAANUAL = 0
            NewLiquidaciones_OT.PRECIO = 0
            NewLiquidaciones_OT.MONTO = 0
            NewLiquidaciones_OT.IDREPRESENTANTELEGAL = Liquidaciones_OTPorDefecto.IDREPRESENTANTELEGAL
            NewLiquidaciones_OT.IDCOMITENTE = Liquidaciones_OTPorDefecto.IDCOMITENTE
            NewLiquidaciones_OT.NROTITULO = Liquidaciones_OTPorDefecto.NROTITULO
            'NewLiquidaciones_OT.INDICADOR = Liquidaciones_OTPorDefecto.INDICADOR
            NewLiquidaciones_OT.INDICADOR = Nothing
            NewLiquidaciones_OT.PUNTOSINDICADOR = 0
            NewLiquidaciones_OT.RENTAFIJA = Liquidaciones_OTPorDefecto.RENTAFIJA
            NewLiquidaciones_OT.PREFIJO = Liquidaciones_OTPorDefecto.PREFIJO
            NewLiquidaciones_OT.IDFACTURA = Liquidaciones_OTPorDefecto.IDFACTURA
            NewLiquidaciones_OT.FACTURADA = Liquidaciones_OTPorDefecto.FACTURADA
            NewLiquidaciones_OT.ESTADO = Liquidaciones_OTPorDefecto.ESTADO
            NewLiquidaciones_OT.NroLote = Liquidaciones_OTPorDefecto.NroLote
            NewLiquidaciones_OT.ACTUALIZACION = Liquidaciones_OTPorDefecto.ACTUALIZACION
            NewLiquidaciones_OT.USUARIO = Program.Usuario
            NewLiquidaciones_OT.NroLoteENC = Liquidaciones_OTPorDefecto.NroLoteENC
            NewLiquidaciones_OT.ContabilidadENC = Liquidaciones_OTPorDefecto.ContabilidadENC
            'NewLiquidaciones_OT.IdLiquidaciones_OTC = Liquidaciones_OTPorDefecto.IdLiquidaciones_OTC
            NewLiquidaciones_OT.IdLiquidaciones_OTC = -1
            NewLiquidaciones_OT.ESPECIE_ESACCION = String.Empty
            Liquidaciones_OTAnterior = Liquidaciones_OTSelected
            Liquidaciones_OTSelected = NewLiquidaciones_OT
            ListaReceptores = Nothing
            ReceptorSelected = Nothing

            MyBase.CambioItem("Liquidaciones_OTC")

            ListaReceptores = Nothing

            If Not IsNothing(dcProxy.ReceptoresOTs) Then
                dcProxy.ReceptoresOTs.Clear()
            End If
            ListaReceptores = dcProxy.ReceptoresOTs

            MyBase.CambioItem("ListaReceptores")

            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Liquidaciones_OTs.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.Liquidaciones_OTCFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones_OTC, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.Liquidaciones_OTCFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones_OTC, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.ID) Or Not IsNothing(cb.NUMEROOPERACION) Or Not IsNothing(cb.OPERACION) _
                Or Not IsNothing(cb.CUMPLIMIENTO) Or Not cb.IDCOMITENTE = String.Empty Or Not cb.IDESPECIE = String.Empty Then 'Validar que ingresó algo en los campos de búsqueda

                Dim logpuedeConsultar = True

                If Not IsNothing(cb.CUMPLIMIENTO) Then
                    If CType(cb.CUMPLIMIENTO, DateTime).Year < 1753 Then
                        logpuedeConsultar = False
                        A2Utilidades.Mensajes.mostrarMensaje("El valor mínimo permitido para la fecha de cumplimiento es '01/01/1753'", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                    End If
                End If

                If Not IsNothing(cb.OPERACION) Then
                    If CType(cb.OPERACION, DateTime).Year < 1753 Then
                        logpuedeConsultar = False
                        A2Utilidades.Mensajes.mostrarMensaje("El valor mínimo permitido para la fecha de operación es '01/01/1753'", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                    End If
                End If

                If logpuedeConsultar Then

                    ErrorForma = ""
                    dcProxy.Liquidaciones_OTs.Clear()
                    Liquidaciones_OTAnterior = Nothing
                    IsBusy = True

                    dcProxy.Load(dcProxy.Liquidaciones_OTCConsultarQuery(cb.ID, cb.NUMEROOPERACION, cb.OPERACION, cb.CUMPLIMIENTO, _
                        cb.IDCOMITENTE, cb.IDESPECIE, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones_OTC, "Busqueda")

                    MyBase.ConfirmarBuscar()
                    cb = New CamposBusquedaLiquidaciones_OT
                    CambioItem("cb")

                End If

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub validaciones()

        'Inicia con las validaciones de campos requeridos

        If IsNothing(Liquidaciones_OTSelected.TIPONEGOCIACION) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la classe de operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If IsNothing(Liquidaciones_OTSelected.CUMPLIMIENTO) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la fecha de cumplimiento del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If IsNothing(Liquidaciones_OTSelected.OPERACION) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la fecha de operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If IsNothing(Liquidaciones_OTSelected.Mercado) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el Mercado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If Liquidaciones_OTSelected.IDCOMITENTE = "" Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el comitente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If Liquidaciones_OTSelected.IDREPRESENTANTELEGAL = "" Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el representante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If Liquidaciones_OTSelected.IDESPECIE = "" Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la especie.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If Liquidaciones_OTSelected.TIPOOPERACION = "V" Then
            If Liquidaciones_OTSelected.NROTITULO = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el número del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidacion = True
                Exit Sub
            End If
        End If

        'SLB20130405 Inicio Validaciones cuando la Especies es de Renta Fija
        If _Liquidaciones_OTSelected.ESPECIE_ESACCION = "C" Then

            'If mstrEsAccion = "C" Then

            If _Liquidaciones_OTSelected.MODALIDADTASANOMINAL = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la modalidad del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidacion = True
                Exit Sub
            End If

            If Not IsDate(_Liquidaciones_OTSelected.EMISION) Or IsNothing(_Liquidaciones_OTSelected.EMISION) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la fecha de emisión del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidacion = True
                Exit Sub
            End If

            If Not IsDate(_Liquidaciones_OTSelected.VENCIMIENTO) Or IsNothing(_Liquidaciones_OTSelected.VENCIMIENTO) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la fecha de vencimiento del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidacion = True
                Exit Sub
            End If

            If CType(_Liquidaciones_OTSelected.VENCIMIENTO, Date).Date <= CType(_Liquidaciones_OTSelected.EMISION, Date).Date Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de vencimiento debe ser mayor que la de emisión.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidacion = True
                Exit Sub
            End If

        End If
        'SLB20130405 Fin Validaciones cuando la Especies es de Renta Fija

        ' EOMC 2013/11/13 -- Inicio
        If String.IsNullOrEmpty(Liquidaciones_OTSelected.NOMBRESISTEMA) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el nombre del sistema.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If
        ' EOMC 2013/11/13 -- Fin

        'SLB20130405
        If Liquidaciones_OTSelected.TASAINTERESNOMINAL = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la tasa nominal.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        'SLB20130405
        If _Liquidaciones_OTSelected.RENTAFIJA Then
            If _Liquidaciones_OTSelected.TASAEFECTIVAANUAL = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la tasa efectiva.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidacion = True
                Exit Sub
            End If
        Else

            If IsNothing(Liquidaciones_OTSelected.INDICADOR) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el indicador.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidacion = True
                Exit Sub
            End If

            If Liquidaciones_OTSelected.PUNTOSINDICADOR = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar los puntos del indicador.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidacion = True
                Exit Sub
            End If

        End If

        'If Liquidaciones_OTSelected.Mercado <> "P" Then

        '    If IsNothing(Liquidaciones_OTSelected.EMISION) Then
        '        A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la fecha de emisión del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '        logValidacion = True
        '        Exit Sub
        '    End If

        '    If IsNothing(Liquidaciones_OTSelected.VENCIMIENTO) Then
        '        A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la fecha de vencimiento del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '        logValidacion = True
        '        Exit Sub
        '    End If

        '    If IsNothing(Liquidaciones_OTSelected.MODALIDADTASANOMINAL) Then
        '        A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la modalidad del título.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '        logValidacion = True
        '        Exit Sub
        '    End If

        'End If


        'Validaciones de Tasa Fija 
        'If Liquidaciones_OTSelected.RENTAFIJA = True Then

        '    If Liquidaciones_OTSelected.TASAINTERESNOMINAL = 0 Then
        '        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la tasa nominal.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '        logValidacion = True
        '        Exit Sub
        '    End If

        '    'Validaciones de Tasa Variable
        'Else

        '    If IsNothing(Liquidaciones_OTSelected.INDICADOR) Then
        '        A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el indicador.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '        logValidacion = True
        '        Exit Sub
        '    End If

        '    If Liquidaciones_OTSelected.PUNTOSINDICADOR = 0 Then
        '        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar los puntos del indicador.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '        logValidacion = True
        '        Exit Sub
        '    End If

        'End If

        'INICIO SLB20130401 Validaciones Detalle Receptores
        Dim intNroLideres As Integer = 0
        Dim intPorcentajeComision As Integer = 0
        Dim intNroRepetidos As Integer = 0
        Dim lstReceptores As New List(Of String)

        If Not IsNothing(ListaReceptores) Then
            If ListaReceptores.Count > 0 Then
                For Each obj In ListaReceptores
                    If Not lstReceptores.Contains(obj.IDReceptor) Then 'CCM20120305 - Validar receptores duplicados y solamente considerar el primero
                        lstReceptores.Add(obj.IDReceptor)
                        intPorcentajeComision += obj.Porcentaje

                        If IsNothing(obj.IDReceptor) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el receptor", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logValidacion = True
                            Exit Sub
                        End If

                        If obj.Porcentaje = 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("El porcentaje de los receptores debe ser mayor a cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logValidacion = True
                            Exit Sub
                        End If

                        If obj.Lider Then 'CCM20120305 - Validar que solamente exista un lider
                            intNroLideres += 1
                        End If
                    Else
                        intPorcentajeComision += obj.Porcentaje
                        intNroRepetidos += 1
                    End If
                Next
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar al menos un receptor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidacion = True
                Exit Sub
            End If
        Else
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar al menos un receptor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If


        If intPorcentajeComision <> 100.0 Then
            A2Utilidades.Mensajes.mostrarMensaje("El porcentaje de distribución de comisión entre los receptores de la orden debe ser cien (100). En este momento está en " & intPorcentajeComision.ToString("N0"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If intNroLideres = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Se debe seleccionar un receptor líder para la distribución de la comisión.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        ElseIf intNroLideres > 1 Then
            A2Utilidades.Mensajes.mostrarMensaje("Solamente puede seleccionarse un receptor líder para la distribución de la comisión. En este momento hay " & intNroLideres.ToString("N0") & " receptores seleccionados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If intNroRepetidos > 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Se encontraron receptores repetidos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If
        'FIN SLB20130401 Validaciones Detalle Receptores

        If IsNothing(Liquidaciones_OTSelected.REGISTROOPERACION) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el tipo de sistema.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If IsNothing(Liquidaciones_OTSelected.NOMBRESISTEMA) Or Liquidaciones_OTSelected.NOMBRESISTEMA = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el nombre del sistema.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If IsNothing(Liquidaciones_OTSelected.NUMEROOPERACION) Or Liquidaciones_OTSelected.NUMEROOPERACION = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el número de operación del sistema respectivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If Liquidaciones_OTSelected.MONTO = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("El monto no debe ser cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If Liquidaciones_OTSelected.CANTIDADNEGOCIADA = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("La cantidad no debe ser cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidacion = True
            Exit Sub
        End If

        If logValidacion = False Then

            If Not IsNothing(Liquidaciones_OTSelected.CUMPLIMIENTO) Then
                If CType(Liquidaciones_OTSelected.CUMPLIMIENTO, DateTime).Year < 1753 Then
                    logValidacion = True
                    A2Utilidades.Mensajes.mostrarMensaje("El valor mínimo permitido para la fecha de cumplimiento es '01/01/1753'", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                    Exit Sub
                End If
            End If

            If Not IsNothing(Liquidaciones_OTSelected.OPERACION) Then
                If CType(Liquidaciones_OTSelected.OPERACION, DateTime).Year < 1753 Then
                    logValidacion = True
                    A2Utilidades.Mensajes.mostrarMensaje("El valor mínimo permitido para la fecha de operación es '01/01/1753'", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                    Exit Sub
                End If
            End If

            If Not IsNothing(Liquidaciones_OTSelected.EMISION) Then
                If CType(Liquidaciones_OTSelected.EMISION, DateTime).Year < 1753 Then
                    logValidacion = True
                    A2Utilidades.Mensajes.mostrarMensaje("El valor mínimo permitido para la fecha de emisión es '01/01/1753'", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                    Exit Sub
                End If
            End If

            If Not IsNothing(Liquidaciones_OTSelected.VENCIMIENTO) Then
                If CType(Liquidaciones_OTSelected.VENCIMIENTO, DateTime).Year < 1753 Then
                    logValidacion = True
                    A2Utilidades.Mensajes.mostrarMensaje("El valor mínimo permitido para la fecha de vencimiento es '01/01/1753'", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                    Exit Sub
                End If
            End If

        End If

        'SLB20130401 Validaciones Faltantes
        If Liquidaciones_OTSelected.CUMPLIMIENTO.Date < CType(Liquidaciones_OTSelected.OPERACION, DateTime).Date Then
            logValidacion = True
            A2Utilidades.Mensajes.mostrarMensaje("El fecha de cumplimiento debe ser mayor o igual que la fecha liquidación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
            Exit Sub
        End If

    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If Not IsNothing(ListaReceptores) Then
                If ListaReceptores.Where(Function(i) String.IsNullOrEmpty(i.IDReceptor)).Count > 0 Then
                    Dim logSalir As Boolean = False
                    While logSalir = False
                        If ListaReceptores.Where(Function(i) String.IsNullOrEmpty(i.IDReceptor)).Count > 0 Then
                            ListaReceptores.Remove(ListaReceptores.Where(Function(i) String.IsNullOrEmpty(i.IDReceptor)).First)
                        Else
                            logSalir = True
                        End If
                    End While
                End If
            End If

            If validaFechaCierre("Actualizar") Then
                IsBusy = True
                logValidacion = False
                validaciones()
                If logValidacion = False Then
                    Dim origen = "update"
                    ErrorForma = ""
                    Liquidaciones_OTAnterior = Liquidaciones_OTSelected
                    If Not ListaLiquidaciones_OTC.Contains(Liquidaciones_OTSelected) Then
                        origen = "insert"
                        For Each item In ListaReceptores
                            Liquidaciones_OTSelected.ReceptoresOTs.Add(item)
                        Next
                        ListaLiquidaciones_OTC.Add(Liquidaciones_OTSelected)
                    End If
                    'IsBusy = True
                    Program.VerificarCambiosProxyServidor(dcProxy)
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                Else
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            Else
                If So.UserState = "insert" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El registro se creó exitosamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                ElseIf So.UserState = "update" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El registro se actualizó exitosamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                End If

                intIDRegistro = _Liquidaciones_OTSelected.ID

                MyBase.QuitarFiltroDespuesGuardar()
                IsBusy = True
                dcProxy.Liquidaciones_OTs.Clear()
                dcProxy.Load(dcProxy.Liquidaciones_OTCFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones_OTC, "TERMINAGUARDAR")
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_Liquidaciones_OTSelected) Then
            If _Liquidaciones_OTSelected.VERSION = 1 Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("La contraparte no se puede modificar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If _Liquidaciones_OTSelected.ESTADO = "A" Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("La liquidación está en estado Anulada, por lo tanto no se puede modificar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ElseIf _Liquidaciones_OTSelected.ESTADO = "I" Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("La liquidación está en estado Impresa, por lo tanto no se puede modificar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ElseIf _Liquidaciones_OTSelected.Cargado = True Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("La liquidación fue cargada por el archivo, por lo tanto no se puede modificar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ElseIf _Liquidaciones_OTSelected.ESTADO = "P" Then
                    Editando = True
                    If _Liquidaciones_OTSelected.ESPECIE_ESACCION = "A" Then
                        logHabilitaAccion = False
                    Else
                        logHabilitaAccion = True
                    End If
                End If
                'If logEstadoAnulada Then
                '    A2Utilidades.Mensajes.mostrarMensaje("La liquidación está en estado Anulada, por lo tanto no se puede modificar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'ElseIf logEstadoImpresa Then
                '    A2Utilidades.Mensajes.mostrarMensaje("La liquidación está en estado Impresa, por lo tanto no se puede modificar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'ElseIf logEstadoActiva Then
                '    Editando = True
                'End If
            End If
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_Liquidaciones_OTSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                logHabilitaAccion = False
                If _Liquidaciones_OTSelected.EntityState = EntityState.Detached Then
                    Liquidaciones_OTSelected = Liquidaciones_OTAnterior
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
            If validaFechaCierre("Anular") Then
                If Not IsNothing(_Liquidaciones_OTSelected) Then
                    If _Liquidaciones_OTSelected.VERSION = 1 Then
                        A2Utilidades.Mensajes.mostrarMensaje("La contraparte no se puede Anular", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        If _Liquidaciones_OTSelected.ESTADO = "A" Then
                            A2Utilidades.Mensajes.mostrarMensaje("La liquidación ya está Anulada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        ElseIf _Liquidaciones_OTSelected.ESTADO = "I" Then
                            A2Utilidades.Mensajes.mostrarMensaje("La liquidación está en estado Impresa, por lo tanto no se puede anular", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        ElseIf _Liquidaciones_OTSelected.ESTADO = "P" Then
                            'C1.Silverlight.C1MessageBox.Show("La liquidación no se puede eliminar, desea anularla", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPregunta)
                            mostrarMensajePregunta("La liquidación no se puede eliminar", _
                                                   Program.TituloSistema, _
                                                   "BORRARREGISTRO", _
                                                   AddressOf TerminaPregunta, True, "¿Desea anularla?")
                        End If
                        'If logEstadoAnulada Then
                        '    A2Utilidades.Mensajes.mostrarMensaje("La liquidación ya está Anulada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'ElseIf logEstadoImpresa Then
                        '    A2Utilidades.Mensajes.mostrarMensaje("La liquidación está en estado Impresa, por lo tanto no se puede anular", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'ElseIf logEstadoActiva Then
                        '    C1.Silverlight.C1MessageBox.Show("La liquidación no se puede eliminar, desea anularla", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPregunta)
                        'End If
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un registro para anular", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            IsBusy = True
            dcProxy1.Liquidaciones_OTCAnular(_Liquidaciones_OTSelected.ID, Program.Usuario, Program.HashConexion, AddressOf TerminoAnulariquidaciones_OTC, "")
        End If
    End Sub

    Public Overrides Sub Buscar()
        cb.CUMPLIMIENTO = Nothing
        cb.OPERACION = Nothing
        cb.ID = Nothing
        cb.NUMEROOPERACION = Nothing
        cb.IDESPECIE = String.Empty
        cb.IDCOMITENTE = String.Empty
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Valida la Fecha de Cierre del Sistema
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>SLB20121005</remarks>
    Private Function validaFechaCierre(ByVal pstrAccion As String) As Boolean
        validaFechaCierre = True
        If Format(CType(_Liquidaciones_OTSelected.OPERACION, Date).Date, "yyyy/MM/dd") <= Format(FechaCierre, "yyyy/MM/dd") Then 'Intentan registrar un documento con fecha inferior a la fecha de cierre registrada en tblInstalacion
            If Format(FechaCierre, "yyyy/MM/dd") <> "1900/01/01" Then
                Select Case pstrAccion
                    Case "Anular"
                        A2Utilidades.Mensajes.mostrarMensaje("El documento con fecha (" & CType(_Liquidaciones_OTSelected.OPERACION, Date).Date.ToLongDateString & ") no puede ser anulado porque su fecha es inferior a la fecha de cierre (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Case "Actualizar"
                        A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & CType(_Liquidaciones_OTSelected.OPERACION, Date).Date.ToLongDateString & ") no puede ser ingresada o modificada porque su fecha es inferior a la fecha de cierre (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End Select
                validaFechaCierre = False
            End If
        End If
        Return validaFechaCierre
    End Function

#End Region

#Region "Receptores"

    Private _ListaReceptores As EntitySet(Of ReceptoresOT)
    Public Property ListaReceptores() As EntitySet(Of ReceptoresOT)
        Get
            Return _ListaReceptores
        End Get
        Set(ByVal value As EntitySet(Of ReceptoresOT))
            _ListaReceptores = value
            MyBase.CambioItem("ListaReceptores")

            If Not IsNothing(value) Then
                ReceptorSelected = ListaReceptores.FirstOrDefault
            End If
        End Set
    End Property

    Private _ReceptorSelected As ReceptoresOT
    Public Property ReceptorSelected() As ReceptoresOT
        Get
            Return _ReceptorSelected
        End Get
        Set(ByVal value As ReceptoresOT)
            _ReceptorSelected = value
            MyBase.CambioItem("ReceptorSelected")
        End Set
    End Property

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptores"
                Dim NewReceptor As New ReceptoresOT
                NewReceptor.IDReceptoresOTC = -1
                If Liquidaciones_OTSelected.IdLiquidaciones_OTC <> -1 Then
                    NewReceptor.Id = Liquidaciones_OTSelected.ID
                Else
                    NewReceptor.Id = -1
                End If

                NewReceptor.Usuario = Program.Usuario

                If IsNothing(ListaReceptores) Then
                    If Not IsNothing(dcProxy.ReceptoresOTs) Then
                        dcProxy.ReceptoresOTs.Clear()
                    End If
                    ListaReceptores = dcProxy.ReceptoresOTs
                End If

                ListaReceptores.Add(NewReceptor)
                ReceptorSelected = NewReceptor
                MyBase.CambioItem("ListaReceptores")
                MyBase.CambioItem("ReceptorSelected")
        End Select
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()

        If Not IsNothing(ListaReceptores) Then
            If Not (IsNothing(ReceptorSelected)) Then
                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptorSelected, ListaReceptores.ToList)

                ListaReceptores.Remove(ReceptorSelected)
                If ListaReceptores.Count > 0 Then
                    Program.PosicionarItemLista(ReceptorSelected, ListaReceptores.ToList, intRegistroPosicionar)
                Else
                    ReceptorSelected = Nothing
                End If
                MyBase.CambioItem("ListaReceptores")
                MyBase.CambioItem("ReceptorSelected")
            End If
        End If

    End Sub

#End Region

#Region "Buscadores"

    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.Entities.ToList.Item(0).CodEstado <> "I" Then
                    _Liquidaciones_OTSelected.IDCOMITENTE = lo.Entities.ToList.Item(0).CodigoOYD
                    _Liquidaciones_OTSelected.NOMBRECLIENTE = lo.Entities.ToList.Item(0).Nombre
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado está inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    _Liquidaciones_OTSelected.IDCOMITENTE = String.Empty
                    _Liquidaciones_OTSelected.NOMBRECLIENTE = String.Empty
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                _Liquidaciones_OTSelected.IDCOMITENTE = String.Empty
                _Liquidaciones_OTSelected.NOMBRECLIENTE = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Private Sub buscarNemotecnicoCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Try
            If lo.Entities.ToList.Count > 0 Then
                _Liquidaciones_OTSelected.IDESPECIE = lo.Entities.ToList.Item(0).Nemotecnico
                _Liquidaciones_OTSelected.NOMBREESPECIE = lo.Entities.ToList.Item(0).Especie
                _Liquidaciones_OTSelected.ESPECIE_ESACCION = IIf(lo.Entities.ToList.Item(0).EsAccion, "A", "C")
                'mstrEsAccion = IIf(lo.Entities.ToList.Item(0).EsAccion, "A", "C")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El nemotécnico ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                _Liquidaciones_OTSelected.IDESPECIE = String.Empty
                _Liquidaciones_OTSelected.NOMBREESPECIE = String.Empty
                _Liquidaciones_OTSelected.ESPECIE_ESACCION = String.Empty
                'mstrEsAccion = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del nemotecnico", Me.ToString(), "buscarNemotecnicoCompleted", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Private Sub buscarRepresentanteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.Entities.ToList.Count > 0 Then
                _Liquidaciones_OTSelected.IDREPRESENTANTELEGAL = lo.Entities.ToList.Item(0).CodItem
                _Liquidaciones_OTSelected.NOMBREREPRESENTANTE = lo.Entities.ToList.Item(0).Nombre
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El representante ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                _Liquidaciones_OTSelected.IDREPRESENTANTELEGAL = String.Empty
                _Liquidaciones_OTSelected.NOMBREREPRESENTANTE = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del representante", Me.ToString(), "buscarRepresentanteCompleted", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

#End Region

    Private Sub _Liquidaciones_OTSelected_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _Liquidaciones_OTSelected.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName = "MONTO" Or e.PropertyName = "CANTIDADNEGOCIADA" Then
                    CalcularPrecio()

                ElseIf e.PropertyName = "VENCIMIENTO" Or e.PropertyName = "OPERACION" Then
                    CalcularDiasVencimiento("fechas")
                ElseIf e.PropertyName = "DIASALVENCIMIENTOTITULO" Then
                    CalcularDiasVencimiento("dias")
                ElseIf e.PropertyName = "IDCOMITENTE" Then
                    If _Liquidaciones_OTSelected.IDCOMITENTE <> String.Empty Then
                        If ConsultodesdeBoton Then
                            IsBusy = True
                            mdcProxyUtilidad01.BuscadorClientes.Clear()
                            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarClienteEspecificoQuery(_Liquidaciones_OTSelected.IDCOMITENTE, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, "")
                        End If
                    End If

                ElseIf e.PropertyName = "IDESPECIE" Then
                    If _Liquidaciones_OTSelected.IDESPECIE <> String.Empty Then
                        If ConsultodesdeBoton Then
                            IsBusy = True
                            mdcProxyUtilidad01.BuscadorEspecies.Clear()
                            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarNemotecnicoEspecificoQuery("T", Liquidaciones_OTSelected.IDESPECIE, Program.Usuario, Program.HashConexion), AddressOf buscarNemotecnicoCompleted, "")
                        End If
                    End If

                ElseIf e.PropertyName = "IDREPRESENTANTELEGAL" Then
                    If _Liquidaciones_OTSelected.IDREPRESENTANTELEGAL <> String.Empty Then
                        If ConsultodesdeBoton Then
                            IsBusy = True
                            mdcProxyUtilidad01.BuscadorGenericos.Clear()
                            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemEspecificoQuery("representantes", Liquidaciones_OTSelected.IDREPRESENTANTELEGAL, Program.Usuario, Program.HashConexion), AddressOf buscarRepresentanteCompleted, "")
                        End If
                    End If

                ElseIf e.PropertyName = "RENTAFIJA" Then
                    If _Liquidaciones_OTSelected.RENTAFIJA Then
                        logTasaFija = True
                        _Liquidaciones_OTSelected.INDICADOR = Nothing
                        _Liquidaciones_OTSelected.PUNTOSINDICADOR = Nothing
                    Else
                        logTasaFija = False
                        _Liquidaciones_OTSelected.TASAEFECTIVAANUAL = Nothing
                        _Liquidaciones_OTSelected.INDICADOR = "0"
                    End If

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir el cambio de la propiedad", Me.ToString(), "_Liquidaciones_OTSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex)
            'Finally
            '    IsBusy = False
        End Try
    End Sub

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaLiquidaciones_OT
    Implements INotifyPropertyChanged


    Private _ID As Nullable(Of Integer)
    <Display(Name:="Nro. Operación")> _
    Public Property ID() As Nullable(Of Integer)
        Get
            Return _ID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _CUMPLIMIENTO As DateTime?
    <Display(Name:="Fecha Cumplimiento")> _
    Public Property CUMPLIMIENTO() As DateTime?
        Get
            Return _CUMPLIMIENTO
        End Get
        Set(ByVal value As DateTime?)
            _CUMPLIMIENTO = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CUMPLIMIENTO"))
        End Set
    End Property

    Private _OPERACION As DateTime?
    <Display(Name:="Fecha Operación")> _
    Public Property OPERACION() As DateTime?
        Get
            Return _OPERACION
        End Get
        Set(ByVal value As DateTime?)
            _OPERACION = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("OPERACION"))
        End Set
    End Property

    <Display(Name:="Contraparte ")> _
    Private _IDCOMITENTE As String
    Public Property IDCOMITENTE() As String
        Get
            Return _IDCOMITENTE
        End Get
        Set(ByVal value As String)
            _IDCOMITENTE = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDCOMITENTE"))
        End Set
    End Property

    <Display(Name:="Especie", Description:="Especie")> _
    Private _IDESPECIE As String
    Public Property IDESPECIE() As String
        Get
            Return _IDESPECIE
        End Get
        Set(ByVal value As String)
            _IDESPECIE = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDESPECIE"))
        End Set
    End Property

    Private _NUMEROOPERACION As Nullable(Of Integer)
    <Display(Name:="Operación")> _
    Public Property NUMEROOPERACION() As Nullable(Of Integer)
        Get
            Return _NUMEROOPERACION
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _NUMEROOPERACION = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NUMEROOPERACION"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class


''' <summary>
''' Clase para manejo en vista de datos de entidad de datos de archivo excel
''' </summary>
''' <remarks></remarks>
Public Class ArchivoReposBanRepVista
    Implements INotifyPropertyChanged

    Private _intID As Integer
    Public Property intID() As Integer
        Get
            Return _intID
        End Get
        Set(ByVal value As Integer)
            _intID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intID"))
        End Set
    End Property

    Private _id As Integer
    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("id"))
        End Set
    End Property

    Private _idProceso As Integer
    Public Property idProceso() As Integer
        Get
            Return _idProceso
        End Get
        Set(ByVal value As Integer)
            _idProceso = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("idProceso"))
        End Set
    End Property

    Private _CodTitulo As String
    Public Property CodTitulo() As String
        Get
            Return _CodTitulo
        End Get
        Set(ByVal value As String)
            _CodTitulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodTitulo"))
        End Set
    End Property

    Private _FechaLiquidacion As DateTime
    Public Property FechaLiquidacion() As DateTime
        Get
            Return _FechaLiquidacion
        End Get
        Set(ByVal value As DateTime)
            _FechaLiquidacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaLiquidacion"))
        End Set
    End Property

    Private _NroEmision As String
    Public Property NroEmision() As String
        Get
            Return _NroEmision
        End Get
        Set(ByVal value As String)
            _NroEmision = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroEmision"))
        End Set
    End Property

    Private _Tipo As String
    Public Property Tipo() As String
        Get
            Return _Tipo
        End Get
        Set(ByVal value As String)
            _Tipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Tipo"))
        End Set
    End Property

    Private _Nemotecnico As String
    Public Property Nemotecnico() As String
        Get
            Return _Nemotecnico
        End Get
        Set(ByVal value As String)
            _Nemotecnico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nemotecnico"))
        End Set
    End Property

    Private _ISIN As String
    Public Property ISIN() As String
        Get
            Return _ISIN
        End Get
        Set(ByVal value As String)
            _ISIN = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ISIN"))
        End Set
    End Property

    Private _Precio As Decimal
    Public Property Precio() As Decimal
        Get
            Return _Precio
        End Get
        Set(ByVal value As Decimal)
            _Precio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Precio"))
        End Set
    End Property

    Private _TasaEfectiva As Decimal
    Public Property TasaEfectiva() As Decimal
        Get
            Return _TasaEfectiva
        End Get
        Set(ByVal value As Decimal)
            _TasaEfectiva = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TasaEfectiva"))
        End Set
    End Property

    Private _ValorNominal As Decimal
    Public Property ValorNominal() As Decimal
        Get
            Return _ValorNominal
        End Get
        Set(ByVal value As Decimal)
            _ValorNominal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ValorNominal"))
        End Set
    End Property

    Private _ValorOperacion As Decimal
    Public Property ValorOperacion() As Decimal
        Get
            Return _ValorOperacion
        End Get
        Set(ByVal value As Decimal)
            _ValorOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ValorOperacion"))
        End Set
    End Property

    Private _NroOferta As String
    Public Property NroOferta() As String
        Get
            Return _NroOferta
        End Get
        Set(ByVal value As String)
            _NroOferta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroOferta"))
        End Set
    End Property

    Private _ValorRestitucion As Decimal
    Public Property ValorRestitucion() As Decimal
        Get
            Return _ValorRestitucion
        End Get
        Set(ByVal value As Decimal)
            _ValorRestitucion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ValorRestitucion"))
        End Set
    End Property

    Private _FechaRestitucion As DateTime
    Public Property FechaRestitucion() As DateTime
        Get
            Return _FechaRestitucion
        End Get
        Set(ByVal value As DateTime)
            _FechaRestitucion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaRestitucion"))
        End Set
    End Property

    Private _strUsuarioArchivo As String
    Public Property strUsuarioArchivo() As String
        Get
            Return _strUsuarioArchivo
        End Get
        Set(ByVal value As String)
            _strUsuarioArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strUsuarioArchivo"))
        End Set
    End Property

    Private _bitGenerarOrden As Boolean
    Public Property bitGenerarOrden() As Boolean
        Get
            Return _bitGenerarOrden
        End Get
        Set(ByVal value As Boolean)
            _bitGenerarOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("bitGenerarOrden"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class





