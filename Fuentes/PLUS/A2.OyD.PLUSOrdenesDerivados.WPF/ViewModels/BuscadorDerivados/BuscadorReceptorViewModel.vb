Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client


Public Class BuscadorReceptorViewModel
    Implements INotifyPropertyChanged

#Region "Eventos"
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- DEFINICIÓN DE EVENTOS
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- Indicar cualquier cambio en una propiedad del objeto
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- Indicar cuando finaliza el servicio que carga la lista de items
    Public Event CargaItemsCompleta(ByVal plogNroItems As Integer, ByVal plogBusquedaItemEspecifico As Boolean)
#End Region

#Region "Variables"
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- DEFINICIÓN DE VARIABLES
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Private mlogMostrarMensajeLog As Boolean = False
    Public mdcProxy As OYDPLUSOrdenesDerivadosDomainContext

#End Region

#Region "Propiedades"

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    ' PROPIEDADES
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Private _mstrCondicionFiltro As String = ""
    Public Property CondicionFiltro() As String
        Get
            Return (_mstrCondicionFiltro)
        End Get
        Set(ByVal value As String)
            _mstrCondicionFiltro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CondicionFiltro"))
        End Set
    End Property

    Private _mobjItems As List(Of OyDPLUSOrdenesDerivados.ReceptoresBusqueda) = Nothing
    Public Property Items() As List(Of OyDPLUSOrdenesDerivados.ReceptoresBusqueda)
        Get
            Return (_mobjItems)
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesDerivados.ReceptoresBusqueda))
            _mobjItems = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Items"))
        End Set
    End Property

    Private _mobjItemSeleccionado As OyDPLUSOrdenesDerivados.ReceptoresBusqueda = Nothing
    Public Property ItemSeleccionado() As OyDPLUSOrdenesDerivados.ReceptoresBusqueda
        Get
            Return (_mobjItemSeleccionado)
        End Get
        Set(ByVal value As OyDPLUSOrdenesDerivados.ReceptoresBusqueda)
            _mobjItemSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ItemSeleccionado"))
        End Set
    End Property

    Private _mlogActivar As Boolean = False
    Public Property Activar As Boolean
        Get
            Return (_mlogActivar)
        End Get
        Set(ByVal value As Boolean)
            _mlogActivar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Activar"))
        End Set
    End Property

    Private _mlogBuscandoItem As Boolean = False
    Public Property Buscando As Boolean
        Get
            Return (_mlogBuscandoItem)
        End Get
        Set(ByVal value As Boolean)
            _mlogBuscandoItem = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Buscando"))
        End Set
    End Property

    Private _mlogInicializado As Boolean = False
    Public Property Inicializado As Boolean
        Get
            Return (_mlogInicializado)
        End Get
        Set(ByVal value As Boolean)
            _mlogInicializado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Inicializado"))
        End Set
    End Property

    Private _MostrarConsultando As Visibility = Visibility.Collapsed
    Public Property MostrarConsultando() As Visibility
        Get
            Return _MostrarConsultando
        End Get
        Set(ByVal value As Visibility)
            _MostrarConsultando = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarConsultando"))
        End Set
    End Property

#End Region

#Region "Inicializaciones"
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- PROCESOS DE INICIALIZACIÓN
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Public Sub New()

        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxy = New OYDPLUSOrdenesDerivadosDomainContext()
            Else
                mdcProxy = New OYDPLUSOrdenesDerivadosDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If

            '-- Inicializar servicios
            inicializarServicios()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del control", Me.ToString(), "New", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try

    End Sub

    ''' <summary>
    ''' Inicializa los proxies para acceder a los servicios web y configura los manejadores de evento de los diferentes métodos asincrónicos disponibles
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub inicializarServicios()
        Try
            mlogMostrarMensajeLog = CBool(IIf(Program.MostrarMensajeLog.ToUpper = "S", True, False))
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del control", Me.ToString(), "New", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos respuesta de servicios"

    Private Sub buscarItemsComplete(ByVal lo As LoadOperation(Of OyDPLUSOrdenesDerivados.ReceptoresBusqueda))
        Dim logBusquedaEspecifica As Boolean = False

        Try
            If lo.HasError Then
                If Not lo.Error Is Nothing Then
                    Throw New Exception(lo.Error.Message, lo.Error.InnerException)
                Else
                    Throw New Exception("Se presentó un error al ejecutar la consulta de Items pero no se recibió detalle del problema generado")
                End If
            Else
                _mlogInicializado = True

                Items = lo.Entities.ToList

                '// Si se está buscando un item específico y no estaba en la lista previamente cargada se busca en la lista actualizada
                If _mlogBuscandoItem Then
                    _mlogBuscandoItem = False '// Desactivar la busqueda del item
                    logBusquedaEspecifica = True

                    Activar = False
                End If

                RaiseEvent CargaItemsCompleta(_mobjItems.Count, logBusquedaEspecifica)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la consulta de Items", Me.ToString(), "buscarItemsComplete", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        Finally
            Activar = False
        End Try
        MostrarConsultando = Visibility.Collapsed
        mdcProxy.RejectChanges()
    End Sub

#End Region

#Region "Métodos públicos"

    Friend Sub consultarItems()
        Try
            'Modificado por Juan David Correa.
            'Se adiciona la condición para no consultar cuando ya se este consultando datos.
            If MostrarConsultando = Visibility.Collapsed Then
                Activar = True
                MostrarConsultando = Visibility.Visible
                mdcProxy.ReceptoresBusquedas.Clear()
                Dim TextoSeguro As String = System.Web.HttpUtility.HtmlEncode(Me.CondicionFiltro)
                mdcProxy.Load(mdcProxy.OYDPLUS_BuscarReceptoresDerivadosQuery(TextoSeguro, "BUSQUEDARECEPTORES", "OYD", "DERIVADOS", "RECEPTORES", String.Empty, String.Empty, Program.UsuarioWindows, Program.HashConexion), AddressOf buscarItemsComplete, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de Items", Me.ToString(), "consultarItems", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
            Activar = False
        End Try
    End Sub

#End Region

End Class
