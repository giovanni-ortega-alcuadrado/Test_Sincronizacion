Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports OpenRiaServices.DomainServices.Client

Public Class ConsultarLiquidacionesClienteViewModel
    Implements INotifyPropertyChanged
    Private objProxyUtils As UtilidadesDomainContext
    Private dcProxy As OyDPLUSutilidadesDomainContext
    Dim plogCalcular As Boolean = True
#Region "Inicialización"

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSutilidadesDomainContext()
                objProxyUtils = New UtilidadesDomainContext()
            Else
                dcProxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
                objProxyUtils = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OyDPLUSutilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 500)

            objProxyUtils.Load(objProxyUtils.cargarCombosEspecificosQuery("LIQUIDACIONESCLIENTE", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, String.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConsultarLiquidacionesClienteViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _IsBusy As Boolean
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Private _ListaLiquidacionesCliente As List(Of OYDPLUSUtilidades.tblLiquidacionesCliente)
    Public Property ListaLiquidacionesCliente() As List(Of OYDPLUSUtilidades.tblLiquidacionesCliente)
        Get
            Return _ListaLiquidacionesCliente
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblLiquidacionesCliente))
            _ListaLiquidacionesCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaLiquidacionesCliente"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaLiquidacionesClientePaged"))
        End Set
    End Property

    Public ReadOnly Property ListaLiquidacionesClientePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLiquidacionesCliente) Then
                Dim view = New PagedCollectionView(_ListaLiquidacionesCliente)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _LiquidacionSeleccionada As OYDPLUSUtilidades.tblLiquidacionesCliente
    Public Property LiquidacionSeleccionada() As OYDPLUSUtilidades.tblLiquidacionesCliente
        Get
            Return _LiquidacionSeleccionada
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblLiquidacionesCliente)
            _LiquidacionSeleccionada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LiquidacionSeleccionada"))
        End Set
    End Property

    Private _ListaTipoOperacion As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTipoOperacion() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTipoOperacion
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaTipoOperacion"))
        End Set
    End Property

    Private _Cliente As String
    Public Property Cliente() As String
        Get
            Return _Cliente
        End Get
        Set(ByVal value As String)
            _Cliente = value
            If Not IsNothing(ViewLiquidacionesCliente) Then
                ViewLiquidacionesCliente.ClienteSeleccionado = _Cliente
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Cliente"))
        End Set
    End Property

    Private _TipoOperacion As String
    Public Property TipoOperacion() As String
        Get
            Return _TipoOperacion
        End Get
        Set(ByVal value As String)
            _TipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOperacion"))
        End Set
    End Property

    Private _FechaInicial As DateTime = Now
    Public Property FechaInicial() As DateTime
        Get
            Return _FechaInicial
        End Get
        Set(ByVal value As DateTime)
            _FechaInicial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaInicial"))
        End Set
    End Property

    Private _FechaFinal As DateTime = Now
    Public Property FechaFinal() As DateTime
        Get
            Return _FechaFinal
        End Get
        Set(ByVal value As DateTime)
            _FechaFinal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaFinal"))
        End Set
    End Property

    Private _TotalLiquidacion As Double
    Public Property TotalLiquidacion() As Double
        Get
            Return _TotalLiquidacion
        End Get
        Set(ByVal value As Double)
            _TotalLiquidacion = value
            If Not IsNothing(ViewLiquidacionesCliente) Then
                ViewLiquidacionesCliente.ValorTotalLiquidaciones = _TotalLiquidacion
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalLiquidacion"))
        End Set
    End Property

    Private _TotalLiquidacionSeleccionada As Double
    Public Property TotalLiquidacionSeleccionada() As Double
        Get
            Return _TotalLiquidacionSeleccionada
        End Get
        Set(ByVal value As Double)
            _TotalLiquidacionSeleccionada = value
            If Not IsNothing(ViewLiquidacionesCliente) Then
                ViewLiquidacionesCliente.ValorTotalLiquidacionesSeleccionadas = _TotalLiquidacionSeleccionada
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalLiquidacionSeleccionada"))
        End Set
    End Property

    Private _HabilitarBuscadorCliente As Boolean = True
    Public Property HabilitarBuscadorCliente() As Boolean
        Get
            Return _HabilitarBuscadorCliente
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBuscadorCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarBuscadorCliente"))
        End Set
    End Property

    Private _HabilitarTipoOperacion As Boolean = True
    Public Property HabilitarTipoOperacion() As Boolean
        Get
            Return _HabilitarTipoOperacion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarTipoOperacion"))
        End Set
    End Property

    Private _ViewLiquidacionesCliente As ConsultarLiquidacionesClienteView
    Public Property ViewLiquidacionesCliente() As ConsultarLiquidacionesClienteView
        Get
            Return _ViewLiquidacionesCliente
        End Get
        Set(ByVal value As ConsultarLiquidacionesClienteView)
            _ViewLiquidacionesCliente = value
        End Set
    End Property

    Private _SeleccionarTodos As Boolean
    Public Property SeleccionarTodos() As Boolean
        Get
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodos = value
            plogCalcular = False
            'Valida sí se habilito la opción de seleccionar todos.
            If _SeleccionarTodos Then
                For Each li In ListaLiquidacionesCliente
                    If li.Seleccione = False Then li.Seleccione = True
                Next
            Else
                For Each li In ListaLiquidacionesCliente
                    li.Seleccione = False
                Next
            End If
            plogCalcular = True
            CalcularValorLiquidaciones()
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarTipoOperacion"))
        End Set
    End Property

    Private _LiquidacionesSeleccionadas As String
    Public Property LiquidacionesSeleccionadas() As String
        Get
            Return _LiquidacionesSeleccionadas
        End Get
        Set(ByVal value As String)
            _LiquidacionesSeleccionadas = value
            If Not IsNothing(ViewLiquidacionesCliente) Then
                ViewLiquidacionesCliente.LiquidacionesSeleccionadas = _LiquidacionesSeleccionadas
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LiquidacionesSeleccionadas"))
        End Set
    End Property

    Private _ListaLiquidacionesSeleccionadas As List(Of OYDPLUSUtilidades.tblLiquidacionesCliente)
    Public Property ListaLiquidacionesSeleccionadas() As List(Of OYDPLUSUtilidades.tblLiquidacionesCliente)
        Get
            Return _ListaLiquidacionesSeleccionadas
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblLiquidacionesCliente))
            _ListaLiquidacionesSeleccionadas = value
            If Not IsNothing(ViewLiquidacionesCliente) Then
                ViewLiquidacionesCliente.ListaLiquidacionesSeleccionadas = _ListaLiquidacionesSeleccionadas
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaLiquidacionesSeleccionadas"))
        End Set
    End Property


