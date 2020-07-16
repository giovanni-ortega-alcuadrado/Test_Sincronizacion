Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: BancosViewModel.vb
'Generado el : 08/08/2011 12:11:50
'Propiedad de Alcuadrado S.A. 2010

'ID0001 - Se optimiza el proceso de consulta de saldo y movimientos para que solo se haga
'cuando se ingrese a cada una de las pestañas
'Santiago Alexander Vergara Orrego
'Febrero 14/2014

'Se comenta la verificación de un valor máximo, cuanod se manejan rangos
'No hay necesidad de ingresar un valor maximo
'Juan Camilo Munera
'Septiembre 21/2016



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
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Collections.Generic
Imports System.Threading.Tasks

Public Class BancosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaBanco
    Private BancoPorDefecto As Banco
    Private BancoAnterior As Banco
    Private mobjDetalleBancoTasaRedimientoPorDefecto As BancosTasasRendimientos
    Dim dcProxy As MaestrosDomainContext
    Dim sw As Integer
    Dim dcProxy1 As MaestrosDomainContext
    Dim dcProxy2 As UtilidadesDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim dcProxyListaTipoCuentaRecaudadora As UtilidadesDomainContext

    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Public Property _mlogNuevo As Boolean = False
    Private VersionAplicacionCliente As String = String.Empty
    Dim bitCargoSaldosMes As Boolean 'ID0001
    Dim bitCargoMovimientos As Boolean 'ID0001
    Dim intIDRegistroGuardado As Integer = 0
    Dim logRecargarSelected As Boolean = True
    'Dim xmlDetalleGrid As String
    Private Const ValorMaxFinal As Double = 999999999999999
    'JCM20160301
    Private mdcProxyUtilidad03 As MaestrosDomainContext
    Private Const PARAM_STR_CALCULOS_FINANCIEROS As String = "CF_UTILIZACALCULOSFINANCIEROS"
    Private Const PARAM_STR_UTILIZA_PASIVA As String = "CF_UTILIZAPASIVA_A2"
    Public IdMonedaCompanias As Integer
    Public intIDCompaniaFirma As Integer
    Public strNombreCompaniaFirma As String
    Private strTipoCuentaCompanias As String
    Dim UtilizaPasiva As Boolean
    Dim pintIDCompaniaBusqueda As Integer = 0

#Region "Variables"

    Private _mobjCompaniaSeleccionadoAntes As OYDUtilidades.BuscadorGenerico
