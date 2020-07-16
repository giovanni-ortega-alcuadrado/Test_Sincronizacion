Imports System.ComponentModel
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client

Public Class BuscadorGenericoViewModel
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

#Region "Constantes"

    Public Enum EstadosItem As Byte
        T '// Todos
        A '// Activos
        I '// Inactivos
    End Enum

#End Region

#Region "Variables"
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- DEFINICIÓN DE VARIABLES
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Private mlogMostrarMensajeLog As Boolean = False
    Public mdcProxy As UtilidadesDomainContext

#End Region

#Region "Propiedades"

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    ' PROPIEDADES
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Private _mstrIdItem As String = String.Empty
    Public Property IdItem() As String
        Get
            Return (_mstrIdItem)
        End Get
        Set(ByVal value As String)
            _mstrIdItem = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdItem"))

            If Not value.Equals(String.Empty) And Not _mstrTipoItem.Equals(String.Empty) Then
                _mlogBuscandoItem = True
                Activar = True

                seleccionarItem(value)
            End If
        End Set
    End Property

    Private _mstrEstadoItem As EstadosItem = EstadosItem.T '// Por defecto consultar sobre todos los items
    Public Property EstadoItem() As EstadosItem
        Get
            Return (_mstrEstadoItem)
        End Get
        Set(ByVal value As EstadosItem)
            _mstrEstadoItem = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EstadoItem"))
        End Set
    End Property

    Private _mstrTipoItem As String = ""
    Public Property TipoItem() As String
        Get
            Return (_mstrTipoItem)
        End Get
        Set(ByVal value As String)
            _mstrTipoItem = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoItem"))
        End Set
    End Property

    Private _mstrAgrupamiento As String = "" '// Por defecto consultar sobre todos los items
    Public Property Agrupamiento() As String
        Get
            Return (_mstrAgrupamiento)
        End Get
        Set(ByVal value As String)
            _mstrAgrupamiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Agrupamiento"))
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>SV20160203</remarks>
    Private _mstrCondicion1 As String = ""
    Public Property Condicion1() As String
        Get
            Return (_mstrCondicion1)
        End Get
        Set(ByVal value As String)
            _mstrCondicion1 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Condicion1"))
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>SV20160203</remarks>
    Private _mstrCondicion2 As String = ""
    Public Property Condicion2() As String
        Get
            Return (_mstrCondicion2)
        End Get
        Set(ByVal value As String)
            _mstrCondicion2 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Condicion2"))
        End Set
    End Property

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

    Private _ListaBusquedaControl As List(Of clsGenerico_Buscador)
    Public Property ListaBusquedaControl() As List(Of clsGenerico_Buscador)
        Get
            Return _ListaBusquedaControl
        End Get
        Set(ByVal value As List(Of clsGenerico_Buscador))
            _ListaBusquedaControl = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaBusquedaControl"))
        End Set
    End Property

    Private _ItemSeleccionadoBuscador As clsGenerico_Buscador
    Public Property ItemSeleccionadoBuscador() As clsGenerico_Buscador
        Get
            Return _ItemSeleccionadoBuscador
        End Get
        Set(ByVal value As clsGenerico_Buscador)
            _ItemSeleccionadoBuscador = value
            'le asigna valor al comitente seleccionado el cual controla el resto del codigo
            If Not IsNothing(value) Then
                ItemSeleccionado = value.ItemBusqueda
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ItemSeleccionadoBuscador"))
        End Set
    End Property

    Private _mobjItems As List(Of OYDUtilidades.BuscadorGenerico) = Nothing
    Public Property Items() As List(Of OYDUtilidades.BuscadorGenerico)
        Get
            Return (_mobjItems)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorGenerico))
            _mobjItems = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Items"))
        End Set
    End Property

    Private _mobjItemSeleccionado As OYDUtilidades.BuscadorGenerico = Nothing
    Public Property ItemSeleccionado() As OYDUtilidades.BuscadorGenerico
        Get
            Return (_mobjItemSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorGenerico)
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
                mdcProxy = New UtilidadesDomainContext()
            Else
                mdcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
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

    Private Sub buscarItemsComplete(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
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

                Dim objListaRespuesta As New List(Of OYDUtilidades.BuscadorGenerico)
                Dim objListaBuscador As New List(Of clsGenerico_Buscador)

                If Not IsNothing(Items) Then
                    For Each li In Items
                        objListaRespuesta.Add(li)
                    Next
                End If

                For Each li In objListaRespuesta
                    objListaBuscador.Add(New clsGenerico_Buscador With {
                    .ItemBusqueda = li,
                    .DescripcionBuscador = LTrim(RTrim(li.IdItem)) + "-" + LTrim(RTrim(li.Nombre))
                    })
                Next

                ListaBusquedaControl = objListaBuscador

                '// Si se está buscando un item específico y no estaba en la lista previamente cargada se busca en la lista actualizada
                If _mlogBuscandoItem Then
                    _mlogBuscandoItem = False '// Desactivar la busqueda del item
                    logBusquedaEspecifica = True

                    Me.seleccionarItem(Me.IdItem)
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
                mdcProxy.BuscadorGenericos.Clear()
                Dim TextoSeguro As String = System.Web.HttpUtility.UrlEncode(Me.CondicionFiltro)
                mdcProxy.Load(mdcProxy.buscarItemsQuery(TextoSeguro, Me.TipoItem, Me.EstadoItem.ToString, Me.Agrupamiento, Me.Condicion1, Me.Condicion2, Program.Usuario, Program.HashConexion), AddressOf buscarItemsComplete, "") 'SV20160203
            End If
        Catch ex As Exception
            MostrarConsultando = Visibility.Collapsed
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de Items", Me.ToString(), "consultarItems", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
            Activar = False
        End Try
    End Sub

#End Region

#Region "Métodos privados"

    Private Sub seleccionarItem(ByVal pstrIdItem As String)
        Dim objRes As OYDUtilidades.BuscadorGenerico = Nothing

        Try
            If pstrIdItem.Equals(String.Empty) Then
                ItemSeleccionado = Nothing
            Else
                If Not Items Is Nothing Then
                    If (From obj In Items Where obj.IdItem = pstrIdItem Select obj).ToList.Count > 0 Then
                        objRes = (From obj In Items Where obj.IdItem = pstrIdItem Select obj).ToList.ElementAt(0)
                    Else
                        objRes = Nothing
                    End If
                End If

                '// mlogBuscandoItem se inicializa en true cuando se asigna un valor a la propiedad IdItem
                If objRes Is Nothing And _mlogBuscandoItem Then
                    '// Buscar un item específico
                    Activar = False
                    MostrarConsultando = Visibility.Visible
                    mdcProxy.BuscadorGenericos.Clear()
                    mdcProxy.Load(mdcProxy.buscarItemsQuery(pstrIdItem, Me.TipoItem, EstadosItem.T.ToString, "IdItem", "", "", Program.Usuario, Program.HashConexion), AddressOf buscarItemsComplete, "")
                Else
                    Activar = False
                    _mlogBuscandoItem = False

                    ItemSeleccionado = objRes
                End If
            End If
        Catch ex As Exception
            ItemSeleccionado = Nothing
            Activar = False
            _mlogBuscandoItem = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el item seleccionado", Me.ToString(), "seleccionarItem", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
            MostrarConsultando = Visibility.Collapsed
        End Try
    End Sub

#End Region

End Class