#End Region

#Region "Metodos"

    Public Sub ConsultarLiquidacionesCliente()
        Try
            If String.IsNullOrEmpty(_Cliente) Then
                mostrarMensaje("Debe de seleccionar un cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(_TipoOperacion) Then
                mostrarMensaje("Debe de seleccionar el tipo de operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            If Not IsNothing(dcProxy.tblLiquidacionesClientes) Then
                dcProxy.tblLiquidacionesClientes.Clear()
            End If

            dcProxy.Load(dcProxy.OYDPLUS_ConsultarLiquidacionesClienteQuery(_Cliente, _TipoOperacion, _FechaInicial, _FechaFinal, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarLiquidaciones, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar el estado del documento.", Me.ToString(), "ConsultarEstadoDocumento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub CalcularValorLiquidaciones()
        Try
            If Not IsNothing(ListaLiquidacionesCliente) Then
                Dim dblValorTotal As Double = 0
                Dim dblValorSeleccionada As Double = 0
                Dim objListaLiquidacionesSeleccionadas As New List(Of OYDPLUSUtilidades.tblLiquidacionesCliente)
                Dim objLiquidaciones As String = String.Empty
                _LiquidacionesSeleccionadas = String.Empty

                'Suma los valores de las liquidaciones seleccionadas.
                For Each li In ListaLiquidacionesCliente
                    dblValorTotal += li.TotalLiquidacion

                    If li.Seleccione Then
                        dblValorSeleccionada += li.TotalLiquidacion

                        If objListaLiquidacionesSeleccionadas.Where(Function(i) i.NroLiquidacion = li.NroLiquidacion And i.Parcial = li.Parcial).Count = 0 Then
                            objListaLiquidacionesSeleccionadas.Add(li)
                        End If

                    End If
                Next

                ListaLiquidacionesSeleccionadas = objListaLiquidacionesSeleccionadas

                For Each li In _ListaLiquidacionesSeleccionadas
                    If String.IsNullOrEmpty(objLiquidaciones) Then
                        objLiquidaciones = String.Format("NroLiq:{0}-Parcial:{1}", li.NroLiquidacion, li.Parcial)
                    Else
                        objLiquidaciones = String.Format("{0}---NroLiq:{1}-Parcial:{2}", objLiquidaciones, li.NroLiquidacion, li.Parcial)
                    End If
                Next

                LiquidacionesSeleccionadas = objLiquidaciones

                TotalLiquidacion = dblValorTotal
                TotalLiquidacionSeleccionada = dblValorSeleccionada

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al calcular los totales de las liquidaciones del cliente.", Me.ToString(), "CalcularValorLiquidaciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Resultados asincronicos"

    Private Sub TerminoConsultarLiquidaciones(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblLiquidacionesCliente))
        Try
            If lo.HasError = False Then
                ListaLiquidacionesCliente = lo.Entities.ToList

                If ListaLiquidacionesCliente.Count = 0 Then
                    mostrarMensaje("No se encontraron registros con los filtros seleccionados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    CalcularValorLiquidaciones()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recebir la consultar las liquidaciones del cliente.", Me.ToString(), "TerminoConsultarLiquidaciones", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recebir la consultar las liquidaciones del cliente.", Me.ToString(), "TerminoConsultarLiquidaciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoCargarCombosEspecificos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If lo.HasError = False Then

                If lo.Entities.Count > 0 Then
                    If CType(lo.Entities.ToList, List(Of OYDUtilidades.ItemCombo)).Where(Function(i) i.Categoria = "TIPOSOPERACION").Count > 0 Then
                        ListaTipoOperacion = CType(lo.Entities.ToList, List(Of OYDUtilidades.ItemCombo)).Where(Function(i) i.Categoria = "TIPOSOPERACION").ToList

                        If HabilitarTipoOperacion = False Then
                            Dim objTipoOperacion As String = TipoOperacion
                            TipoOperacion = String.Empty
                            TipoOperacion = objTipoOperacion
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar los combos especificos.", Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar los combos especificos.", Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos"

    Private Sub _LiquidacionSeleccionadaChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _LiquidacionSeleccionada.PropertyChanged
        Try
            If e.PropertyName.ToUpper = "SELECCIONE" Then
                If plogCalcular Then
                    CalcularValorLiquidaciones()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar al realizar el cambio de propiedad.", Me.ToString(), "_LiquidacionSeleccionadaChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged



End Class