#End Region

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            dcProxy2 = New UtilidadesDomainContext()
            objProxy = New UtilidadesDomainContext()
            dcProxyListaTipoCuentaRecaudadora = New UtilidadesDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
            mdcProxyUtilidad02 = New UtilidadesDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy2 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            dcProxyListaTipoCuentaRecaudadora = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.MaestrosDomainContext.IMaestrosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 150)
        DirectCast(objProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.BancosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos, "")
                dcProxy1.Load(dcProxy1.TraerBancoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosPorDefecto_Completed, "Default")


                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  BancosViewModel)(Me)

                VersionAplicacionCliente = Program.VersionAplicacionCliente


                mdcProxyUtilidad01.Verificaparametro(PARAM_STR_CALCULOS_FINANCIEROS, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroCF, Nothing)

                mdcProxyUtilidad01.Verificaparametro("MOSTRAR_TIPO_CUENTA_RECAUDADORA_BANCOS", Program.Usuario, Program.HashConexion, AddressOf TerminoTraerParametro, Nothing)

                dcProxy2.ItemCombos.Clear()
                dcProxy2.Load(dcProxy2.cargarCombosCondicionalQuery("COMPANIA_FIRMA", Nothing, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoConultarCompaniaFirma, String.Empty)

                dcProxyListaTipoCuentaRecaudadora.ItemCombos.Clear()
                dcProxyListaTipoCuentaRecaudadora.Load(dcProxyListaTipoCuentaRecaudadora.cargarCombosCondicionalQuery("TIPOCTA_RECAUDADORA", Nothing, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoConultarTipoCuentaRecaudadora, String.Empty)
                mdcProxyUtilidad01.Verificaparametro(PARAM_STR_UTILIZA_PASIVA, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroPasiva, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "BancosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerBancosPorDefecto_Completed(ByVal lo As LoadOperation(Of Banco))
        If Not lo.HasError Then
            BancoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Banco por defecto",
                                             Me.ToString(), "TerminoTraerBancoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub




    Private Sub TerminoTraerBancos(ByVal lo As LoadOperation(Of Banco))
        If Not lo.HasError Then
            If lo.UserState = "actualizacion" Then
                logRecargarSelected = False
            End If

            ListaBancos = dcProxy.Bancos

            logRecargarSelected = True


            If dcProxy.Bancos.Count > 0 Then
                If lo.UserState = "actualizacion" Then
                    If ListaBancos.Where(Function(i) i.ID = intIDRegistroGuardado).Count > 0 Then
                        BancoSelected = ListaBancos.Where(Function(i) i.ID = intIDRegistroGuardado).First
                    Else
                        BancoSelected = ListaBancos.First
                    End If
                End If

            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    visNavegando = "Collapsed"
                    MyBase.CambioItem("visNavegando")
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Bancos", Me.ToString, "TerminoTraerBanco", lo.Error)
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos",
            '                                 Me.ToString(), "TerminoTraerBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerSaldosBancoMes(ByVal lo As LoadOperation(Of SaldosBancoMes))
        IsBusySaldos = False
        If Not lo.HasError Then
            ListaSaldosBancoMes = dcProxy.SaldosBancoMes
            bitCargoSaldosMes = True
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de SaldosBancoMes",
                                             Me.ToString(), "TerminoTraerSaldosBancoMes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerMovimientosBancos(ByVal lo As LoadOperation(Of MovimientosBancos))
        IsBusyMovimientos = False
        If Not lo.HasError Then
            ListaMovimientosBancos = dcProxy.MovimientosBancos
            bitCargoMovimientos = True
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Movimientos Bancos",
                                             Me.ToString(), "TerminoTraerMovimientosBancos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub Terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            'If Not (So.Value) = String.Empty Then

            If So.Value.ToString.Contains("No se puede borrar,") Then
                If _BancoSelected.CtaActiva Then
                    mostrarMensajePregunta("No se puede eliminar este banco, Existe documentos asociados.",
                                           Program.TituloSistema,
                                           "INACTIVARBANCOS",
                                           AddressOf TerminoPreguntaInactivar, True, "¿Desea Inactivarlo?")
                Else
                    mostrarMensajePregunta("El banco está Inactivo.",
                                           Program.TituloSistema,
                                           "INACTIVARBANCOS",
                                           AddressOf TerminoPreguntaInactivar, True, "¿Desea Activarlo?")
                End If
            ElseIf So.Value.ToString.Contains("BORRAR") Then

                mostrarMensajePregunta("Borrar registro seleccionado.",
                                           Program.TituloSistema,
                                           "INACTIVARBANCOS",
                                           AddressOf TerminoMensajePreguntaBorrar, True, "¿Desea Borrarlo?")
            Else
                If Not IsNothing(So.Error) Then
                    A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
            'Else
            If So.UserState = "borrar" And So.Value = "ELIMINAR" Then
                dcProxy.Bancos.Clear()
                dcProxy.Load(dcProxy.BancosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos, "insert") ' Recarga la lista para que carguen los include
            End If
            'End If
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoMensajePreguntaBorrar(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            IsBusy = True
            dcProxy.EliminarCuentasBancarias("ELIMINAR", BancoSelected.IDBanco, Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
        End If
    End Sub

    Private Sub TerminoPreguntaInactivar(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            If _BancoSelected.CtaActiva Then
                BancoSelected.CtaActiva = False
            Else
                BancoSelected.CtaActiva = True
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "anular")
        End If
    End Sub


    ''' <summary>
    ''' JCM20160301
    ''' Método Asincronono para consultar los detalles del gri de tasas de redimientos
    ''' 
    ''' </summary>


    Public Async Function ConsultarDetalleTasasRendimientos(ByVal pintIdBanco As Integer) As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of BancosTasasRendimientos)

        Try
            If Not dcProxy.BancosTasasRendimientos Is Nothing Then
                dcProxy.BancosTasasRendimientos.Clear()
            End If

            objRet = Await dcProxy.Load(dcProxy.BancosTasasRendimientosConsultarQuery(pintIdBanco, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de las tasas de rendimiento pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de las tasas de rendimientos.", Me.ToString(), "ConsultarDetalleTasasRendimientos", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaBancoTasasRendimientos = objRet.Entities.ToList
                End If
            End If

            If ListaBancoTasasRendimientos IsNot Nothing Then
                If ListaBancoTasasRendimientos.Count = 1 Then
                    _BancoSelected.logManejaRangos = False
                    _BancoSelected.dblTasaRendimientoUnica = ListaBancoTasasRendimientos(0).dblTasaRendimiento
                    HabilitarGridTasas = Visibility.Collapsed
                    HabilitarTextTasas = Visibility.Visible
                    ListaBancoTasasRendimientos = Nothing
                Else
                    _BancoSelected.logManejaRangos = True
                    _BancoSelected.dblTasaRendimientoUnica = 0
                    HabilitarGridTasas = Visibility.Visible
                    HabilitarTextTasas = Visibility.Collapsed
                End If

            End If
            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de las tasas de rendimiento detalladas.", Me.ToString(), "ConsultarDetalleTasasRendimientos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function


    ''' <summary>
    ''' Carga el valor del parámetro CF_UTILIZACALCULOSFINANCIEROS -- JCM2016/03/01
    ''' 
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Private Sub TerminotraerparametroCF(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminotraerparametroCF", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)

        Else
            If obj.Value = "SI" Then
                MostrarTabTasas = Visibility.Visible
            Else
                MostrarTabTasas = Visibility.Collapsed
            End If
        End If
    End Sub

    Private Sub TerminoTraerParametro(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminoTraerParametro", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)

        Else
            If obj.Value = "SI" Then
                MOSTRAR_TIPO_CUENTA_RECAUDADORA_BANCOS = Visibility.Visible
            Else
                MOSTRAR_TIPO_CUENTA_RECAUDADORA_BANCOS = Visibility.Collapsed
            End If
        End If
    End Sub



    ''' <summary>
    ''' Carga el valor del parámetro CF_CF_UTILIZAPASIVA_A2 -- JCM2016/11/10
    ''' 
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Private Sub TerminotraerparametroPasiva(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminotraerparametroPasiva", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)

        Else
            If obj.Value = "SI" Then
                UtilizaPasiva = True
            Else
                UtilizaPasiva = False
            End If
        End If
    End Sub

#End Region

#Region "Propiedades"
    ''' <summary>
    ''' JCM20160301
    ''' Lista encargada de contener la informacion del Grid: Tabla BancosTasasRendimientos
    ''' 
    ''' </summary>
    Private _ListaBancoTasasRendimientos As List(Of BancosTasasRendimientos)
    Public Property ListaBancoTasasRendimientos As List(Of BancosTasasRendimientos)
        Get
            Return _ListaBancoTasasRendimientos
        End Get
        Set(value As List(Of BancosTasasRendimientos))
            _ListaBancoTasasRendimientos = value
            MyBase.CambioItem("ListaBancoTasasRendimientos")
            MyBase.CambioItem("ListaBancoTasasRendimientosPaginada")

        End Set
    End Property
    ''' <summary>
    ''' JCM20160301
    ''' Lista paginada encargada de contener la informacion del Grid: Tabla BancosTasasRendimientos
    ''' 
    ''' </summary>
    Public ReadOnly Property ListaBancoTasasRendimientosPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBancoTasasRendimientos) Then
                Dim view = New PagedCollectionView(_ListaBancoTasasRendimientos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    ''' <summary>
    ''' JCM20160301
    ''' Lista encargada de contener el registro seleccionado del grid de Tasas: Tabla BancosTasasRendimientos
    ''' 
    ''' </summary>
    ''' 

    Private WithEvents _ListaBancoTasasRendimientosSeleccionado As BancosTasasRendimientos
    Public Property ListaBancoTasasRendimientosSeleccionado() As BancosTasasRendimientos
        Get
            Return _ListaBancoTasasRendimientosSeleccionado
        End Get
        Set(ByVal value As BancosTasasRendimientos)
            _ListaBancoTasasRendimientosSeleccionado = value
            MyBase.CambioItem("ListaBancoTasasRendimientosSeleccionado")
        End Set
    End Property

    Private WithEvents _ListaBancoTasasRendimientosProximosValores As BancosTasasRendimientos
    Public Property ListaBancoTasasRendimientosProximosValores() As BancosTasasRendimientos
        Get
            Return _ListaBancoTasasRendimientosProximosValores
        End Get
        Set(ByVal value As BancosTasasRendimientos)
            _ListaBancoTasasRendimientosProximosValores = value
            MyBase.CambioItem("ListaBancoTasasRendimientosProximosValores")
        End Set
    End Property




    Private _HabilitarGridTasas As Visibility = Visibility.Collapsed
    Public Property HabilitarGridTasas() As Visibility
        Get
            Return _HabilitarGridTasas

        End Get
        Set(value As Visibility)
            _HabilitarGridTasas = value
            MyBase.CambioItem("HabilitarGridTasas")
        End Set
    End Property

    Private _HabilitarTextTasas As Visibility = Visibility.Collapsed
    Public Property HabilitarTextTasas() As Visibility
        Get
            Return _HabilitarTextTasas

        End Get
        Set(value As Visibility)
            _HabilitarTextTasas = value
            MyBase.CambioItem("HabilitarTextTasas")
        End Set
    End Property




    Private _ListaBancos As EntitySet(Of Banco)
    Public Property ListaBancos() As EntitySet(Of Banco)
        Get
            Return _ListaBancos
        End Get
        Set(ByVal value As EntitySet(Of Banco))
            _ListaBancos = value

            MyBase.CambioItem("ListaBancos")
            MyBase.CambioItem("ListaBancosPaged")

            If Not IsNothing(value) Then
                If _ListaBancos.Count > 0 Then
                    BancoSelected = _ListaBancos.FirstOrDefault
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaBancosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBancos) Then
                Dim view = New PagedCollectionView(_ListaBancos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Dim loSaldos As LoadOperation(Of SaldosBancoMes)
    Dim loMovimientos As LoadOperation(Of MovimientosBancos)


    Private WithEvents _BancoSelected As Banco
    Public Property BancoSelected() As Banco
        Get
            Return _BancoSelected
        End Get
        Set(ByVal value As Banco)

            _BancoSelected = value


            If Not value Is Nothing And logRecargarSelected Then
                buscarItem("ciudades")
                If Not value Is Nothing Then
                    CompaniaBanco = _BancoSelected.Compania
                    'JCM 20160229
                    If _BancoSelected.IDBanco > 0 Then
                        ' dcProxy.Load(dcProxy.BancosTasasRendimientosConsultar(_BancoSelected.IDBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTasasRendimientos, "")

                        ConsultarDetalleTasasRendimientos(_BancoSelected.IDBanco)
                    End If
                    'JCM 20160229

                    'inicia ID0001
                    bitCargoMovimientos = False
                    bitCargoSaldosMes = False
                    dcProxy.MovimientosBancos.Clear()
                    dcProxy.SaldosBancoMes.Clear()

                    If _BancoSelected.IDBanco <> -1 And _BancoSelected.IDBanco <> 0 Then

                        If _TabPrincipal = 1 Then

                            If Not IsNothing(loMovimientos) Then
                                If Not loMovimientos.IsComplete Then
                                    loMovimientos.Cancel()
                                End If
                            End If

                            IsBusyMovimientos = True
                            loMovimientos = dcProxy.Load(dcProxy.MovimientosBancosConsultarQuery(_BancoSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMovimientosBancos, Nothing)
                        End If

                        If _TabPrincipal = 2 Then

                            If Not IsNothing(loSaldos) Then
                                If Not loSaldos.IsComplete Then
                                    loSaldos.Cancel()
                                End If
                            End If

                            IsBusySaldos = True
                            loSaldos = dcProxy.Load(dcProxy.SaldosBancoMesConsultarQuery(_BancoSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSaldosBancoMes, Nothing)
                        End If

                    Else
                        bitCargoMovimientos = True
                        bitCargoSaldosMes = True
                    End If
                End If
                'fin ID0001
            End If

            MyBase.CambioItem("BancoSelected")
        End Set
    End Property

    Private _CiudadesClaseSelected As CiudadesClase = New CiudadesClase
    Public Property CiudadesClaseSelected As CiudadesClase
        Get
            Return _CiudadesClaseSelected
        End Get
        Set(ByVal value As CiudadesClase)
            _CiudadesClaseSelected = value
            MyBase.CambioItem("CiudadesClaseSelected")
        End Set
    End Property

    Private _Habilitar As Boolean
    Public Property Habilitar As Boolean
        Get
            Return _Habilitar
        End Get
        Set(ByVal value As Boolean)
            _Habilitar = value
            MyBase.CambioItem("Habilitar")
        End Set
    End Property


    Private _HabilitarCodigo As Boolean = False
    Public Property HabilitarCodigo() As Boolean
        Get
            Return _HabilitarCodigo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCodigo = value
            MyBase.CambioItem("HabilitarCodigo")
            Habilitar = False
            MyBase.CambioItem("Habilitar")
        End Set
    End Property

    Private _habilitarCompania As Boolean = False
    Public Property habilitarCompania() As Boolean
        Get
            Return _habilitarCompania
        End Get
        Set(ByVal value As Boolean)
            _habilitarCompania = value
            MyBase.CambioItem("habilitarCompania")
        End Set
    End Property

    Private _IsBusyMovimientos As Boolean
    Public Property IsBusyMovimientos() As Boolean
        Get
            Return _IsBusyMovimientos
        End Get
        Set(ByVal value As Boolean)
            _IsBusyMovimientos = value
            MyBase.CambioItem("IsBusyMovimientos")
        End Set
    End Property

    Private _IsBusySaldos As Boolean
    Public Property IsBusySaldos() As Boolean
        Get
            Return _IsBusySaldos
        End Get
        Set(ByVal value As Boolean)
            _IsBusySaldos = value
            MyBase.CambioItem("IsBusySaldos")
        End Set
    End Property

    Private _MostrarTabTasas As Visibility = Visibility.Collapsed
    Public Property MostrarTabTasas() As Visibility
        Get
            Return _MostrarTabTasas
        End Get
        Set(value As Visibility)
            _MostrarTabTasas = value
            MyBase.CambioItem("MostrarTabTasas")
        End Set
    End Property



    Private _TabPrincipal As Integer = 0
    Public Property TabPrincipal
        Get
            Return _TabPrincipal
        End Get
        Set(ByVal value)
            _TabPrincipal = value

            'inicia ID0001
            If _TabPrincipal = 1 Then
                If _BancoSelected.IDBanco <> -1 And _BancoSelected.IDBanco <> 0 Then
                    If bitCargoMovimientos = False Then
                        dcProxy.MovimientosBancos.Clear()
                        IsBusyMovimientos = True
                        loMovimientos = dcProxy.Load(dcProxy.MovimientosBancosConsultarQuery(_BancoSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMovimientosBancos, Nothing)
                    End If
                End If
            End If

            If _TabPrincipal = 2 Then
                If _BancoSelected.IDBanco <> -1 And _BancoSelected.IDBanco <> 0 Then
                    If bitCargoSaldosMes = False Then
                        dcProxy.SaldosBancoMes.Clear()
                        IsBusySaldos = True
                        loSaldos = dcProxy.Load(dcProxy.SaldosBancoMesConsultarQuery(_BancoSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSaldosBancoMes, Nothing)
                    End If
                End If
            End If
            'fin ID0001 

            MyBase.CambioItem("TabPrincipal")
        End Set
    End Property

    Private _ListaTipoCuentaRecaudadora As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTipoCuentaRecaudadora As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTipoCuentaRecaudadora
        End Get
        Set(value As List(Of OYDUtilidades.ItemCombo))
            _ListaTipoCuentaRecaudadora = value
            MyBase.CambioItem("ListaTipoCuentaRecaudadora")
        End Set
    End Property

    Private _MOSTRAR_TIPO_CUENTA_RECAUDADORA_BANCOS As Visibility = Visibility.Collapsed
    Public Property MOSTRAR_TIPO_CUENTA_RECAUDADORA_BANCOS As Visibility
        Get
            Return _MOSTRAR_TIPO_CUENTA_RECAUDADORA_BANCOS
        End Get
        Set(value As Visibility)
            _MOSTRAR_TIPO_CUENTA_RECAUDADORA_BANCOS = value
            MyBase.CambioItem("MOSTRAR_TIPO_CUENTA_RECAUDADORA_BANCOS")
        End Set
    End Property


#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim NewBanco As New Banco

            'TODO: Verificar cuales son los campos que deben inicializarse
            NewBanco.IDComisionista = BancoPorDefecto.IDComisionista
            NewBanco.IDSucComisionista = BancoPorDefecto.IDSucComisionista
            NewBanco.ID = BancoPorDefecto.ID
            NewBanco.Nombre = BancoPorDefecto.Nombre
            NewBanco.NombreSucursal = BancoPorDefecto.NombreSucursal
            NewBanco.NroCuenta = BancoPorDefecto.NroCuenta
            NewBanco.Telefono1 = BancoPorDefecto.Telefono1
            NewBanco.Telefono2 = BancoPorDefecto.Telefono2
            NewBanco.Fax1 = BancoPorDefecto.Fax1
            NewBanco.Fax2 = BancoPorDefecto.Fax2
            NewBanco.Direccion = BancoPorDefecto.Direccion
            NewBanco.Internet = BancoPorDefecto.Internet
            NewBanco.ChequeraAutomatica = BancoPorDefecto.ChequeraAutomatica
            NewBanco.NombreConsecutivo = BancoPorDefecto.NombreConsecutivo
            NewBanco.IDPoblacion = Nothing
            NewBanco.IDDepartamento = BancoPorDefecto.IDDepartamento
            NewBanco.IDPais = BancoPorDefecto.IDPais
            NewBanco.NombreGerente = BancoPorDefecto.NombreGerente
            NewBanco.NombreCajero = BancoPorDefecto.NombreCajero
            NewBanco.NombrePortero = BancoPorDefecto.NombrePortero
            NewBanco.Creacion = BancoPorDefecto.Creacion
            NewBanco.IDOwner = BancoPorDefecto.IDOwner
            NewBanco.IdCuentaCtble = BancoPorDefecto.IdCuentaCtble
            NewBanco.CtaActiva = BancoPorDefecto.CtaActiva
            NewBanco.Usuario = Program.Usuario
            NewBanco.Reporte = BancoPorDefecto.Reporte
            NewBanco.CobroGMF = BancoPorDefecto.CobroGMF
            NewBanco.TipoCta = BancoPorDefecto.TipoCta
            NewBanco.TipoCuenta = BancoPorDefecto.TipoCuenta
            NewBanco.IdCodBanco = BancoPorDefecto.IdCodBanco
            NewBanco.IDBanco = BancoPorDefecto.IDBanco
            NewBanco.IdMoneda = BancoPorDefecto.IdMoneda
            NewBanco.lngNumCheque = Nothing
            NewBanco.Compania = intIDCompaniaFirma
            NewBanco.NombreCompania = strNombreCompaniaFirma
            'JCM20160229
            NewBanco.xmlDetalleGrid = String.Empty
            ListaBancoTasasRendimientos = Nothing
            ListaBancoTasasRendimientosSeleccionado = Nothing
            HabilitarGridTasas = Visibility.Collapsed
            HabilitarTextTasas = Visibility.Visible
            NewBanco.logManejaRangos = False




            BancoAnterior = BancoSelected
            BancoSelected = NewBanco
            PropiedadTextoCombos = ""
            MyBase.CambioItem("Bancos")
            Editando = True
            MyBase.CambioItem("Editando")
            HabilitarCodigo = True
            CiudadesClaseSelected.Ciudad = String.Empty
            CiudadesClaseSelected.Departamento = String.Empty
            CiudadesClaseSelected.Pais = String.Empty
            'If NewBanco.Compania = 0 Then

            habilitarCompania = True

            'Else
            'habilitarCompania = False
            'End If
            _mlogNuevo = True

            MyBase.CambiarFormulario_Forma_Manual()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConultarCompaniaFirma(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If lo.Entities.Count > 0 Then
                    intIDCompaniaFirma = lo.Entities.First.intID
                    strNombreCompaniaFirma = lo.Entities.First.Descripcion

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de la compañia",
                                                 Me.ToString(), "TerminoConultarCompaniaFirma", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de compañia",
                                                 Me.ToString(), "TerminoConultarCompaniaFirma", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConultarTipoCuentaRecaudadora(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If lo.Entities.Count > 0 Then
                    ListaTipoCuentaRecaudadora = lo.Entities.ToList
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error en la obtención de la lista de tipo cuenta recaudadora.",
                                                 Me.ToString(), "TerminoConultarTipoCuentaRecaudadora", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de tipo cuenta recaudadora.",
                                                 Me.ToString(), "TerminoConultarTipoCuentaRecaudadora", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Bancos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.BancosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.BancosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Or cb.Nombre <> String.Empty Or cb.NombreSucursal <> String.Empty Or cb.CompaniaBanco <> 0 Or cb.NroCuenta <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Bancos.Clear()
                BancoAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " ID = " & cb.ID.ToString() & " Nombre = " & cb.Nombre.ToString() & " NombreSucursal = " & cb.NombreSucursal.ToString()
                dcProxy.Load(dcProxy.BancosConsultarQuery(cb.ID, cb.Nombre, cb.NombreSucursal, cb.CompaniaBanco, cb.NroCuenta, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaBanco
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim strmensaje As String
            Dim strMsg As String = String.Empty
            strmensaje = ""
            'strmensaje = "Se creará el código de compañia: " + BancoSelected.Compania.ToString() + " - " + BancoSelected.NombreCompania + ", Confirma la grabación de la compañia?"
            If ValidarBanco() Then

                'JCM 20160229
                CrearDetalleTasas()

                If BancoSelected.ChequeraAutomatica = True Then
                    If BancoSelected.NombreConsecutivo = "" Or (BancoSelected.lngNumCheque = 0 Or IsNothing(_BancoSelected.lngNumCheque)) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe validar que los campos: Consecutivo y/o Próximo cheque estén llenos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

                'Santiago Vergara - Diciembre 05/2013 - Se añade lógica para que no valide cuendo es CITI
                If BancoSelected.ChequeraAutomatica = True And VersionAplicacionCliente <> EnumVersionAplicacionCliente.C.ToString Then
                    If String.IsNullOrEmpty(_BancoSelected.Reporte) Then
                        A2Utilidades.Mensajes.mostrarMensaje("El nombre del reporte para cheques del Banco es obligatorio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

                If _BancoSelected.logManejaRangos = True And MostrarTabTasas = Visibility.Visible Then

                    'JCM 20160921 - Se comenta la verificación de un registro máximo en las tasas de rendimientos
                    If _ListaBancoTasasRendimientos IsNot Nothing Then
                        If _ListaBancoTasasRendimientos.Count = 1 Then
                            strMsg = String.Format("{0}{1} + No es posible ingresar un solo registro en el detalle de las tasas de rendimiento, si se manejan rangos", strMsg, vbCrLf)
                        End If
                    End If

                    'JCM 20160301 -Se añade lógica para verifcar los datos del detalle no se solapen
                    If _ListaBancoTasasRendimientos IsNot Nothing Then
                        If _ListaBancoTasasRendimientos.Count > 1 Then
                            _ListaBancoTasasRendimientos = _ListaBancoTasasRendimientos.OrderBy(Function(i) i.dblValorInicial).ToList
                            Dim k As Integer
                            Dim l As Integer
                            For k = 0 To _ListaBancoTasasRendimientos.Count - 1
                                For l = k + 1 To _ListaBancoTasasRendimientos.Count - 1
                                    If Not _ListaBancoTasasRendimientos(l).dblValorInicial > _ListaBancoTasasRendimientos(k).dblValorFinal Then
                                        strMsg = String.Format("{0}{1} + Existe un conflicto ya que el valor inicial" & " " & _ListaBancoTasasRendimientos(l).dblValorInicial & "," & " " & "no es mayor que el valor final en el detalle de las tasas de rendimientos" & " " & _ListaBancoTasasRendimientos(k).dblValorFinal, strMsg, vbCrLf)
                                    End If
                                Next
                            Next
                        End If
                    End If

                    ''JCM 20160302- Se añade lógica para garantizar que se ingrese el ultimo registro como el máximo
                    ''JCM 20160921 - Se comenta la verificación de un registro máximo en las tasas de rendimientos
                    'If _ListaBancoTasasRendimientos IsNot Nothing Then
                    '    If _ListaBancoTasasRendimientos.Count >= 1 Then
                    '        '_ListaBancoTasasRendimientos = _ListaBancoTasasRendimientos.OrderByDescending(Function(i) i.dblValorFinal).ToList
                    '        Dim G As Integer
                    '        Dim strMsgAux As String
                    '        strMsgAux = String.Empty
                    '        For G = 0 To _ListaBancoTasasRendimientos.Count - 1
                    '            If _ListaBancoTasasRendimientos(G).dblValorFinal = Program.ValorMaximoRangoTasasBanco Then
                    '                strMsgAux = "Encontro el maximo valor"
                    '                Exit For
                    '            Else
                    '                strMsgAux = "No encontro el maximo valor"
                    '            End If
                    '        Next

                    '        If strMsgAux = "No encontro el maximo valor" Then
                    '            strMsg = String.Format("{0}{1} + Existe un conflicto ya que no se ha ingresado un valor máximo en el detalle de las tasas de rendimientos", strMsg, vbCrLf)
                    '        End If


                    '    End If
                    'End If

                    'JCM20160302- Se añade la lógica para garantizar que se mantenga un consecutivo 
                    If _ListaBancoTasasRendimientos IsNot Nothing Then
                        If _ListaBancoTasasRendimientos.Count >= 1 Then
                            _ListaBancoTasasRendimientos = _ListaBancoTasasRendimientos.OrderBy(Function(i) i.dblValorInicial).ToList
                            If _ListaBancoTasasRendimientos(0).dblValorInicial <> 0 Then
                                strMsg = String.Format("{0}{1} + Existe un conflicto ya que el valor inicial debe iniciar en 0 en el detalle de las tasas de rendimiento", strMsg, vbCrLf)
                            End If
                            Dim L As Integer
                            Dim M As Integer
                            For L = 0 To _ListaBancoTasasRendimientos.Count - 1

                                For M = L + 1 To _ListaBancoTasasRendimientos.Count - 1
                                    If _ListaBancoTasasRendimientos(M).dblValorInicial - _ListaBancoTasasRendimientos(L).dblValorFinal <> 1 Then
                                        ' JFSB 20160827 Se agrega if para comparar el valor final con el valor máximo de rango permitido y si es igual, no le sume 1
                                        If _ListaBancoTasasRendimientos(L).dblValorFinal = Program.ValorMaximoRangoTasasBanco Then
                                            strMsg = String.Format("{0}{1} + Existe un conflicto ya que el valor inicial" & " " & _ListaBancoTasasRendimientos(M).dblValorInicial & "," & " " & "no corresponde al consecutivo en el detalle de las tasas de rendimiento" & " " & _ListaBancoTasasRendimientos(L).dblValorFinal, strMsg, vbCrLf)
                                        Else
                                            strMsg = String.Format("{0}{1} + Existe un conflicto ya que el valor inicial" & " " & _ListaBancoTasasRendimientos(M).dblValorInicial & "," & " " & "no corresponde al consecutivo en el detalle de las tasas de rendimiento" & " " & _ListaBancoTasasRendimientos(L).dblValorFinal + 1, strMsg, vbCrLf)
                                        End If

                                    End If
                                    Exit For
                                Next
                            Next
                        End If
                    End If


                    'JCM20160303
                    If _ListaBancoTasasRendimientos IsNot Nothing Then
                        If _ListaBancoTasasRendimientos.Count >= 1 Then
                            '_ListaBancoTasasRendimientos = _ListaBancoTasasRendimientos.OrderByDescending(Function(i) i.dblValorFinal).ToList
                            Dim G As Integer
                            Dim strMsgAux2 As String
                            strMsgAux2 = String.Empty
                            For G = 0 To _ListaBancoTasasRendimientos.Count - 1
                                If _ListaBancoTasasRendimientos(G).dblValorFinal > Program.ValorMaximoRangoTasasBanco Then
                                    strMsgAux2 = "Encontró el dato"
                                    Exit For
                                Else
                                    strMsgAux2 = "No encontró el dato"
                                End If
                            Next

                            If strMsgAux2 = "Encontró el dato" Then
                                strMsg = String.Format("{0}{1} + Existe un conflicto ya que hay un registro que supera el valor máximo permitido en el detalle de las tasas de rendimientos", strMsg, vbCrLf)
                            End If


                        End If
                    End If

                    If Not strMsg.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias" & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If

                End If


                If _BancoSelected.logManejaRangos = False And MostrarTabTasas = Visibility.Visible Then
                    If BancoSelected.dblTasaRendimientoUnica < 0 Or BancoSelected.dblTasaRendimientoUnica > 100 Then
                        strMsg = String.Format("{0}{1} + La tasa de rendimimiento se debe especificar en un porcentaje de rango entre 0-100", strMsg, vbCrLf)
                        If Not strMsg.Equals(String.Empty) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias" & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                    End If
                End If


                Dim origen = "update"
                ErrorForma = ""
                'BancoAnterior = BancoSelected
                If Not ListaBancos.Where(Function(i) i.IDBanco = _BancoSelected.IDBanco).Count > 0 Then
                    origen = "insert"
                    ListaBancos.Add(BancoSelected)
                End If

                'Dim Result As MessageBoxResult

                'If _mlogNuevo And habilitarCompania Then
                '    Result = MessageBox.Show(strmensaje, "Advertencia", MessageBoxButton.OKCancel)
                '    If Result = MessageBoxResult.Cancel Then
                '        A2Utilidades.Mensajes.mostrarMensaje("Se cancela la grabación de la compañia", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '        Exit Sub
                '    End If
                'End If

                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
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
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'If So.UserState = "insert" Then
                    '    ListaEmpleados.Remove(EmpleadoSelected)
                    'End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                So.MarkErrorAsHandled()
                Exit Try
            Else
                _mlogNuevo = False
                Habilitar = False
                HabilitarCodigo = False
                If So.UserState = "update" Or So.UserState = "insert" Then
                    intIDRegistroGuardado = _BancoSelected.ID
                    dcProxy.Bancos.Clear()
                    IsBusy = True
                    If FiltroVM.Length > 0 Then
                        Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                        dcProxy.Load(dcProxy.BancosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos, "actualizacion")
                    Else
                        dcProxy.Load(dcProxy.BancosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos, "actualizacion")
                    End If
                Else
                    habilitarCompania = False
                    Filtrar()
                End If

            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_BancoSelected) Then
            If Not _BancoSelected.CtaActiva Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("No se puede editar este Banco ya que se encuentra Inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            _mlogNuevo = False
            Editando = True
            HabilitarCodigo = False
            habilitarCompania = False
            If BancoSelected.ChequeraAutomatica Then
                Habilitar = True
            Else
                Habilitar = False
            End If
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub



    Public Overrides Async Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_BancoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _BancoSelected.EntityState = EntityState.Detached Then
                    BancoSelected = BancoAnterior
                End If
            End If
            Habilitar = False
            HabilitarCodigo = False
            habilitarCompania = False
            _mlogNuevo = False
            buscarItem("ciudades")
            'JCM 20160303
            If _BancoSelected.IDBanco > 0 Then
                ConsultarDetalleTasasRendimientos(_BancoSelected.IDBanco)
            End If
            'JCM 20160303
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_BancoSelected) Then
                IsBusy = True
                dcProxy.EliminarCuentasBancarias(String.Empty, BancoSelected.IDBanco, Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
            End If
            Habilitar = False
            HabilitarCodigo = False
            _mlogNuevo = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _BancoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _BancoSelected.PropertyChanged
        If Editando Then
            If e.PropertyName.Equals("ChequeraAutomatica") Then
                If BancoSelected.ChequeraAutomatica = True Then
                    Habilitar = True
                Else
                    Habilitar = False
                    'BancoSelected.lngNumCheque = Nothing
                    'BancoSelected.NombreConsecutivo = String.Empty
                    BancoSelected.Reporte = Nothing
                End If
            ElseIf e.PropertyName.Equals("logManejaRangos") Then
                If BancoSelected.logManejaRangos = True Then
                    HabilitarGridTasas = Visibility.Visible
                    HabilitarTextTasas = Visibility.Collapsed
                Else
                    HabilitarGridTasas = Visibility.Collapsed
                    HabilitarTextTasas = Visibility.Visible
                End If
            ElseIf e.PropertyName.Equals("IdCuentaCtble") Then
                If Not String.IsNullOrEmpty(_BancoSelected.IdCuentaCtble) Then
                    buscarGenerico(_BancoSelected.IdCuentaCtble, "CuentasContables")
                End If
            End If
        End If
    End Sub

    Friend Sub buscarGenerico(Optional ByVal pstrCentroCostos As String = "", Optional ByVal pstrBusqueda As String = "")
        Try
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemEspecificoQuery(pstrBusqueda, pstrCentroCostos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBuscadorGenerico, pstrBusqueda)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerBuscadorGenerico(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "CuentasContables"
                        If lo.Entities.ToList.Count > 0 Then
                            _BancoSelected.IdCuentaCtble = lo.Entities.First.IdItem
                        Else
                            sw = 1
                            A2Utilidades.Mensajes.mostrarMensaje("La cuenta contable ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _BancoSelected.IdCuentaCtble = Nothing
                        End If
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos",
                                             Me.ToString(), "TerminoTraerBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el banco", Me.ToString(),
                                                             "TerminoTraerCuentasBancarias", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Function ValidarBanco()
        Try
            Dim Mensaje As String = String.Empty

            If _BancoSelected.ID = 0 Then
                Mensaje &= "El código es requerido." & vbCrLf
            End If

            If IsNothing(BancoSelected.Nombre) Or BancoSelected.Nombre = String.Empty Then
                Mensaje &= "El nombre es requerido." & vbCrLf
            End If

            If IsNothing(BancoSelected.NombreSucursal) Or BancoSelected.NombreSucursal = String.Empty Then
                Mensaje &= "El nombre de la sucursal es requerido." & vbCrLf
            End If

            'SLB20130716 Validaciones faltantes
            If String.IsNullOrEmpty(_BancoSelected.NroCuenta) Then
                Mensaje &= "Debe ingresar el número de la cuenta." & vbCrLf
            End If

            'Se comenta la validación para que no contemple el código cero del banco nacional ya que este código puede existir CVA20171221
            If IsNothing(BancoSelected.IdCodBanco) Then 'Or BancoSelected.IdCodBanco = 0 Then
                Mensaje &= "El banco es requerido." & vbCrLf
            End If

            If IsNothing(BancoSelected.IDPoblacion) Then
                Mensaje &= "La ciudad es requerida." & vbCrLf
            End If

            If IsNothing(BancoSelected.IDOwner) Then
                Mensaje &= "El módulo es requerido." & vbCrLf
            End If

            'SLB20130716 Inicio Validaciones faltantes
            If String.IsNullOrEmpty(_BancoSelected.TipoCuenta) Then
                Mensaje &= "El submódulo es requerido." & vbCrLf
            End If

            If IsNothing(_BancoSelected.IDFormato) Then
                Mensaje &= "Debe elegir el formato ACH para este banco." & vbCrLf
                'JFSB 201709222 Se agrega validación para cuando el formato sea vacío
            ElseIf _BancoSelected.IDFormato = 0 Then
                Mensaje &= "Debe elegir el formato ACH para este banco." & vbCrLf
            End If

            If IsNothing(_BancoSelected.IdMoneda) Or _BancoSelected.IdMoneda = 0 Then
                Mensaje &= "La moneda es un dato requerido." & vbCrLf
            End If
            'SLB20130716 Fin Validaciones faltantes

            If _BancoSelected.Compania <= 0 Or IsNothing(_BancoSelected.Compania) Then
                Mensaje &= "La compañía es un dato requerido." & vbCrLf
            End If

            ' JFSB 20160827 Se agrega validación para el tipo de cuenta
            If IsNothing(_BancoSelected.TipoCta) Or String.IsNullOrEmpty(_BancoSelected.TipoCta) Then
                Mensaje &= "El tipo de cuenta es un dato requerido." & vbCrLf
            End If

            'If IdMonedaCompanias <> 0 Then
            '    If _BancoSelected.IdMoneda <> IdMonedaCompanias Then
            '        Mensaje &= "La moneda seleccionada no es igual a la moneda de la compañía" & vbCrLf
            '    End If
            'End If

            If UtilizaPasiva = True Then
                If strTipoCuentaCompanias <> "" Then
                    If strTipoCuentaCompanias <> _BancoSelected.TipoCuenta Then
                        Mensaje &= "El módulo seleccionado en el banco es distinto al módulo al que pertenece la compañía." & vbCrLf
                    End If
                End If
            End If



            If Mensaje <> String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(Mensaje, "OyD Server", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            Else
                Return True
            End If


            'Valida si el codigo ya existe.
            'For Each Lista In ListaBancos
            '    If Lista.ID = BancoSelected.ID Then
            '        A2Utilidades.Mensajes.mostrarMensaje("El Código elegido ya ha sido asignado.", "OyD Server", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        Exit Sub
            '    End If
            'Next

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar la validación de los bancos.", _
                                 Me.ToString(), "ValidarBanco", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabPrincipal = miTab
        End If
    End Sub

    Public Sub llenarDiccionario()
        DicCamposTab.Add("IDBanco", 1)
        DicCamposTab.Add("IdcodBanco", 1)
        'DicCamposTab.Add("Nombre", 0)
        'DicCamposTab.Add("IDSucCliente", 0)
        'DicCamposTab.Add("RetFuente", 1)
        'DicCamposTab.Add("IDSector", 0)
        'DicCamposTab.Add("TipoIdentificacion", 0)
        'DicCamposTab.Add("IDSubSector", 0)
        'DicCamposTab.Add("IDGrupo", 0)
        'DicCamposTab.Add("IDSubGrupo", 0)
        'DicCamposTab.Add("OrigenIngresos", 1)
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaBanco
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la búsqueda", _
             Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' JCM20160229
    ''' Método encargado de ingresar un registro al Grid de Tasas de rendimientos
    ''' 
    ''' </summary>

    Public Sub IngresarDetalleBancoTasasRendimientos(ByVal pdblvalorInicial As Double, ByVal pdblValorFinal As Double, ByVal pdblTasaRendimiento As Double)
        Try


            'Se ingresa un nuevo registro JCM20160229
            Dim objNvoDetalle As New A2.OyD.OYDServer.RIA.Web.BancosTasasRendimientos
            Dim objNuevaLista As New List(Of A2.OyD.OYDServer.RIA.Web.BancosTasasRendimientos)



            'Program.CopiarObjeto(Of A2.OYD.OYDServer.RIA.Web.BancosTasasRendimientos)(mobjDetalleBancoTasaRedimientoPorDefecto, objNvoDetalle)
            If pdblvalorInicial < Program.ValorMaximoRangoTasasBanco And pdblValorFinal <= Program.ValorMaximoRangoTasasBanco Then

                If pdblTasaRendimiento > 0 And pdblTasaRendimiento <= 100 Then

                    objNvoDetalle.intIDBancoTasasRendimientos = -New Random().Next(0, 1000000)

                    If IsNothing(ListaBancoTasasRendimientos) Then
                        ListaBancoTasasRendimientos = New List(Of BancosTasasRendimientos)
                    End If


                    objNuevaLista = ListaBancoTasasRendimientos


                    objNuevaLista.Add(New BancosTasasRendimientos With {.intIDBancoTasasRendimientos = objNvoDetalle.intIDBancoTasasRendimientos,
                                                                        .dblValorInicial = pdblvalorInicial,
                                                                        .dblValorFinal = pdblValorFinal,
                                                                        .dblTasaRendimiento = pdblTasaRendimiento
                                                                       })



                    ListaBancoTasasRendimientos = objNuevaLista
                    ListaBancoTasasRendimientosSeleccionado = _ListaBancoTasasRendimientos.First



                    MyBase.CambioItem("ListaBancoTasasRendimientos")
                    MyBase.CambioItem("ListaBancoTasasRendimientosPaginada")
                    MyBase.CambioItem("ListaBancoTasasRendimientosSeleccionado")

                Else
                    A2Utilidades.Mensajes.mostrarMensaje("La tasa de rendimiento debe ser un porcentaje entre 0-100. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If


            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle para las tasas de de rendimientos de el banco.", Me.ToString(), "IngresarDetalleBancoTasasRendimientos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' JCM20160229
    ''' Método encargado de editar un registro en el grid
    ''' 
    ''' </summary>


    Public Sub EditarDetalleBancoTasasRendimientos(ByVal pdblValorInicial As Double, ByVal pdblValorFinal As Double, ByVal pdblTasaRendimiento As Double)
        Try

            If pdblTasaRendimiento > 0 And pdblTasaRendimiento <= 100 Then

                'Se edita el registro seleccionado JCM20160229
                Dim objNvoDetalle As New A2.OyD.OYDServer.RIA.Web.BancosTasasRendimientos
                Dim objNuevaLista As New List(Of A2.OyD.OYDServer.RIA.Web.BancosTasasRendimientos)

                ListaBancoTasasRendimientosSeleccionado.dblValorInicial = pdblValorInicial
                ListaBancoTasasRendimientosSeleccionado.dblValorFinal = pdblValorFinal
                ListaBancoTasasRendimientosSeleccionado.dblTasaRendimiento = pdblTasaRendimiento


                MyBase.CambioItem("ListaBancoTasasRendimientos")
                MyBase.CambioItem("ListaBancoTasasRendimientosPaginada")
                MyBase.CambioItem("ListaBancoTasasRendimientosSeleccionado")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("La tasa de rendimiento debe ser un porcentaje entre 0-100. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición del registro de las tasas de rendimiento.", Me.ToString(), "EditarDetalleBancoTasasRendimientos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' JCM20160229
    ''' Método encargado de borrar un registro del grid
    ''' 
    ''' </summary>

    Private Sub BorrarDetalleBancoTasasRendimientos(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If objResultado.DialogResult Then

                'Se edita el registro se
                If Not IsNothing(ListaBancoTasasRendimientosSeleccionado) Then
                    If _ListaBancoTasasRendimientos.Where(Function(i) i.intIDBancoTasasRendimientos = _ListaBancoTasasRendimientosSeleccionado.intIDBancoTasasRendimientos).Count > 0 Then
                        _ListaBancoTasasRendimientos.Remove(_ListaBancoTasasRendimientos.Where(Function(i) i.intIDBancoTasasRendimientos = _ListaBancoTasasRendimientosSeleccionado.intIDBancoTasasRendimientos).First)
                    End If

                    Dim objNuevaListaBancoTasasRendimientos As New List(Of BancosTasasRendimientos)

                    For Each li In _ListaBancoTasasRendimientos
                        objNuevaListaBancoTasasRendimientos.Add(li)
                    Next

                    If objNuevaListaBancoTasasRendimientos.Where(Function(i) i.intIDBancoTasasRendimientos = _ListaBancoTasasRendimientosSeleccionado.intIDBancoTasasRendimientos).Count > 0 Then
                        objNuevaListaBancoTasasRendimientos.Remove(objNuevaListaBancoTasasRendimientos.Where(Function(i) i.intIDBancoTasasRendimientos = _ListaBancoTasasRendimientosSeleccionado.intIDBancoTasasRendimientos).First)
                    End If

                    ListaBancoTasasRendimientos = objNuevaListaBancoTasasRendimientos

                    If Not IsNothing(_ListaBancoTasasRendimientos) Then
                        If _ListaBancoTasasRendimientos.Count > 0 Then
                            ListaBancoTasasRendimientosSeleccionado = _ListaBancoTasasRendimientos.First
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar las tasas de rendimientos de los bancos", Me.ToString(), "BorrarDetalleBancoTasasRendimientos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' JCM20160229
    ''' Método encargado de "armar" el xml que inserta los datos en la tabla tblBancosRendimientos
    ''' 
    ''' </summary>

    Private Sub CrearDetalleTasas()
        Try
            Dim XmlCompletoTasasRendimientos As String
            Dim XmlTasasRendimientos As String

            'Construir el XML detalle de las Tasas Rendimientos
            XmlCompletoTasasRendimientos = "<BancosRendimientos>"
            If MostrarTabTasas = Visibility.Visible Then

                If _BancoSelected.logManejaRangos = True Then

                    If Not IsNothing(ListaBancoTasasRendimientos) Then
                        For Each Objeto In (From c In ListaBancoTasasRendimientos)
                            XmlTasasRendimientos = "<Detalle intIDBanco=""" & Objeto.intIDBanco &
                                                   """ dblValorInicial=""" & Objeto.dblValorInicial &
                                                   """ dblValorFinal=""" & Objeto.dblValorFinal &
                                                   """ dblTasaRendimiento=""" & Objeto.dblTasaRendimiento & """></Detalle>"

                            XmlCompletoTasasRendimientos = XmlCompletoTasasRendimientos & XmlTasasRendimientos
                        Next
                    End If

                    XmlCompletoTasasRendimientos = XmlCompletoTasasRendimientos & "</BancosRendimientos>"
                    _BancoSelected.xmlDetalleGrid = XmlCompletoTasasRendimientos
                Else
                    XmlTasasRendimientos = "<Detalle intIDBanco=""" & 0 &
                                               """ dblValorInicial=""" & 0 &
                                               """ dblValorFinal=""" & Program.ValorMaximoRangoTasasBanco &
                                               """ dblTasaRendimiento=""" & _BancoSelected.dblTasaRendimientoUnica & """></Detalle>"

                    XmlCompletoTasasRendimientos = XmlCompletoTasasRendimientos & XmlTasasRendimientos
                    XmlCompletoTasasRendimientos = XmlCompletoTasasRendimientos & "</BancosRendimientos>"
                    _BancoSelected.xmlDetalleGrid = XmlCompletoTasasRendimientos

                End If
            Else
                XmlCompletoTasasRendimientos = XmlCompletoTasasRendimientos & "</BancosRendimientos>"
                _BancoSelected.xmlDetalleGrid = XmlCompletoTasasRendimientos
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ValidarTasas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se pregunta si en realidad se desea eliminar el detalle del Grid de tasas de Rendimientos.
    ''' ID caso de prueba:  
    ''' </summary>
    Public Sub ValidarBorrarConfirmacionTasasRendimientos()
        Dim strMsg As String
        ' JFSB 20160827 Se agrega tilde y signo de interrogación al mensaje
        strMsg = "Está seguro que desea eliminar el registro ?"
        A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf BorrarDetalleBancoTasasRendimientos)

    End Sub

    ''' <summary>
    ''' JCM20160301
    ''' Método que verifica si ya existe ingresao un registro con valores inicial en 0 y valor final en 0
    ''' 
    ''' </summary>


    Public Function ValidarRegistroDetalleTasasRendimientos() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            If Not IsNothing(_ListaBancoTasasRendimientos) Then
                If _ListaBancoTasasRendimientos.Count >= 1 Then
                    '_ListaBancoTasasRendimientos = _ListaBancoTasasRendimientos.OrderByDescending(Function(i) i.dblValorFinal).ToList
                    Dim k As Integer
                    k = _ListaBancoTasasRendimientos.Count - 1

                    _ListaBancoTasasRendimientosProximosValores = New BancosTasasRendimientos

                    If _ListaBancoTasasRendimientos(k).dblValorFinal + 1 < Program.ValorMaximoRangoTasasBanco Then
                        _ListaBancoTasasRendimientosProximosValores.dblValorInicial = _ListaBancoTasasRendimientos(k).dblValorFinal + 1
                    Else
                        _ListaBancoTasasRendimientosProximosValores.dblValorInicial = 0
                    End If

                Else
                    strMsg = "No existe ningun registro en el grid"
                End If
            Else
                strMsg = "No existe ningun registro en el grid"
            End If

            If strMsg <> String.Empty Then
                logResultado = False
            End If


        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle de tasas de rendimientos.", Me.ToString(), "ValidarDetalleTasasRendimientos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function


    Public Sub ValidarDatosGrid(ByVal pdblValorInicial As Double, ByVal pdblValorFinal As Double, ByVal pdblTasaRendimiento As Double)

        Try


            'Se edita el registro seleccionado JCM20160229
            Dim objNvoDetalle As New A2.OyD.OYDServer.RIA.Web.BancosTasasRendimientos
            Dim objNuevaLista As New List(Of A2.OyD.OYDServer.RIA.Web.BancosTasasRendimientos)

            ListaBancoTasasRendimientosSeleccionado.dblValorInicial = pdblValorInicial
            ListaBancoTasasRendimientosSeleccionado.dblValorFinal = pdblValorFinal
            ListaBancoTasasRendimientosSeleccionado.dblTasaRendimiento = pdblTasaRendimiento

            MyBase.CambioItem("ListaBancoTasasRendimientos")
            MyBase.CambioItem("ListaBancoTasasRendimientosPaginada")
            MyBase.CambioItem("ListaBancoTasasRendimientosSeleccionado")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición del registro de las tasas de rendimiento.", Me.ToString(), "EditarDetalleBancoTasasRendimientos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Tablas Hijas"

#Region "SaldosBancoMes"
    '******************************************************** SaldosBancoMes 
    Private _ListaSaldosBancoMes As EntitySet(Of SaldosBancoMes)
    Public Property ListaSaldosBancoMes() As EntitySet(Of SaldosBancoMes)
        Get
            Return _ListaSaldosBancoMes
        End Get
        Set(ByVal value As EntitySet(Of SaldosBancoMes))
            _ListaSaldosBancoMes = value
            MyBase.CambioItem("ListaSaldosBancoMes")
            MyBase.CambioItem("ListaSaldosBancoMesPaged")
        End Set
    End Property

    Public ReadOnly Property ListaSaldosBancoMesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaSaldosBancoMes) Then
                Dim view = New PagedCollectionView(_ListaSaldosBancoMes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _ListaSaldosBancoMesAnterior As List(Of SaldosBancoMes)
    Public Property ListaSaldosBancoMesAnterior() As List(Of SaldosBancoMes)
        Get
            Return _ListaSaldosBancoMesAnterior
        End Get
        Set(ByVal value As List(Of SaldosBancoMes))
            _ListaSaldosBancoMesAnterior = value
            MyBase.CambioItem("ListaSaldosBancoMesAnterior")
        End Set
    End Property

    Private _SaldosBancoMesSelected As SaldosBancoMes
    Public Property BancoSaldosBancoMesSelected() As SaldosBancoMes
        Get
            Return _SaldosBancoMesSelected
        End Get
        Set(ByVal value As SaldosBancoMes)

            If Not value Is Nothing Then
                _SaldosBancoMesSelected = value
                MyBase.CambioItem("SaldosBancoMesSelected")
            End If
        End Set
    End Property

#End Region

#Region "MovimientosBancos"
    '******************************************************** MovimientosBancos 
    Private _ListaMovimientosBancos As EntitySet(Of MovimientosBancos)
    Public Property ListaMovimientosBancos() As EntitySet(Of MovimientosBancos)
        Get
            Return _ListaMovimientosBancos
        End Get
        Set(ByVal value As EntitySet(Of MovimientosBancos))
            _ListaMovimientosBancos = value
            MyBase.CambioItem("ListaMovimientosBancos")
            MyBase.CambioItem("ListaMovimientosBancosPaged")
        End Set
    End Property

    Public ReadOnly Property ListaMovimientosBancosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaMovimientosBancos) Then
                Dim view = New PagedCollectionView(_ListaMovimientosBancos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _ListaMovimientosBancosAnterior As List(Of MovimientosBancos)
    Public Property ListaMovimientosBancosAnterior() As List(Of MovimientosBancos)
        Get
            Return _ListaMovimientosBancosAnterior
        End Get
        Set(ByVal value As List(Of MovimientosBancos))
            _ListaMovimientosBancosAnterior = value
            MyBase.CambioItem("ListaMovimientosBancosAnterior")
        End Set
    End Property

    Private _MovimientosBancosSelected As MovimientosBancos
    Public Property MovimientosBancosSelected() As MovimientosBancos
        Get
            Return _MovimientosBancosSelected
        End Get
        Set(ByVal value As MovimientosBancos)

            If Not value Is Nothing Then
                _MovimientosBancosSelected = value
                MyBase.CambioItem("MovimientosBancosSelected")
            End If
        End Set
    End Property

#End Region
#End Region

    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.BancoSelected Is Nothing Then
                Select Case pstrTipoItem.ToLower()
                    Case "ciudades"


                        pstrIdItem = pstrIdItem.Trim()
                        If pstrIdItem.Equals(String.Empty) Then

                            If Not IsNothing(Me.BancoSelected.IDPoblacion) Then
                                strIdItem = Me.BancoSelected.IDPoblacion
                            End If
                        Else
                            strIdItem = pstrIdItem
                        End If
                        If Not strIdItem.Equals(String.Empty) Then
                            logConsultar = True
                        End If
                        If logConsultar Then
                            If mdcProxyUtilidad01.IsLoading Then
                                mdcProxyUtilidad01.RejectChanges()
                            End If

                            mdcProxyUtilidad01.BuscadorGenericos.Clear()
                            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                        End If
                    Case Else
                        logConsultar = False
                End Select


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarItem", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

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
                    Case "ciudades"
                        Me.CiudadesClaseSelected.Ciudad = lo.Entities.ToList.Item(0).Nombre
                        Me.CiudadesClaseSelected.Departamento = lo.Entities.ToList.Item(0).CodigoAuxiliar
                        Me.CiudadesClaseSelected.Pais = lo.Entities.ToList.Item(0).InfoAdicional02
                End Select
            Else
                Me.CiudadesClaseSelected.Ciudad = String.Empty
                Me.CiudadesClaseSelected.Departamento = String.Empty
                Me.CiudadesClaseSelected.Pais = String.Empty

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarGenericoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#Region "Propiedades que definen atributos de la orden"

    Private _mCompania As Integer
    <Display(Name:="Compañía")>
    Public Property CompaniaBanco() As Integer
        Get
            Return (_mCompania)
        End Get
        Set(ByVal value As Integer)
            If value.Equals(String.Empty) Then
                _mCompania = value
                _CompaniaSeleccionado = Nothing
            ElseIf Not Versioned.IsNumeric(value) Then
                A2Utilidades.Mensajes.mostrarMensaje("La compañìa debe ser un valor numérico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                If _CompaniaSeleccionado Is Nothing Then
                    _mCompania = String.Empty
                Else
                    _mCompania = _CompaniaSeleccionado.IdItem
                End If
            ElseIf value.ToString.Length() > 7 Then
                A2Utilidades.Mensajes.mostrarMensaje("La longitud máxima de la compañìa es de 7 caracteres", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                If _CompaniaSeleccionado Is Nothing Then
                    _mCompania = String.Empty
                Else
                    _mCompania = _CompaniaSeleccionado.IdItem
                End If
            Else
                _mCompania = value
                '  If _BancoSelected IsNot Nothing AndAlso (_CompaniaSeleccionado IsNot Nothing AndAlso Not value.ToString.Equals(_CompaniaSeleccionado.IdItem)) Then
                If value.ToString IsNot Nothing AndAlso value > 0 Then
                    buscarCompania(value, "buscarCompaniaBanco")
                End If
            End If
            MyBase.CambioItem("CompaniaBanco")
        End Set
    End Property

    Private _CompaniaSeleccionado As OYDUtilidades.BuscadorGenerico
    Public Property CompaniaSeleccionado As OYDUtilidades.BuscadorGenerico
        Get
            Return (_CompaniaSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorGenerico)
            Dim logIgual As Boolean = False

            If Not IsNothing(_CompaniaSeleccionado) AndAlso _CompaniaSeleccionado.Equals(value) Then
                Exit Property
            End If

            _CompaniaSeleccionado = value

            If Not value Is Nothing Then

                If _mlogNuevo Then
                    _BancoSelected.Compania = _CompaniaSeleccionado.IdItem
                End If

                '// Actualizar el campo para que se vea la compañia seleccionado
                _mCompania = _CompaniaSeleccionado.IdItem

            End If
            MyBase.CambioItem("CompaniaSeleccionado")
        End Set
    End Property
#End Region

#Region "Métodos para controlar cambio de campos asociados a buscadores"

    ''' <summary>
    ''' Buscar los datos de la compañia que tiene asignada el consecutivo documento.
    ''' </summary>
    ''' <param name="pintCompania">Compania que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    Friend Sub buscarCompania(Optional ByVal pintCompania As Integer = 0, Optional ByVal pstrBusqueda As String = "")
        Dim intCompania As Integer = 1

        Try
            If Not Me.BancoSelected Is Nothing Then
                If Not Me.CompaniaSeleccionado Is Nothing And pintCompania.Equals(String.Empty) Then
                    intCompania = Me.CompaniaSeleccionado.IdItem
                End If

                If Not intCompania.Equals(Me.BancoSelected.Compania) Then
                    If pintCompania = 0 Then
                        intCompania = Me.BancoSelected.Compania
                    Else
                        intCompania = pintCompania
                    End If

                    If Not intCompania = 0 Then
                        pintIDCompaniaBusqueda = intCompania
                        mdcProxyUtilidad02.BuscadorGenericos.Clear()
                        mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarItemsQuery(intCompania, "compania", "A", "incluircompaniasclasespadresconfirma", "", "", Program.Usuario, Program.HashConexion), AddressOf buscarCompaniaCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la compañia", Me.ToString(), "buscarCompania", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Buscar los datos de la compañia que tiene asignado el consecutivo documento
    ''' Se dispara cuando la busqueda de la compañia iniciada desde el procedimiento buscarCompania finaliza
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub buscarCompaniaCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.Entities.ToList.Where(Function(i) i.IdItem = pintIDCompaniaBusqueda).Count > 0 Then
                    If lo.UserState.ToString = "buscarCompaniaBanco" Then
                        Me.CompaniaSeleccionado = lo.Entities.ToList.Where(Function(i) i.IdItem = pintIDCompaniaBusqueda).First
                        BancoSelected.Compania = CompaniaBanco
                        BancoSelected.NombreCompania = Me.CompaniaSeleccionado.CodigoAuxiliar
                        If UtilizaPasiva = True Then
                            If Me.CompaniaSeleccionado.InfoAdicional04 <> "" Then
                                strTipoCuentaCompanias = Me.CompaniaSeleccionado.InfoAdicional04
                                BancoSelected.TipoCuenta = Me.CompaniaSeleccionado.InfoAdicional04
                            End If
                        End If
                    Else
                        Me.CompaniaSeleccionado = lo.Entities.ToList.Where(Function(i) i.IdItem = pintIDCompaniaBusqueda).First

                        If lo.UserState.ToString = "buscar" Then
                            _mobjCompaniaSeleccionadoAntes = _CompaniaSeleccionado
                        End If
                    End If
                Else
                    If Editando Then
                        Me.CompaniaSeleccionado = Nothing
                        If _mlogNuevo Then
                            A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                        CompaniaBanco = 0
                    End If
                End If
            Else
                If Editando Then
                    Me.CompaniaSeleccionado = Nothing
                    If _mlogNuevo Then
                        A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                    CompaniaBanco = 0
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la compañia del banco", Me.ToString(), "buscarCompaniaCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaBanco
    Implements INotifyPropertyChanged

    Private _ID As Integer
    <Display(Name:="Código")> _
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _Nombre As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    Private _NombreSucursal As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre Sucursal")> _
    Public Property NombreSucursal() As String
        Get
            Return _NombreSucursal
        End Get
        Set(ByVal value As String)
            _NombreSucursal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreSucursal"))
        End Set
    End Property

    Private _CompaniaBanco As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Compañia")> _
    Public Property CompaniaBanco() As String
        Get
            Return _CompaniaBanco
        End Get
        Set(ByVal value As String)
            _CompaniaBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CompaniaBanco"))
        End Set
    End Property

    Private _NombreCompania As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")>
    <Display(Name:="Nombre Compañia")>
    Public Property NombreCompania() As String
        Get
            Return _NombreCompania
        End Get
        Set(ByVal value As String)
            _NombreCompania = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCompania"))
        End Set
    End Property

    Private _NroCuenta As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")>
    <Display(Name:="Nro Cuenta")>
    Public Property NroCuenta() As String
        Get
            Return _NroCuenta
        End Get
        Set(ByVal value As String)
            _NroCuenta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroCuenta"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

End Class


Public Class CiudadesClase
    Implements INotifyPropertyChanged

    Private _Ciudad As String
    <Display(Name:="Ciudad")> _
    Public Property Ciudad As String
        Get
            Return _Ciudad
        End Get
        Set(ByVal value As String)
            _Ciudad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ciudad"))
        End Set
    End Property

    Private _Departamento As String
    <Display(Name:="Departamento")> _
    Public Property Departamento As String
        Get
            Return _Departamento
        End Get
        Set(ByVal value As String)
            _Departamento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Departamento"))
        End Set
    End Property

    Private _Pais As String
    <Display(Name:="Pais")> _
    Public Property Pais As String
        Get
            Return _Pais
        End Get
        Set(ByVal value As String)
            _Pais = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Pais"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class



