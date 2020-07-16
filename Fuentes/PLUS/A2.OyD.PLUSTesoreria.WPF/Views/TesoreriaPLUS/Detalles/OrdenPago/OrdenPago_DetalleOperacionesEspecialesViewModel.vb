

Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSTesoreria
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Threading
Imports A2.Notificaciones.Cliente
Imports A2OYDPLUSUtilidades
Imports OpenRiaServices.DomainServices.Client

Public Class OrdenPago_DetalleOperacionesEspecialesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"
    Private dcProxy As OYDPLUSTesoreriaDomainContext
    Private dcProxyUtilidades As UtilidadesDomainContext
    Private dcProxyUtilidadesPLUS As OYDPLUSUtilidadesDomainContext

    Public logCambiarPropiedades As Boolean = False

    Private TIPOCONCEPTOCONCOBRO As String = String.Empty
    Private logEsFondosOYD As Boolean = False
    Private strTipoGMF_TesoreriaFondosOYD As String = String.Empty
    Private logRegistroExistente As Boolean = False
    Private logPermitirHabilitarConcepto As Boolean = True
    Private logEsFondosUnity As Boolean = False
    Private strMensajeValidacion As String = String.Empty
    Public EsNuevoRegistro As Boolean = False
    Public EsEdicionRegistro As Boolean = False
    Private strConceptoDefecto_Fondos As String = String.Empty
    Public HabilitarValor_Inicio As Boolean = False
    Private ConceptoDefectoFondos_Retiro As Nullable(Of Integer) = Nothing
    Private ConceptoDescripcionDefectoFondos_Retiro As String = String.Empty
    Private ConceptoDefectoFondos_Cancelacion As Nullable(Of Integer) = Nothing
    Private ConceptoDescripcionDefectoFondos_Cancelacion As String = String.Empty

