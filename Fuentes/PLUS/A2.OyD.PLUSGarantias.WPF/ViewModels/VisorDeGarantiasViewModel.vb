Imports System.ComponentModel

Imports A2.OyD.OYDServer.RIA.Web
Imports C1.WPF.DataGrid
Imports System.Windows.Data
Imports OpenRiaServices.DomainServices.Client

Public Class VisorDeGarantiasViewModel
    Implements INotifyPropertyChanged

#Region "Variables"

    ''' <summary>
    ''' Acceso a datos
    ''' </summary>
    ''' <remarks></remarks>
    Private _dcProxy As OyDPLUSGarantiasDomainContext
    Private _dcProxy2 As OyDPLUSGarantiasDomainContext

    ''' <summary>
    ''' Color verde mpara semaforo OK
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ColorOk As Color = Color.FromArgb(255, 3, 95, 7)
    ''' <summary>
    ''' Color rojo para semaforo 
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ColorError As Color = Color.FromArgb(255, 185, 0, 0)
    ''' <summary>
    ''' Color amarillo para la alarma
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ColorAlarma As Color = Color.FromArgb(255, 250, 250, 0)

    Private logValidarSaldoBloqueo As Boolean = True
    Private logDetalle As Boolean = False
    Private logConsultaPorCliente As Boolean = False
    Private logConsultaDesdeBloqueo As Boolean = False

    'Variables para manejo del dato anterior, devolver el estado de la pantalla al usuario cancelar una acción
    Private lstSaldosBloqueadosAnterior As List(Of SaldosBloqueados) = New List(Of SaldosBloqueados)
    Private lstTitulosAnterior As List(Of TitulosGarantia) = New List(Of TitulosGarantia)
    Private objRegistroSeleccionadoAnterior As VisorDeGarantias = New VisorDeGarantias

    'Variables para manojo de los datos a bloquear
    Private lstSaldosaDesbloquear As List(Of SaldosBloqueados) = New List(Of SaldosBloqueados)
    Private lstTitulosBloquearDesbloquear As List(Of TitulosGarantia) = New List(Of TitulosGarantia)
    Private logCargaDesdeBloqueo As Boolean = False

    'Load operations para saber si las consultas estan en ejecución
    Dim loTitulosgarantia As LoadOperation(Of TitulosGarantia)
    Dim loSaldosBloqueados As LoadOperation(Of SaldosBloqueados)

    Private IDLiquidacion As Integer 'SV20180411
    Private Parcial As Integer 'SV20180411
    Private Tipo As String 'SV20180411
    Private ClaseOrden As String 'SV20180411
    Private FechaLiquidacion As Date? 'SV20180411

#End Region

#Region "Constantes"
    ''' <summary>
    ''' Formato para mostrar el valor positivo
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_FORMATO_POSITIVO As String = "+ {0:N2}"
    ''' <summary>
    ''' Formato para mostrar el valor negativo
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_FORMATO_NEGATIVO As String = "- {0:N2}"
    ''' <summary>
    ''' Formato para mostrar el valor
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_FORMATO_VALOR As String = "{0:N2}"
    ''' <summary>
    ''' Nombre de la columna prioridadgrupo
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_COL_PRIORIDAD_GRUPO As String = "PrioridadGrupo"
    ''' <summary>
    ''' Nombre de la columna prioridad
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_COL_PRIORIDAD As String = "Prioridad"
    ''' <summary>
    ''' Header de la columna cantidad a bloquear
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_TXTCOL_CANTIDAD_BLOQUEO As String = "Cant. bloq."
    ''' <summary>
    ''' Header de la columna cantidad a desbloquear
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_TXTCOL_CANTIDAD_DESBLOQUEO As String = "Cant. desbloq."
    ''' <summary>
    ''' Texto Bloquear
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_TEXTO_BLOQUEAR As String = "bloquear"
    ''' <summary>
    ''' Texto desbloquear
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_TEXTO_DESBLOQUEAR As String = "debloquear"
    ''' <summary>
    ''' Mensaje para mostrar cuando falta registrar la cantidad a bloquear/desbloquear
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_MENSAJE_CANTIDAD_TITULOS As String = "Debe registrar la cantidad a {0} para todos los títulos."
    ''' <summary>
    ''' Mensaje cuando la cantidad a bloquear/desbloquear es mayor que la cantidad del título
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_MENSAJE_CANTIDAD_BLOQUEAR_MAYOR As String = "La 'Cantidad a {0}' debe ser menor o igual a la cantidad del título"
    ''' <summary>
    ''' Mensaje para mostrar cuando el usuario no ha seleccionado nada para bloquear
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_MENSAJE_DEBE_SELECCIONAR As String = "Debe registrar saldo y/o títulos a {0}."
    ''' <summary>
    ''' Mensaje para cuando el valor admisible del titulo es vacio o menor o igual a cero
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_MENSAJE_NO_TIENE_VALOR_ADMISIBLE As String = "Algunos títulos seleccionados no presentan valor disponible"
    ''' <summary>
    ''' Mensaje para mostrar cuando el valor a bloquear sea mayor que el saldo del cliente
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_MENSAJE_SALDO_BLOQUEAR_MAYOR As String = "El valor a bloquear debe ser menor o igual al saldo disponible del cliente"
    ''' <summary>
    ''' Descripcion de la operacion para bloquear saldo en base de datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_OPERACION_BLOQUEAR_SALDO As String = "DB"
    ''' <summary>
    ''' Descripcion de la operacion para liberar el saldo en base de datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_OPERACION_DESBLOQUEAR_SALDO As String = "CR"
    ''' <summary>
    ''' Descripcion de la operacion para bloquear titulos en base de datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_OPERACION_BLOQUEAR_TITULOS As String = "B"
    ''' <summary>
    ''' Descripcion de la operacion para liberar los titulos en base de datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_OPERACION_DESBLOQUEAR_TITULOS As String = "L"
    ''' <summary>
    ''' Formato para la asignacion de la prioridad del grupo
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_FORMATO_PRIORIDAD_GRUPO As String = "{0}{1}"
    ''' <summary>
    ''' Longitud del campo prioridad, es decir, cuandos digitos va a tener el campo prioridad para la cantidad. (PadLeft = 2)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const INT_LONGITUD_PRIORIDAD As Integer = 2
    ''' <summary>
    ''' Cero para el PADLEFT de la longitud
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STR_CERO As String = "0"

#End Region

