Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: CustodiaViewModel.vb
'Generado el : 06/17/2011 11:58:21
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
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio
Imports A2Utilidades.Mensajes
Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades

Public Class CustodiaViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCustodi
    Private CustodiPorDefecto As CFPortafolio.Custodia
    Private CustodiAnterior As CFPortafolio.Custodia
    Private DetalleCustodiaPorDefecto As CFPortafolio.DetalleCustodia
    Dim dcProxy As PortafolioDomainContext
    Dim dcProxy1 As PortafolioDomainContext
    Public objProxy As UtilidadesDomainContext
    Dim dcProxy3 As PortafolioDomainContext
    Dim mensaje As String
    Dim MakeAndCheck As Integer = 0
    Dim esVersion As Boolean = False
    Dim codigo As Integer
    Dim secuencias As Integer = 1
    Dim fechaCierre As DateTime
    Dim intSecuencia As Integer
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim dtmFechaEmision As Date
    Dim dtmFechaVencimiento As Date

    Dim logBorrarRegistro As Boolean = False

    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se adiciona variable para control de busqueda de los clientes ya sea por la lupita o por ID para autocompletar.
    ''' Fecha            : Marzo 06/2013
    ''' Pruebas Negocio  : Yeiny Adenis Marín Zapata - Marzo 06/2013 - Resultado Ok 
    ''' </history>
    Public _mlogBuscarClienteEncabezado As Boolean = True
    Public _mLogBuscarClienteDetalle As Boolean = True
    Public _mlogBuscarEspecie As Boolean = True
    'Dim _mlngEmision As Integer = Nothing
    'Dim _mLogTipoTasa As String
    'Dim _mLogEsAccion As Boolean = False
    Dim _mStrIDClienteSeleccionadoEncabezado As String
    Public _mLogAgregarRegistroDetalle As Boolean = True
    Public _mlogBuscadorCuenta As Boolean = False
    Public _mlogBuscarFondo As Boolean = False
    Public _strParametroCrucePorComercial As String = "NO"
    Public _mStrFondo As String

    Dim intNroDetallesValidar As Integer
    Dim strDeposito As String = String.Empty
    Private mstrAccion As String = String.Empty 'SLB20131211 Este indicador ayuda a controla la ejecución de consultas inutiles durante el ingreso especialmente
    Private intIDInsertado As Integer = 0
    Private logRecargarSelected As Boolean = True
    Private logRecargarDetallesSelected As Boolean = True
    Private mdtmFechaCierrePortafolio As DateTime? = Nothing
    Dim cwDetalleCustodiasCambiarFechaReciboView As cwDetalleCustodiasCambiarFechaReciboView
    Dim totalCustodiasCargadas As Integer = 0
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista = Nothing

#Region "Constantes"

    Private Const MSTR_MC_ACCION_INGRESAR As String = "I"
    Private Const MSTR_MC_ACCION_ACTUALIZAR As String = "U"
    Private Const MSTR_MC_ACCION_BORRAR As String = "D"
    Private MDTM_FECHA_CIERRE_SIN_ACTUALIZAR As Date = New Date(1999, 1, 1) 'JP20151120: Fecha para inicializar fecha de cierre

#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New PortafolioDomainContext()
                dcProxy1 = New PortafolioDomainContext()
                dcProxy3 = New PortafolioDomainContext()
                objProxy = New UtilidadesDomainContext()
            Else
                dcProxy = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
                dcProxy1 = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
                dcProxy3 = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            End If

            If Not Program.IsDesignMode() Then
                DirectCast(dcProxy.DomainClient, WebDomainClient(Of PortafolioDomainContext.IPortafolioDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)
                DirectCast(dcProxy1.DomainClient, WebDomainClient(Of PortafolioDomainContext.IPortafolioDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)
                DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)
                DirectCast(dcProxy3.DomainClient, WebDomainClient(Of PortafolioDomainContext.IPortafolioDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)

                IsBusy = True
                dcProxy.Load(dcProxy.CustodiaFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodia, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerCustodiPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodiaPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerDetalleCustodiaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleCustodias_Completed, "Default")
                objProxy.consultarFechaCierre("T", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "") 'SLB20140424 Módulo Títulos (T)
                objProxy.Load(objProxy.listaVerificaparametroQuery("", "Custodias", Program.Usuario, Program.HashConexion), AddressOf Terminotraerparametrolista, Nothing) 'SV20150305
                objProxy.Verificaparametro("CF_CRUCE_OPERACIONES_X_COMERCIAL", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "CustodiaViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCustodiaPorDefecto_Completed(ByVal lo As LoadOperation(Of CFPortafolio.Custodia))
        If Not lo.HasError Then
            CustodiPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Custodi por defecto",
                                             Me.ToString(), "TerminoTraerCustodiPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <summary>
    ''' Operacion asincrona que se ejecuta cuando se termina de consultar las custodias para actualizar las listas de datos y setear valores de acuerdo a 
    ''' estados u otras condicionales.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se adiciona logica para ocultar columnas cuando la version no posee maker and Checker.
    ''' Fecha            : Febrero 19/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Febrero 20/2013 - Resultado Ok 
    ''' </history>
    Private Sub TerminoTraerCustodia(ByVal lo As LoadOperation(Of CFPortafolio.Custodia))
        If Not lo.HasError Then
            If lo.UserState = "insercionregistro" Then
                logRecargarSelected = False
                If Not IsNothing(intIDInsertado) Then
                    logRecargarDetallesSelected = False
                End If
            End If

            ListaCustodia = dcProxy.Custodias

            If lo.UserState = "insercionregistro" Then
                logRecargarSelected = True
                logRecargarDetallesSelected = True
            End If

            ManejaMakerAndChecker = False
            RegistroTieneCambios = False
            TextoVersion = String.Empty
            ColumnasVisibles = Visibility.Collapsed
            MyBase.CambioItem("ColumnasVisibles")

            If dcProxy.Custodias.Count > 0 Then

                If lo.UserState = "insercionregistro" Then
                    If Not IsNothing(intIDInsertado) Then
                        If ListaCustodia.Where(Function(i) i.IdRecibo = intIDInsertado).Count > 0 Then
                            CustodiSelected = Nothing
                            CustodiSelected = ListaCustodia.Where(Function(i) i.IdRecibo = intIDInsertado).FirstOrDefault
                        Else
                            If ListaCustodia.Count > 0 Then
                                CustodiSelected = Nothing
                                CustodiSelected = ListaCustodia.FirstOrDefault
                            Else
                                CustodiSelected = Nothing
                            End If
                        End If
                    End If
                ElseIf lo.UserState = "insert" Then
                    CustodiSelected = ListaCustodia.FirstOrDefault
                End If

                If Not IsNothing(CustodiSelected.MakeAndCheck) Then
                    MakeAndCheck = CustodiSelected.MakeAndCheck
                End If

                'JCS Feb 19 2013 
                If MakeAndCheck = 1 Then 'And lo.UserState = Nothing Then
                    If CustodiSelected.Por_Aprobar = "Por Aprobar" Then
                        'Mostrar columnas Estado Aprobación, Descripción y Usuario Aprobador
                        ColumnasVisibles = Visibility.Visible
                        MyBase.CambioItem("ColumnasVisibles")
                        CustodiSelected = ListaCustodia.FirstOrDefault 'Where(Function(ic) ic.IdRecibo = codigo).First
                    Else
                        'Ocultar columnas Estado Aprobación, Descripción y Usuario Aprobador
                        ColumnasVisibles = Visibility.Collapsed
                        MyBase.CambioItem("ColumnasVisibles")
                    End If
                Else
                    'Ocultar columnas Estado Aprobación, Descripción y Usuario Aprobador
                    ColumnasVisibles = Visibility.Collapsed
                    MyBase.CambioItem("ColumnasVisibles")
                End If

                'Si maneja Maker and Cheker
                If MakeAndCheck = 1 Then

                    'Si está mostrando la columna Por Aprobar
                    If Not IsNothing(CustodiSelected.Por_Aprobar) Then

                        'Está mostrando las custodias Pendientes por Aprobar

                        If CustodiSelected.DescripcionEstado.Equals("Ingreso") Then
                            RegistroTieneCambios = False
                        Else
                            RegistroTieneCambios = True
                        End If

                        ManejaMakerAndChecker = True

                        TextoVersion = POR_APROBAR 'Mostrar el texto 'versión por aprobar' si tiene cambios pendientes

                    Else

                        'Está mostrando las custodias Aprobadas

                        TextoVersion = APROBADA 'Mostrar el texto 'version aprobada' si tiene cambios pendientes

                        If Not IsNothing(CustodiSelected.Por_Aprobar) Then
                            RegistroTieneCambios = False
                            ManejaMakerAndChecker = False
                        End If

                    End If
                End If
            Else
                CustodiSelected = Nothing
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro que cumpla con las condiciones de búsqueda ingresadas", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Custodia",
                                             Me.ToString(), "TerminoTraerCustodi", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres

    Private Sub TerminoTraerDetalleCustodias(ByVal lo As LoadOperation(Of CFPortafolio.DetalleCustodia))
        If Not lo.HasError Then
            ListaDetalleCustodias = dcProxy.DetalleCustodias
            totalCustodiasCargadas = ListaDetalleCustodias.Count
            If Not IsNothing(ListaDetalleCustodias) Then
                For Each li In ListaDetalleCustodias
                    li.logModificarDetalleRespaldo = False
                Next
            End If

            'IsBusy = True
            'For Each i In ListaDetalleCustodias

            '    If Not IsNothing(listaCuentas) Then
            '        listaCuentas.Clear()
            '    End If
            '    dcProxy.Load(dcProxy.TraerCuentasConsultarQuery(0, i.Comitente), AddressOf TerminoTraerCuentas, Nothing)
            'Next
            'IsBusy = False
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleCustodias",
                                             Me.ToString(), "TerminoTraerDetalleCustodias", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    ''' <history>
    ''' Creado por      : Juan Esteban Restrepo Franco.
    ''' Descripción     : Logica para determinar si se debe habilitar el receptor en el detalle
    ''' Fecha           : Enero 09/2019
    ''' Pruebas Negocio : 
    ''' </history>
    Friend Sub ValidarHabilitarReceptor()
        Try
            LogHabilitarReceptor = False
            ''Si es posición propia se habilita el recepto
            If CustodiSelected IsNot Nothing AndAlso Not String.IsNullOrEmpty(CustodiSelected.strTipoCompania) AndAlso CustodiSelected.strTipoCompania = "PP" AndAlso _strParametroCrucePorComercial = "SI" Then
                LogHabilitarReceptor = True
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrió un error al validar si se habilita el receptor",
                                             Me.ToString(), "ValidarHabilitarReceptor", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            _strParametroCrucePorComercial = lo.Value.ToString()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los paramétros",
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <summary>
    ''' Operacion de carga asincronica de tipo BeneficiariosCustodia
    ''' </summary>
    ''' <param name="lo">LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OyDTitulos.BeneficiariosCustodia)</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por      : Juan Carlos Soto.
    ''' Descripción     : Se adiciona el llamado a la operacion de consulta de la lista de Beneficiarios de Custodias.
    ''' Fecha           : Febrero 26/2013
    ''' Pruebas Negocio : No se le han hecho pruebas de caja blanca. 
    ''' </history>
    Private Sub TerminoTraerBeneficiariosCustodia(ByVal lo As LoadOperation(Of CFPortafolio.BeneficiariosCustodia))
        If Not lo.HasError Then
            ListaBeneficiariosCustodias = dcProxy.BeneficiariosCustodias
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BeneficiariosCustodias",
                                             Me.ToString(), "TerminoTraerBeneficiariosCustodia", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub TerminoTraerDetalleCustodias_Completed(ByVal lo As LoadOperation(Of CFPortafolio.DetalleCustodia))
        If Not lo.HasError Then
            DetalleCustodiaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del detallecustodia por defecto",
                                             Me.ToString(), "TerminoTraerDetalleCustodias_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Async Function consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date))) As Task
        Try
            If obj.HasError Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            Else
                fechaCierre = obj.Value

                If obj.UserState = "EDITAR" Then
                    If Await ValidarFechaCierrePortafolio() Then
                        ContinuarEdicionDocumento()
                    Else
                        MyBase.RetornarValorEdicionNavegacion()
                    End If
                ElseIf obj.UserState = "GUARDAR" Then
                    If Await ValidarFechaCierrePortafolio() Then
                        ContinuarGuardadoDocumento()
                    End If
                ElseIf obj.UserState = "BORRAR" Then
                    Dim strMsg As String = String.Empty
                    Dim logPortafolioHabilitado As Boolean = True ' JP20151120: Si el portafolio no está cerrado se puede modificar

                    logPortafolioHabilitado = Await ValidarFechaCierrePortafolio(False)
                    strMsg = "anular"
                    EstaBorrando = True
                    If logPortafolioHabilitado Then ' JP20151120: Validar si el portafolio está cerrado o no para permitir modificar la operación
                        ContinuarBorradoDocumento()
                    Else
                        If Not IsNothing(DetalleCustodiaSelected) Then
                            mostrarMensaje("La custodia no se puede " & strMsg & " porque el portafolio del cliente " & DetalleCustodiaSelected.Comitente & "-" & DetalleCustodiaSelected.Nombre & " se encuentra cerrado para la fecha de la custodia (Fecha de cierre de portafolio: " & mdtmFechaCierrePortafolio.Value.Year & "/" & mdtmFechaCierrePortafolio.Value.Month & "/" & mdtmFechaCierrePortafolio.Value.Day & ", fecha de la custodia: " & CustodiSelected.Recibo.Year & "/" & CustodiSelected.Recibo.Month & "/" & CustodiSelected.Recibo.Day & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema",
                                                 Me.ToString(), "consultarFechaCierreCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Private Sub TerminoValidarEdicion(ByVal lo As LoadOperation(Of CFPortafolio.Custodia))
        Try
            If Not lo.HasError Then
                If dcProxy3.Custodias.Count > 0 Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("La custodia no se puede editar porque tiene una versión pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    Editar()
                End If
            Else
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener las custodias por aprobar.",
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener las cutodias por aprobar.",
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoValidarborrado(ByVal lo As LoadOperation(Of CFPortafolio.Custodia))
        Try
            If Not lo.HasError Then
                If dcProxy3.Custodias.Count > 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("La custodia no se puede anular porque tiene una versión pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    borrar()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener las cutodias por aprobar.",
                                     Me.ToString(), "TerminoValidarborrado", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener las custodias por aprobar.",
                                     Me.ToString(), "TerminoValidarborrado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Recibe el resultado de la consulta de parámetros
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks>SV20150305</remarks>
    Private Sub Terminotraerparametrolista(ByVal obj As LoadOperation(Of OYDUtilidades.valoresparametro))
        If obj.HasError Then
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la lista de parametros", Me.ToString(), "Terminotraerparametrolista", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            'Dim lista = obj.Entities.ToList
            For Each li In obj.Entities.ToList
                Select Case li.Parametro
                    Case "CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES"
                        If li.Valor = "SI" Then
                            _logIncrementarDecimalesCantidad = True
                        Else
                            _logIncrementarDecimalesCantidad = False
                        End If
                End Select
            Next
        End If
    End Sub

#End Region

#Region "Propiedades"
    Private _logHabilitarReceptor As Boolean = False

    Public Property LogHabilitarReceptor As Boolean
        Get
            Return _logHabilitarReceptor
        End Get
        Set(ByVal value As Boolean)
            _logHabilitarReceptor = value
            MyBase.CambioItem("LogHabilitarReceptor")
        End Set
    End Property


    Private _ListaCustodia As EntitySet(Of CFPortafolio.Custodia)
    Public Property ListaCustodia() As EntitySet(Of CFPortafolio.Custodia)
        Get
            Return _ListaCustodia
        End Get
        Set(ByVal value As EntitySet(Of CFPortafolio.Custodia))
            _ListaCustodia = value
            MyBase.CambioItem("ListaCustodia")
            MyBase.CambioItem("ListaCustodiaPaged")
            If logRecargarSelected Then
                If Not IsNothing(value) Then
                    If IsNothing(CustodiAnterior) Then
                        CustodiSelected = _ListaCustodia.FirstOrDefault
                    Else
                        CustodiSelected = CustodiAnterior
                    End If
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCustodiaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCustodia) Then
                Dim view = New PagedCollectionView(_ListaCustodia)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _EstadoAnulada As Boolean
    Public Property EstadoAnulada As Boolean
        Get
            Return _EstadoAnulada
        End Get
        Set(ByVal value As Boolean)
            _EstadoAnulada = value
            MyBase.CambioItem("EstadoAnulada")
        End Set
    End Property
    Private _EstadoImpresa As Boolean
    Public Property EstadoImpresa As Boolean
        Get
            Return _EstadoImpresa
        End Get
        Set(ByVal value As Boolean)
            _EstadoImpresa = value
            MyBase.CambioItem("EstadoImpresa")
        End Set
    End Property
    Private _EstadoPendiente As Boolean
    Public Property EstadoPendiente As Boolean
        Get
            Return _EstadoPendiente
        End Get
        Set(ByVal value As Boolean)
            _EstadoPendiente = value
            MyBase.CambioItem("EstadoPendiente")
        End Set
    End Property
    Private _Editareg As Boolean
    Public Property Editareg As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            _Editareg = value
            MyBase.CambioItem("Editareg")
        End Set
    End Property

    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Propiedad para deshabilitar los campos Nombre, NroDocumento, Tipo de Documento, Dirección y Teléfono.
    ''' Fecha            : Marzo 06/2013
    ''' Pruebas Negocio  : Yeiny Adenis Marín Zapata - Marzo 06/2013 - Resultado Ok 
    ''' </history>

    Private _HabilitarEdicion As Boolean
    Public Property HabilitarEdicion() As Boolean
        Get
            Return _HabilitarEdicion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicion = value
            MyBase.CambioItem("HabilitarEdicion")
        End Set
    End Property

    Private _HabilitarCamposCliente As Boolean
    Public Property HabilitarCamposCliente() As Boolean
        Get
            Return _HabilitarCamposCliente
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposCliente = value
            MyBase.CambioItem("HabilitarCamposCliente")
        End Set
    End Property


    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Propiedad para controlar los cambios en ID Cliente. Después de validar que algunos datos no esten vacíos, desencadena el método buscar comitente.
    ''' Fecha            : Marzo 06/2013
    ''' Pruebas Negocio  : Yeiny Adenis Marín Zapata - Marzo 06/2013 - Resultado Ok 
    ''' </history>
    Private Sub _CustodiSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CustodiSelected.PropertyChanged
        Try

            If e.PropertyName.Equals("Comitente") Then
                _mlogBuscarClienteEncabezado = True
                _mLogBuscarClienteDetalle = False
                If Not String.IsNullOrEmpty(CustodiSelected.Comitente) And _mlogBuscarClienteEncabezado Then
                    buscarComitente(_CustodiSelected.Comitente)
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_CustodiSelected.PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    Public Overrides Sub QuitarFiltro()
        MyBase.QuitarFiltro()

    End Sub


    ''' <history>
    ''' Modificado por       : Juan Carlos Soto.
    ''' Descripción          : Se adiciona el llamado a la operacion de consulta de la lista de Beneficiarios de Custodias.
    '''                        Se marcan las lineas del cambio asi: JCS - Feb 26/2013 - Nuevo llamado a operacion de consulta de beneficiarios.
    ''' Fecha                : Febrero 26/2013
    ''' Pruebas Negocio      : No se le han hecho pruebas de caja blanca. 
    ''' 
    ''' Modificado por       : Yeiny Adenis Marín Zapata.
    ''' Descripción          : Se agrega WithEvents a la propiedad _CustodiSelected para que este accesible desde _CustodiSelected_PropertyChanged
    ''' Fecha                : Febrero 26/2013
    ''' Pruebas Negocio      : Yeiny Adenis Marín Zapata - Marzo 06/2013 - Resultado Ok 
    ''' 
    ''' Modificado por       : Javier Eduardo Pardo Moreno
    ''' Descripción          : Se añade opción else para mostrar botón de aprobación cuando la descripción es diferente a "Ingreso" (Es decir, "Modificacion")
    ''' Fecha                : Mayo 15/2017
    ''' Pruebas Negocio      : Javier Eduardo Pardo Moreno - Mayo 15/2017 - Resultado Ok
    ''' </history>

    Private WithEvents _CustodiSelected As CFPortafolio.Custodia
    Public Property CustodiSelected() As CFPortafolio.Custodia
        Get
            Return _CustodiSelected
        End Get
        Set(ByVal value As CFPortafolio.Custodia)
            _CustodiSelected = value
            If Not value Is Nothing And logRecargarDetallesSelected Then
                If CustodiSelected.Estado.Equals("P") Then
                    EstadoPendiente = True
                    EstadoAnulada = False
                    EstadoImpresa = False
                ElseIf CustodiSelected.Estado.Equals("I") Then
                    EstadoPendiente = False
                    EstadoAnulada = False
                    EstadoImpresa = True
                ElseIf CustodiSelected.Estado.Equals("A") Then
                    EstadoPendiente = False
                    EstadoAnulada = True
                    EstadoImpresa = False

                End If

                'If esVersion = False Then
                If _CustodiSelected.Por_Aprobar Is Nothing Then
                    If Not IsNothing(ListaDetalleCustodias) Then
                        ListaDetalleCustodias = Nothing
                    End If

                    If mstrAccion = MSTR_MC_ACCION_INGRESAR Then
                        dcProxy.DetalleCustodias.Clear()
                        ListaDetalleCustodias = dcProxy.DetalleCustodias
                        dcProxy.BeneficiariosCustodias.Clear()
                        ListaBeneficiariosCustodias = dcProxy.BeneficiariosCustodias
                    Else
                        dcProxy.DetalleCustodias.Clear()
                        dcProxy.Load(dcProxy.Traer_DetalleCustodias_CustodiQuery(0, CustodiSelected.IdRecibo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleCustodias, Nothing)

                        'JCS - Feb 26/2013 - Nuevo llamado a operacion de consulta de beneficiarios.
                        dcProxy.BeneficiariosCustodias.Clear()
                        dcProxy.Load(dcProxy.Traer_BeneficiariosCustodias_CustodiQuery(CustodiSelected.IdRecibo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosCustodia, Nothing)
                        'FIN JCS - Feb 26 / 2013
                    End If
                Else
                    If Not IsNothing(ListaDetalleCustodias) Then
                        ListaDetalleCustodias = Nothing
                    End If
                    dcProxy.DetalleCustodias.Clear()
                    dcProxy.Load(dcProxy.Traer_DetalleCustodias_CustodiQuery(1, CustodiSelected.IdRecibo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleCustodias, Nothing)

                    'JCS - Feb 26/2013 - Nuevo llamado a operacion de consulta de beneficiarios.
                    dcProxy.BeneficiariosCustodias.Clear()
                    dcProxy.Load(dcProxy.Traer_BeneficiariosCustodias_CustodiQuery(CustodiSelected.IdRecibo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosCustodia, Nothing)
                    'FIN JCS - Feb 26 / 2013

                    If CustodiSelected.DescripcionEstado.Equals("Ingreso") Then
                        RegistroTieneCambios = False
                    Else
                        RegistroTieneCambios = True 'JEPM20170515
                    End If
                End If

                'End If
                esVersion = False
                _mStrIDClienteSeleccionadoEncabezado = CustodiSelected.Comitente
            Else
                If IsNothing(_CustodiSelected) Then
                    ListaDetalleCustodias = Nothing
                End If
            End If
            MyBase.CambioItem("CustodiSelected")
        End Set
    End Property

    'JEPM20170517 Se cambia nombre por uno más descriptivo
    Private _RegistroTieneCambios As Boolean
    Public Property RegistroTieneCambios As Boolean
        Get
            Return _RegistroTieneCambios
        End Get
        Set(ByVal value As Boolean)
            _RegistroTieneCambios = value
            MyBase.CambioItem("RegistroTieneCambios")
        End Set
    End Property

    Private _visibilidad As Visibility = Visibility.Collapsed
    Public Property visibilidad As Visibility
        Get
            Return _visibilidad
        End Get
        Set(ByVal value As Visibility)
            _visibilidad = value
            MyBase.CambioItem("visibilidad")
        End Set
    End Property

    'JEPM20170517 Se cambia nombre por uno más descriptivo
    Private _ManejaMakerAndChecker As Boolean
    Public Property ManejaMakerAndChecker As Boolean
        Get
            Return _ManejaMakerAndChecker
        End Get
        Set(ByVal value As Boolean)
            _ManejaMakerAndChecker = value
            MyBase.CambioItem("ManejaMakerAndChecker")
        End Set
    End Property

    Private _Read As Boolean = True
    Public Property Read As Boolean
        Get
            Return _Read
        End Get
        Set(ByVal value As Boolean)
            _Read = value
            MyBase.CambioItem("Read")
        End Set
    End Property

    Private _ConceptoA As Boolean
    Public Property ConceptoA As Boolean
        Get
            Return _ConceptoA
        End Get
        Set(ByVal value As Boolean)
            _ConceptoA = value
            MyBase.CambioItem("ConceptoA")
        End Set
    End Property

    Private _FechaReciboHabilitada As Boolean = False
    Public Property FechaReciboHabilitada As Boolean
        Get
            Return _FechaReciboHabilitada
        End Get
        Set(ByVal value As Boolean)
            _FechaReciboHabilitada = value
            MyBase.CambioItem("FechaReciboHabilitada")
        End Set
    End Property

    Private _Editarcampos As Boolean
    Public Property Editarcampos As Boolean
        Get
            Return _Editarcampos
        End Get
        Set(ByVal value As Boolean)
            _Editarcampos = value
            MyBase.CambioItem("Editarcampos")
        End Set
    End Property

    'JEPM20170517 Se cambia nombre por uno más descriptivo
    Private _TextoVersion As String
    Public Property TextoVersion As String
        Get
            Return _TextoVersion
        End Get
        Set(ByVal value As String)
            _TextoVersion = value
            MyBase.CambioItem("TextoVersion")
        End Set
    End Property

    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")

        End Set
    End Property

    ''' <summary>logVisualizarMasDecimalesCantidad
    '''  Propiedad para Ocultar Columnas dependiendo de si tienen o no MakeAndChecker.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 19/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Febrero 20/2013 - Resultado Ok 
    ''' </history>
    Private _ColumnasVisibles As Visibility = Visibility.Collapsed
    Public Property ColumnasVisibles() As Visibility
        Get
            Return _ColumnasVisibles
        End Get
        Set(ByVal value As Visibility)
            _ColumnasVisibles = value
            MyBase.CambioItem("ColumnasVisibles")
        End Set
    End Property

    'SV20150305
    'Almacena el valor del parámetro CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES
    Private _logIncrementarDecimalesCantidad As Boolean = False

    'SV20150305
    'Indica si el campo cantidad se visualiza con mas decimales, esto es para la edición y cuando está prendido el parámetro CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES
    Private _logVisualizarMasDecimalesCantidad As Boolean = False
    Public Property logVisualizarMasDecimalesCantidad() As Boolean
        Get
            Return _logVisualizarMasDecimalesCantidad
        End Get
        Set(ByVal value As Boolean)
            _logVisualizarMasDecimalesCantidad = value
            MyBase.CambioItem("logVisualizarMasDecimalesCantidad")
        End Set
    End Property

    Private _HabilitarEstEnt_Y_Origen As Boolean = False
    Public Property HabilitarEstEnt_Y_Origen As Boolean
        Get
            Return _HabilitarEstEnt_Y_Origen
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEstEnt_Y_Origen = value
            MyBase.CambioItem("HabilitarEstEnt_Y_Origen")
        End Set
    End Property

    'JAEZ 20161110 Para que al momento de borrar no muestre las validaciones 
    Private _EstaBorrando As Boolean = False
    Public Property EstaBorrando As Boolean
        Get
            Return _EstaBorrando
        End Get
        Set(ByVal value As Boolean)
            _EstaBorrando = value
            MyBase.CambioItem("EstaBorrando")
        End Set
    End Property

#End Region

#Region "Métodos"

    ''' <history>
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega propiedad para controlar si se habilitan los campos del cliente o no.
    ''' Fecha            : Marzo 06/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 06/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000002
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega propiedad para controlar si se habilita la fecha de recibo o no.
    ''' Fecha            : Abril 12/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 12/2013 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID de cambio: CP0017
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        Agosto 13/2015
    ''' Pruebas CB:   Jorge Peña (Alcuadrado S.A.) - Agosto 13/2015 - OK
    ''' </history>
    Public Overrides Sub NuevoRegistro()

        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            mstrAccion = MSTR_MC_ACCION_INGRESAR
            LogHabilitarReceptor = False

            Dim NewCustodi As New CFPortafolio.Custodia
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCustodi.IdComisionista = CustodiPorDefecto.IdComisionista
            NewCustodi.IdSucComisionista = CustodiPorDefecto.IdSucComisionista
            NewCustodi.IdRecibo = CustodiPorDefecto.IdRecibo
            NewCustodi.Comitente = CustodiPorDefecto.Comitente
            NewCustodi.TipoIdentificacion = CustodiPorDefecto.TipoIdentificacion
            NewCustodi.NroDocumento = CustodiPorDefecto.NroDocumento
            NewCustodi.Nombre = CustodiPorDefecto.Nombre
            NewCustodi.Telefono1 = CustodiPorDefecto.Telefono1
            NewCustodi.Direccion = CustodiPorDefecto.Direccion
            NewCustodi.Recibo = CustodiPorDefecto.Recibo
            NewCustodi.Estado = "P"
            NewCustodi.Fecha_Estado = CustodiPorDefecto.Fecha_Estado
            NewCustodi.ConceptoAnulacion = CustodiPorDefecto.ConceptoAnulacion
            NewCustodi.Notas = CustodiPorDefecto.Notas
            NewCustodi.NroLote = CustodiPorDefecto.NroLote
            NewCustodi.Elaboracion = CustodiPorDefecto.Elaboracion
            NewCustodi.Actualizacion = CustodiPorDefecto.Actualizacion
            NewCustodi.Usuario = Program.Usuario
            NewCustodi.IDCustodia = CustodiPorDefecto.IDCustodia
            NewCustodi.Aprobacion = 0
            CustodiAnterior = CustodiSelected
            CustodiSelected = NewCustodi
            intSecuencia = 0
            MyBase.CambioItem("Custodias")

            Editando = True
            Editarcampos = True
            Editareg = True
            Read = False
            ConceptoA = False
            EstaBorrando = False  'JAEZ20161110  
            'ID Modificación  : 000001
            HabilitarCamposCliente = True
            'ID Modificación  : 000002
            FechaReciboHabilitada = True
            _mlogBuscarClienteEncabezado = True
            _mLogBuscarClienteDetalle = True
            _mlogBuscarEspecie = True
            '_mlngEmision = Nothing
            '_mLogTipoTasa = String.Empty
            '_mLogEsAccion = False
            _mStrIDClienteSeleccionadoEncabezado = String.Empty
            _mLogAgregarRegistroDetalle = True
            HabilitarEstEnt_Y_Origen = True

            If _logIncrementarDecimalesCantidad Then
                logVisualizarMasDecimalesCantidad = True
            End If

            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            mstrAccion = String.Empty
        End Try
    End Sub

    ''' <history>
    ''' ID de cambio: CP0015
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        Agosto 13/2015
    ''' Pruebas CB:   Jorge Peña (Alcuadrado S.A.) - Agosto 13/2015 - OK
    ''' </history>
    Public Overrides Sub Filtrar()
        Try
            dcProxy.Custodias.Clear()
            IsBusy = True
            LogHabilitarReceptor = _strParametroCrucePorComercial = "SI"
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CustodiaFiltrarQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodia, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CustodiaFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodia, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID de cambio: CP0015
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        Agosto 13/2015
    ''' Pruebas CB:   Jorge Peña (Alcuadrado S.A.) - Agosto 13/2015 - OK
    ''' </history>
    Public Overrides Sub Buscar()
        If MakeAndCheck = 1 Then
            visibilidad = Visibility.Visible
        End If
        CustodiAnterior = Nothing
        cb = New CamposBusquedaCustodi
        cb.FechaLimiteRecibo = Now.Date
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Funcion que ejecuta la busqueda de custodias basado en los criterios definidos por pantalla.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se incluye el campo cb.FechaRecibo, para que se filtre por Fecha del Recibo.
    ''' Fecha            : Febrero 20/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Febrero 20/2013 - Resultado Ok 
    ''' </history> 
    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IdRecibo <> 0 Or cb.Comitente <> String.Empty Or cb.Filtro = 1 Or cb.Filtro = 0 Or cb.FechaRecibo IsNot Nothing Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Custodias.Clear()
                CustodiAnterior = Nothing
                IsBusy = True
                ' DescripcionFiltroVM = " IdRecibo = " & cb.IdRecibo.ToString() & " Comitente = " & cb.Comitente.ToString()
                dcProxy.Load(dcProxy.CustodiaConsultarQuery(cb.Filtro, cb.IdRecibo, cb.Comitente, cb.FechaRecibo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodia, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCustodi
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se invocan los métodos de validación de campos requeridos a nivel de encabezado y detalle Custodia.
    ''' Fecha            : Marzo 06/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 06/2013 - Resultado Ok 
    ''' </history>
    Public Overrides Sub ActualizarRegistro()
        Try
            ConsultarFechaCierreSistema("GUARDAR")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ContinuarGuardadoDocumento()
        Try
            If Not IsNothing(_CustodiSelected) Then
                If _CustodiSelected.Recibo.ToShortDateString <= fechaCierre Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("La fecha de recibo es menor o igual a la fecha de cierre (" & fechaCierre.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            'INICIO ID Modificación 000001
            If Validar_EncabezadoCustodia() Then
                Exit Sub
            End If
            If Validar_DetalleCustodia() Then
                Exit Sub
            End If
            'FIN ID Modificación 000001

            If CustodiSelected.Recibo.Date > Now.Date Then

                A2Utilidades.Mensajes.mostrarMensaje("La fecha de recibo no puede ser mayor a la fecha actual.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ConceptoA = True And CustodiSelected.ConceptoAnulacion = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("Digite La causa de la anulación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'objProxy.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")

            ValidacionesServidor()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ContinuarGuardadoDocumento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de realizar todas la validaciones de las Custodias en el Servidor
    ''' </summary>
    ''' <param name="YaValidoCuentaDEC"></param>
    ''' <remarks>SLB20131114</remarks>
    Private Sub ValidacionesServidor(Optional ByVal YaValidoCuentaDEC As Boolean = False)
        Try
            If Not YaValidoCuentaDEC Then
                intNroDetallesValidar = 0
                Validar_CuentasDEC()
                Exit Sub
            End If


            GuardarRegistro()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ValidacionesServidorCE", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub Validar_CuentasDEC()
        Try
            If intNroDetallesValidar < ListaDetalleCustodias.Count Then
                If Not IsNothing(_ListaDetalleCustodias.ElementAt(intNroDetallesValidar).IdCuentaDeceval) And _ListaDetalleCustodias.ElementAt(intNroDetallesValidar).IdCuentaDeceval <> 0 Then

                    IsBusy = True
                    dcProxy.Validar_CuentasDEC(_ListaDetalleCustodias.ElementAt(intNroDetallesValidar).IdCuentaDeceval, _ListaDetalleCustodias.ElementAt(intNroDetallesValidar).Comitente,
                                               _ListaDetalleCustodias.ElementAt(intNroDetallesValidar).Fondo, Program.Usuario, Program.HashConexion, AddressOf TerminoValidar_CuentaDEC, "")
                Else
                    intNroDetallesValidar = intNroDetallesValidar + 1
                    Validar_CuentasDEC()
                End If
            Else
                IsBusy = False
                ValidacionesServidor(True)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerdatosencuenta",
                     Me.ToString(), "ExistenDatosEnCuenta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Descripción:    Método para validar cuando se ingresa el número de liquidación, parcial, tipo, clase, fecha de liquidación y
    '''                 se selecciona un tipo de inversión se busca el tipo de inversión de la orden asociada a la liquidación y debe 
    '''                 corresponder al tipo de inversión seleccionado sino el sistema mostrará un mensaje de alerta.
    ''' </summary>
    ''' <history>
    ''' ID Caso Prueba: Id_3, Id_4, Id_5, Id_6
    ''' Responsable:    Jorge Peña (alcuadrado S.A.)
    ''' Fecha:          14 de Abril 2014
    ''' Pruebas CB:     Jorge Peña - 14 de Abril 2014 - Resultado OK
    ''' </history>
    Private Sub Validar_TipoInversion()
        Try
            'If Not String.IsNullOrEmpty(DetalleCustodiaSelected.strTipoInversion) And
            '    DetalleCustodiaSelected.IDLiquidacion <> 0 And
            '    Not IsNothing(DetalleCustodiaSelected.Parcial) And
            '    Not IsNothing(DetalleCustodiaSelected.TipoLiquidacion) And
            '    Not IsNothing(DetalleCustodiaSelected.ClaseLiquidacion) And
            '    Not IsNothing(DetalleCustodiaSelected.Liquidacion) Then

            '    dcProxy.Validar_TipoInversion(DetalleCustodiaSelected.strTipoInversion, _
            '                                  DetalleCustodiaSelected.IDLiquidacion, _
            '                                  DetalleCustodiaSelected.Parcial, _
            '                                  DetalleCustodiaSelected.TipoLiquidacion, _
            '                                  DetalleCustodiaSelected.ClaseLiquidacion, _
            '                                  DetalleCustodiaSelected.Liquidacion, AddressOf TerminoValidar_TipoInversion, "")
            'Else

            ' End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Validar_TipoInversion",
                     Me.ToString(), "Validar_TipoInversion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ' ''' <summary>
    ' ''' Descripción:    Método para retornar de manera asincrónica la respuesta del método Validar_TipoInversion.
    ' ''' </summary>
    ' ''' <history>
    ' ''' ID Caso Prueba: Id_3, Id_4, Id_5, Id_6
    ' ''' Responsable:    Jorge Peña (alcuadrado S.A.)
    ' ''' Fecha:          14 de Abril 2014
    ' ''' Pruebas CB:     Jorge Peña - 14 de Abril 2014 - Resultado OK
    ' ''' </history>
    'Private Sub TerminoValidar_TipoInversion(ByVal lo As InvokeOperation(Of String))
    '    Try
    '        If Not lo.HasError Then
    '            If Not IsNothing(lo.Value) Then
    '                If DetalleCustodiaSelected.strTipoInversion <> lo.Value Then
    '                    DetalleCustodiaSelected.strTipoInversion = lo.Value
    '                    A2Utilidades.Mensajes.mostrarMensaje("El tipo inversión debe ser igual al tipo inversión de la orden. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '                End If
    '            End If
    '        Else
    '            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema validando el tipo de inversión", _
    '                              Me.ToString(), "TerminoValidar_TipoInversion", Application.Current.ToString(), Program.Maquina, lo.Error)
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema validando el tipo de inversión", _
    '                 Me.ToString(), "TerminoValidar_TipoInversion", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub



    Private Sub TerminoValidar_CuentaDEC(ByVal lo As InvokeOperation(Of Boolean))
        Try
            If Not lo.HasError Then
                If lo.Value Then
                    intNroDetallesValidar = intNroDetallesValidar + 1
                    Validar_CuentasDEC()
                Else
                    IsBusy = False

                    If Not IsNothing(CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("UBICACIONTITULO")) Then
                        strDeposito = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("UBICACIONTITULO").Where(Function(obj) obj.ID = _ListaDetalleCustodias.ElementAt(intNroDetallesValidar).Fondo).First.Descripcion
                    Else
                        strDeposito = _ListaDetalleCustodias.ElementAt(intNroDetallesValidar).Fondo
                    End If
                    A2Utilidades.Mensajes.mostrarMensaje("La cuenta  " & CStr(_ListaDetalleCustodias.ElementAt(intNroDetallesValidar).IdCuentaDeceval) & " del depósito " & strDeposito &
                                                         " y del cliente " & LTrim(_ListaDetalleCustodias.ElementAt(intNroDetallesValidar).Comitente) & " no se encuentra matriculada en el maestro de Cuentas de OyD", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    _ListaDetalleCustodias.ElementAt(intNroDetallesValidar).IdCuentaDeceval = Nothing
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerdatosencuenta",
                                  Me.ToString(), "TerminoValidar_CuentaDEC", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerdatosencuenta",
                     Me.ToString(), "TerminoValidar_CuentaDEC", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <history>
    ''' ID caso de prueba: Id_9
    ''' Descripción:       Se coloca la instrucción "ConceptoA = False" porque al anular el recibo quedaba habilitado el campo.
    ''' Responsable:       Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             21 de Abril/2014
    ''' Pruebas CB:        Jorge Peña - 21 de Abril/2014
    ''' </history>
    Private Sub GuardarRegistro()
        Try
            If CustodiSelected.Recibo.ToShortDateString <= fechaCierre Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha del documento no puede ser menor o igual a la fecha de cierre (" & fechaCierre.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim numeroErrores = (From lr In ListaDetalleCustodias Where lr.HasValidationErrors = True).Count
            If numeroErrores <> 0 Then
                Dim strMsg As String = String.Empty

                For intI As Integer = 0 To DetalleCustodiaSelected.ValidationErrors.Count - 1
                    strMsg &= DetalleCustodiaSelected.ValidationErrors(intI).ErrorMessage & vbNewLine
                Next
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Dim origen = "update"
            ErrorForma = ""
            CustodiAnterior = CustodiSelected
            'ListaCustodia.Remove(_ListaClientesReceptore.Where(Function(i) i.IDReceptor = _ClientesReceptoreSelected.IDReceptor).First)
            If Not ListaCustodia.Contains(CustodiSelected) Then
                origen = "insert"
                If ListaDetalleCustodias.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar minimo un Detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    ListaCustodia.Add(CustodiSelected)
                End If

            End If
            Editarcampos = False
            If Not IsNothing(ListaDetalleCustodias) Then
                For Each li In ListaDetalleCustodias
                    li.logModificarDetalleRespaldo = False
                Next
            End If
            logVisualizarMasDecimalesCantidad = False
            Read = True
            IsBusy = True
            ConceptoA = False
            EstaBorrando = False ' JAEZ 2016110

            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "GuardarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <history>
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega propiedad para controlar si se habilitan los campos del cliente o no.
    ''' Fecha            : Marzo 06/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 06/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : Id_7
    ''' Modificado por   : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Se agrega lógica para capturar el mensaje de validación al modificar una custodia.
    ''' Fecha            : Abril 16/2014
    ''' Pruebas CB       : Jorge Peña - Abril 16/2014 - Resultado Ok 
    ''' 
    ''' Descipción:        Se coloca la instrucción _CustodiSelected.Actualizacion = Now.ToString para mostrar la fecha de actualización y no tener que ir a la base de datos solo por este campo.
    ''' Responsable:       Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             24 de AGosto/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - 24 de AGosto/2015 - Resultado OK
    ''' </history>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                    Editarcampos = True
                    If Not IsNothing(ListaDetalleCustodias) Then
                        For Each li In ListaDetalleCustodias
                            li.logModificarDetalleRespaldo = li.logModificarDetalle
                        Next
                    End If
                    Read = False
                    If _logIncrementarDecimalesCantidad Then
                        logVisualizarMasDecimalesCantidad = True
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()
                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                ' So.MarkErrorAsHandled()
                Exit Try
            End If

            If Not IsNothing(MakeAndCheck) Then
                If MakeAndCheck = 1 Then
                    If ((So.UserState = "insert") Or (So.UserState = "update") Or (So.UserState = "BorrarRegistro")) Then
                        MyBase.QuitarFiltroDespuesGuardar()
                        CustodiAnterior = Nothing
                        dcProxy.Custodias.Clear()
                        dcProxy.Load(dcProxy.CustodiaFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodia, "insert") ' Recarga la lista para que carguen los include
                        Exit Try
                    End If
                End If
            End If

            'JP Abril 16/2014 - INICIO
            Dim strMsg2 As String = String.Empty
            If Not IsNothing(ListaDetalleCustodias) Then
                For Each li In ListaDetalleCustodias
                    If Not String.IsNullOrEmpty(li.strMsgValidacion) Then
                        strMsg2 = String.Format("{0}{1}" + li.strMsgValidacion, strMsg2, vbCrLf)
                    End If
                Next
            End If

            If Not String.IsNullOrEmpty(strMsg2) Then
                A2Utilidades.Mensajes.mostrarMensaje(strMsg2, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            If logBorrarRegistro Then
                RefrescarForma()
            End If
            'JP Abril 16/2014 - FIN

            ' ID Modificación  : 000001
            HabilitarCamposCliente = False
            FechaReciboHabilitada = False

            HabilitarEstEnt_Y_Origen = False
            If Not IsNothing(_CustodiSelected) Then
                _CustodiSelected.Actualizacion = Now.ToString
            End If
            DetalleCustodiaSelected.logModificarMoneda = False

            If Not logBorrarRegistro Then
                MyBase.QuitarFiltroDespuesGuardar()
                CustodiAnterior = Nothing
                If Not IsNothing(DetalleCustodiaSelected.FechaReclasificacionInversion) Then ' Quiere decir que se ha reclasificado el tipo de inversión, por lo tanto ya cambió el recibo
                    intIDInsertado = _CustodiSelected.IdRecibo + 1
                Else
                    intIDInsertado = _CustodiSelected.IdRecibo
                End If


                dcProxy.Custodias.Clear()
                dcProxy.Load(dcProxy.CustodiaFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodia, "insercionregistro") ' Recarga la lista para que carguen los include
            End If

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega propiedad para controlar si se habilita la fecha de recibo o no.
    ''' Fecha            : Abril 12/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 12/2013 - Resultado Ok 

    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(CustodiSelected) Then
                If dcProxy.IsLoading Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If CustodiSelected.Estado.Equals("I") Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("No se puede modificar esta impreso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                ElseIf CustodiSelected.Estado.Equals("A") Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("No se puede modificar esta anulada .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                'CustodiSelected.Recibo = Date.Now.ToShortDateString
                'CustodiSelected.Elaboracion = Date.Now.ToShortDateString

                HabilitarEstEnt_Y_Origen = False
                LogHabilitarReceptor = _strParametroCrucePorComercial = "SI"
                ConsultarFechaCierreSistema("EDITAR")
            Else
                MyBase.RetornarValorEdicionNavegacion()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición del registro",
                                 Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <history>
    ''' Descipción:        Se cambia de  Now.Date a  Now.ToString.
    ''' Responsable:       Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             24 de AGosto/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - 24 de AGosto/2015 - Resultado OK
    ''' </history>
    Private Sub ContinuarEdicionDocumento()
        Try
            If Not IsNothing(_CustodiSelected) Then
                If _CustodiSelected.Recibo.ToShortDateString <= fechaCierre Then
                    MyBase.RetornarValorEdicionNavegacion()
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("El titulo no se puede editar porque la fecha de recibo es menor o igual a la fecha de cierre (" & fechaCierre.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If Not IsNothing(ListaDetalleCustodias) Then
                For Each li In ListaDetalleCustodias
                    li.logModificarDetalleRespaldo = li.logModificarDetalle
                    li.logModificarMoneda = If(DetalleCustodiaSelected.strOrigen = "MA", True, False)
                Next
            End If

            If MakeAndCheck = 1 Then
                ValidarEdicion()
            Else
                If Not IsNothing(_CustodiSelected) Then
                    Editando = True
                    Editarcampos = True
                    Editareg = False
                    Read = False
                    ConceptoA = False
                    EstaBorrando = False 'JAEZ  20161110
                    CustodiSelected.Usuario = Program.Usuario
                    CustodiSelected.Actualizacion = Now.ToString
                    FechaReciboHabilitada = False 'ID Modificación  : 000001
                    HabilitarCamposCliente = True
                    If Not IsNothing(ListaDetalleCustodias) Then
                        'Debo tomar exactamente el número de secuencia actual, porque como estaba fallaba porque por alguna razón unas custodias no tenian secuencia 1 entonces generaba error de llave primaria
                        intSecuencia = ListaDetalleCustodias.OrderByDescending(Function(i) i.IdRecibo).Last.Secuencia
                    End If
                    If _logIncrementarDecimalesCantidad Then
                        logVisualizarMasDecimalesCantidad = True
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición del registro",
                                 Me.ToString(), "ContinuarEdicionDocumento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID de cambio: CP0016
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        Agosto 13/2015
    ''' Pruebas CB:   Jorge Peña (Alcuadrado S.A.) - Agosto 13/2015 - OK
    ''' </history>
    ''' <history>
    ''' Descipción:        Se cambia de  Now.Date a  Now.ToString.
    ''' Responsable:       Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             24 de AGosto/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - 24 de AGosto/2015 - Resultado OK
    ''' </history>
    Public Sub Editar()
        If Not IsNothing(_CustodiSelected) Then
            ValidarHabilitarReceptor()
            Editando = True
            Editarcampos = True
            Editareg = False
            Read = False
            ConceptoA = False
            EstaBorrando = False 'JAEZ 20161110
            CustodiSelected.Usuario = Program.Usuario
            CustodiSelected.Actualizacion = Now.ToString
            FechaReciboHabilitada = False ' ID Modificación  : 000001
            HabilitarCamposCliente = True
            If Not IsNothing(ListaDetalleCustodias) Then
                intSecuencia = ListaDetalleCustodias.Count
            End If
            If _logIncrementarDecimalesCantidad Then
                logVisualizarMasDecimalesCantidad = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' JEPM20170517
    ''' Este código solamente funciona cuando se cambia a forma, pero no cuando se cambia de registro. Es decir, sólo funciona cuando 
    ''' el registro resaltado es el mismo que se visualiza en la vista forma.
    ''' De lo contrario, no tendrá efecto este código si se cambia de Custodia seleccionada.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub CambiarAForma()
        LogHabilitarReceptor = _strParametroCrucePorComercial = "SI"
        MyBase.CambiarAForma()

    End Sub

    Public Overrides Sub AprobarRegistro()
        Try
            esVersion = True
            Dim origen = "update"
            ErrorForma = ""
            CustodiAnterior = CustodiSelected
            If (CustodiSelected.DescripcionEstado.Equals("Modificacion") Or CustodiSelected.DescripcionEstado.Equals("Ingreso")) Then
                CustodiSelected.Aprobacion = 2
                CustodiSelected.UsuarioAprobador = Program.Usuario
                DetalleCustodiaSelected = ListaDetalleCustodias.FirstOrDefault
                If Not IsNothing(DetalleCustodiaSelected) Then
                    If Not IsNothing(DetalleCustodiaSelected.DescripcionEstado) Then
                        If (DetalleCustodiaSelected.DescripcionEstado.Equals("Modificacion") Or DetalleCustodiaSelected.DescripcionEstado.Equals("Ingreso") Or DetalleCustodiaSelected.DescripcionEstado.Equals("Retiro")) Then
                            For Each li In ListaDetalleCustodias
                                'DetalleCustodiaSelected = li
                                'DetalleCustodiaSelected.Aprobacion = 2
                                li.Aprobacion = 2
                            Next
                        End If
                    End If
                End If
            ElseIf (CustodiSelected.DescripcionEstado.Equals("Retiro")) Then
                origen = "BorrarRegistro"
                CustodiSelected.Aprobacion = 2
                CustodiSelected.UsuarioAprobador = Program.Usuario
            End If

            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        MyBase.AprobarRegistro()
    End Sub

    Public Overrides Sub RechazarRegistro()
        Try
            esVersion = True
            Dim origen = "update"
            ErrorForma = ""
            CustodiAnterior = CustodiSelected
            If (CustodiSelected.DescripcionEstado.Equals("Modificacion") Or CustodiSelected.DescripcionEstado.Equals("Ingreso")) Then
                CustodiSelected.Aprobacion = 1
                CustodiSelected.UsuarioAprobador = Program.Usuario
                DetalleCustodiaSelected = ListaDetalleCustodias.FirstOrDefault
                If Not IsNothing(DetalleCustodiaSelected) Then
                    If Not IsNothing(DetalleCustodiaSelected.DescripcionEstado) Then
                        If (DetalleCustodiaSelected.DescripcionEstado.Equals("Modificacion") Or DetalleCustodiaSelected.DescripcionEstado.Equals("Ingreso") Or DetalleCustodiaSelected.DescripcionEstado.Equals("Retiro")) Then
                            For Each li In ListaDetalleCustodias
                                'DetalleCustodiaSelected = li
                                'DetalleCustodiaSelected.Aprobacion = 2
                                li.Aprobacion = 1
                            Next
                        End If
                    End If
                End If
            ElseIf (CustodiSelected.DescripcionEstado.Equals("Retiro")) Then
                origen = "BorrarRegistro"
                CustodiSelected.Aprobacion = 1
                CustodiSelected.UsuarioAprobador = Program.Usuario
            End If

            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        MyBase.RechazarRegistro()
    End Sub

    ''' <summary>
    ''' Funcion para consultar custodias dependiendo del estado del MakerAndChecker.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se incluye el parametro correspondiente a pdtmRecibo en Nothing, en la sobrecarga de la operacion CustodiaConsultarQuery.
    ''' Fecha            : Febrero 20/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 20/2013 - Resultado Ok 
    ''' </history>     
    Public Overrides Sub VersionRegistro()
        Try
            Dim intFiltro As Integer
            esVersion = True
            codigo = CustodiSelected.IdRecibo

            If CustodiSelected.Por_Aprobar Is Nothing Then
                intFiltro = 0
            Else
                intFiltro = 1
            End If

            dcProxy.Custodias.Clear()
            IsBusy = True
            dcProxy.Load(dcProxy.CustodiaConsultarQuery(intFiltro, CustodiSelected.IdRecibo, CustodiSelected.Comitente, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodia, "VersionRegistro") 'JEPM20170517 Se asigna nombre al evento que llama TerminoTraerCustodia

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        MyBase.VersionRegistro()
    End Sub

    ''' <history>
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega propiedad para controlar si se habilitan los campos para ingresar la información del comitente.
    ''' Fecha            : Marzo 11/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 13/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000002
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega propiedad para controlar si se habilita la fecha de recibo o no.
    ''' Fecha            : Abril 12/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 12/2013 - Resultado Ok 
    ''' </history>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CustodiSelected) Then
                Editando = False
                dcProxy.RejectChanges()
                Editarcampos = False
                If Not IsNothing(ListaDetalleCustodias) Then
                    For Each li In ListaDetalleCustodias
                        li.logModificarDetalleRespaldo = False
                        li.logModificarMoneda = False
                    Next
                End If
                Editareg = False
                Read = True
                ConceptoA = False
                EstaBorrando = False  ' JAEz20161110
                'ID Modificación  : 000001
                HabilitarCamposCliente = False
                'ID Modificación  : 000002
                FechaReciboHabilitada = False
                If Not IsNothing(_CustodiSelected) Then
                    If _CustodiSelected.EntityState = EntityState.Detached Then
                        CustodiSelected = CustodiAnterior
                        If Not IsNothing(CustodiSelected) Then
                            If CustodiSelected.IdRecibo <= 0 Then
                                CustodiSelected.Comitente = Nothing
                                CustodiSelected.Direccion = String.Empty
                                CustodiSelected.Nombre = String.Empty
                                CustodiSelected.Telefono1 = String.Empty
                                CustodiSelected.TipoIdentificacion = String.Empty
                                CustodiSelected.NroDocumento = Nothing
                                CustodiSelected.dtmFechaCierrePortafolio = Nothing
                            End If
                        End If
                    End If
                End If
                logVisualizarMasDecimalesCantidad = False
                HabilitarEstEnt_Y_Origen = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID caso de prueba: Id_8
    ''' Descripción:   Se evalúan los detalles del recibo para poderlo anular. Si por lo menos uno de ellos tiene estado diferente a Pendiente o Bloqueado no se debe dejar anular.
    ''' Responsable:   Jorge Peña(Alcuadrado S.A.)
    ''' Fecha:         21 de Abril/2014
    ''' Pruebas CB:    Jorge Peña -  21 de Abril/2014 OK
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(ListaDetalleCustodias) Then
                logBorrarRegistro = True
                If dcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Not IsNothing(ListaDetalleCustodias) Then
                    For Each li In ListaDetalleCustodias
                        If Not li.EstadoActual = " " And Not li.EstadoActual = "B" Then
                            A2Utilidades.Mensajes.mostrarMensaje("No se puede anular, el estado actual de por lo menos uno de los detalles es diferente a pendiente o bloqueado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                    Next
                End If
                ConsultarFechaCierreSistema("BORRAR")
                End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro",
                                 Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' Descripción:  Se agrega la instrucción "CustodiSelected.Usuario = Program.Usuario" para llevarlo a la base de datos.
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        21 de Agosto/2015
    ''' </history>
    ''' <remarks></remarks>
    Private Sub ContinuarBorradoDocumento()
        Try
            If Not IsNothing(_CustodiSelected) Then
                If _CustodiSelected.Recibo.ToShortDateString <= fechaCierre Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("El titulo no se puede borrar porque la fecha de recibo es menor o igual a la fecha de cierre (" & fechaCierre.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If MakeAndCheck = 1 Then

                Validarborrado()
            Else
                'If CustodiSelected.Estado.Equals("P") Then
                '    A2Utilidades.Mensajes.mostrarMensaje("No se puede anular el estado es pendiente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    Exit Sub
                'Else
                If CustodiSelected.Estado.Equals("A") Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se puede anular ya se encuentra anulado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                Editando = True
                If visNavegando = "Collapsed" Then
                    MyBase.CambiarFormulario_Forma_Manual()
                End If
                ConceptoA = True
                EstaBorrando = True 'JAEZ 20161110
                Editarcampos = False
                If Not IsNothing(ListaDetalleCustodias) Then
                    For Each li In ListaDetalleCustodias
                        li.logModificarDetalleRespaldo = False
                    Next
                End If
                FechaReciboHabilitada = False
                logVisualizarMasDecimalesCantidad = False
            End If

            CustodiSelected.Usuario = Program.Usuario
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro",
                                 Me.ToString(), "ContinuarBorradoDocumento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub borrar()
        If CustodiSelected.Estado.Equals("P") Then
            A2Utilidades.Mensajes.mostrarMensaje("No se puede anular el estado es pendiente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        ElseIf CustodiSelected.Estado.Equals("A") Then
            A2Utilidades.Mensajes.mostrarMensaje("No se puede anular ya se encuentra anulado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        Editando = True
        ConceptoA = True
        EstaBorrando = True  'JAEZ 20161110
        Editarcampos = False
        EstaBorrando = True
        If Not IsNothing(ListaDetalleCustodias) Then
            For Each li In ListaDetalleCustodias
                li.logModificarDetalleRespaldo = False
            Next
        End If
        logVisualizarMasDecimalesCantidad = False
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub

    Public Sub llenarDiccionario()
        DicCamposTab.Add("TipoIdentificacion", 1)
        DicCamposTab.Add("NroDocumento", 1)
        DicCamposTab.Add("Comitente", 1)
        DicCamposTab.Add("Nombre", 1)
    End Sub

    ''' <summary>
    ''' Funcion para consultar custodias en modo "ValidarEdicion".
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se incluye el parametro correspondiente a pdtmRecibo en Nothing, en la sobrecarga de la operacion CustodiaConsultarQuery.
    ''' Fecha            : Febrero 20/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 20/2013 - Resultado Ok 
    ''' </history>         
    Public Sub ValidarEdicion()
        Try
            If Not IsNothing(dcProxy3.Custodias) Then
                dcProxy3.Custodias.Clear()
            End If

            dcProxy3.Load(dcProxy3.CustodiaConsultarQuery(0, CustodiSelected.IdRecibo, CustodiSelected.Comitente, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarEdicion, "ValidarEdicion")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar validar la edición",
                                 Me.ToString(), "ValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Funcion para consultar custodias en modo "Validarborrado".
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se incluye el parametro correspondiente a pdtmRecibo en Nothing, en la sobrecarga de la operacion CustodiaConsultarQuery.
    ''' Fecha            : Febrero 20/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 20/2013 - Resultado Ok 
    ''' </history>
    Public Sub Validarborrado()
        Try
            If Not IsNothing(dcProxy3.Custodias) Then
                dcProxy3.Custodias.Clear()
            End If
            dcProxy3.Load(dcProxy3.CustodiaConsultarQuery(0, CustodiSelected.IdRecibo, CustodiSelected.Comitente, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarborrado, "ValidarEdicion")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar validar la edición",
                                 Me.ToString(), "ValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del comitente
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : 
    ''' Fecha            : Marzo 06/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 06/2013 - Resultado Ok 
    ''' </history>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strIdComitente As String = String.Empty
        Try
            If (_mlogBuscarClienteEncabezado) Then
                If Not Me.CustodiSelected Is Nothing Then
                    If Not strIdComitente.Equals(Me.CustodiSelected.Comitente) Then
                        If pstrIdComitente.Trim.Equals(String.Empty) Then
                            strIdComitente = Me.CustodiSelected.Comitente
                        Else
                            strIdComitente = pstrIdComitente
                        End If
                    End If
                End If
            End If

            If (_mLogBuscarClienteDetalle) Then
                If Not Me.DetalleCustodiaSelected Is Nothing Then
                    If Not strIdComitente.Equals(Me.DetalleCustodiaSelected.Comitente) Then
                        If pstrIdComitente.Trim.Equals(String.Empty) Then
                            strIdComitente = Me.DetalleCustodiaSelected.Comitente
                        Else
                            strIdComitente = pstrIdComitente
                        End If
                    End If
                End If
            End If

            If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                objProxy.BuscadorClientes.Clear()
                objProxy.Load(objProxy.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID Modificación  : 000001
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega propiedad para controlar si se habilitan los campos para ingresar la informacion del comitente.
    '''                    Se agrega código para limpiar los controles donde se ingresa la información del comitente.
    ''' Fecha            : Marzo 11/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 13/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000002
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega variable _mStrIDClienteSeleccionadoEncabezado almacenar el cliente que se selecciona en el encabezado
    '''                    y poder recuperar dicho valor en caso de que el usuario digite el ID de un cliente que esta en bloqueado o inactivo.
    ''' Fecha            : Abril 16/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 16/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000003
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Cada vez que se cambia un cliente en el detalle se hace un llamado al método LimpiarCamposEspecie.
    ''' Fecha            : Abril 16/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 16/2013 - Resultado Ok 
    ''' </history>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If (_mlogBuscarClienteEncabezado) Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                        A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        _CustodiSelected.Comitente = _mStrIDClienteSeleccionadoEncabezado 'ID Modificación: 000002
                        _mlogBuscarClienteEncabezado = False
                    Else
                        Me.ComitenteSeleccionadoM(lo.Entities.ToList.Item(0))
                        HabilitarCamposCliente = False
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    _CustodiSelected.Comitente = _mStrIDClienteSeleccionadoEncabezado
                    'ID Modificación  : 000001
                    HabilitarCamposCliente = True
                End If
            End If

            If (_mLogBuscarClienteDetalle) Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                        A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el detalle se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        DetalleCustodiaSelected.Comitente = String.Empty
                        DetalleCustodiaSelected.Nombre = String.Empty
                        'DetalleCustodiaSelected.IdEspecie = String.Empty
                        'DetalleCustodiaSelected.ISIN = Nothing
                        DetalleCustodiaSelected.IdCuentaDeceval = Nothing
                        DetalleCustodiaSelected.Fondo = Nothing
                        'LimpiarCamposEspecie()
                        _mLogBuscarClienteDetalle = False
                    Else
                        Me.ComitenteSeleccionadoM(lo.Entities.ToList.Item(0))
                        'ID Modificación  : 000004
                        VerificarCuentaDecevalCliente()
                        'DetalleCustodiaSelected.IdCuentaDeceval = Nothing
                        'DetalleCustodiaSelected.Fondo = Nothing
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el detalle no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    DetalleCustodiaSelected.Comitente = String.Empty
                    DetalleCustodiaSelected.Nombre = String.Empty
                    'DetalleCustodiaSelected.IdEspecie = String.Empty
                    'DetalleCustodiaSelected.ISIN = Nothing
                    DetalleCustodiaSelected.IdCuentaDeceval = Nothing
                    DetalleCustodiaSelected.Fondo = Nothing
                    'LimpiarCamposEspecie()
                End If
                'DetalleCustodiaSelected.IdEspecie = String.Empty
                'DetalleCustodiaSelected.ISIN = Nothing
                'DetalleCustodiaSelected.IdCuentaDeceval = Nothing
                'DetalleCustodiaSelected.Fondo = Nothing
                'LimpiarCamposEspecie() ' ID Modificación  : 000003
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>Autocompletar campos Nombre y Cliente en el detalle</summary>
    ''' <param name="pobjComitente"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID Modificación  : 000001
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Según el comitente seleccionado en el encabezado se autocompleta el cliente en el detalle.
    ''' Fecha            : Marzo 11/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 13/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000002
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se asigna el valor que tenga configurado el cliente en el campo AdmonValores a la propiedad mstrAdmonValores 
    ''' Fecha            : Marzo 27/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 27/2013 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descipción:        Se coloca la instrucción " _CustodiSelected.dtmFechaCierrePortafolio = pobjComitente.dtmFechaCierrePortafolio" para mostrar el dato en pantalla.
    ''' Responsable:       Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             24 de AGosto/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - 24 de AGosto/2015 - Resultado OK
    ''' </history>
    Sub ComitenteSeleccionadoM(ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            If (_mlogBuscarClienteEncabezado) Then
                Dim Existe As Boolean = True
                If ListaDetalleCustodias.Count = 0 Then
                    Existe = False
                End If

                Dim logEsNumerico As Boolean = True

                Try
                    Dim lngNroConvertido As Decimal? = pobjComitente.NroDocumento
                Catch ex As Exception
                    logEsNumerico = False
                End Try

                _mStrIDClienteSeleccionadoEncabezado = CustodiSelected.Comitente
                _CustodiSelected.Comitente = pobjComitente.IdComitente
                _CustodiSelected.Nombre = pobjComitente.Nombre
                _CustodiSelected.dtmFechaCierrePortafolio = pobjComitente.dtmFechaCierrePortafolio
                _CustodiSelected.strTipoCompania = pobjComitente.strTipoCompania

                ValidarHabilitarReceptor()
                If logEsNumerico Then
                    _CustodiSelected.NroDocumento = pobjComitente.NroDocumento
                Else
                    _CustodiSelected.NroDocumento = "0"
                End If

                _CustodiSelected.TipoIdentificacion = pobjComitente.CodTipoIdentificacion
                _CustodiSelected.Direccion = pobjComitente.DireccionEnvio
                _CustodiSelected.Telefono1 = pobjComitente.Telefono
                HabilitarEdicion = False

                'ID Modificación  : 000001
                If (_mLogAgregarRegistroDetalle) Then
                    NuevoRegistroDetalle()
                    _DetalleCustodiaSelected.Nombre = pobjComitente.Nombre
                    _DetalleCustodiaSelected.Comitente = pobjComitente.IdComitente
                    _DetalleCustodiaSelected.IdCuentaDeceval = Nothing
                    _DetalleCustodiaSelected.Fondo = Nothing
                    _mLogBuscarClienteDetalle = False
                    _mLogAgregarRegistroDetalle = False
                End If

                'ID Modificación  : 000002
                Select Case pobjComitente.AdmonValores
                    Case "T", "N" 'T-->Ambos, N-->Ninguno
                        DetalleCustodiaSelected.Admonvalores = "T" 'Todos

                    Case "A" 'Acciones
                        DetalleCustodiaSelected.Admonvalores = "A" 'Renta Variable

                    Case "R" 'Renta Fija
                        DetalleCustodiaSelected.Admonvalores = "C" 'Renta Fija
                End Select

                If Not Existe Then
                    NombreColeccionDetalle = "cmDetalleCustodia"
                    'NuevoRegistroDetalle()
                End If
            End If

            If (_mLogBuscarClienteDetalle) Then
                Dim Existe As Boolean = True
                If ListaDetalleCustodias.Count = 0 Then
                    Existe = False
                End If

                _DetalleCustodiaSelected.Comitente = pobjComitente.IdComitente
                _DetalleCustodiaSelected.Nombre = pobjComitente.Nombre
                Select Case pobjComitente.AdmonValores
                    Case "T", "N" 'T-->Ambos, N-->Ninguno
                        DetalleCustodiaSelected.Admonvalores = "T" 'Todos

                    Case "A" 'Acciones
                        DetalleCustodiaSelected.Admonvalores = "A" 'Renta Variable

                    Case "R" 'Renta Fija
                        DetalleCustodiaSelected.Admonvalores = "C" 'Renta Fija
                End Select

                If Not Existe Then
                    NombreColeccionDetalle = "cmDetalleCustodia"
                    'NuevoRegistroDetalle()
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Buscar los datos de la especie según el ID digitado por el usuario.
    ''' </summary>
    ''' <param name="pstrIdEspecie">ID de la Especie que se debe buscar.</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID Modificación  : 000001
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : En el método buscarNemotecnicoEspecificoQuery se envía el valor que tenga asignada la propiedad mstrAdmonValores
    ''' Fecha            : Marzo 14/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 21/2013 - Resultado Ok 
    ''' </history>
    Friend Sub BuscarEspecie(Optional ByVal pstrIdEspecie As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strIdEspecie As String = String.Empty
        Try
            If Not Me.DetalleCustodiaSelected Is Nothing Then
                If Not strIdEspecie.Equals(Me.DetalleCustodiaSelected.IdEspecie) Then
                    If pstrIdEspecie.Trim.Equals(String.Empty) Then
                        strIdEspecie = Me.DetalleCustodiaSelected.IdEspecie
                    Else
                        strIdEspecie = pstrIdEspecie
                    End If

                    If Not strIdEspecie Is Nothing AndAlso Not strIdEspecie.Trim.Equals(String.Empty) Then
                        objProxy.BuscadorEspecies.Clear()

                        'ID Modificación  : 000001
                        objProxy.Load(objProxy.buscarNemotecnicoEspecificoQuery(DetalleCustodiaSelected.Admonvalores, strIdEspecie, Program.Usuario, Program.HashConexion), AddressOf BuscarEspecieCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la especie", Me.ToString(), "BuscarEspecie", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el resultado de buscar la especie cuando se digita el ID en el control
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID Modificación  : 000001
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se incluye mensaje de advertencia de las especies que puede manejar el cliente según el tipo de mercado.
    ''' Fecha            : Marzo 14/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 21/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000002
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Si la especie tiene un solo ISIN se autocompleta la informacion de la especie con la  primera fila de la lista. 
    '''                    Si la especie tiene varios ISINES se obliga al usuario a que seleccione uno de ellos, luego se identifica la fila de la lista que contiene la información del ISIN seleccionado para autocompletar la información de la especie.
    ''' Fecha            : Junio 05/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 05/2013 - Resultado Ok 
    ''' </history>
    Private Sub BuscarEspecieCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Try
            Dim _mContador As Integer = 0
            If lo.Entities.ToList.Count > 0 Then
                If lo.Entities.ToList.Count = 1 Then    'ID:000002
                    Me.EspecieSeleccionadaM(lo.Entities.ToList.Item(0))
                Else
                    DetalleCustodiaSelected.TipoTasaFija = lo.Entities.ToList.First.CodTipoTasaFija

                    For Each ItemISIN In lo.Entities.ToList
                        If DetalleCustodiaSelected.ISIN = ItemISIN.ISIN Then
                            EspecieSeleccionadaM(lo.Entities.ToList.Item(_mContador))
                        End If
                        _mContador = _mContador + 1
                    Next
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("La especie ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                DetalleCustodiaSelected.IdEspecie = String.Empty
                _DetalleCustodiaSelected.ISIN = Nothing
                'LimpiarCamposEspecie() SLB20140423 No se debe limpar las faciales como en VB.6
                'ID Modificación  : 000001
                If DetalleCustodiaSelected.Admonvalores = "A" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente sólo puede manejar especies de Renta Variable", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                If DetalleCustodiaSelected.Admonvalores = "C" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente sólo puede manejar especies de Renta Fija", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie", Me.ToString(), "BuscarEspecieCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se valida si la especie existe y se autocompleta el campo Especie.
    ''' </summary>
    ''' <param name="pobjEspecie"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID Modificación  : 000001
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se incluye validación para autocompletar algunos campos según algunos criterios de la especie seleccionada.
    ''' Fecha            : Marzo 14/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 21/2013 - Resultado Ok 
    ''' </history>
    Sub EspecieSeleccionadaM(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try

            If Not IsNothing(pobjEspecie) Then

                Dim Existe As Boolean = True
                If ListaDetalleCustodias.Count = 0 Then
                    Existe = False
                End If

                'ID Modificación  : 000001
                AutocompletarCamposSegunEspecie(pobjEspecie)

                If Not Existe Then
                    NombreColeccionDetalle = "cmDetalleCustodia"
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar la especie",
                                 Me.ToString(), "EspecieSeleccionadaM", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Teniendo en cuenta los campos logEsAccion y strTipoTasaFija de la especie, se debe obtener la información del ISIN Fungible
    ''' y autocompletar algunos campos en el detalle.
    ''' 
    ''' Si LogEsAccion=1 entonces autocompletar el Nemo e ISIN si lo tiene, sino tiene ISIN entonces es de obligatorio diligenciamiento por el usuario.
    ''' Si logEsAccion=0 y TipoTasa='V' entonces autocompletar los datos Fecha Emisión, Fecha Vencimiento, Indicador y Puntos Indicador. Si alguno de estos datos queda vacío debe ser obligatorio para el usuario.
    ''' Si logEsAccion=0 y TipoTasa='F' o TipoTasa=Null entonces autocompletar los datos Fecha Emisión, Fecha Vencimiento, Modalidad y Tasa Descuento. Si alguno de estos datos queda vacío debe ser obligatorio para el usuario.
    '''
    ''' </summary>
    ''' <param name="pobjEspecie"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID Modificación  : 000001
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : El campo renta variable se debe autodiligenciar según la información del campo LogEsAccion de tblEspecies.
    ''' Fecha            : Marzo 14/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 21/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000002
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se asigna información en variables para realizar validaciones de datos obligatorios en el detalle.
    ''' Fecha            : Marzo 15/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 21/2013 - Resultado Ok 
    '''     
    ''' ID Modificación  : 000003
    ''' Agregado por     : Sebastiam Londoño Benitez.
    ''' Descripción      : Para las especies de RF solo se asignan las faciales que sean diferente a Nothing.
    ''' Fecha            : Abril 23/2014
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 23/2014 - Resultado Ok 
    ''' </history>
    Sub AutocompletarCamposSegunEspecie(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            'ID Modificación  : 000002
            If pobjEspecie.lngEmision IsNot Nothing And pobjEspecie.lngEmision <> 0 Then
                DetalleCustodiaSelected.lngEmision = pobjEspecie.lngEmision
            End If

            DetalleCustodiaSelected.ISIN = pobjEspecie.ISIN
            DetalleCustodiaSelected.RentaVariable = pobjEspecie.EsAccion 'ID Modificación  : 000001

            If (pobjEspecie.EsAccion) Then
                '_mLogEsAccion = True 'ID Modificación  : 000002
                DetalleCustodiaSelected.Emision = Nothing
                DetalleCustodiaSelected.Vencimiento = Nothing
                DetalleCustodiaSelected.Modalidad = Nothing
                DetalleCustodiaSelected.IndicadorEconomico = Nothing
                DetalleCustodiaSelected.PuntosIndicador = Nothing
                DetalleCustodiaSelected.TasaDescuento = Nothing
            Else
                '_mLogEsAccion = False 'ID Modificación  : 000002

                If pobjEspecie.CodTipoTasaFija = "V" Then
                    If Not IsNothing(pobjEspecie.Emision) Then DetalleCustodiaSelected.Emision = pobjEspecie.Emision 'ID Modificación  : 000003
                    If Not IsNothing(pobjEspecie.Vencimiento) Then DetalleCustodiaSelected.Vencimiento = pobjEspecie.Vencimiento 'ID Modificación  : 000003
                    If Not IsNothing(pobjEspecie.CodModalidad) Then DetalleCustodiaSelected.Modalidad = pobjEspecie.CodModalidad 'ID Modificación  : 000003
                    If (pobjEspecie.IdIndicador IsNot Nothing) Then
                        DetalleCustodiaSelected.IndicadorEconomico = pobjEspecie.IdIndicador
                    End If
                    If Not IsNothing(pobjEspecie.PuntosIndicador) Then DetalleCustodiaSelected.PuntosIndicador = pobjEspecie.PuntosIndicador 'ID Modificación  : 000003
                    '_mLogTipoTasa = "V" 'ID Modificación  : 000002
                    DetalleCustodiaSelected.TipoTasaFija = "V"
                    DetalleCustodiaSelected.TasaDescuento = Nothing
                Else
                    If Not IsNothing(pobjEspecie.Emision) Then DetalleCustodiaSelected.Emision = pobjEspecie.Emision 'ID Modificación  : 000003
                    If Not IsNothing(pobjEspecie.Vencimiento) Then DetalleCustodiaSelected.Vencimiento = pobjEspecie.Vencimiento 'ID Modificación  : 000003
                    If Not IsNothing(pobjEspecie.CodModalidad) Then DetalleCustodiaSelected.Modalidad = pobjEspecie.CodModalidad 'ID Modificación  : 000003
                    If Not IsNothing(pobjEspecie.TasaFacial) Then DetalleCustodiaSelected.TasaDescuento = pobjEspecie.TasaFacial 'ID Modificación  : 000003
                    '_mLogTipoTasa = "F" 'ID Modificación  : 000002
                    DetalleCustodiaSelected.TipoTasaFija = "F"
                    DetalleCustodiaSelected.IndicadorEconomico = Nothing
                    DetalleCustodiaSelected.PuntosIndicador = Nothing
                End If

                If (DetalleCustodiaSelected.Emision IsNot Nothing) Then
                    dtmFechaEmision = DetalleCustodiaSelected.Emision
                End If

                If (DetalleCustodiaSelected.Vencimiento IsNot Nothing) Then
                    dtmFechaVencimiento = DetalleCustodiaSelected.Vencimiento
                End If

                DetalleCustodiaSelected.DiasVencimiento = DateDiff(DateInterval.Day, dtmFechaEmision, dtmFechaVencimiento)

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al autocompletar algunos campos según la especie seleccionada",
                                 Me.ToString(), "AutocompletarCamposSegunEspecie", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Este método se encarga de limpiar los campos que se autodiligencian al seleccionar una especie.
    ''' Fecha            : Abril 16/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 16/2013 - Resultado Ok 
    Sub LimpiarCamposEspecie()
        '_DetalleCustodiaSelected.IdEspecie = String.Empty
        _DetalleCustodiaSelected.Emision = Nothing
        _DetalleCustodiaSelected.Vencimiento = Nothing
        _DetalleCustodiaSelected.Modalidad = Nothing
        _DetalleCustodiaSelected.IndicadorEconomico = Nothing
        _DetalleCustodiaSelected.PuntosIndicador = Nothing
        _DetalleCustodiaSelected.TasaDescuento = Nothing
        _DetalleCustodiaSelected.RentaVariable = False
        ' _DetalleCustodiaSelected.ISIN = Nothing
    End Sub

    ''' <summary>
    ''' Agregado por     : Yeiny Adenis Marín Zapata
    ''' Descripción      : Validaciones de campos requeridos a nivel de encabezado
    ''' Fecha            : Marzo 07/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 12/2013 - Resultado Ok 
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Validar_EncabezadoCustodia() As Boolean
        If Not IsNothing(CustodiSelected.IDCustodia) Then
            If HabilitarCamposCliente = False Then
                If IsNothing(CustodiSelected.Comitente) Or CustodiSelected.Comitente = String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return True
                    Exit Function
                End If
            End If
            If IsNothing(CustodiSelected.Nombre) Or CustodiSelected.Nombre = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("El nombre del cliente es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return True
                Exit Function
            End If
            If IsNothing(CustodiSelected.TipoIdentificacion) Or CustodiSelected.TipoIdentificacion = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("El tipo de identificación es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return True
                Exit Function
            End If
            If String.IsNullOrEmpty(CustodiSelected.NroDocumento) Then
                A2Utilidades.Mensajes.mostrarMensaje("El número de documento es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return True
                Exit Function
            End If
            If IsNothing(CustodiSelected.Estado) Or CustodiSelected.Estado = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("El estado es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return True
                Exit Function
            End If
            If IsNothing(CustodiSelected.Fecha_Estado) Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha estado es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return True
                Exit Function
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' Validaciones de campos requeridos a nivel de detalle
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : 
    ''' Fecha            : Marzo 07/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 12/2013 - Resultado Ok 
    '''
    ''' ID Modificación  : 000001
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Validación Campo NroTitulo (Detalle)
    '''                    Si el usuario ingresa digiga el NroTítulo, se debe almacenar, sino
    '''                    validar si el lngEmision es diferente de Null y asignarle el valor al campo NroTitulo
    '''                    si el lngEmision es Null entonces asignarle al campo NroTitulo el ISIN + La cuenta depósito
    '''                    si el campo NroTitulo queda vacío, el usuario debe ingresar obligatoriamente el dato.
    ''' Fecha            : Marzo 15/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 21/2013 - Resultado Ok 
    '''
    ''' ID Modificación  : 000002
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se incluyen validaciones de campos requeridos según las propiedades LogEsAccion y strTipoTasaFija
    '''                    correspondientes a la especie.
    ''' Fecha            : Marzo 15/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 21/2013 - Resultado Ok 
    ''' 
    ''' ID Modificación  : 000003
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se incluyen validaciones de campos requeridos:
    '''                    La cuenta depósito es obligatoria si el depósito es DECEVAL O DCV
    '''                    El ISIN es obligatorio si el depósito es DECEVAL
    '''                    correspondientes a la especie.
    ''' Fecha            : Marzo 15/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Marzo 27/2013 - Resultado Ok 
    ''' </history>

    Public Function Validar_DetalleCustodia() As Boolean
        If Not IsNothing(DetalleCustodiaSelected) Then
            For Each LineaDetalle In ListaDetalleCustodias
                If IsNothing(LineaDetalle.Comitente) Or LineaDetalle.Comitente = String.Empty And EstaBorrando = False Then
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return True
                    Exit Function
                End If
                If IsNothing(LineaDetalle.IdEspecie) Or LineaDetalle.IdEspecie = String.Empty And EstaBorrando = False Then
                    A2Utilidades.Mensajes.mostrarMensaje("La especie es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return True
                    Exit Function
                End If
                If IsNothing(LineaDetalle.Nombre) Or LineaDetalle.Nombre = String.Empty And EstaBorrando = False Then
                    A2Utilidades.Mensajes.mostrarMensaje("El nombre es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return True
                    Exit Function
                End If
                If IsNothing(LineaDetalle.Cantidad) Or LineaDetalle.Cantidad = 0 And EstaBorrando = False Then
                    A2Utilidades.Mensajes.mostrarMensaje("La cantidad es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return True
                    Exit Function
                End If
                If IsNothing(LineaDetalle.Fondo) Or LineaDetalle.Fondo = String.Empty And EstaBorrando = False Then
                    A2Utilidades.Mensajes.mostrarMensaje("El depósito es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return True
                    Exit Function
                End If

                'ID Modificación  : 000002
                'If (_mLogEsAccion) Then
                If (LineaDetalle.RentaVariable) Then
                    If (IsNothing(LineaDetalle.ISIN) Or LineaDetalle.ISIN = String.Empty) And LineaDetalle.Fondo <> "F" And EstaBorrando = False Then
                        A2Utilidades.Mensajes.mostrarMensaje("El ISIN es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Return True
                        Exit Function
                    End If
                Else
                    'If (_mLogTipoTasa = "V") Then
                    If (LineaDetalle.TipoTasaFija = "V") Then
                        If IsNothing(LineaDetalle.Emision) And EstaBorrando = False Then
                            A2Utilidades.Mensajes.mostrarMensaje("La fecha de emisión es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return True
                            Exit Function
                        End If
                        If IsNothing(LineaDetalle.Vencimiento) And EstaBorrando = False Then
                            A2Utilidades.Mensajes.mostrarMensaje("La fecha de vencimiento es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return True
                            Exit Function
                        End If
                        If IsNothing(LineaDetalle.Modalidad) Or LineaDetalle.Modalidad = String.Empty And EstaBorrando = False Then
                            A2Utilidades.Mensajes.mostrarMensaje("La modalidad es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return True
                            Exit Function
                        End If
                        If IsNothing(LineaDetalle.IndicadorEconomico) Or LineaDetalle.IndicadorEconomico = String.Empty And EstaBorrando = False Then
                            A2Utilidades.Mensajes.mostrarMensaje("El indicador económico es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return True
                            Exit Function
                        End If
                        'Descripción: Se comenta por autorización de Jorge Arango en correo del 20160104
                        'Responsable: Jorge Peña(Alcuadrado S.A.)
                        'Fecha:       1 de Enero\2016
                        If IsNothing(LineaDetalle.PuntosIndicador) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Los puntos indicador son requeridos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return True
                            Exit Function
                        End If
                    Else
                        If IsNothing(LineaDetalle.Emision) And EstaBorrando = False Then
                            A2Utilidades.Mensajes.mostrarMensaje("La fecha de emisión es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return True
                            Exit Function
                        End If
                        If IsNothing(LineaDetalle.Vencimiento) And EstaBorrando = False Then
                            A2Utilidades.Mensajes.mostrarMensaje("La fecha de vencimiento es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return True
                            Exit Function
                        End If
                        If IsNothing(LineaDetalle.Modalidad) Or LineaDetalle.Modalidad = String.Empty And EstaBorrando = False Then
                            A2Utilidades.Mensajes.mostrarMensaje("La modalidad es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return True
                            Exit Function
                        End If
                        If IsNothing(LineaDetalle.TasaDescuento) Or LineaDetalle.TasaDescuento < 0 And EstaBorrando = False Then
                            A2Utilidades.Mensajes.mostrarMensaje("La tasa facial es requerida o no puede ser valores negativos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                            Return True
                            Exit Function
                        End If
                    End If
                End If
                'ID Modificación  : 000003
                If (LineaDetalle.Fondo = "D") Then
                    If IsNothing(LineaDetalle.ISIN) Or LineaDetalle.ISIN = String.Empty And EstaBorrando = False Then
                        A2Utilidades.Mensajes.mostrarMensaje("El ISIN es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Return True
                        Exit Function
                    End If
                    If IsNothing(LineaDetalle.IdCuentaDeceval) Or LineaDetalle.IdCuentaDeceval = 0 And EstaBorrando = False Then
                        A2Utilidades.Mensajes.mostrarMensaje("La cuenta depósito es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Return True
                        Exit Function
                    End If
                End If
                If (LineaDetalle.Fondo = "V") Then 'D.C.V
                    If IsNothing(LineaDetalle.IdCuentaDeceval) Or LineaDetalle.IdCuentaDeceval = 0 And EstaBorrando = False Then
                        A2Utilidades.Mensajes.mostrarMensaje("La cuenta depósito es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Return True
                        Exit Function
                    End If
                End If

                'ID Modificación  : 000001
                If IsNothing(LineaDetalle.NroTitulo) Or LineaDetalle.NroTitulo = String.Empty Or LineaDetalle.NroTitulo = "0" Then
                    If LineaDetalle.lngEmision <> Nothing And LineaDetalle.lngEmision <> 0 Then
                        LineaDetalle.NroTitulo = Convert.ToString(LineaDetalle.lngEmision)
                    Else
                        LineaDetalle.NroTitulo = Convert.ToString(LineaDetalle.IdCuentaDeceval) + LineaDetalle.ISIN
                    End If
                    If IsNothing(LineaDetalle.NroTitulo) Or LineaDetalle.NroTitulo = "0" And EstaBorrando = False Then
                        A2Utilidades.Mensajes.mostrarMensaje("El Nro Título es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Return True
                        Exit Function
                    End If
                End If
                If LineaDetalle.strOrigen = "MA" And Not logBorrarRegistro And EstaBorrando = False Then
                    If LineaDetalle.IDMoneda <= 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("La moneda es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Return True
                        Exit Function
                    End If
                End If
            Next
        End If
        Return False
    End Function

    Public Sub ReiniciarSecuencia()
        Dim intConsecutivoSecuencia = 1
        For Each LineaDetalle In ListaDetalleCustodias
            LineaDetalle.Secuencia = intConsecutivoSecuencia
            intSecuencia = intConsecutivoSecuencia
            intConsecutivoSecuencia = intConsecutivoSecuencia + 1
        Next
    End Sub

    Public Sub RefrescarForma()
        Try
            If Not IsNothing(_CustodiSelected) Then
                'SLB20131211
                cb = New CamposBusquedaCustodi
                cb.IdRecibo = _CustodiSelected.IdRecibo
                LogHabilitarReceptor = False
                ConfirmarBuscar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta para refrescar la forma", Me.ToString(), "RefrescarForma", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' ID Modificación  : 000004
    ''' Agregado por     : Natalia Andrea Otalvaro Castrillon.
    ''' Descripción      : Se incluye logica para el caso 1027, cuando el cliente en detalle custodia tenga la misma cuenta sociada la deja por defecto, si tiene una cuenta diferente la borra. 
    ''' Fecha            : Julio 28 /2014
    ''' Pruebas CB       . Natalia Andrea Otalvaro Castrillon- Julio 28/2014 - Resultado Ok 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub VerificarCuentaDecevalCliente()
        Try
            If Not IsNothing(_DetalleCustodiaSelected) Then
                If Not IsNothing(_DetalleCustodiaSelected.Comitente) Then
                    IsBusy = True
                    If Not IsNothing(objProxy.BuscadorCuentasDepositos) Then
                        objProxy.BuscadorCuentasDepositos.Clear()
                    End If
                    objProxy.Load(objProxy.buscarCuentasDepositoComitenteQuery(_DetalleCustodiaSelected.Comitente, False, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentas, Nothing)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar la cuentas del cliente.", Me.ToString(), "VerificarCuentaDecevalCliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerCuentas(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorCuentasDeposito))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim logExisteCuenta As Boolean = False

                    For Each li In lo.Entities.ToList
                        If CInt(li.NroCuentaDeposito) = _DetalleCustodiaSelected.IdCuentaDeceval Then
                            logExisteCuenta = True
                            Exit For
                        End If
                    Next

                    If logExisteCuenta = False Then
                        _DetalleCustodiaSelected.IdCuentaDeceval = Nothing
                        _DetalleCustodiaSelected.Fondo = Nothing
                    End If
                Else
                    _DetalleCustodiaSelected.IdCuentaDeceval = Nothing
                    _DetalleCustodiaSelected.Fondo = Nothing
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar la cuentas del cliente.", Me.ToString(), "TerminoTraerCuentas", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                _DetalleCustodiaSelected.IdCuentaDeceval = Nothing
                _DetalleCustodiaSelected.Fondo = Nothing

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar la cuentas del cliente.", Me.ToString(), "TerminoTraerCuentas", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            _DetalleCustodiaSelected.IdCuentaDeceval = Nothing
            _DetalleCustodiaSelected.Fondo = Nothing
        End Try
        IsBusy = False
    End Sub

    Private Sub ConsultarFechaCierreSistema(Optional ByVal pstrUserState As String = "")
        Try
            objProxy.consultarFechaCierre("T", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cierre del sistema.",
                               Me.ToString(), "ConsultarFechaCierreSistema", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub





    ''' <summary>
    ''' JP20151120: Validar que la fecha de cierre del portafolio del comitente seleccionado no sea mayor o igual a la de la operación
    ''' </summary>
    ''' 
    Public Async Function ValidarFechaCierrePortafolio(Optional ByVal plogMostrarAdvertencia As Boolean = True) As Task(Of Boolean)
        Dim logResultadoValidacion As Boolean = False
        Dim dtmFechaOperacion As Date

        Try
            If Not IsNothing(DetalleCustodiaSelected) Then
                'If mdtmFechaCierrePortafolio.Equals(MDTM_FECHA_CIERRE_SIN_ACTUALIZAR) Then
                mdtmFechaCierrePortafolio = Await ObtenerFechaCierrePortafolio(DetalleCustodiaSelected.Comitente)
                'End If

                If Not IsNothing(mdtmFechaCierrePortafolio) And Not IsNothing(CustodiSelected.Recibo) Then
                    dtmFechaOperacion = New Date(CustodiSelected.Recibo.Year, CustodiSelected.Recibo.Month, CustodiSelected.Recibo.Day) ' Eliminar la hora
                    If mdtmFechaCierrePortafolio >= dtmFechaOperacion Then
                        If plogMostrarAdvertencia Then
                            A2Utilidades.Mensajes.mostrarMensaje("El portafolio del cliente " & DetalleCustodiaSelected.Comitente.Trim & "-" & DetalleCustodiaSelected.Nombre & " está cerrado para la fecha de la custodia (Fecha de cierre " & Year(mdtmFechaCierrePortafolio) & "/" & Month(mdtmFechaCierrePortafolio) & "/" & Day(mdtmFechaCierrePortafolio) & ", Fecha de la custodia " & Year(CustodiSelected.Recibo) & "/" & Month(CustodiSelected.Recibo) & "/" & Day(CustodiSelected.Recibo) & "). ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        logResultadoValidacion = True
                    End If
                Else
                    logResultadoValidacion = True
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la fecha de cierre del portafolio del cliente. ", Me.ToString(), "ObtenerFechaCierrePortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return logResultadoValidacion
    End Function

    ''' <summary>
    ''' JP20151120: Consultar fecha de cierre del portafolio para validar el ingreso de las custodias
    ''' </summary>
    ''' 
    Public Async Function ObtenerFechaCierrePortafolio(ByVal pstrIdComitente As String) As Task(Of DateTime?)
        Dim dtmFechaCierre As DateTime? = Nothing

        Try
            If String.IsNullOrEmpty(pstrIdComitente) Then
                Return (Nothing)
            End If

            Dim objRet As InvokeOperation(Of DateTime?)
            Dim objProxyUtil As UtilidadesCFDomainContext

            objProxyUtil = inicializarProxyUtilidades()

            objRet = Await objProxyUtil.ConsultarFechaCierrePortafolioSync(pstrIdComitente, Program.Usuario, Program.HashConexion).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar la fecha de cierre del portafolio del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If Not IsNothing(objRet.Value) Then
                        dtmFechaCierre = objRet.Value
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cierre del portafolio. ", Me.ToString(), "ObtenerFechaCierrePortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return dtmFechaCierre
    End Function
    ''' <summary>
    ''' JERF20181227 Se agrega logica para mostrar el buscardor de receptores 
    ''' </summary>
    Public Sub BuscarReceptor()
        If Not IsNothing(DetalleCustodiaSelected) Then
            mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("Receptores", "receptores", "receptores", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, DetalleCustodiaSelected.strIdReceptor, "", "")
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
            mobjBuscadorLst.ShowDialog()
        End If
    End Sub

    ''' <summary>
    ''' JERF20181227 Metodo donde se asigna el receptor seleccionado en el buscador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mobjBuscadorLst_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscadorLst.Closed
        If Not mobjBuscadorLst.ItemSeleccionado Is Nothing Then
            DetalleCustodiaSelected.strIdReceptor = mobjBuscadorLst.ItemSeleccionado.IdItem
        End If
    End Sub

#End Region

#Region "Tablas hijas"


    ' '******************************************************** DetalleCustodias 

    Private _listaCuentas As List(Of CFPortafolio.ListadoCuenta)
    Public Property listaCuentas As List(Of CFPortafolio.ListadoCuenta)
        Get
            Return _listaCuentas
        End Get
        Set(ByVal value As List(Of CFPortafolio.ListadoCuenta))
            _listaCuentas = value
            MyBase.CambioItem("listaCuentas")
        End Set
    End Property

    Private _ListaDetalleCustodias As EntitySet(Of CFPortafolio.DetalleCustodia)
    Public Property ListaDetalleCustodias() As EntitySet(Of CFPortafolio.DetalleCustodia)
        Get
            Return _ListaDetalleCustodias
        End Get
        Set(ByVal value As EntitySet(Of CFPortafolio.DetalleCustodia))
            _ListaDetalleCustodias = value
            MyBase.CambioItem("ListaDetalleCustodias")
            MyBase.CambioItem("ListaDetalleCustodiasPaged")
            If Not IsNothing(value) Then
                DetalleCustodiaSelected = _ListaDetalleCustodias.FirstOrDefault
            End If
        End Set
    End Property

    Public ReadOnly Property DetalleCustodiasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleCustodias) Then
                Dim view = New PagedCollectionView(_ListaDetalleCustodias)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _DetalleCustodiaSelected As CFPortafolio.DetalleCustodia
    Public Property DetalleCustodiaSelected() As CFPortafolio.DetalleCustodia
        Get
            Return _DetalleCustodiaSelected
        End Get
        Set(ByVal value As CFPortafolio.DetalleCustodia)

            If Not value Is Nothing Then
                _DetalleCustodiaSelected = value
                If value.EstadoActual <> " " And value.EstadoActual <> Nothing Then
                    Read = True
                End If
                MyBase.CambioItem("DetalleCustodiaSelected")
            End If
        End Set
    End Property


    ''' <history>
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega al case la opcion Comitente, para permitir digitar el código del cliente en el detalle.
    ''' Fecha            : Abril 10/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 10/2013 - Resultado Ok 
    ''' 
    ''' </history>

    Private Sub _DetalleCustodiaSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetalleCustodiaSelected.PropertyChanged
        Try
            If Editando Then
                Select Case e.PropertyName

                    Case "IdEspecie"
                        If Not String.IsNullOrEmpty(_DetalleCustodiaSelected.IdEspecie) And _mlogBuscarEspecie Then
                            DetalleCustodiaSelected.ISIN = String.Empty
                            'LimpiarCamposEspecie() SLB20140423 No se debe limpar las faciales como en VB.6
                            BuscarEspecie(_DetalleCustodiaSelected.IdEspecie)
                        End If
                        'INICIO ID:000001
                    Case "Comitente"
                        _mlogBuscarClienteEncabezado = False
                        _mLogBuscarClienteDetalle = True
                        If Not String.IsNullOrEmpty(_DetalleCustodiaSelected.Comitente) And _mLogBuscarClienteDetalle Then
                            buscarComitente(_DetalleCustodiaSelected.Comitente)
                        End If
                        'FIN ID:000001
                        'Jorge Peña
                        'Case "strTipoInversion", "IDLiquidacion", "Parcial", "ClaseLiquidacion", "TipoLiquidacion", "Liquidacion"
                        '    Validar_TipoInversion()
                    Case "strTipoInversion"
                        'Se debe permitir reclasificar el tipo de inversión siempre y cuando se cumplan las siguientes condiciones:
                        '1.El tipo de inversión tenga valor, 2.Se haya obtenido la fecha actual de cierre de portafolio, 
                        '3.No se trate de un nuevo registro en el detalle (secuencia de la custodia menor que el total de custodias cargadas, 4.Que no se trate de un nuevo registro
                        'Si estas condiciones se cumplen quiere decir que se trata de una custodia existente y con un tipo de inversión anterior
                        If Not IsNothing(DetalleCustodiaSelected.strTipoInversion) And Not IsNothing(mdtmFechaCierrePortafolio) _
                            And DetalleCustodiaSelected.Secuencia <= totalCustodiasCargadas And DetalleCustodiaSelected.IdRecibo > 0 Then
                            cwDetalleCustodiasCambiarFechaReciboView = New cwDetalleCustodiasCambiarFechaReciboView(Me, DetalleCustodiaSelected.strTipoInversion, mdtmFechaCierrePortafolio)
                            Program.Modal_OwnerMainWindowsPrincipal(cwDetalleCustodiasCambiarFechaReciboView)
                            cwDetalleCustodiasCambiarFechaReciboView.ShowDialog()
                        End If
                End Select
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_CustodiSelected.PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try




    End Sub

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmDetalleCustodia"
                Dim NewDetalleCustodia As New CFPortafolio.DetalleCustodia
                NewDetalleCustodia.IdRecibo = CustodiSelected.IdRecibo
                NewDetalleCustodia.Usuario = Program.Usuario
                NewDetalleCustodia.ObjVenta = DetalleCustodiaPorDefecto.ObjVenta
                NewDetalleCustodia.ObjSuscripcion = DetalleCustodiaPorDefecto.ObjSuscripcion
                NewDetalleCustodia.ObjRenovReinv = DetalleCustodiaPorDefecto.ObjRenovReinv
                NewDetalleCustodia.ObjCobroIntDiv = DetalleCustodiaPorDefecto.ObjCobroIntDiv
                NewDetalleCustodia.ObjCancelacion = DetalleCustodiaPorDefecto.ObjCancelacion
                NewDetalleCustodia.EstadoActual = " "
                NewDetalleCustodia.NombreEstadoActual = "Pendiente"
                NewDetalleCustodia.CargadoArchivo = False
                intSecuencia = intSecuencia + 1
                NewDetalleCustodia.Secuencia = intSecuencia
                NewDetalleCustodia.Notas = DetalleCustodiaPorDefecto.Notas
                NewDetalleCustodia.strOrigen = "MA"
                NewDetalleCustodia.CargadoArchivo = False
                NewDetalleCustodia.logModificarDetalle = True
                NewDetalleCustodia.logModificarDetalleRespaldo = True
                NewDetalleCustodia.IDMoneda = -1
                NewDetalleCustodia.logModificarMoneda = True
                'NewDetalleCustodia.Secuencia = intSecuencia + 1
                ListaDetalleCustodias.Add(NewDetalleCustodia)
                DetalleCustodiaSelected = NewDetalleCustodia
                MyBase.CambioItem("DetalleCustodiaSelected")
                MyBase.CambioItem("ListaDetalleCustodia")
                Read = False
                '_mlngEmision = Nothing
                '_mLogTipoTasa = String.Empty
                '_mLogEsAccion = False
        End Select
    End Sub
    ''' <history>
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega confirmación de eliminación de registro. Adicional se agrega validación que no permite eliminar una custodia si se encuentra en estado diferente de pendiente.
    ''' Fecha            : Junio 05/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 05/2013 - Resultado Ok 
    ''' </history>

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmDetalleCustodia"
                    If Not IsNothing(ListaDetalleCustodias) Then
                        If Not IsNothing(DetalleCustodiaSelected) Then
                            If (DetalleCustodiaSelected.IDDetalleCustodias = 0) Then
                                'C1.Silverlight.C1MessageBox.Show("¿Desea eliminar el registro seleccionado?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question, AddressOf TerminaPreguntaBorrarDetalle)
                                mostrarMensajePregunta("¿Desea eliminar el registro seleccionado?",
                                                       Program.TituloSistema,
                                                       "BORRARDETALLE",
                                                       AddressOf TerminaPreguntaBorrarDetalle, False)
                            Else

                                If (DetalleCustodiaSelected.EstadoActual = " ") Then
                                    A2Utilidades.Mensajes.mostrarMensaje(String.Format("Para ejecutar esta acción debe remitirse a la opción entrega custodias"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Exit Sub
                                Else
                                    A2Utilidades.Mensajes.mostrarMensaje(String.Format("Un título en estado " + DetalleCustodiaSelected.NombreEstadoActual + " no puede ser retirado"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                End If
                            End If
                        End If
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub TerminaPreguntaBorrarDetalle(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            If Not IsNothing(ListaDetalleCustodias) Then
                If Not IsNothing(DetalleCustodiaSelected) Then
                    Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(DetalleCustodiaSelected, ListaDetalleCustodias.ToList)
                    ListaDetalleCustodias.Remove(_DetalleCustodiaSelected)
                    ReiniciarSecuencia()
                    If ListaDetalleCustodias.Count > 0 Then
                        Program.PosicionarItemLista(DetalleCustodiaSelected, ListaDetalleCustodias.ToList, intRegistroPosicionar)
                    End If
                    MyBase.CambioItem("DetalleCustodiaSelected")
                    MyBase.CambioItem("ListaDetalleCustodias")
                Else
                    ListaDetalleCustodias.Remove(ListaDetalleCustodias.FirstOrDefault)
                    If ListaDetalleCustodias.Count > 0 Then
                        DetalleCustodiaSelected = ListaDetalleCustodias.FirstOrDefault
                    End If
                End If
            End If
        Else
            Exit Sub
        End If
    End Sub

    ''' <summary>
    ''' Propiedad que almacena los Beneficiarios de Custodias.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creadspecieo.
    ''' Fecha            : Febrero 26/2013
    ''' Pruebas Negocio  : No se le han hecho pruebas de caja blanca. 
    ''' </history>
    Private _ListaBeneficiariosCustodias As EntitySet(Of CFPortafolio.BeneficiariosCustodia)
    Public Property ListaBeneficiariosCustodias() As EntitySet(Of CFPortafolio.BeneficiariosCustodia)
        Get
            Return _ListaBeneficiariosCustodias
        End Get
        Set(ByVal value As EntitySet(Of CFPortafolio.BeneficiariosCustodia))
            _ListaBeneficiariosCustodias = value
            MyBase.CambioItem("ListaBeneficiariosCustodias")
            MyBase.CambioItem("ListaBeneficiariosCustodiasPaged")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que almacena el Beneficiario de custodia seleccionado.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creado.
    ''' Fecha            : Febrero 26/2013
    ''' Pruebas Negocio  : No se le han hecho pruebas de caja blanca. 
    ''' </history>''' 
    Private WithEvents _BeneficiarioCustodiaSelected As CFPortafolio.BeneficiariosCustodia
    Public Property BeneficiarioCustodiaSelected() As CFPortafolio.BeneficiariosCustodia
        Get
            Return _BeneficiarioCustodiaSelected
        End Get
        Set(ByVal value As CFPortafolio.BeneficiariosCustodia)
            _BeneficiarioCustodiaSelected = value
            If Not value Is Nothing Then
                'If value.EstadoActual <> " " And value.EstadoActual <> Nothing Then
                Read = True

                'End If
                MyBase.CambioItem("BeneficiarioCustodiaSelected")
            End If
        End Set
    End Property
#End Region

End Class

#Region "Campos Busqueda Custodia"

Public Class CamposBusquedaCustodi
    Implements INotifyPropertyChanged

    <Display(Name:="Recibo No", Description:="Recibo Custodia")>
    Public Property IdRecibo As Integer

    ''' <summary>
    ''' Propiedad para validar y manejar los cambios que se realicen al campo cliente en la vista de Busqueda
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se modifica la propiedad para que detecte el cambio de comitente que se realice mediante el buscador de clientes.
    ''' Fecha            : Febrero 19/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 20/2013 - Resultado Ok 
    ''' </history>    
    Private _Comitente As String
    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")>
    <Display(Name:="Cliente   ")>
    Public Property Comitente As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property


    Private _Nombre As String
    <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
    <Display(Name:=" ")>
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    <Display(Name:="Filtro")>
    Public Property Filtro As Byte = 1

    ''' <summary>
    ''' Propiedad para validar y manejar los cambios que se realicen al campo FechaRecibo en la vista de Busqueda
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se modifica la propiedad para que detecte el cambio de fecha.
    ''' Fecha            : Febrero 20/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 20/2013 - Resultado Ok 
    ''' </history> 
    Private _FechaRecibo As System.Nullable(Of DateTime)
    Public Property FechaRecibo As System.Nullable(Of DateTime)
        Get
            Return _FechaRecibo
        End Get
        Set(ByVal value As System.Nullable(Of DateTime))
            _FechaRecibo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaRecibo"))
        End Set
    End Property
    Private _FechaLimiteRecibo As DateTime
    <Display(Name:="Fecha Recibo")>
    Public Property FechaLimiteRecibo As DateTime
        Get
            Return _FechaLimiteRecibo
        End Get
        Set(ByVal value As DateTime)
            _FechaLimiteRecibo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaLimiteRecibo"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

#End Region