#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            IsBusy = True
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSTesoreriaDomainContext()
                dcProxyUtilidades = New UtilidadesDomainContext()
                dcProxyUtilidadesPLUS = New OYDPLUSUtilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSTesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxyUtilidades = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
                dcProxyUtilidadesPLUS = New OYDPLUSUtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYDPLUS)))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OYDPLUSTesoreriaDomainContext.IOYDPLUSTesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "OrdenPago_DetalleOperacionesEspecialesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _DiccionarioCombosOYDPlus As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
    Public Property DiccionarioCombosOYDPlus() As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
        Get
            Return _DiccionarioCombosOYDPlus
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)))
            _DiccionarioCombosOYDPlus = value
            MyBase.CambioItem("DiccionarioCombosOYDPlus")
        End Set
    End Property

    Private _DiccionarioCombosOYDPlusCompleto As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
    Public Property DiccionarioCombosOYDPlusCompleto() As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
        Get
            Return _DiccionarioCombosOYDPlusCompleto
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)))
            _DiccionarioCombosOYDPlusCompleto = value
            MyBase.CambioItem("DiccionarioCombosOYDPlusCompleto")
        End Set
    End Property

    Private _IDEncabezado As Integer
    Public Property IDEncabezado() As Integer
        Get
            Return _IDEncabezado
        End Get
        Set(ByVal value As Integer)
            _IDEncabezado = value
            MyBase.CambioItem("IDEncabezado")
        End Set
    End Property

    Private _IDDetalle As Integer
    Public Property IDDetalle() As Integer
        Get
            Return _IDDetalle
        End Get
        Set(ByVal value As Integer)
            _IDDetalle = value
            MyBase.CambioItem("IDDetalle")
        End Set
    End Property

    Private _MostrarInformacionFondos As Visibility
    Public Property MostrarInformacionFondos() As Visibility
        Get
            Return _MostrarInformacionFondos
        End Get
        Set(ByVal value As Visibility)
            _MostrarInformacionFondos = value
            MyBase.CambioItem("MostrarInformacionFondos")
        End Set
    End Property

    Private _CarteraColectivaFondos As String
    Public Property CarteraColectivaFondos() As String
        Get
            Return _CarteraColectivaFondos
        End Get
        Set(ByVal value As String)
            _CarteraColectivaFondos = value
            MyBase.CambioItem("CarteraColectivaFondos")
        End Set
    End Property

    Private _NroEncargoFondos As String
    Public Property NroEncargoFondos() As String
        Get
            Return _NroEncargoFondos
        End Get
        Set(ByVal value As String)
            _NroEncargoFondos = value
            MyBase.CambioItem("NroEncargoFondos")
        End Set
    End Property

    Private _TipoRetiroFondos As String
    Public Property TipoRetiroFondos() As String
        Get
            Return _TipoRetiroFondos
        End Get
        Set(ByVal value As String)
            _TipoRetiroFondos = value
            MyBase.CambioItem("TipoRetiroFondos")
        End Set
    End Property

    Private _Receptor As String
    Public Property Receptor() As String
        Get
            Return _Receptor
        End Get
        Set(ByVal value As String)
            _Receptor = value
            MyBase.CambioItem("Receptor")
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

    Private _NroDocumentoComitente As String
    Public Property NroDocumentoComitente() As String
        Get
            Return _NroDocumentoComitente
        End Get
        Set(ByVal value As String)
            _NroDocumentoComitente = value
            MyBase.CambioItem("NroDocumentoComitente")
        End Set
    End Property

    Private _TipoDocumentoComitente As String
    Public Property TipoDocumentoComitente() As String
        Get
            Return _TipoDocumentoComitente
        End Get
        Set(ByVal value As String)
            _TipoDocumentoComitente = value
            MyBase.CambioItem("TipoDocumentoComitente")
        End Set
    End Property

    Private _DescripcionTipoDocumentoComitente As String
    Public Property DescripcionTipoDocumentoComitente() As String
        Get
            Return _DescripcionTipoDocumentoComitente
        End Get
        Set(ByVal value As String)
            _DescripcionTipoDocumentoComitente = value
            MyBase.CambioItem("DescripcionTipoDocumentoComitente")
        End Set
    End Property

    Private _NombreComitente As String
    Public Property NombreComitente() As String
        Get
            Return _NombreComitente
        End Get
        Set(ByVal value As String)
            _NombreComitente = value
            MyBase.CambioItem("NombreComitente")
        End Set
    End Property

    Private _TipoProducto As String
    Public Property TipoProducto() As String
        Get
            Return _TipoProducto
        End Get
        Set(ByVal value As String)
            _TipoProducto = value
            MyBase.CambioItem("TipoProducto")
        End Set
    End Property

    Private _ValorNetoOrden As Double
    Public Property ValorNetoOrden() As Double
        Get
            Return _ValorNetoOrden
        End Get
        Set(ByVal value As Double)
            _ValorNetoOrden = value
            MyBase.CambioItem("ValorNetoOrden")
        End Set
    End Property

    Private _TieneOrdenEnCero As Boolean
    Public Property TieneOrdenEnCero() As Boolean
        Get
            Return _TieneOrdenEnCero
        End Get
        Set(ByVal value As Boolean)
            _TieneOrdenEnCero = value
            MyBase.CambioItem("TieneOrdenEnCero")
        End Set
    End Property

    Private _ValorOriginalDetalle As Double
    Public Property ValorOriginalDetalle() As Double
        Get
            Return _ValorOriginalDetalle
        End Get
        Set(ByVal value As Double)
            _ValorOriginalDetalle = value
            MyBase.CambioItem("ValorOriginalDetalle")
        End Set
    End Property

    Private _ValorEdicionDetalle As Double
    Public Property ValorEdicionDetalle() As Double
        Get
            Return _ValorEdicionDetalle
        End Get
        Set(ByVal value As Double)
            _ValorEdicionDetalle = value
            MyBase.CambioItem("ValorEdicionDetalle")
        End Set
    End Property

    Private _HabilitarTipo As Boolean
    Public Property HabilitarTipo() As Boolean
        Get
            Return _HabilitarTipo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipo = value
            MyBase.CambioItem("HabilitarTipo")
        End Set
    End Property

    Private _Tipo As String
    Public Property Tipo() As String
        Get
            Return _Tipo
        End Get
        Set(ByVal value As String)
            _Tipo = value
            If logCambiarPropiedades Then
                If _Tipo <> GSTR_TIPOOPEESPECIAL_OPERACIONESPECIAL Then
                    OperacionEspecial = String.Empty
                End If

                HabilitarControles()
            End If
            MyBase.CambioItem("Tipo")
        End Set
    End Property

    Private _DescripcionTipo As String
    Public Property DescripcionTipo() As String
        Get
            Return _DescripcionTipo
        End Get
        Set(ByVal value As String)
            _DescripcionTipo = value
            MyBase.CambioItem("DescripcionTipo")
        End Set
    End Property

    Private _HabilitarOperacionEspecial As Boolean
    Public Property HabilitarOperacionEspecial() As Boolean
        Get
            Return _HabilitarOperacionEspecial
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOperacionEspecial = value
            MyBase.CambioItem("HabilitarOperacionEspecial")
        End Set
    End Property

    Private _OperacionEspecial As String
    Public Property OperacionEspecial() As String
        Get
            Return _OperacionEspecial
        End Get
        Set(ByVal value As String)
            _OperacionEspecial = value
            MyBase.CambioItem("OperacionEspecial")
        End Set
    End Property

    Private _DescripcionOperacionEspecial As String
    Public Property DescripcionOperacionEspecial() As String
        Get
            Return _DescripcionOperacionEspecial
        End Get
        Set(ByVal value As String)
            _DescripcionOperacionEspecial = value
            MyBase.CambioItem("DescripcionOperacionEspecial")
        End Set
    End Property

    Private _CodigoOYDComprador As String
    Public Property CodigoOYDComprador() As String
        Get
            Return _CodigoOYDComprador
        End Get
        Set(ByVal value As String)
            _CodigoOYDComprador = value
            MyBase.CambioItem("CodigoOYDComprador")
        End Set
    End Property

    Private _NombreComprador As String
    Public Property NombreComprador() As String
        Get
            Return _NombreComprador
        End Get
        Set(ByVal value As String)
            _NombreComprador = value
            MyBase.CambioItem("NombreComprador")
        End Set
    End Property

    Private _TipoDocumentoComprador As String
    Public Property TipoDocumentoComprador() As String
        Get
            Return _TipoDocumentoComprador
        End Get
        Set(ByVal value As String)
            _TipoDocumentoComprador = value
            MyBase.CambioItem("TipoDocumentoComprador")
        End Set
    End Property

    Private _DescripcionTipoDocumentoComprador As String
    Public Property DescripcionTipoDocumentoComprador() As String
        Get
            Return _DescripcionTipoDocumentoComprador
        End Get
        Set(ByVal value As String)
            _DescripcionTipoDocumentoComprador = value
            MyBase.CambioItem("DescripcionTipoDocumentoComprador")
        End Set
    End Property

    Private _NroDocumentoComprador As String
    Public Property NroDocumentoComprador() As String
        Get
            Return _NroDocumentoComprador
        End Get
        Set(ByVal value As String)
            _NroDocumentoComprador = value
            MyBase.CambioItem("NroDocumentoComprador")
        End Set
    End Property

    Private _CodigoOYDVendedor As String
    Public Property CodigoOYDVendedor() As String
        Get
            Return _CodigoOYDVendedor
        End Get
        Set(ByVal value As String)
            _CodigoOYDVendedor = value
            MyBase.CambioItem("CodigoOYDVendedor")
        End Set
    End Property

    Private _NombreVendedor As String
    Public Property NombreVendedor() As String
        Get
            Return _NombreVendedor
        End Get
        Set(ByVal value As String)
            _NombreVendedor = value
            MyBase.CambioItem("NombreVendedor")
        End Set
    End Property

    Private _TipoDocumentoVendedor As String
    Public Property TipoDocumentoVendedor() As String
        Get
            Return _TipoDocumentoVendedor
        End Get
        Set(ByVal value As String)
            _TipoDocumentoVendedor = value
            MyBase.CambioItem("TipoDocumentoVendedor")
        End Set
    End Property

    Private _DescripcionTipoDocumentoVendedor As String
    Public Property DescripcionTipoDocumentoVendedor() As String
        Get
            Return _DescripcionTipoDocumentoVendedor
        End Get
        Set(ByVal value As String)
            _DescripcionTipoDocumentoVendedor = value
            MyBase.CambioItem("DescripcionTipoDocumentoVendedor")
        End Set
    End Property

    Private _NroDocumentoVendedor As String
    Public Property NroDocumentoVendedor() As String
        Get
            Return _NroDocumentoVendedor
        End Get
        Set(ByVal value As String)
            _NroDocumentoVendedor = value
            MyBase.CambioItem("NroDocumentoVendedor")
        End Set
    End Property

    Private _ProvieneDinero As String
    Public Property ProvieneDinero() As String
        Get
            Return _ProvieneDinero
        End Get
        Set(ByVal value As String)
            _ProvieneDinero = value
            MyBase.CambioItem("ProvieneDinero")
        End Set
    End Property

    Private _Observaciones As String
    Public Property Observaciones() As String
        Get
            Return _Observaciones
        End Get
        Set(ByVal value As String)
            _Observaciones = value
            MyBase.CambioItem("Observaciones")
        End Set
    End Property

    Private _HabilitarValor As Boolean
    Public Property HabilitarValor() As Boolean
        Get
            Return _HabilitarValor
        End Get
        Set(ByVal value As Boolean)
            _HabilitarValor = value
            MyBase.CambioItem("HabilitarValor")
        End Set
    End Property

    Private _ValorGenerar As Double
    Public Property ValorGenerar() As Double
        Get
            Return _ValorGenerar
        End Get
        Set(ByVal value As Double)
            _ValorGenerar = value
            MyBase.CambioItem("ValorGenerar")
        End Set
    End Property

    Private _HabilitarGuardarYSalir As Boolean = True
    Public Property HabilitarGuardarYSalir() As Boolean
        Get
            Return _HabilitarGuardarYSalir
        End Get
        Set(ByVal value As Boolean)
            _HabilitarGuardarYSalir = value
            MyBase.CambioItem("HabilitarGuardarYSalir")
        End Set
    End Property

    Private _HabilitarGuardarContinuar As Boolean = True
    Public Property HabilitarGuardarContinuar() As Boolean
        Get
            Return _HabilitarGuardarContinuar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarGuardarContinuar = value
            MyBase.CambioItem("HabilitarGuardarContinuar")
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Sub PrepararNuevoRegistro()
        Try
            Tipo = Nothing
            OperacionEspecial = Nothing
            CodigoOYDComprador = Nothing
            NombreComprador = Nothing
            TipoDocumentoComprador = Nothing
            NroDocumentoComprador = Nothing
            CodigoOYDVendedor = Nothing
            NombreVendedor = Nothing
            TipoDocumentoVendedor = Nothing
            NroDocumentoVendedor = Nothing
            ProvieneDinero = Nothing
            Observaciones = Nothing
            ValorGenerar = 0
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar el nuevo registro.",
                                 Me.ToString(), "PrepararNuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub CargarCombosOYDPLUS(ByVal pstrUserState As String)
        Try
            IsBusy = True

            If Not IsNothing(dcProxyUtilidadesPLUS.CombosReceptors) Then
                dcProxyUtilidadesPLUS.CombosReceptors.Clear()
            End If

            dcProxyUtilidadesPLUS.Load(dcProxyUtilidadesPLUS.OYDPLUS_ConsultarCombosReceptorQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosOYDCompleta, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos de la pantalla.",
                                 Me.ToString(), "CargarCombosOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Función que retorna el valor de un texto que este contenido dentro de los dos caracteres enviados como parametros.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 12 de marzo del 2013
    ''' </summary>
    ''' <param name="pstrDetalle"></param>
    ''' <param name="pstrCaracterInicial"></param>
    ''' <param name="pstrCaracterFinal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RetornarValorDetalle(ByVal pstrDetalle As String, ByVal pstrCaracterInicial As String, ByVal pstrCaracterFinal As String) As String
        Try
            Dim strResultado As String = String.Empty
            Dim strExpresionBusqueda As String = String.Format("\{0}\S*\s*\S*\{1}", pstrCaracterInicial, pstrCaracterFinal)
            Dim regexp As New Regex(strExpresionBusqueda)

            pstrDetalle = pstrDetalle.Replace(" ", "*_*")

            Dim m = regexp.Match(pstrDetalle)

            strResultado = m.Groups(0).Value
            strResultado = strResultado.Replace(pstrCaracterInicial, String.Empty)
            strResultado = strResultado.Replace(pstrCaracterFinal, String.Empty)
            strResultado = strResultado.Replace("*_*", " ")

            Return strResultado
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al retornar el valor del detalle.",
                                Me.ToString(), "RetornarValorDetalle", Application.Current.ToString(), Program.Maquina, ex)
            Return String.Empty
        End Try
    End Function


    Private Sub HabilitarControles()
        Try
            If Tipo = GSTR_TIPOOPEESPECIAL_OPERACIONESPECIAL Then
                HabilitarOperacionEspecial = True
            Else
                HabilitarOperacionEspecial = False
            End If

            HabilitarValor = True

            If TipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
                MostrarInformacionFondos = Visibility.Visible
            Else
                MostrarInformacionFondos = Visibility.Collapsed
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los controles.",
                                 Me.ToString(), "VerificarHabilitarControles", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Function ValidarRegistro() As Boolean
        Try
            Dim logTieneError As Boolean
            strMensajeValidacion = String.Empty

            If String.IsNullOrEmpty(_Tipo) Then
                strMensajeValidacion = String.Format("{0}{1} - Tipo operación: elegir un tipo de operación.", strMensajeValidacion, vbCrLf)
                logTieneError = True
            ElseIf _Tipo = GSTR_TIPOOPEESPECIAL_OPERACIONESPECIAL Then
                If String.IsNullOrEmpty(OperacionEspecial) Then
                    strMensajeValidacion = String.Format("{0}{1} - Operación especial: elegir una operación especial", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
            End If


            If String.IsNullOrEmpty(CodigoOYDComprador) Then
                strMensajeValidacion = String.Format("{0}{1} - Código OYD comprador: elegir el código del comprador", strMensajeValidacion, vbCrLf)
                logTieneError = True
            End If

            If String.IsNullOrEmpty(CodigoOYDVendedor) Then
                strMensajeValidacion = String.Format("{0}{1} - Código OYD vendedor: elegir el código del vendedor", strMensajeValidacion, vbCrLf)
                logTieneError = True
            End If

            If String.IsNullOrEmpty(ProvieneDinero) Then
                strMensajeValidacion = String.Format("{0}{1} - Proviene dinero: Ingresar de donde proviene el dinero", strMensajeValidacion, vbCrLf)
                logTieneError = True
            Else
                Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(ProvieneDinero, clsExpresiones.TipoExpresion.Caracteres)
                If Not IsNothing(objValidacionExpresion) Then
                    If objValidacionExpresion.TextoValido = False Then
                        strMensajeValidacion = String.Format("{0}{1} - Proviene dinero: Hay caracteres invalidos", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
            End If

            If String.IsNullOrEmpty(Observaciones) Then
                strMensajeValidacion = String.Format("{0}{1} - Observación: Ingresar la observación", strMensajeValidacion, vbCrLf)
                logTieneError = True
            Else
                Dim objValidacionExpresion = clsExpresiones.ValidarCaracteresEnCadena(Observaciones, clsExpresiones.TipoExpresion.Caracteres)
                If Not IsNothing(objValidacionExpresion) Then
                    If objValidacionExpresion.TextoValido = False Then
                        strMensajeValidacion = String.Format("{0}{1} - Observación: Hay caracteres invalidos", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
            End If

            If TipoProducto = GSTR_FONDOS_TIPOPRODUCTO And TipoRetiroFondos = GSTR_FONDOS_TIPOACCION_CANCELACION Then
                Dim logValidarValorEnCero As Boolean = True

                If logEsFondosOYD Then
                    If ValorGenerar > 0 Then
                        strMensajeValidacion = String.Format("{0}{1} - Cuando la orden es de Fondos y es una cancelación no puede existir detalles con valor.", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                        logValidarValorEnCero = False
                    End If
                End If

                If logValidarValorEnCero Then
                    If TieneOrdenEnCero Then
                        If (IsNothing(ValorGenerar) Or ValorGenerar <= 0) Then
                            strMensajeValidacion = String.Format("{0}{1} - Cuando la orden es de Fondos y es una cancelación solo puede existir 1 detalle con el valor en cero.", strMensajeValidacion, vbCrLf)
                            logTieneError = True
                        End If
                    End If
                End If
            ElseIf HabilitarValor Then
                If ValorGenerar <= 0 Then
                    strMensajeValidacion = String.Format("{0}{1} - Valor: Ingresar un valor mayor que cero", strMensajeValidacion, vbCrLf)
                    logTieneError = True
                End If
            End If

            If logTieneError Then
                mostrarMensaje("Para guardar el Registro es necesario completar los siguientes datos con sus valores correspondientes:" & vbCrLf & strMensajeValidacion, "Ordenes de Pago - Bloqueos.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            Else
                strMensajeValidacion = String.Empty
                Return True
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Validar Campos de Cheque",
                                 Me.ToString(), "ValidarCamposDiligenciadosCheque", Application.Current.ToString(), Program.Maquina, ex)

            IsBusy = False
            Return False
        End Try
    End Function

    Public Function GuardarRegistro() As Boolean
        Try
            If Not IsNothing(DiccionarioCombosOYDPlus) Then
                DescripcionTipo = ObtenerDescripcionEnDiccionario("TIPOOPEESPECIAL", _Tipo)
                DescripcionOperacionEspecial = ObtenerDescripcionEnDiccionario("OPERACIONESPECIAL", _OperacionEspecial)
                DescripcionTipoDocumentoComprador = ObtenerDescripcionEnDiccionario("TIPOID", _TipoDocumentoComprador)
                DescripcionTipoDocumentoVendedor = ObtenerDescripcionEnDiccionario("TIPOID", _TipoDocumentoVendedor)
            End If

            If ValidarRegistro() Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar guardar el registro.",
                                 Me.ToString(), "GuardarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    Public Sub CancelarEdicionRegistro()
        Try
            If EsNuevoRegistro = False Then
                If IDDetalle > 0 Then
                    dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(IDDetalle, GSTR_OTD, Program.Usuario, Program.HashConexion, AddressOf TerminoCancelarEditarRegistro, String.Empty)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar cancelar la edición del registro.",
                                 Me.ToString(), "CancelarEdicionRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ObtenerDescripcionEnDiccionario(ByVal pstrTopico As String, ByVal pstrIDItem As String) As String
        Dim strRetorno As String = String.Empty
        If DiccionarioCombosOYDPlus.ContainsKey(pstrTopico) Then
            If DiccionarioCombosOYDPlus(pstrTopico).Where(Function(i) i.Retorno = pstrIDItem).Count > 0 Then
                strRetorno = DiccionarioCombosOYDPlus(pstrTopico).Where(Function(i) i.Retorno = pstrIDItem).First.Descripcion
            End If
        End If
        Return strRetorno
    End Function

#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoConsultarCombosOYDCompleta(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDPLUSUtilidades.CombosReceptor) = Nothing
                    Dim objDiccionarioCompleto As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionarioCompleto.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    DiccionarioCombosOYDPlusCompleto = Nothing
                    DiccionarioCombosOYDPlusCompleto = objDiccionarioCompleto

                    If DiccionarioCombosOYDPlusCompleto.ContainsKey("OYDPLUS_CONCEPTOSCONFIGURADOSCOBROGMF") Then
                        If DiccionarioCombosOYDPlusCompleto("OYDPLUS_CONCEPTOSCONFIGURADOSCOBROGMF").Count > 0 Then
                            TIPOCONCEPTOCONCOBRO = DiccionarioCombosOYDPlusCompleto("OYDPLUS_CONCEPTOSCONFIGURADOSCOBROGMF").First.Retorno
                        End If
                    End If

                    If DiccionarioCombosOYDPlusCompleto.ContainsKey("CF_UTILIZAPASIVA_A2") Then
                        If DiccionarioCombosOYDPlusCompleto("CF_UTILIZAPASIVA_A2").Count > 0 Then
                            If DiccionarioCombosOYDPlusCompleto("CF_UTILIZAPASIVA_A2").First.Retorno = "SI" Then
                                logEsFondosOYD = True
                            Else
                                logEsFondosOYD = False
                            End If
                        End If
                    End If

                    If DiccionarioCombosOYDPlusCompleto.ContainsKey("TIPOGMF_FONDOSOYD") Then
                        If DiccionarioCombosOYDPlusCompleto("TIPOGMF_FONDOSOYD").Count > 0 Then
                            strTipoGMF_TesoreriaFondosOYD = DiccionarioCombosOYDPlusCompleto("TIPOGMF_FONDOSOYD").First.Retorno
                        End If
                    End If

                    If DiccionarioCombosOYDPlusCompleto.ContainsKey("A2_UTILIZAUNITY") Then
                        If DiccionarioCombosOYDPlusCompleto("A2_UTILIZAUNITY").Count > 0 Then
                            If DiccionarioCombosOYDPlusCompleto("A2_UTILIZAUNITY").First.Retorno = "SI" Then
                                logEsFondosUnity = True
                            Else
                                logEsFondosUnity = False
                            End If
                        End If
                    End If

                    If DiccionarioCombosOYDPlusCompleto.ContainsKey("CONCEPTODEFECTO_ORDENGIRO_FONDOS") Then
                        If DiccionarioCombosOYDPlusCompleto("CONCEPTODEFECTO_ORDENGIRO_FONDOS").Count > 0 Then
                            strConceptoDefecto_Fondos = DiccionarioCombosOYDPlusCompleto("CONCEPTODEFECTO_ORDENGIRO_FONDOS").First.Retorno
                        End If
                    End If

                    If Not IsNothing(dcProxyUtilidadesPLUS.CombosReceptors) Then
                        dcProxyUtilidadesPLUS.CombosReceptors.Clear()
                    End If

                    dcProxyUtilidadesPLUS.Load(dcProxyUtilidadesPLUS.OYDPLUS_ConsultarCombosReceptorQuery(Receptor, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosOYDReceptor, lo.UserState)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarCombosOYDReceptor(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDPLUSUtilidades.CombosReceptor) = Nothing
                    Dim objDiccionarioCompleto As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionarioCompleto.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    DiccionarioCombosOYDPlus = Nothing
                    DiccionarioCombosOYDPlus = objDiccionarioCompleto

                    If Not IsNothing(dcProxyUtilidades.ItemCombos) Then
                        dcProxyUtilidades.ItemCombos.Clear()
                    End If

                    dcProxyUtilidades.Load(dcProxyUtilidades.cargarCombosCondicionalQuery("COMBOS_DEPENDIENTES_CARTERA", CarteraColectivaFondos, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCombosEspecificosCartera, lo.UserState)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoTraerCombosEspecificosCartera(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_RETIRO").Count > 0 Then
                    ConceptoDefectoFondos_Retiro = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_RETIRO").First.intID
                    ConceptoDescripcionDefectoFondos_Retiro = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_RETIRO").First.Descripcion
                Else
                    ConceptoDefectoFondos_Retiro = Nothing
                    ConceptoDescripcionDefectoFondos_Retiro = Nothing
                End If

                If lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_CANCELACION").Count > 0 Then
                    ConceptoDefectoFondos_Cancelacion = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_CANCELACION").First.intID
                    ConceptoDescripcionDefectoFondos_Cancelacion = lo.Entities.ToList.Where(Function(i) i.Categoria = "CONCEPTODEFECTO_CANCELACION").First.Descripcion
                Else
                    ConceptoDefectoFondos_Cancelacion = Nothing
                    ConceptoDescripcionDefectoFondos_Cancelacion = Nothing
                End If

                HabilitarControles()
                If EsNuevoRegistro Then
                    PrepararNuevoRegistro()
                End If

                IsBusy = False
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los combos de la cartera",
                                                 Me.ToString(), "TerminoTraerCombosEspecificosCartera", Program.TituloSistema, Program.Maquina, lo.Error)
                'lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los combos de la cartera",
                                                 Me.ToString(), "TerminoTraerCombosEspecificosCartera", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub TerminoCancelarEditarRegistro(ByVal lo As InvokeOperation(Of Integer))
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cancelar la edición del registro.", Me.ToString(), "TerminoCancelarEditarRegistro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class