#Region "Constructores"
    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                _dcProxy = New OyDPLUSGarantiasDomainContext
                _dcProxy2 = New OyDPLUSGarantiasDomainContext
            Else
                _dcProxy = New OyDPLUSGarantiasDomainContext(New System.Uri(Program.RutaServicioNegocio))
                _dcProxy2 = New OyDPLUSGarantiasDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If

            DirectCast(_dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OyDPLUSGarantiasDomainContext.IOyDPLUSGarantiasDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)
            DirectCast(_dcProxy2.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OyDPLUSGarantiasDomainContext.IOyDPLUSGarantiasDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)

            ' inicialmente se consultan los combos de la pantalla
            IsBusy = True
            _dcProxy.Load(_dcProxy.Garantias_CargarCombosQuery("GARANTIAS", Nothing, Nothing, Program.Usuario, Program.HashConexion), LoadBehavior.MergeIntoCurrent, AddressOf TerminoTraerCombos, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar.", Me.ToString(),
                                                        "VisorDeGarantiasViewModel", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region

#Region "Propiedades"
    ''' <summary>
    ''' Listado de datos recuperados inicialmente
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaDatos As IEnumerable(Of VisorDeGarantias)
    Public Property ListaDatos As IEnumerable(Of VisorDeGarantias)
        Get
            Return _ListaDatos
        End Get
        Set(value As IEnumerable(Of VisorDeGarantias))
            _ListaDatos = value
            RaisePropertyChanged("ListaDatos")
        End Set
    End Property

    ''' <summary>
    ''' Listado de datos que se muestran en pantalla, 
    ''' Se utiliza por el caso de filtro rapido y ordenamiento
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaVisorDeGarantias As PagedCollectionView
    Public Property ListaVisorDeGarantias As PagedCollectionView
        Get
            Return _ListaVisorDeGarantias
        End Get
        Set(value As PagedCollectionView)
            _ListaVisorDeGarantias = value

            'Asignar el ordenamiento por PrioridadGrupo y Prioridad
            If Not _ListaVisorDeGarantias Is Nothing Then
                _ListaVisorDeGarantias.SortDescriptions.Clear()
                _ListaVisorDeGarantias.SortDescriptions.Add(New ComponentModel.SortDescription(STR_COL_PRIORIDAD_GRUPO, ListSortDirection.Descending))
                _ListaVisorDeGarantias.SortDescriptions.Add(New ComponentModel.SortDescription(STR_COL_PRIORIDAD, ListSortDirection.Descending))
            End If

            RaisePropertyChanged("ListaVisorDeGarantias")
        End Set
    End Property

    ''' <summary>
    ''' Listado de filtros para el auto-completar del filtro en pantalla
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaFiltros As New List(Of String)
    Public Property ListaFiltros As List(Of String)
        Get
            Return _ListaFiltros
        End Get
        Set(value As List(Of String))
            _ListaFiltros = value
            RaisePropertyChanged("ListaFiltros")
        End Set
    End Property

    ''' <summary>
    ''' Filtro registrado en pantalla
    ''' </summary>
    ''' <remarks></remarks>
    Private _Filtro As String
    Public Property Filtro As String
        Get
            Return _Filtro
        End Get
        Set(value As String)
            _Filtro = value
            RaisePropertyChanged("Filtro")
            RaisePropertyChanged("ColorFiltro")
            CambioFiltroSeleccionado()
        End Set
    End Property

    ''' <summary>
    ''' Color del boton que indica el filtro en pantalla
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ColorFiltro As SolidColorBrush
        Get
            If Not String.IsNullOrWhiteSpace(Filtro) Then
                Return New SolidColorBrush(Colors.Orange)
            End If
            Return New SolidColorBrush(Colors.Black)
        End Get
    End Property

    ''' <summary>
    ''' Indica el registro seleccionado en pantalla
    ''' </summary>
    ''' <remarks></remarks>
    Private WithEvents _RegistroSeleccionado As VisorDeGarantias
    Public Property RegistroSeleccionado As VisorDeGarantias
        Get
            Return _RegistroSeleccionado
        End Get
        Set(value As VisorDeGarantias)

            _RegistroSeleccionado = value

            If logDetalle Then
                If Not IsNothing(_RegistroSeleccionado) AndAlso Not IsNothing(_RegistroSeleccionado.ID) Then
                    strCodigoClienteBloqueo = _RegistroSeleccionado.IDCliente
                    strNombreClienteBloqueo = _RegistroSeleccionado.NombreCliente
                    logCargaDesdeBloqueo = False
                    CambiarBloqueos(False)
                    _RegistroSeleccionado.PuedeDesBloquear = True
                    _RegistroSeleccionado.PuedeBloquear = True
                    logConsultaPorCliente = False
                    CargarDetalles()

                End If
            End If

            RaisePropertyChanged("RegistroSeleccionado")
            RaisePropertyChanged("LiquidacionesSeleccionadas")
            RaisePropertyTitulos()

        End Set
    End Property

    Private _RegistroSeleccionadoCliente As VisorDeGarantias
    Public Property RegistroSeleccionadoCliente() As VisorDeGarantias
        Get
            Return _RegistroSeleccionadoCliente
        End Get
        Set(ByVal value As VisorDeGarantias)
            If Bloqueando = False And DesBloqueando = False Then
                _RegistroSeleccionadoCliente = value
                If Not IsNothing(_RegistroSeleccionadoCliente) Then
                    If Not _RegistroSeleccionadoCliente.Equals(_RegistroSeleccionado) Then
                        RegistroSeleccionado = _RegistroSeleccionadoCliente
                        CambiarBloqueos(False)
                        _RegistroSeleccionadoCliente.PuedeDesBloquear = True
                        _RegistroSeleccionadoCliente.PuedeBloquear = True
                    End If
                End If
            End If
            RaisePropertyChanged("RegistroSeleccionadoCliente")
        End Set
    End Property

    ''' <summary>
    ''' Indica la liquidacion que se va a bloquear en pantalla
    ''' </summary>
    ''' <remarks></remarks>
    Private _LiquidacionABloquear As VisorDeGarantias
    Public Property LiquidacionABloquear As VisorDeGarantias
        Get
            Return _LiquidacionABloquear
        End Get
        Set(value As VisorDeGarantias)
            _LiquidacionABloquear = value
            RaisePropertyChanged("LiquidacionABloquear")
        End Set
    End Property

    ''' <summary>
    ''' Lista de titulos del cliente seleccionado en pantalla
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property VisorDeGarantiasTitulos As List(Of TitulosGarantia)
        Get
            If lstTitulosGarantia IsNot Nothing Then
                'Ordenar los títulos de acuerdo a si se esta bloqueando o si se esta desbloqueando
                If Bloqueando Then
                    Return lstTitulosGarantia.OrderBy(Function(r) r.Bloqueado).ToList
                ElseIf DesBloqueando Then
                    Return lstTitulosGarantia.OrderByDescending(Function(r) r.Bloqueado).ToList
                End If

                Return lstTitulosGarantia.ToList
            End If
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Listado de las liquidaciones del cliente seleccionado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property LiquidacionesSeleccionadas As List(Of VisorDeGarantias)
        Get
            Dim lista As List(Of VisorDeGarantias) = Nothing
            If Not RegistroSeleccionado Is Nothing Then
                lista = ListaDatos.Where(Function(r) r.IDCliente = RegistroSeleccionado.IDCliente).ToList
            End If

            Return lista
        End Get
    End Property

    ''' <summary>
    ''' Indica si el usuario en pantalla se encuentra en proceso de bloqueo
    ''' </summary>
    ''' <remarks></remarks>
    Private _Bloqueando As Boolean
    Public Property Bloqueando As Boolean
        Get
            Return _Bloqueando
        End Get
        Private Set(value As Boolean)
            _Bloqueando = value
            RaisePropertyChanged("Bloqueando")
            RaisePropertyChanged("Editando")
            RaisePropertyTitulos()
        End Set
    End Property

    ''' <summary>
    ''' Indica si el usuario en pantalla se encuentra en proceso de desbloqueo
    ''' </summary>
    ''' <remarks></remarks>
    Private _DesBloqueando As Boolean
    Public Property DesBloqueando As Boolean
        Get
            Return _DesBloqueando
        End Get
        Private Set(value As Boolean)
            _DesBloqueando = value
            RaisePropertyChanged("DesBloqueando")
            RaisePropertyChanged("Editando")
            RaisePropertyTitulos()
        End Set
    End Property

    ''' <summary>
    ''' Indica si la pantlla se encuentra en estado de consulta
    ''' </summary>
    ''' <remarks></remarks>
    Private _Navegando As Boolean = True
    Public Property Navegando As Boolean
        Get
            Return _Navegando
        End Get
        Private Set(value As Boolean)
            _Navegando = value
            RaisePropertyChanged("Navegando")
        End Set
    End Property

    ''' <summary>
    ''' Descripcion, observaciones, comentarios o texto que se registra cuando se realiza un bloqueo/desbloqueo
    ''' </summary>
    ''' <remarks></remarks>
    Private _NotasBloqueo As String
    Public Property NotasBloqueo As String
        Get
            Return _NotasBloqueo
        End Get
        Set(value As String)
            _NotasBloqueo = value
            RaisePropertyChanged("NotasBloqueo")
        End Set
    End Property

    Private _logSAG As Boolean
    Public Property logSAG() As Boolean
        Get
            Return _logSAG
        End Get
        Set(ByVal value As Boolean)
            _logSAG = value
            RaisePropertyChanged("logSAG")
        End Set
    End Property

    Private _strRadicado As String
    Public Property strRadicado() As String
        Get
            Return _strRadicado
        End Get
        Set(ByVal value As String)
            _strRadicado = value
            RaisePropertyChanged("strRadicado")
        End Set
    End Property

    ''' <summary>
    ''' Indica si se deshabilita la ventana cuando se esta bloqueando
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Editando As Boolean
        Get
            Return Bloqueando Or DesBloqueando
        End Get
    End Property

    ''' <summary>
    ''' Indica el valor total del portafolio del cliente seleccionado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TotalPortafolioDisponible As Decimal
        Get
            Dim dblTotal As Double = 0
            If Not IsNothing(RegistroSeleccionado) And Not IsNothing(lstTitulosGarantia) Then

                For Each obj In lstTitulosGarantia
                    If (IsNothing(obj.Bloqueado) OrElse obj.Bloqueado = False) AndAlso Not IsNothing(obj.TotalAdmisible) Then
                        dblTotal = dblTotal + obj.TotalAdmisible
                    End If
                Next
            End If
            Return dblTotal
        End Get
    End Property

    ''' <summary>
    ''' Indica el valor total del portafolio que ha sido bloqueado en la liquidación
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TotalPortafolioBloqueadoLiq As Decimal
        Get
            If RegistroSeleccionado IsNot Nothing Then
                Return RegistroSeleccionado.BloqueadoTitulosXLiq
            End If
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Indica el valor total del saldo que ha sido bloqueado en la liquidación
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TotalSaldoBloqueadoLiq As Decimal
        Get
            If RegistroSeleccionado IsNot Nothing Then
                Return RegistroSeleccionado.SaldoBloqueadoXLiq
            End If
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Indica si se muestra en pantalla el indicador de cargando
    ''' </summary>
    ''' <remarks></remarks>
    Private _IsBusy As Boolean
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(value As Boolean)
            _IsBusy = value
            RaisePropertyChanged("IsBusy")
        End Set
    End Property

    ''' <summary>
    ''' Propiedas para indicar que está cargando la consulta de portafolio
    ''' </summary>
    ''' <remarks></remarks>
    Private _IsBusyPortafolio As Boolean
    Public Property IsBusyPortafolio() As Boolean
        Get
            Return _IsBusyPortafolio
        End Get
        Set(ByVal value As Boolean)
            _IsBusyPortafolio = value
            RaisePropertyChanged("IsBusyPortafolio")
        End Set
    End Property

    ''' <summary>
    ''' Indicar que se está cargando la consulta de saldos bloqueados
    ''' </summary>
    ''' <remarks>SV20160422</remarks>
    Private _IsBusySaldosBloqueados As Boolean
    Public Property IsBusySaldosBloqueados() As Boolean
        Get
            Return _IsBusySaldosBloqueados
        End Get
        Set(ByVal value As Boolean)
            _IsBusySaldosBloqueados = value
            RaisePropertyChanged("IsBusySaldosBloqueados")
        End Set
    End Property

    ' Columnas del grid de títulpos
    Public Property ColumnaCantidadBloquear As DataGridColumn

    ''' <summary>
    ''' Ventana con el detalle del cliente para el bloqueo y desbloqueo de la información
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GridDetalle As Grid
    ''' <summary>
    ''' Vista del grid principal donde se listan las operaciones
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property GridPrincipal As Grid

    ''' <summary>
    ''' Código del cliente para el cual se consulta el saldo y los títulos para bloquear
    ''' </summary>
    ''' <remarks>SV20160424</remarks>
    Private _strCodigoClienteBloqueo As String
    Public Property strCodigoClienteBloqueo() As String
        Get
            Return _strCodigoClienteBloqueo
        End Get
        Set(ByVal value As String)
            _strCodigoClienteBloqueo = value
            RaisePropertyChanged("strCodigoClienteBloqueo")
        End Set
    End Property

    ''' <summary>
    ''' Nombre del cliente para el cual se consulta el saldo y los títulos para bloquear
    ''' </summary>
    ''' <remarks>SV20160424</remarks>
    Private _strNombreClienteBloqueo As String
    Public Property strNombreClienteBloqueo() As String
        Get
            Return _strNombreClienteBloqueo
        End Get
        Set(ByVal value As String)
            _strNombreClienteBloqueo = value
            RaisePropertyChanged("strNombreClienteBloqueo")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite el manejo de eventos cuando se borra un cliente
    ''' </summary>
    ''' <remarks>SV20160424</remarks>
    Private _logBorrarCliente As Boolean = False
    Public Property logBorrarCliente() As Boolean
        Get
            Return _logBorrarCliente
        End Get
        Set(ByVal value As Boolean)
            _logBorrarCliente = value
            RaisePropertyChanged("logBorrarCliente")
        End Set
    End Property

    ''' <summary>
    ''' Tipo de mercado que se esta filtrando en pantalla, inicialmente bolsa y derivados
    ''' </summary>
    ''' <remarks>SV20160505</remarks>
    Private _intTipoMercado As Integer
    Public Property intTipoMercado() As Integer
        Get
            Return _intTipoMercado
        End Get
        Set(ByVal value As Integer)
            _intTipoMercado = value
            RaisePropertyChanged("intTipoMercado")
        End Set
    End Property

    ''' <summary>
    ''' Indica el estado de visibilidad de los detalles de saldos bloqueados
    ''' </summary>
    ''' <remarks>SV20160505</remarks>
    Private _logMostrarDetallesSaldos As Boolean = False
    Public Property logMostrarDetallesSaldos() As Boolean
        Get
            Return _logMostrarDetallesSaldos
        End Get
        Set(ByVal value As Boolean)
            _logMostrarDetallesSaldos = value
            If _logMostrarDetallesSaldos = False Then
                strLabelBtnSaldos = "Mostrar"
            Else
                strLabelBtnSaldos = "Ocultar"
            End If
            RaisePropertyChanged("logMostrarDetallesSaldos")
        End Set
    End Property

    ''' <summary>
    ''' Texto para el botón de mostrar u ocultar el detalle de saldos bloqueados
    ''' </summary>
    ''' <remarks>SV20160514</remarks>
    Private _strLabelBtnSaldos As String
    Public Property strLabelBtnSaldos() As String
        Get
            Return _strLabelBtnSaldos
        End Get
        Set(ByVal value As String)
            _strLabelBtnSaldos = value
            RaisePropertyChanged("strLabelBtnSaldos")
        End Set
    End Property

    ''' <summary>
    ''' Valor lógico que indica si se pueden hacer bloqueo de cliete diferente al de la liquidación
    ''' </summary>
    ''' <remarks>SV20160505</remarks>
    Private _logBloquearOtroCliente As Boolean = False
    Public Property logBloquearOtroCliente() As Boolean
        Get
            Return _logBloquearOtroCliente
        End Get
        Set(ByVal value As Boolean)
            _logBloquearOtroCliente = value
            RaisePropertyChanged("logBloquearOtroCliente")
        End Set
    End Property

    ''' <summary>
    ''' Lista de datos para llenar el combo de tipo garantía
    ''' </summary>
    ''' <remarks>SV20160505</remarks>
    Private _lstTipoGarantia As List(Of ItemComboGarantia)
    Public Property lstTipoGarantia() As List(Of ItemComboGarantia)
        Get
            Return _lstTipoGarantia
        End Get
        Set(ByVal value As List(Of ItemComboGarantia))
            _lstTipoGarantia = value
            RaisePropertyChanged("lstTipoGarantia")
        End Set
    End Property

    ''' <summary>
    ''' Lista de datos para llenar el combo de tipo de mercado
    ''' </summary>
    ''' <remarks>SV20160505</remarks>
    Private _lstTipoMercado As List(Of ItemComboGarantia)
    Public Property lstTipoMercado() As List(Of ItemComboGarantia)
        Get
            Return _lstTipoMercado
        End Get
        Set(ByVal value As List(Of ItemComboGarantia))
            _lstTipoMercado = value
            If Not IsNothing(_lstTipoMercado) AndAlso _lstTipoMercado.Count = 1 Then
                intTipoMercado = _lstTipoMercado.FirstOrDefault.intID
            End If
            RaisePropertyChanged("lstTipoMercado")
        End Set
    End Property

    ''' <summary>
    ''' Lista de saldos bloquedos para la operación seleccionada
    ''' </summary>
    ''' <remarks>SV20160505</remarks>
    Private _lstSaldosBloqueados As IEnumerable(Of SaldosBloqueados)
    Public Property lstSaldosBloqueados() As IEnumerable(Of SaldosBloqueados)
        Get
            Return _lstSaldosBloqueados
        End Get
        Set(ByVal value As IEnumerable(Of SaldosBloqueados))
            _lstSaldosBloqueados = value
            RaisePropertyChanged("lstSaldosBloqueados")
        End Set
    End Property

    ''' <summary>
    ''' Listado de titulos consultados para el manejo de las garantias
    ''' </summary>
    ''' <remarks>SV20160505</remarks>
    Private _lstTitulosGarantia As IEnumerable(Of TitulosGarantia)
    Public Property lstTitulosGarantia() As IEnumerable(Of TitulosGarantia)
        Get
            Return _lstTitulosGarantia
        End Get
        Set(ByVal value As IEnumerable(Of TitulosGarantia))
            _lstTitulosGarantia = value
            RaisePropertyChanged("lstTitulosGarantia")
        End Set
    End Property

    ''' <summary>
    ''' Indica si para la consulta de portafolio se estan adicionando filtros 
    ''' </summary>
    ''' <remarks>SV20160515</remarks>
    Private _logFiltroTitulos As Boolean = False
    Public Property logFiltroTitulos() As Boolean
        Get
            Return _logFiltroTitulos
        End Get
        Set(ByVal value As Boolean)
            _logFiltroTitulos = value
            If _logFiltroTitulos = False Then
                strIdEspecieFiltro = String.Empty
                dblCantidadFiltro = 0
            End If
            RaisePropertyChanged("logFiltroTitulos")
        End Set
    End Property

    ''' <summary>
    ''' Especie seleccionada para el filtro de títulos
    ''' </summary>
    ''' <remarks>SV20160515</remarks>
    Private _strIdEspecieFiltro As String
    Public Property strIdEspecieFiltro() As String
        Get
            Return _strIdEspecieFiltro
        End Get
        Set(ByVal value As String)
            _strIdEspecieFiltro = value
            RaisePropertyChanged("strIdEspecieFiltro")
        End Set
    End Property

    ''' <summary>
    ''' Cantiad mayor para el filtro de títulos
    ''' </summary>
    ''' <remarks>SV20160515</remarks>
    Private _dblCantidadFiltro As Double
    Public Property dblCantidadFiltro() As Double
        Get
            Return _dblCantidadFiltro
        End Get
        Set(ByVal value As Double)
            _dblCantidadFiltro = value
            RaisePropertyChanged("dblCantidadFiltro")
        End Set
    End Property

#End Region

#Region "Métodos"


    ''' <summary>
    ''' Consultar la información del visor de garantías
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ConsultarInformacion()
        Try
            IsBusy = True
            _dcProxy.VisorDeGarantias.Clear()
            _dcProxy.Load(_dcProxy.Garantias_Operaciones_ConsultarQuery(Nothing,
                                                                        Nothing,
                                                                        Nothing,
                                                                        Nothing,
                                                                        Nothing,
                                                                        Nothing,
                                                                        Date.Now,
                                                                        _intTipoMercado,
                                                                        "",
                                                                        False,
                                                                        True,
                                                                        Program.Usuario, Program.HashConexion), True, AddressOf TerminoTraerOperaciones, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de consultar la información de las liquidaciones.", Me.ToString(),
                                                         "ConsultarInformacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el listado de filtros para el auto-completar del filtro en pantalla
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ObtenerFiltros()
        Try
            Dim lFiltro As New List(Of String)

            lFiltro = lFiltro.Union(ListaDatos.Where(Function(r) Not String.IsNullOrWhiteSpace(r.NombreCliente)).Select(Function(r) r.NombreCliente).Distinct()).ToList

            lFiltro = lFiltro.Union(ListaDatos.Where(Function(r) Not String.IsNullOrWhiteSpace(r.IDEspecie)).Select(Function(r) r.IDEspecie).Distinct()).ToList

            lFiltro = lFiltro.Union(ListaDatos.Where(Function(r) Not String.IsNullOrWhiteSpace(r.DescripcionTipoOferta)).Select(Function(r) r.DescripcionTipoOferta).Distinct()).ToList

            lFiltro = lFiltro.Union(ListaDatos.Select(Function(r) r.IDLiquidacion.ToString).Distinct()).ToList

            ListaFiltros = lFiltro
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obrener los filtros.", Me.ToString(),
                                                         "ObtenerFiltros", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Muestra la información respectiva cuando se registra o cambia el filtro en pantalla
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CambioFiltroSeleccionado()
        Try
            If String.IsNullOrWhiteSpace(Filtro) Then
                If Not IsNothing(ListaDatos) Then
                    ListaVisorDeGarantias = New PagedCollectionView(ListaDatos)
                Else
                    ListaVisorDeGarantias = New PagedCollectionView(New List(Of VisorDeGarantias))
                End If
                Return
            End If

            Dim filtroLower = Filtro.ToLower

            Dim lista As IEnumerable(Of VisorDeGarantias)

            If Not IsNothing(ListaDatos) Then
                'Consultar la información que coincide con el filtro
                lista = ListaDatos.Where(Function(r) Not String.IsNullOrWhiteSpace(r.NombreCliente)).Where(Function(r) r.NombreCliente.ToLower = filtroLower).ToList

                lista = lista.Union(ListaDatos.Where(Function(r) Not String.IsNullOrWhiteSpace(r.IDEspecie)).Where(Function(r) r.IDEspecie.ToLower = filtroLower)).ToList

                lista = lista.Union(ListaDatos.Where(Function(r) Not String.IsNullOrWhiteSpace(r.DescripcionTipoOferta)).Where(Function(r) r.DescripcionTipoOferta.ToLower = filtroLower)).ToList

                lista = lista.Union(ListaDatos.Where(Function(r) r.IDLiquidacion.ToString.ToLower = filtroLower)).ToList

                'Si no se encuentra información, se busca información que contiene el filtro
                If Not lista.Any Then

                    lista = ListaDatos.Where(Function(r) Not String.IsNullOrWhiteSpace(r.NombreCliente)).Where(Function(r) r.NombreCliente.ToLower.Contains(filtroLower)).ToList

                    lista = lista.Union(ListaDatos.Where(Function(r) Not String.IsNullOrWhiteSpace(r.IDEspecie)).Where(Function(r) r.IDEspecie.ToLower.Contains(filtroLower))).ToList

                    lista = lista.Union(ListaDatos.Where(Function(r) Not String.IsNullOrWhiteSpace(r.DescripcionTipoOferta)).Where(Function(r) r.DescripcionTipoOferta.ToLower.Contains(filtroLower))).ToList

                    lista = lista.Union(ListaDatos.Where(Function(r) r.IDLiquidacion.ToString.ToLower.Contains(filtroLower))).ToList
                End If
            Else
                lista = New List(Of VisorDeGarantias)
            End If

            ListaVisorDeGarantias = New PagedCollectionView(lista)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de cambiar el filtro.", Me.ToString(),
                                                         "CambioFiltroSeleccionado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Limpiar el filtro en pantalla
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LimpiarFiltro()
        Filtro = String.Empty
    End Sub

    ''' <summary>
    ''' Consultar los detalles, pero solo los que se desean bloquear por cliente
    ''' </summary>
    ''' <remarks>SV20160507</remarks>
    Public Sub CargarDetallesCliente()
        Try
            logCargaDesdeBloqueo = False
            logConsultaPorCliente = True
            CargarDetalles()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de cargar los detalles por cliente.", Me.ToString(),
                                                         "CargarDetallesCliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private _bitConsultandoSaldo As Boolean = False
    Public Property bitConsultandoSaldo() As Boolean
        Get
            Return _bitConsultandoSaldo
        End Get
        Set(ByVal value As Boolean)
            _bitConsultandoSaldo = value
            RaisePropertyChanged("bitConsultandoSaldo")
        End Set
    End Property



    ''' <summary>
    ''' Cargar los títulos y el saldo del cliente seleccionado
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarDetalles()
        Try
            If _RegistroSeleccionado IsNot Nothing Then

                CargarPortafolio()

                If logConsultaDesdeBloqueo = True OrElse logConsultaPorCliente OrElse IsNothing(RegistroSeleccionado.SaldoCliente) OrElse RegistroSeleccionado.SaldoCliente = 0 Then
                    logConsultaDesdeBloqueo = False
                    If Not IsNothing(RegistroSeleccionado.SaldoCliente) Then
                        RegistroSeleccionado.SaldoCliente = 0
                    End If

                    _dcProxy.SaldosClientes.Clear()
                    bitConsultandoSaldo = True
                    _dcProxy.Load(_dcProxy.Garantias_SaldoCliente_ConsultarQuery(strCodigoClienteBloqueo, Program.Usuario, Program.HashConexion),
                                  LoadBehavior.MergeIntoCurrent, AddressOf TerminoTraerSaldo, strCodigoClienteBloqueo)
                End If

                If logConsultaPorCliente = False Then

                    If Not IsNothing(loSaldosBloqueados) Then
                        If Not loSaldosBloqueados.IsComplete Then
                            loSaldosBloqueados.Cancel()
                        End If
                    End If

                    IsBusySaldosBloqueados = True
                    _dcProxy.SaldosBloqueados.Clear()
                    loSaldosBloqueados = _dcProxy.Load(_dcProxy.Garantias_SaldosBloqueados_ConsultarQuery(strCodigoClienteBloqueo,
                                                                                     RegistroSeleccionado.IDLiquidacion,
                                                                                     RegistroSeleccionado.Parcial,
                                                                                     RegistroSeleccionado.Tipo,
                                                                                     RegistroSeleccionado.ClaseOrden,
                                                                                     RegistroSeleccionado.FechaLiquidacion,
                                                                                     intTipoMercado,
                                                                                     Program.Usuario, Program.HashConexion),
                                                    LoadBehavior.MergeIntoCurrent, AddressOf TerminoTraerSaldosBloqueados, Nothing)

                Else
                    lstSaldosBloqueados = Nothing
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de cargar los detalles.", Me.ToString(),
                                                         "CargarDetalles", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta del portafolio del cliente seleccionado
    ''' </summary>
    ''' <remarks>SV20160515</remarks>
    Public Sub CargarPortafolio()
        Try
            Dim strEstadoTituloConsulta As String = "T"

            If logConsultaPorCliente = True Then
                If strCodigoClienteBloqueo <> _RegistroSeleccionado.IDCliente Then
                    strEstadoTituloConsulta = "L"
                End If
            End If

            If Not IsNothing(loTitulosgarantia) Then
                If Not loTitulosgarantia.IsComplete Then
                    loTitulosgarantia.Cancel()
                End If
            End If

            IsBusyPortafolio = True
            _dcProxy.TitulosGarantias.Clear()
            loTitulosgarantia = _dcProxy.Load(_dcProxy.Garantias_Custodias_ConsultarQuery(strCodigoClienteBloqueo,
                                                                    RegistroSeleccionado.IDLiquidacion,
                                                                    RegistroSeleccionado.Parcial,
                                                                    RegistroSeleccionado.Tipo,
                                                                    RegistroSeleccionado.ClaseOrden,
                                                                        RegistroSeleccionado.FechaLiquidacion,
                                                                    strEstadoTituloConsulta,
                                                                    intTipoMercado,
                                                                    strIdEspecieFiltro,
                                                                    dblCantidadFiltro,
                                                                    Program.Usuario, Program.HashConexion),
                        LoadBehavior.MergeIntoCurrent, AddressOf TerminoTraerTitulosGarantia, logConsultaPorCliente)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de cargar el portafolio.", Me.ToString(),
                                                         "CargarPortafolio", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Obtener el texto para mostrar de acuerdo al estado bloquear o desbloquear
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenerTextoBloquear() As String
        If DesBloqueando Then
            Return STR_TEXTO_DESBLOQUEAR
        End If
        Return STR_TEXTO_BLOQUEAR
    End Function

    ''' <summary>
    ''' Configurar la ventana para el proceso de bloqueo
    ''' </summary>
    ''' <param name="visor"></param>
    ''' <remarks></remarks>
    Public Sub Bloquear(visor As VisorDeGarantias)
        Try
            If Not IsNothing(visor) Then
                If _dcProxy.IsLoading Then

                    A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Not Bloqueando Then
                    lstSaldosBloqueadosAnterior = lstSaldosBloqueados
                    lstTitulosAnterior = lstTitulosGarantia
                    objRegistroSeleccionadoAnterior = RegistroSeleccionado
                    LiquidacionABloquear = visor
                    _LiquidacionABloquear.ValorBloquear = 0
                    'CambiarBloqueos(False)
                    visor.PuedeDesBloquear = False
                    visor.PuedeBloquear = True
                    DesBloqueando = False
                    Bloqueando = True
                    lstTitulosBloquearDesbloquear = Nothing
                    lstSaldosaDesbloquear = Nothing
                    NotasBloqueo = String.Empty
                    strRadicado = String.Empty
                    logSAG = True
                    ColumnaCantidadBloquear.Visibility = Visibility.Visible
                    ColumnaCantidadBloquear.Header = STR_TXTCOL_CANTIDAD_BLOQUEO
                    Navegando = False
                    For Each vTitulo As TitulosGarantia In lstTitulosGarantia
                        vTitulo.BloquearDesbloqueado = Not Equals(vTitulo.Bloqueado, True) And Equals(vTitulo.TituloEditable, True) And Bloqueando
                        vTitulo.CantidadBloquear = vTitulo.Cantidad
                        vTitulo.Bloquear = False
                    Next
                    logMostrarDetallesSaldos = False
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de configurar el bloqueo.", Me.ToString(),
                                                         "Bloquear", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Configurar la vantana para el proceso de desbloqueo
    ''' </summary>
    ''' <param name="visor"></param>
    ''' <remarks></remarks>
    Public Sub DesBloquear(visor As VisorDeGarantias)
        Try
            If Not IsNothing(visor) Then
                If _dcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Not DesBloqueando Then
                    lstSaldosBloqueadosAnterior = lstSaldosBloqueados
                    lstTitulosAnterior = lstTitulosGarantia
                    objRegistroSeleccionadoAnterior = RegistroSeleccionado
                    LiquidacionABloquear = visor
                    _LiquidacionABloquear.ValorBloquear = 0
                    'CambiarBloqueos(False)
                    visor.PuedeBloquear = False
                    visor.PuedeDesBloquear = True
                    Bloqueando = False
                    DesBloqueando = True
                    lstTitulosBloquearDesbloquear = Nothing
                    lstSaldosaDesbloquear = Nothing
                    NotasBloqueo = String.Empty
                    ColumnaCantidadBloquear.Visibility = Visibility.Visible
                    ColumnaCantidadBloquear.Header = STR_TXTCOL_CANTIDAD_DESBLOQUEO
                    Navegando = False
                    For Each vTitulo As TitulosGarantia In lstTitulosGarantia
                        vTitulo.DesBloquearBloqueado = Equals(vTitulo.Bloqueado, True) And Equals(vTitulo.TituloEditable, True) And DesBloqueando
                        vTitulo.CantidadBloquear = vTitulo.Cantidad
                        vTitulo.Bloquear = True
                    Next
                    For Each objSaldo As SaldosBloqueados In lstSaldosBloqueados
                        objSaldo.Desbloqueando = True
                    Next
                    logMostrarDetallesSaldos = True
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de configurar el desbloqueo.", Me.ToString(),
                                                         "DesBloquear", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    ''' Evento cuando el usuario da clic en el boton aceptar para bloquear/desbloquear
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Aceptar()
        Try
            Dim lstSaldosDesbloquear As List(Of SaldosBloqueados) = New List(Of SaldosBloqueados)

            'Retornar si no esta en proceso de bloqueo/desbloqueo
            If Not Bloqueando And Not DesBloqueando Then
                Return
            End If

            Dim texto = ObtenerTextoBloquear()

            'Obtener titulos seleccionados de acuerdo a la operacion
            Dim titulosBloquear As List(Of TitulosGarantia) = ObtenerTitulosBloquear()

            If DesBloqueando Then
                lstSaldosDesbloquear = lstSaldosBloqueados.Where(Function(r) Equals(r.DesbloquearSaldo, True)).ToList
            End If

            'Verificar que la cantidad a bloquear/desbloquear sea mayor que 0
            If titulosBloquear.Any(Function(r) r.CantidadBloquear Is Nothing Or r.CantidadBloquear <= 0) Then
                A2Utilidades.Mensajes.mostrarMensaje(String.Format(STR_MENSAJE_CANTIDAD_TITULOS, texto), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return
            End If

            'Verificar que la cantidad a bloquear/desbloquear no sea mayor que la cantidad del titulo
            If titulosBloquear.Any(Function(r) r.CantidadBloquear > r.Cantidad) Then
                A2Utilidades.Mensajes.mostrarMensaje(String.Format(STR_MENSAJE_CANTIDAD_BLOQUEAR_MAYOR, texto), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return
            End If

            ''Verificar que la acción tenga valor admisible
            'If Bloqueando And titulosBloquear.Any(Function(r) r.TotalAdmisible Is Nothing Or r.TotalAdmisible <= 0) Then
            '    A2Utilidades.Mensajes.mostrarMensaje(String.Format(STR_MENSAJE_NO_TIENE_VALOR_ADMISIBLE, texto), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Return
            'End If

            'Verificar que el usuario ha registrado un valor o ha seleccionado titulos para bloquear
            If (LiquidacionABloquear Is Nothing Or LiquidacionABloquear.ValorBloquear Is Nothing Or LiquidacionABloquear.ValorBloquear <= 0) And
                    Not titulosBloquear.Any And Not lstSaldosDesbloquear.Any Then
                A2Utilidades.Mensajes.mostrarMensaje(String.Format(STR_MENSAJE_DEBE_SELECCIONAR, texto), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return
            End If

            'Obtener el valor para bloquear
            Dim valorBloquear As Double = 0
            If LiquidacionABloquear.ValorBloquear IsNot Nothing Then
                valorBloquear = LiquidacionABloquear.ValorBloquear
            End If


            'Validar que el valor a bloquear no sea mayor que el saldo del cliente
            If logValidarSaldoBloqueo Then
                If Bloqueando And valorBloquear > 0 And valorBloquear > LiquidacionABloquear.SaldoCliente Then
                    A2Utilidades.Mensajes.mostrarMensaje(STR_MENSAJE_SALDO_BLOQUEAR_MAYOR, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return
                End If
            End If

            'Iniciar la ventana de confirmación que muestra el detalle de la operación a realizar
            Dim cConfirmar As New WConfirmarView(Bloqueando,
                                                 RegistroSeleccionado.NombreCliente,
                                                 RegistroSeleccionado.IDCliente,
                                                 LiquidacionABloquear.IDLiquidacion,
                                                 IIf(Bloqueando, valorBloquear, (lstSaldosDesbloquear.Sum(Function(r) r.ValorBloqueado))),
                                                 titulosBloquear.Sum(Function(r) (r.TotalAdmisible * r.CantidadBloquear) / r.Cantidad))
            AddHandler cConfirmar.Closed, AddressOf TerminoConfirmarBloquear
            Program.Modal_OwnerMainWindowsPrincipal(cConfirmar)
            cConfirmar.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de realizar la operación.", Me.ToString(),
                                                         "Aceptar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Cerrar la ventana de confirmacion y realizar la operacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TerminoConfirmarBloquear(sender As Object, e As EventArgs)
        Try
            Dim cConfirmar As WConfirmarView = sender
            If Equals(cConfirmar.DialogResult, True) Then
                lstSaldosaDesbloquear = New List(Of SaldosBloqueados)
                lstTitulosBloquearDesbloquear = New List(Of TitulosGarantia)
                'Obtener los titulos de acuerdo al estado
                lstTitulosBloquearDesbloquear = ObtenerTitulosBloquear()

                If DesBloqueando Then
                    lstSaldosaDesbloquear = lstSaldosBloqueados.Where(Function(r) Equals(r.DesbloquearSaldo, True)).ToList
                End If

                _dcProxy.RespuestaValidaciones.Clear()
                'Bloquea el saldo en caso de que se haya registrado o bloquea los titulos seleccionados
                If Bloqueando And LiquidacionABloquear.ValorBloquear > 0 Then
                    IsBusy = True

                    _dcProxy.Load(_dcProxy.Garantias_Saldos_Bloquear_LiberarQuery(strCodigoClienteBloqueo,
                                                                             Nothing,
                                                                             LiquidacionABloquear.ValorBloquear,
                                                                             ObtenerOperacionSaldo(),
                                                                             DateTime.Now,
                                                                             NotasBloqueo,
                                                                             LiquidacionABloquear.IDLiquidacion,
                                                                             LiquidacionABloquear.Parcial,
                                                                             LiquidacionABloquear.Tipo,
                                                                             LiquidacionABloquear.ClaseOrden,
                                                                             LiquidacionABloquear.FechaLiquidacion,
                                                                             intTipoMercado,
                                                                             logSAG,
                                                                             strRadicado,
                                                                             False,
                                                                             Nothing,
                                                                             0,
                                                                             Program.Usuario, Program.HashConexion),
                                                                         AddressOf BloquearDesbloquearResgistrosRecursivo, AccionEjecucion.BloqueoSaldo)

                ElseIf DesBloqueando AndAlso lstSaldosaDesbloquear.Any Then

                    Dim objSaldo = lstSaldosaDesbloquear.First
                    lstSaldosaDesbloquear.Remove(objSaldo)
                    IsBusy = True
                    _dcProxy.Load(_dcProxy.Garantias_Saldos_Bloquear_LiberarQuery(strCodigoClienteBloqueo,
                                                                                 Nothing,
                                                                                 objSaldo.ValorBloqueado,
                                                                                 ObtenerOperacionSaldo(),
                                                                                 DateTime.Now,
                                                                                 NotasBloqueo,
                                                                                 LiquidacionABloquear.IDLiquidacion,
                                                                                 LiquidacionABloquear.Parcial,
                                                                                 LiquidacionABloquear.Tipo,
                                                                                 LiquidacionABloquear.ClaseOrden,
                                                                                 LiquidacionABloquear.FechaLiquidacion,
                                                                                 objSaldo.TipoMercado,
                                                                                 objSaldo.SAG,
                                                                                 objSaldo.Radicado,
                                                                                  False,
                                                                                  Nothing,
                                                                                  objSaldo.IdBloqueo,
                                                                                 Program.Usuario, Program.HashConexion),
                                                                              AddressOf BloquearDesbloquearResgistrosRecursivo, AccionEjecucion.DesbloqueoSaldo)

                Else
                    If lstTitulosBloquearDesbloquear.Any Then
                        Dim objTitulo = lstTitulosBloquearDesbloquear.First
                        lstTitulosBloquearDesbloquear.Remove(objTitulo)
                        IsBusy = True
                        _dcProxy.Load(_dcProxy.Garantias_Custodias_Bloquear_LiberarQuery(objTitulo.IDRecibo,
                                                                                         objTitulo.Secuencia,
                                                                                         Nothing,
                                                                                         NotasBloqueo,
                                                                                         objTitulo.CantidadBloquear,
                                                                                         ObtenerOperacionTitulos(),
                                                                                         objTitulo.FechaRecibo,
                                                                                         LiquidacionABloquear.IDLiquidacion,
                                                                                         LiquidacionABloquear.Parcial,
                                                                                         LiquidacionABloquear.Tipo,
                                                                                         LiquidacionABloquear.ClaseOrden,
                                                                                         LiquidacionABloquear.FechaLiquidacion,
                                                                                         intTipoMercado,
                                                                                         objTitulo.SAG,
                                                                                         objTitulo.Radicado,
                                                                                        False,
                                                                                        Nothing,
                                                                                         objTitulo.IdBloqueo,
                                                                                         Program.Usuario, Program.HashConexion),
                                                                                     AddressOf BloquearDesbloquearResgistrosRecursivo, AccionEjecucion.BloqueodesbloqueoTitulo)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de confirmar la operación.", Me.ToString(),
                                                         "TerminoConfirmarBloquear", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Evento ejecutado al dar clic en el boton cancelar en la ventana
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Cancelar()
        Try
            If Not Bloqueando And Not DesBloqueando Then
                Return
            End If

            lstSaldosBloqueados = lstSaldosBloqueadosAnterior
            lstTitulosGarantia = lstTitulosAnterior
            RegistroSeleccionado.SaldoCliente = objRegistroSeleccionadoAnterior.SaldoCliente

            FinalizarBloqueos()
            DesBloqueando = False
            Bloqueando = False
            Navegando = True

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la operación.", Me.ToString(),
                                                         "Cancelar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Marcar el título para ser bloqueado
    ''' </summary>
    ''' <param name="titulo"></param>
    ''' <remarks></remarks>
    Public Sub BloquearTitulo(titulo As TitulosGarantia)
        Try
            If titulo.Bloquear Is Nothing Then
                titulo.Bloquear = False
            End If

            titulo.Bloquear = Not titulo.Bloquear
            If Bloqueando Then
                If titulo.Bloquear Then
                    titulo.SAG = True
                    titulo.EditarBloqueando = True
                Else
                    titulo.SAG = False
                    titulo.Radicado = String.Empty
                    titulo.EditarBloqueando = False
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al selecccionar la opción para bloquear o desbloquear títulos.", Me.ToString(),
                                                         "BloquearTitulo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Marcar el título para ser desbloqueado
    ''' </summary>
    ''' <param name="objSaldo"></param>
    ''' <remarks>SV20160505</remarks>
    Public Sub DesbloquearSaldoBloqueado(objSaldo As SaldosBloqueados)
        Try
            If objSaldo.DesbloquearSaldo Is Nothing Then
                objSaldo.DesbloquearSaldo = False
            End If

            objSaldo.DesbloquearSaldo = Not objSaldo.DesbloquearSaldo
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al selecccionar la opción para desbloqear saldos.", Me.ToString(),
                                                         "DesbloquearSaldoBloqueado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub DistribuirTituloBloqueado(objTitulo As TitulosGarantia)
        Try
            TituloSeleccionado = objTitulo 'Santiago Vergara 20191010
            Dim cDistribuirBloqueo As New WDistribuirBloqueoView()
            AddHandler cDistribuirBloqueo.Closed, AddressOf TerminoDistribuirBloqueoTitulo
            If objTitulo.SAG Then
                cDistribuirBloqueo.nbxBloqueoSag.Value = objTitulo.Cantidad
                cDistribuirBloqueo.nbxBloqueoSag.IsEnabled = False
                cDistribuirBloqueo.txtRadicado.Text = objTitulo.Radicado
                cDistribuirBloqueo.nbxBloqueoInterno.Maximum = objTitulo.Cantidad
            Else
                cDistribuirBloqueo.nbxBloqueoInterno.Value = objTitulo.Cantidad
                cDistribuirBloqueo.nbxBloqueoInterno.IsEnabled = False
                cDistribuirBloqueo.nbxBloqueoSag.Maximum = objTitulo.Cantidad
            End If
            Program.Modal_OwnerMainWindowsPrincipal(cDistribuirBloqueo)
            cDistribuirBloqueo.ShowDialog()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar distribuir el bloqueo.", Me.ToString(),
                                                         "DistribuirTituloBloqueado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub DistribuirSaldoBloqueado(objSaldo As SaldosBloqueados)
        Try
            SaldoSeleccionado = objSaldo 'Santiago Vergara 20191010
            Dim cDistribuirBloqueo As New WDistribuirBloqueoView()
            AddHandler cDistribuirBloqueo.Closed, AddressOf TerminoDistribuirBloqueoSaldo
            If objSaldo.SAG Then
                cDistribuirBloqueo.nbxBloqueoSag.Value = objSaldo.ValorBloqueado
                cDistribuirBloqueo.nbxBloqueoSag.IsEnabled = False
                cDistribuirBloqueo.txtRadicado.Text = objSaldo.Radicado
                cDistribuirBloqueo.nbxBloqueoInterno.Maximum = objSaldo.ValorBloqueado
            Else
                cDistribuirBloqueo.nbxBloqueoInterno.Value = objSaldo.ValorBloqueado
                cDistribuirBloqueo.nbxBloqueoInterno.IsEnabled = False
                cDistribuirBloqueo.nbxBloqueoSag.Maximum = objSaldo.ValorBloqueado
            End If

            Program.Modal_OwnerMainWindowsPrincipal(cDistribuirBloqueo)
            cDistribuirBloqueo.ShowDialog()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar distribuir el bloqueo.", Me.ToString(),
                                                         "DistribuirSaldoBloqueado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private _TituloSeleccionado As TitulosGarantia
    Public Property TituloSeleccionado() As TitulosGarantia
        Get
            Return _TituloSeleccionado
        End Get
        Set(ByVal value As TitulosGarantia)
            _TituloSeleccionado = value
            RaisePropertyChanged("TituloSeleccionado")
        End Set
    End Property


    Private _SaldoSeleccionado As SaldosBloqueados
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property SaldoSeleccionado() As SaldosBloqueados
        Get
            Return _SaldoSeleccionado
        End Get
        Set(ByVal value As SaldosBloqueados)
            _SaldoSeleccionado = value
            RaisePropertyChanged("SaldoSeleccionado")
        End Set
    End Property


    ''' <summary>
    ''' Callback cuando termina de confirmar la distribución del bloqueo de título
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e">SV20160809</param>
    ''' <remarks></remarks>
    Private Sub TerminoDistribuirBloqueoTitulo(sender As Object, e As EventArgs)
        Try
            Dim cDistribuirBloqueo As WDistribuirBloqueoView = sender
            If Equals(cDistribuirBloqueo.DialogResult, True) Then
                If TituloSeleccionado.SAG Then
                    TituloSeleccionado.CantidadReasignar = cDistribuirBloqueo.dbValorInterno
                Else
                    TituloSeleccionado.CantidadReasignar = cDistribuirBloqueo.dblValorLegal
                End If
                TituloSeleccionado.RadicadoReasignar = TituloSeleccionado.Radicado
                TituloSeleccionado.Reasignar = True


                _dcProxy.Load(_dcProxy.Garantias_Custodias_Bloquear_LiberarQuery(TituloSeleccionado.IDRecibo,
                                                                                       TituloSeleccionado.Secuencia,
                                                                                     Nothing,
                                                                                       String.Empty,
                                                                                     0,
                                                                                       "RE",
                                                                                       TituloSeleccionado.FechaRecibo,
                                                                                       RegistroSeleccionado.IDLiquidacion,
                                                                                       RegistroSeleccionado.Parcial,
                                                                                       RegistroSeleccionado.Tipo,
                                                                                       RegistroSeleccionado.ClaseOrden,
                                                                                       RegistroSeleccionado.FechaLiquidacion,
                                                                                     intTipoMercado,
                                                                                       TituloSeleccionado.SAG,
                                                                                       TituloSeleccionado.RadicadoReasignar,
                                                                                        True,
                                                                                        TituloSeleccionado.CantidadReasignar,
                                                                                         TituloSeleccionado.IdBloqueo,
                                                                                     Program.Usuario, Program.HashConexion),
                                                                                   AddressOf BloquearDesbloquearResgistrosRecursivo, AccionEjecucion.DistribuirBloqueoTitulo)

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de confirmar la distribución del bloqueo.", Me.ToString(),
                                                         "TerminoDistribuirBloqueo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Callback cuando termina de confirmar la distribución del bloqueo de saldo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e">SV20160809</param>
    ''' <remarks></remarks>
    Private Sub TerminoDistribuirBloqueoSaldo(sender As Object, e As EventArgs)
        Try

            Dim cDistribuirBloqueo As WDistribuirBloqueoView = sender
            If Equals(cDistribuirBloqueo.DialogResult, True) Then
                If SaldoSeleccionado.SAG Then
                    SaldoSeleccionado.ValorReasignar = cDistribuirBloqueo.dbValorInterno
                Else
                    SaldoSeleccionado.ValorReasignar = cDistribuirBloqueo.dblValorLegal

                End If
                SaldoSeleccionado.RadicadoReasignar = SaldoSeleccionado.Radicado
                SaldoSeleccionado.Reasignar = True
                _dcProxy.Load(_dcProxy.Garantias_Saldos_Bloquear_LiberarQuery(strCodigoClienteBloqueo,
                                                                                         Nothing,
                                                                                         SaldoSeleccionado.ValorBloqueado,
                                                                                         ObtenerOperacionSaldo(),
                                                                                         DateTime.Now,
                                                                                         NotasBloqueo,
                                                                                         RegistroSeleccionado.IDLiquidacion,
                                                                                         RegistroSeleccionado.Parcial,
                                                                                         RegistroSeleccionado.Tipo,
                                                                                         RegistroSeleccionado.ClaseOrden,
                                                                                         RegistroSeleccionado.FechaLiquidacion,
                                                                                         SaldoSeleccionado.TipoMercado,
                                                                                         SaldoSeleccionado.SAG,
                                                                                         SaldoSeleccionado.RadicadoReasignar,
                                                                                         True,
                                                                                         SaldoSeleccionado.ValorReasignar,
                                                                                         SaldoSeleccionado.IdBloqueo,
                                                                                         Program.Usuario, Program.HashConexion),
                                                                                      AddressOf BloquearDesbloquearResgistrosRecursivo, AccionEjecucion.DistribuirBloqueoSaldo)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de confirmar la distribución del bloqueo.", Me.ToString(),
                                                         "TerminoDistribuirBloqueo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub



    ''' <summary>
    ''' Finalizar el estado de bloqueo de la ventana
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FinalizarBloqueos()
        Try
            LiquidacionABloquear = Nothing
            If Bloqueando Then
                For Each vTitulo As TitulosGarantia In lstTitulosGarantia
                    vTitulo.BloquearDesbloqueado = False
                    vTitulo.EditarBloqueando = False
                    If vTitulo.Bloqueado = False Then
                        vTitulo.Radicado = String.Empty
                        vTitulo.SAG = False
                    End If
                Next
            ElseIf DesBloqueando Then
                For Each vTitulo As TitulosGarantia In lstTitulosGarantia
                    vTitulo.DesBloquearBloqueado = False
                Next
                For Each objSaldo As SaldosBloqueados In lstSaldosBloqueados
                    objSaldo.Desbloqueando = False
                Next

                logMostrarDetallesSaldos = False

            End If

            'CambiarBloqueos(True)
            RegistroSeleccionado.PuedeDesBloquear = True
            RegistroSeleccionado.PuedeBloquear = True

            If Not IsNothing(RegistroSeleccionadoCliente) Then
                RegistroSeleccionadoCliente.PuedeDesBloquear = True
                RegistroSeleccionadoCliente.PuedeBloquear = True
            End If

            logFiltroTitulos = False

            RaisePropertyChanged("RegistroSeleccionadoCliente")
            RaisePropertyChanged("RegistroSeleccionado")

            ColumnaCantidadBloquear.Visibility = Visibility.Collapsed
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al finalizar los bloqueos o desbloqueos.", Me.ToString(),
                                                         "FinalizarBloqueos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Determina la cantidad de clientes seleccionados en pantalla del visor cuando se agrupa
    ''' Si es un solo cliente, lo asigna a registroseleccionado, de lo contrario, asigna null
    ''' </summary>
    ''' <param name="dataGridSelectedItemsCollection"></param>
    ''' <remarks></remarks>
    Public Sub CambioSeleccion(dataGridSelectedItemsCollection As DataGridSelectedItemsCollection(Of DataGridRow))
        Try
            Dim reg As VisorDeGarantias = Nothing

            If Not dataGridSelectedItemsCollection Is Nothing Then
                If dataGridSelectedItemsCollection.Any Then
                    If dataGridSelectedItemsCollection(0).Type = DataGridRowType.Group Then
                        Dim clientes = ObtenerClientesSeleccionadosPorGrupo(dataGridSelectedItemsCollection.ToList)
                        If clientes.Count = 1 Then
                            reg = ListaDatos.FirstOrDefault(Function(r) Equals(r.IDCliente, clientes.First))
                        End If
                    ElseIf dataGridSelectedItemsCollection.Count = 1 And dataGridSelectedItemsCollection(0).Type = DataGridRowType.Item Then
                        reg = dataGridSelectedItemsCollection(0).DataItem
                    End If
                End If
            End If
            RegistroSeleccionado = reg
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el registro seleccionado.", Me.ToString(),
                                                         "CambioSeleccion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Retorna los clientes del grupo seleccionado, para verificar que es un solo cliente seleccionado en el grupo
    ''' </summary>
    ''' <param name="gridRows"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenerClientesSeleccionadosPorGrupo(gridRows As IEnumerable(Of DataGridRow)) As List(Of String)

        Dim lista As New List(Of String)

        If gridRows(0).Type = DataGridRowType.Group Then
            lista.AddRange(ObtenerClientesSeleccionadosPorGrupo(CType(gridRows(0), DataGridGroupRow).Rows))
        ElseIf gridRows(0).Type = DataGridRowType.Item Then
            lista.AddRange(gridRows.Select(Function(r) CType(r.DataItem, VisorDeGarantias).IDCliente).Distinct.ToList)
        End If

        Return lista.Distinct.ToList

    End Function

    ''' <summary>
    ''' Asignar la prioridad a la lista de acuerdo al estado de cada registro
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ObtenerPrioridad()
        Try
            For Each v As VisorDeGarantias In ListaDatos
                v.Prioridad = VisorDeGarantiasViewModel.ObtenerEstadoInterno(v)
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la prioridad del visor de garantias.", Me.ToString(),
                                                         "ObtenerPrioridad", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Indica si se puede bloquear o desbloquear las liquidacoines del cliente seleccionado
    ''' </summary>
    ''' <param name="Habilitar"></param>
    ''' <remarks></remarks>
    Private Sub CambiarBloqueos(Habilitar As Boolean)
        Try
            For Each liquidacion As VisorDeGarantias In LiquidacionesSeleccionadas
                liquidacion.PuedeBloquear = Habilitar
                liquidacion.PuedeDesBloquear = Habilitar
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar el estado de los bloqueos.", Me.ToString(),
                                                         "CambiarBloqueos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se llama desde el view para visualizar o no los detalles de bloqueos de saldo
    ''' </summary>
    ''' <remarks>SV20160506</remarks>
    Public Sub MostrarOcultarSaldos()
        Try
            If IsNothing(logMostrarDetallesSaldos) Then
                logMostrarDetallesSaldos = False
            End If

            logMostrarDetallesSaldos = Not logMostrarDetallesSaldos
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al mostrar u ocultar los saldos bloqueados.", Me.ToString(),
                                                         "MostrarOcultarSaldos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza el estado del control y limpia los datos al ejecutar un bloqueo
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FinalizarEjecutarBloqueos()
        Try
            _dcProxy2.VisorDeGarantias.Clear()
            AsignarDatosOperacionActual(_RegistroSeleccionado) 'SV20180411
            _dcProxy2.Load(_dcProxy2.Garantias_Operaciones_ConsultarQuery(Nothing,
                                                                        Nothing,
                                                                        _RegistroSeleccionado.IDLiquidacion,
                                                                        _RegistroSeleccionado.Parcial,
                                                                        _RegistroSeleccionado.Tipo,
                                                                        _RegistroSeleccionado.ClaseOrden,
                                                                        _RegistroSeleccionado.FechaLiquidacion,
                                                                        intTipoMercado,
                                                                        "",
                                                                        False,
                                                                        True,
                                                                        Program.Usuario, Program.HashConexion), LoadBehavior.MergeIntoCurrent, AddressOf TerminoActualizarOperacion, _RegistroSeleccionado.IDLiquidacion)

            logCargaDesdeBloqueo = True
            strCodigoClienteBloqueo = _RegistroSeleccionado.IDCliente
            strNombreClienteBloqueo = _RegistroSeleccionado.NombreCliente
            logConsultaPorCliente = False
            logConsultaDesdeBloqueo = True
            CargarDetalles()

            FinalizarBloqueos()

            lstTitulosBloquearDesbloquear = Nothing
            lstSaldosaDesbloquear = Nothing

            Bloqueando = False
            DesBloqueando = False
            Navegando = True
            For Each objTitulo As TitulosGarantia In lstTitulosGarantia
                objTitulo.Bloquear = False
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al finalizar la ejecución de los bloqueos", Me.ToString(),
                                                         "FinalizarEjecutarBloqueos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Actualizar la informacion de los titulos en pantalla
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RaisePropertyTitulos()
        RaisePropertyChanged("lstTitulosGarantia")
        RaisePropertyChanged("TotalPortafolioDisponible")
        RaisePropertyChanged("TotalPortafolioBloqueadoLiq")
        RaisePropertyChanged("TotalSaldoBloqueadoLiq")
    End Sub

    ''' <summary>
    ''' Obtener los titulos seleccionados de acuerdo a la operacion que se esta ejecutando
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenerTitulosBloquear() As List(Of TitulosGarantia)
        If Bloqueando Then
            Return lstTitulosGarantia.Where(Function(r) Equals(r.Bloquear, True)).ToList
        Else
            Return lstTitulosGarantia.Where(Function(r) Equals(r.Bloquear, False)).ToList
        End If

    End Function

    ''' <summary>
    ''' Obtener la operacion que se envia a base de datos para los saldos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenerOperacionSaldo() As Object
        If Bloqueando Then
            Return STR_OPERACION_BLOQUEAR_SALDO
        End If
        Return STR_OPERACION_DESBLOQUEAR_SALDO
    End Function

    ''' <summary>
    ''' Obtener la operacion que se envia a base de datos para los titulos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenerOperacionTitulos() As Object
        If Bloqueando Then
            Return STR_OPERACION_BLOQUEAR_TITULOS
        End If
        Return STR_OPERACION_DESBLOQUEAR_TITULOS
    End Function

    ''' <summary>
    ''' Cambiar la vista del visor de garantias
    ''' </summary>
    ''' <param name="visor"></param>
    ''' <remarks></remarks>
    Public Sub AbrirVistaDetalle(visor As VisorDeGarantias)
        Try
            If Not IsNothing(visor) Then
                logDetalle = True
                RegistroSeleccionado = visor
                GridDetalle.Visibility = Visibility.Visible
                GridPrincipal.Visibility = Visibility.Collapsed
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al abrir la vista de detalle", Me.ToString(),
                                                         "AbrirVistaDetalle", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Cerrar la vista detalle del visor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CerrarVistaDetalle()
        Try
            logDetalle = False
            GridDetalle.Visibility = Visibility.Collapsed
            GridPrincipal.Visibility = Visibility.Visible
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la ventana de detalle", Me.ToString(),
                                                         "CerrarVistaDetalle", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Evento cuando el usuario hace clic en el botón informe
    ''' </summary>
    ''' <remarks>SV20160516</remarks>
    Public Sub VerInforme()
        Try
            Dim cGenerarReporte As New WGenerarReporteView()
            AddHandler cGenerarReporte.Closed, AddressOf TerminoConfirmarGenerarReporte
            Program.Modal_OwnerMainWindowsPrincipal(cGenerarReporte)
            cGenerarReporte.ShowDialog()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar ver el informe.", Me.ToString(),
                                                         "VerInforme", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Callback cuando termina de confirmar la generación del reporte
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e">SV20160516</param>
    ''' <remarks></remarks>
    Private Sub TerminoConfirmarGenerarReporte(sender As Object, e As EventArgs)
        Try
            Dim strReporte As String = String.Empty
            Dim strParametros As String = String.Empty
            Dim cGenerarReporte As WGenerarReporteView = sender
            If Equals(cGenerarReporte.DialogResult, True) Then
                If cGenerarReporte.bitTiporesumido = True Then
                    strReporte = Application.Current.Resources(Program.REPORTE_GARANTIAS_RESUMIDO).ToString.Trim()

                Else
                    strReporte = Application.Current.Resources(Program.REPORTE_GARANTIAS_DETALLADO).ToString.Trim()
                End If

                strParametros = "&pintTipoMercado=" & _intTipoMercado _
                                & "&pstrUsuario=" & Program.Usuario


                If cGenerarReporte.bitClienteActual = True Then
                    strParametros = strParametros & "&plngIDComitente=" & _RegistroSeleccionado.IDCliente
                End If


                MostrarReporte(strParametros, Me.ToString, strReporte)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de confirmar la generación del reporte.", Me.ToString(),
                                                         "TerminoConfirmarGenerarReporte", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Procedimiento para asiganar los datos a las variable y saber al final cual es la operación que se consultó
    ''' </summary>
    ''' <param name="OperacionVisor"></param>
    ''' <remarks>SV20180411</remarks>
    Public Sub AsignarDatosOperacionActual(OperacionVisor As VisorDeGarantias)
        If Not IsNothing(OperacionVisor) Then
            IDLiquidacion = OperacionVisor.IDLiquidacion
            Parcial = OperacionVisor.Parcial
            Tipo = OperacionVisor.Tipo
            ClaseOrden = OperacionVisor.ClaseOrden
            FechaLiquidacion = OperacionVisor.FechaLiquidacion
        End If
    End Sub


#End Region

#Region "Resultados Async"
    ''' <summary>
    ''' Carga los datos al terminar de consultarlos
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoTraerOperaciones(lo As LoadOperation(Of VisorDeGarantias))
        Try

            If Not lo.HasError Then

                IsBusy = False
                ListaDatos = CType(_dcProxy.VisorDeGarantias, IEnumerable(Of VisorDeGarantias))
                ObtenerPrioridad()
                logMostrarDetallesSaldos = False
                ObtenerFiltros()
                Filtro = String.Empty

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de operaciones",
                                                 Me.ToString(), "TerminoTraerOperaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de operaciones", Me.ToString(),
                                                         "TerminoTraerOperaciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Recibe la información para actualizar en pantalla una operación que cambió
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20160522</remarks>
    Private Sub TerminoActualizarOperacion(lo As LoadOperation(Of VisorDeGarantias))
        Try
            If Not lo.HasError Then
                If Not IsNothing(lo.UserState) Then
                    Dim objOperacionActual As VisorDeGarantias
                    Dim objOperacionRecibida As VisorDeGarantias

                    objOperacionRecibida = _dcProxy2.VisorDeGarantias.FirstOrDefault

                    'Actualizar la lista de datos
                    objOperacionActual = (From c In ListaDatos Where c.IDLiquidacion = IDLiquidacion And c.Parcial = Parcial And c.Tipo = Tipo And c.ClaseOrden = ClaseOrden And c.FechaLiquidacion = FechaLiquidacion Select c).FirstOrDefault 'SV20180411

                    With objOperacionActual
                        .PorcCastigoLegal = objOperacionRecibida.PorcCastigoLegal
                        .ValorLegalRequerido = objOperacionRecibida.ValorLegalRequerido
                        .PorcCastigoInterno = objOperacionRecibida.PorcCastigoInterno
                        .ValorInternoRequerido = objOperacionRecibida.ValorInternoRequerido
                        .ValorTotalRequerido = objOperacionRecibida.ValorTotalRequerido
                        .ValorLegalAsociado = objOperacionRecibida.ValorLegalAsociado
                        .ValorInternoAsociado = objOperacionRecibida.ValorInternoAsociado
                        .ValorTotalAsociado = objOperacionRecibida.ValorTotalAsociado
                        .PorcCumplimientoLegal = objOperacionRecibida.PorcCumplimientoLegal
                        .PorcCumplimientoInterno = objOperacionRecibida.PorcCumplimientoInterno
                        .PorcAmarilloCumpLegal = objOperacionRecibida.PorcAmarilloCumpLegal
                        .PorcRojoCumpLegal = objOperacionRecibida.PorcRojoCumpLegal
                        .PorcAmarilloCumpInterno = objOperacionRecibida.PorcAmarilloCumpInterno
                        .PorcRojoCumpInterno = objOperacionRecibida.PorcRojoCumpInterno
                        .SaldoBloqueado = objOperacionRecibida.SaldoBloqueado
                        .BloqueadoTitulos = objOperacionRecibida.BloqueadoTitulos
                        .SaldoBloqueadoXLiq = objOperacionRecibida.SaldoBloqueadoXLiq
                        .BloqueadoTitulosXLiq = objOperacionRecibida.BloqueadoTitulosXLiq
                        .CoberturaTotal = objOperacionRecibida.CoberturaTotal
                        .CoberturaBolsa = objOperacionRecibida.CoberturaBolsa
                        .CoberturaValores = objOperacionRecibida.CoberturaValores
                    End With

                    'Actualizar el PagedCollection
                    objOperacionActual = (From c In ListaVisorDeGarantias Where c.IDLiquidacion = IDLiquidacion And c.Parcial = Parcial And c.Tipo = Tipo And c.ClaseOrden = ClaseOrden And c.FechaLiquidacion = FechaLiquidacion Select c).FirstOrDefault 'SV20180411

                    With objOperacionActual
                        .PorcCastigoLegal = objOperacionRecibida.PorcCastigoLegal
                        .ValorLegalRequerido = objOperacionRecibida.ValorLegalRequerido
                        .PorcCastigoInterno = objOperacionRecibida.PorcCastigoInterno
                        .ValorInternoRequerido = objOperacionRecibida.ValorInternoRequerido
                        .ValorTotalRequerido = objOperacionRecibida.ValorTotalRequerido
                        .ValorLegalAsociado = objOperacionRecibida.ValorLegalAsociado
                        .ValorInternoAsociado = objOperacionRecibida.ValorInternoAsociado
                        .ValorTotalAsociado = objOperacionRecibida.ValorTotalAsociado
                        .PorcCumplimientoLegal = objOperacionRecibida.PorcCumplimientoLegal
                        .PorcCumplimientoInterno = objOperacionRecibida.PorcCumplimientoInterno
                        .PorcAmarilloCumpLegal = objOperacionRecibida.PorcAmarilloCumpLegal
                        .PorcRojoCumpLegal = objOperacionRecibida.PorcRojoCumpLegal
                        .PorcAmarilloCumpInterno = objOperacionRecibida.PorcAmarilloCumpInterno
                        .PorcRojoCumpInterno = objOperacionRecibida.PorcRojoCumpInterno
                        .SaldoBloqueado = objOperacionRecibida.SaldoBloqueado
                        .BloqueadoTitulos = objOperacionRecibida.BloqueadoTitulos
                        .SaldoBloqueadoXLiq = objOperacionRecibida.SaldoBloqueadoXLiq
                        .BloqueadoTitulosXLiq = objOperacionRecibida.BloqueadoTitulosXLiq
                        .CoberturaTotal = objOperacionRecibida.CoberturaTotal
                        .CoberturaBolsa = objOperacionRecibida.CoberturaBolsa
                        .CoberturaValores = objOperacionRecibida.CoberturaValores

                    End With

                    ObtenerPrioridad()
                    logMostrarDetallesSaldos = False

                    RaisePropertyChanged("ListaDatos")
                    RaisePropertyChanged("ListaVisorDeGarantias")
                    RaisePropertyChanged("LiquidacionesSeleccionadas")
                    RaisePropertyChanged("LiquidacionesSeleccionadas")
                    RaisePropertyChanged("TotalSaldoBloqueadoLiq")
                    RaisePropertyChanged("TotalPortafolioBloqueadoLiq")

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención los datos de la operación actualizada",
                                                 Me.ToString(), "TerminoActualizarOperacion", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención los datos de la operación actualizada", Me.ToString(),
                                                         "TerminoActualizarOperacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Establece el estado y actualiza la informacion cuando se terminó de recuperar los títulos del cliente seleccionado
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20160505</remarks>
    Private Sub TerminoTraerTitulosGarantia(lo As LoadOperation(Of TitulosGarantia))
        Try
            If Not lo.HasError Then
                lstTitulosGarantia = _dcProxy.TitulosGarantias.ToList()
                For Each objTitulo As TitulosGarantia In lstTitulosGarantia
                    If objTitulo.Bloqueado And objTitulo.TituloEditable Then
                        objTitulo.PuedeReasignar = True
                    End If
                    objTitulo.EditarBloqueando = False
                Next
                If Bloqueando = True Or DesBloqueando = True Then
                    lstTitulosBloquearDesbloquear = Nothing
                    lstSaldosaDesbloquear = Nothing
                    NotasBloqueo = String.Empty
                    strRadicado = String.Empty
                    logSAG = True
                    ColumnaCantidadBloquear.Visibility = Visibility.Visible
                    ColumnaCantidadBloquear.Header = STR_TXTCOL_CANTIDAD_BLOQUEO

                    If Bloqueando Then
                        For Each vTitulo As TitulosGarantia In lstTitulosGarantia
                            vTitulo.BloquearDesbloqueado = Not Equals(vTitulo.Bloqueado, True) And Equals(vTitulo.TituloEditable, True) And Bloqueando
                            vTitulo.CantidadBloquear = vTitulo.Cantidad
                            vTitulo.Bloquear = False
                        Next
                    End If

                    If DesBloqueando Then
                        For Each vTitulo As TitulosGarantia In lstTitulosGarantia
                            vTitulo.DesBloquearBloqueado = Equals(vTitulo.Bloqueado, True) And Equals(vTitulo.TituloEditable, True) And DesBloqueando
                            vTitulo.CantidadBloquear = vTitulo.Cantidad
                            vTitulo.Bloquear = True
                        Next
                    End If
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de títulos para garantía",
                                                 Me.ToString(), "TerminoTraerTitulosGarantia", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
            IsBusyPortafolio = False
            IsBusy = False

            RaisePropertyTitulos()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de títulos para garantía", Me.ToString(),
                                                         "TerminoTraerTitulosGarantia", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusyPortafolio = False
        End Try
    End Sub

    ''' <summary>
    ''' Termina la carga de los saldos bloqueados asociados a la liquidación 
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20160505</remarks>
    Private Sub TerminoTraerSaldosBloqueados(lo As LoadOperation(Of SaldosBloqueados))
        Try
            If Not lo.HasError Then
                lstSaldosBloqueados = _dcProxy.SaldosBloqueados.ToList()
                If logCargaDesdeBloqueo Then
                    logMostrarDetallesSaldos = True
                    logCargaDesdeBloqueo = False
                Else
                    logMostrarDetallesSaldos = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de saldos bloqueados para la liquidación",
                                                 Me.ToString(), "TerminoTraerSaldosBloqueados", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
            IsBusySaldosBloqueados = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de saldos bloqueados para la liquidación", Me.ToString(),
                                                         "TerminoTraerSaldosBloqueados", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusySaldosBloqueados = False
        End Try
    End Sub

    ''' <summary>
    ''' Indica cuando finalizo la consulta del saldo del cliente
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20160505</remarks>
    Private Sub TerminoTraerSaldo(lo As LoadOperation(Of SaldosCliente))
        Try
            bitConsultandoSaldo = False
            If Not lo.HasError Then
                If Not IsNothing(_RegistroSeleccionado) Then
                    If strCodigoClienteBloqueo = lo.UserState Then
                        RegistroSeleccionado.SaldoCliente = _dcProxy.SaldosClientes.FirstOrDefault.SaldoCliente
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del saldo del cliente",
                                                 Me.ToString(), "TerminoTraerSaldo", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del saldo del cliente", Me.ToString(),
                                                         "TerminoTraerSaldo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Termina de cargar las listas de combos
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20160505</remarks>
    Private Sub TerminoTraerCombos(lo As LoadOperation(Of ItemComboGarantia))
        Try
            If Not lo.HasError Then
                IsBusy = False
                Dim lstResultado As IEnumerable(Of ItemComboGarantia)
                lstResultado = _dcProxy.ItemComboGarantias.ToList()

                lstTipoMercado = (From c In lstResultado Where c.Categoria = "TIPOMERCADO" Select c).ToList
                lstTipoGarantia = (From c In lstResultado Where c.Categoria = "TIPOGARANTIA" Select c).ToList

                If Not IsNothing(lstTipoMercado) AndAlso lstTipoMercado.Count = 1 Then
                    ConsultarInformacion()
                Else
                    CambioFiltroSeleccionado()
                End If

                'Se obtienen los valores de los parámetros usados en la pantalla
                For Each objParametro As ItemComboGarantia In (From c In lstResultado Where c.Categoria = "PARAMETROS" Select c).ToList
                    Select Case objParametro.Descripcion
                        Case "GARANTIAS_BLOQUEAR_RECURSOS_OTRO_CLIENTE"
                            If objParametro.Retorno = "SI" Then
                                logBloquearOtroCliente = True
                            End If
                        Case "GARANTIAS_VALIDAR_SALDO_BLOQUEO"
                            If objParametro.Retorno = "NO" Then
                                logValidarSaldoBloqueo = False
                            End If
                    End Select
                Next

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de combos de garantias.",
                                                 Me.ToString(), "TerminoTraerCombos", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de combos de garantias.", Me.ToString(),
                                                         "TerminoTraerCombos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Llamado y respuesta a los servicios para el bloqueo o desbloqueo de saldo o títulos de manera recursiva
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20160505</remarks>
    Private Sub BloquearDesbloquearResgistrosRecursivo(lo As LoadOperation(Of RespuestaValidaciones))
        Try
            Dim Accion As AccionEjecucion
            If lo.HasError = False Then
                'Si todas las respuestas son existosas
                If _dcProxy.RespuestaValidaciones.All(Function(r) r.Exitoso) Then
                    Accion = lo.UserState

                    If Accion = AccionEjecucion.DistribuirBloqueoTitulo Or Accion = AccionEjecucion.DistribuirBloqueoSaldo Then
                        FinalizarEjecutarBloqueos()
                        A2Utilidades.Mensajes.mostrarMensaje("La operación ha sido realizada exitosamente.", "Visor de garantías.")
                        IsBusy = False
                    Else


                        If Accion = AccionEjecucion.DesbloqueoSaldo AndAlso DesBloqueando AndAlso lstSaldosaDesbloquear.Any Then

                            Dim objSaldo = lstSaldosaDesbloquear.First
                            lstSaldosaDesbloquear.Remove(objSaldo)
                            IsBusy = True
                            _dcProxy.Load(_dcProxy.Garantias_Saldos_Bloquear_LiberarQuery(strCodigoClienteBloqueo,
                                                                                         Nothing,
                                                                                         objSaldo.ValorBloqueado,
                                                                                         ObtenerOperacionSaldo(),
                                                                                         DateTime.Now,
                                                                                         NotasBloqueo,
                                                                                         LiquidacionABloquear.IDLiquidacion,
                                                                                         LiquidacionABloquear.Parcial,
                                                                                         LiquidacionABloquear.Tipo,
                                                                                         LiquidacionABloquear.ClaseOrden,
                                                                                         LiquidacionABloquear.FechaLiquidacion,
                                                                                         objSaldo.TipoMercado,
                                                                                         objSaldo.SAG,
                                                                                         objSaldo.Radicado,
                                                                                        False,
                                                                                        Nothing,
                                                                                        objSaldo.IdBloqueo,
                                                                                         Program.Usuario, Program.HashConexion),
                                                                                      AddressOf BloquearDesbloquearResgistrosRecursivo, AccionEjecucion.DesbloqueoSaldo)

                        ElseIf lstTitulosBloquearDesbloquear.Any Then

                            Dim objTitulo = lstTitulosBloquearDesbloquear.First
                            lstTitulosBloquearDesbloquear.Remove(objTitulo)
                            IsBusy = True
                            _dcProxy.Load(_dcProxy.Garantias_Custodias_Bloquear_LiberarQuery(objTitulo.IDRecibo,
                                                                                             objTitulo.Secuencia,
                                                                                             Nothing,
                                                                                             NotasBloqueo,
                                                                                             objTitulo.CantidadBloquear,
                                                                                             ObtenerOperacionTitulos(),
                                                                                             objTitulo.FechaRecibo,
                                                                                             LiquidacionABloquear.IDLiquidacion,
                                                                                             LiquidacionABloquear.Parcial,
                                                                                             LiquidacionABloquear.Tipo,
                                                                                             LiquidacionABloquear.ClaseOrden,
                                                                                             LiquidacionABloquear.FechaLiquidacion,
                                                                                             intTipoMercado,
                                                                                             objTitulo.SAG,
                                                                                             objTitulo.Radicado,
                                                                                            False,
                                                                                            Nothing,
                                                                                            objTitulo.IdBloqueo,
                                                                                             Program.Usuario, Program.HashConexion),
                                                                                         AddressOf BloquearDesbloquearResgistrosRecursivo, AccionEjecucion.BloqueodesbloqueoTitulo)


                        Else
                            FinalizarEjecutarBloqueos()
                            A2Utilidades.Mensajes.mostrarMensaje("La operación ha sido realizada exitosamente.", "Visor de garantías.")
                            IsBusy = False
                        End If

                    End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, _dcProxy.RespuestaValidaciones.FirstOrDefault(Function(r) Not r.Exitoso).Mensaje, Me.ToString(), "BloquearDesbloquearResgistrosRecursivo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                    'lo.MarkErrorAsHandled()
                    IsBusy = False
                    'FinalizarEjecutarBloqueos()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el bloqueo de registros recursivo.", Me.ToString(), "BloquearDesbloquearResgistrosRecursivo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al tratar de hacer el bloqueo de registros recursivo.", Me.ToString(), "BloquearDesbloquearResgistrosRecursivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Calculos del visor"
    ''' <summary>
    ''' Asignar la prioridad a todas las filas del grupo para el ordenamiento en pantalla
    ''' </summary>
    ''' <param name="rows"></param>
    ''' <remarks></remarks>
    Public Shared Sub AsignarPrioridad(ByVal rows As IEnumerable(Of VisorDeGarantias))

        Dim estadocantidad As KeyValuePair(Of EstadosAlarma, Integer) = ObtenerEstadoCantidad(rows.ToList)

        Dim prioridad As String = CType(estadocantidad.Key, Integer).ToString()

        Dim iPrioridad As Integer = Convert.ToInt32(String.Format(STR_FORMATO_PRIORIDAD_GRUPO, prioridad, estadocantidad.Value.ToString.PadLeft(INT_LONGITUD_PRIORIDAD, "0")))

        For Each row In rows
            If row.PrioridadGrupo IsNot Nothing And row.PrioridadGrupo <> 0 Then
                row.PrioridadGrupo = Convert.ToInt64(String.Format(STR_FORMATO_PRIORIDAD_GRUPO, row.PrioridadGrupo, iPrioridad))
            Else
                row.PrioridadGrupo = iPrioridad
            End If
        Next
    End Sub

    ''' <summary>
    ''' Obtener el color a mostrar de acuerdo al estado de la alarma
    ''' </summary>
    ''' <param name="estado"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenerColorEstado(estado As EstadosAlarma) As Color
        Dim color As Color = ColorError

        Select Case estado
            Case EstadosAlarma.EstadoOk
                color = ColorOk
            Case EstadosAlarma.EstadoAlarma
                color = ColorAlarma
        End Select

        Return color
    End Function

    ''' <summary>
    ''' Realiza el caluclo y obtiene el estado interno de acuerdo a los valores del visor
    ''' </summary>
    ''' <param name="visor"></param>
    ''' <returns></returns>
    ''' <remarks>SV20160505</remarks>
    Public Shared Function ObtenerEstadoInterno(visor As VisorDeGarantias) As EstadosAlarma
        If Not IsNothing(visor) Then
            Dim estado As EstadosAlarma = ObtenerEstado(visor.ValorInternoRequerido, visor.ValorInternoAsociado,
                                                    visor.PorcCumplimientoInterno, visor.PorcAmarilloCumpInterno, visor.PorcRojoCumpInterno)

            Return estado
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Realiza el caluclo y obtiene el estado legal de acuerdo a los valores del visor
    ''' </summary>
    ''' <param name="visor"></param>
    ''' <returns></returns>
    ''' <remarks>SV20160505</remarks>
    Public Shared Function ObtenerEstadoLegal(visor As VisorDeGarantias) As EstadosAlarma
        If Not IsNothing(visor) Then
            Dim estado As EstadosAlarma = ObtenerEstado(visor.ValorLegalRequerido, visor.ValorLegalAsociado,
                                                    visor.PorcCumplimientoLegal, visor.PorcAmarilloCumpLegal, visor.PorcRojoCumpLegal)

            'visor.PuedeBloquear = True
            'visor.PuedeDesBloquear = True

            Return estado
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Obtiene el estado de acuerdo a los valores enviados
    ''' </summary>
    ''' <param name="valorAsociado"></param>
    ''' <param name="porcentajeCumplimiento"></param>
    ''' <param name="porcentajeAmarillo"></param>
    ''' <param name="porcentajeRojo"></param>
    ''' <returns></returns>
    ''' <remarks>SV20160505</remarks>
    Public Shared Function ObtenerEstado(valorRequerido As Decimal, valorAsociado As Decimal, Optional porcentajeCumplimiento As Decimal = 0, Optional porcentajeAmarillo As Decimal = 0, Optional porcentajeRojo As Decimal = 0)

        Dim estado As EstadosAlarma

        If porcentajeCumplimiento <= porcentajeRojo Then
            estado = EstadosAlarma.EstadoError
        ElseIf porcentajeCumplimiento <= porcentajeAmarillo Then
            estado = EstadosAlarma.EstadoAlarma
        Else
            estado = EstadosAlarma.EstadoOk
        End If

        Return estado
    End Function

    ''' <summary>
    ''' Obtiene el saldo de la liquidacion con los valores enviados
    ''' </summary>
    ''' <param name="estadoAlarma"></param>
    ''' <param name="valorRequerido"></param>
    ''' <param name="valorAsociado"></param>
    ''' <returns></returns>
    ''' <remarks>SV20160505</remarks>
    Public Shared Function ObtenerSaldoLiquidacion(estadoAlarma As EstadosAlarma, valorRequerido As Decimal, valorAsociado As Decimal) As String

        Dim valorFormateado As String

        If valorRequerido > valorAsociado Then
            valorFormateado = String.Format(STR_FORMATO_NEGATIVO, valorRequerido - valorAsociado)
        Else
            valorFormateado = String.Format(STR_FORMATO_POSITIVO, valorAsociado - valorRequerido)
        End If

        Return valorFormateado
    End Function


    ' ''' <summary>
    ' ''' Obtener el cálculo de la cobertura total
    ' ''' </summary>
    ' ''' <param name="visor"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Shared Function ObtenerCoberturaTotalLiquidacion(visor As VisorDeGarantias) As String
    '    Dim valorCoberturaLegal As Decimal = 0
    '    Dim valorCoberturaInterna As Decimal = 0
    '    Dim valorFormateado As String = "0"

    '    If visor.ValorLegalRequerido > visor.ValorLegalAsociado Then
    '        valorCoberturaLegal = visor.ValorLegalRequerido - visor.ValorLegalAsociado
    '    End If

    '    If visor.ValorInternoRequerido > visor.ValorInternoAsociado Then
    '        valorCoberturaInterna = visor.ValorInternoRequerido - visor.ValorInternoAsociado
    '    End If

    '    If valorCoberturaLegal > 0 Or valorCoberturaInterna > 0 Then
    '        If valorCoberturaLegal > valorCoberturaInterna Then
    '            valorFormateado = String.Format(STR_FORMATO_NEGATIVO, valorCoberturaLegal)
    '        Else
    '            valorFormateado = String.Format(STR_FORMATO_NEGATIVO, valorCoberturaInterna)
    '        End If
    '    End If

    '    Return valorFormateado
    'End Function

    ''' <summary>
    ''' Obtener el saldo interno de la liquidacion enviada
    ''' </summary>
    ''' <param name="visor"></param>
    ''' <returns></returns>
    ''' <remarks>SV20160505</remarks>
    Public Shared Function ObtenerSaldoInternoLiquidacion(visor As VisorDeGarantias) As String
        If Not IsNothing(visor) Then
            Return ObtenerSaldoLiquidacion(ObtenerEstadoInterno(visor), visor.ValorInternoRequerido, visor.ValorInternoAsociado)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Obtener el saldo legal de la liquidacion enviada
    ''' </summary>
    ''' <param name="visor"></param>
    ''' <returns></returns>
    ''' <remarks>SV20160505</remarks>
    Public Shared Function ObtenerSaldoLegalLiquidacion(visor As VisorDeGarantias) As String
        If Not IsNothing(visor) Then
            Return ObtenerSaldoLiquidacion(ObtenerEstadoLegal(visor), visor.ValorLegalRequerido, visor.ValorLegalAsociado)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Obtiene el estado y la cantidad de registros con ese estado, para los datos agrupados y lo muestra en pantalla
    ''' </summary>
    ''' <param name="listaRegistros"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ObtenerEstadoCantidad(listaRegistros As List(Of VisorDeGarantias)) As KeyValuePair(Of EstadosAlarma, Integer)
        Dim estadoAlarma As EstadosAlarma = EstadosAlarma.EstadoOk
        Dim estadoParcial As EstadosAlarma
        Dim cantidad As Integer = 0

        For Each row In listaRegistros
            estadoParcial = VisorDeGarantiasViewModel.ObtenerEstadoInterno(row)

            If estadoParcial = EstadosAlarma.EstadoError Then
                If estadoAlarma = EstadosAlarma.EstadoError Then
                    cantidad += 1
                Else
                    estadoAlarma = EstadosAlarma.EstadoError
                    cantidad = 1
                End If
            ElseIf estadoParcial = EstadosAlarma.EstadoAlarma And estadoAlarma <> EstadosAlarma.EstadoError Then
                If estadoAlarma = EstadosAlarma.EstadoAlarma Then
                    cantidad += 1
                Else
                    estadoAlarma = EstadosAlarma.EstadoAlarma
                    cantidad = 1
                End If
            ElseIf estadoParcial = EstadosAlarma.EstadoOk And estadoAlarma = EstadosAlarma.EstadoOk Then
                If estadoAlarma = EstadosAlarma.EstadoOk Then
                    cantidad += 1
                Else
                    estadoAlarma = EstadosAlarma.EstadoOk
                    cantidad = 1
                End If
            End If
        Next

        Return New KeyValuePair(Of EstadosAlarma, Integer)(estadoAlarma, cantidad)
    End Function

    ' ''' <summary>
    ' ''' Obtener el valor total bloqueado de la liquidacion
    ' ''' </summary>
    ' ''' <param name="visor"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Shared Function ObtenerValorTotalBloqueado(visor As VisorDeGarantias) As String
    '    Return String.Format(STR_FORMATO_VALOR, visor.ValorInternoAsociado)
    'End Function

    ''' <summary>
    ''' Obtener el tamaño del estado interno de la liquidacion
    ''' </summary>
    ''' <param name="visor"></param>
    ''' <returns></returns>
    ''' <remarks>SV20160505</remarks>
    Public Shared Function ObtenerTamanoEstadoInterno(visor As VisorDeGarantias) As Decimal
        If Not IsNothing(visor) Then
            Return ObtenerTamanoEstado(visor.ValorInternoRequerido, visor.ValorInternoAsociado, visor.PorcCumplimientoInterno)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Obtener el tamaño del estado legal de la liquidacion
    ''' </summary>
    ''' <param name="visor"></param>
    ''' <returns></returns>
    ''' <remarks>SV20160505</remarks>
    Public Shared Function ObtenerTamanoEstadoLegal(visor As VisorDeGarantias) As Decimal
        If Not IsNothing(visor) Then
            Return ObtenerTamanoEstado(visor.ValorLegalRequerido, visor.ValorLegalAsociado, visor.PorcCumplimientoLegal)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Obtiene el tamano del estado para los colores de acuerdo a los porcentajes
    ''' </summary>
    ''' <param name="valorRequerido"></param>
    ''' <param name="valorAsociado"></param>
    ''' <returns></returns>
    ''' <remarks>SV20160505</remarks>
    Public Shared Function ObtenerTamanoEstado(valorRequerido As Decimal, valorAsociado As Decimal, porcentajeCumplimiento As Decimal) As Decimal
        Dim valorEstado As Decimal = 0
        Dim valorMinimo As Decimal = 0
        Dim valorMaximo As Decimal = 0

        If porcentajeCumplimiento <= 100 Then
            valorMinimo = valorAsociado
            valorMaximo = valorRequerido
        Else
            valorMinimo = valorRequerido
            valorMaximo = valorAsociado
        End If

        If valorMinimo > 0 And valorMaximo > 0 Then
            valorEstado = (valorMinimo * 100) / valorMaximo
        End If

        Return valorEstado

    End Function

#End Region

    ''' <summary>
    ''' Evento para recibir cualquier cambio de los atributos de la entidad RegistroSeleccionado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SV20160515</remarks>
    Private Sub _RegistroSeleccionado_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _RegistroSeleccionado.PropertyChanged
        If e.PropertyName = "TipoGarantia" Then
            _dcProxy2.VisorDeGarantias.Clear()
            AsignarDatosOperacionActual(_RegistroSeleccionado) 'SV20180411
            _dcProxy2.Load(_dcProxy2.Garantias_Operaciones_ConsultarQuery(Nothing,
                                                                         Nothing,
                                                                         _RegistroSeleccionado.IDLiquidacion,
                                                                         _RegistroSeleccionado.Parcial,
                                                                         _RegistroSeleccionado.Tipo,
                                                                         _RegistroSeleccionado.ClaseOrden,
                                                                         _RegistroSeleccionado.FechaLiquidacion,
                                                                         _intTipoMercado,
                                                                         _RegistroSeleccionado.TipoGarantia,
                                                                         True,
                                                                         True,
                                                                         Program.Usuario, Program.HashConexion), True, AddressOf TerminoActualizarOperacion, _RegistroSeleccionado.IDLiquidacion)
        End If
    End Sub

    Private Sub RaisePropertyChanged(ByVal pstrPropiedad As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(pstrPropiedad))
    End Sub
End Class