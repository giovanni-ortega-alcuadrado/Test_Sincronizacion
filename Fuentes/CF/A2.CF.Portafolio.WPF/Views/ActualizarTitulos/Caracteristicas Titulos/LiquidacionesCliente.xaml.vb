Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio

''' <summary>
''' Clase para el manejo de la ventana modal de Liquidaciones de clientes.
''' </summary>
''' <remarks></remarks>
''' <history>
''' Creado por       : Juan Carlos Soto Cruz.
''' Descripción      : Creacion.
''' Fecha            : Marzo 07/2013
''' Pruebas CB       : Juan Carlos Soto Cruz - Marzo 07/2013 - Resultado Ok 
''' </history>
Partial Public Class LiquidacionesCliente
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Declaraciones"
    Dim dcProxy As PortafolioDomainContext
#End Region

#Region "Inicializacion"
    Public Sub New(ByVal lngIdComitente As String, ByVal strIdEspecie As String)
        InitializeComponent()
        Me.LiquidacionesCliente.DataContext = Me
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New PortafolioDomainContext()
        Else
            dcProxy = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.TraerLiquidacionesClienteConsultarQuery(lngIdComitente, strIdEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesCliente, "FiltroInicial")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "LiquidacionesCliente.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Eventos"
    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub
#End Region

#Region "Eventos Asincronicos"
    Private Sub TerminoTraerLiquidacionesCliente(ByVal lo As LoadOperation(Of CFPortafolio.LiquidacionesCliente))
        If Not lo.HasError Then
            ListaLiquidacionesCliente = dcProxy.LiquidacionesClientes.ToList
            IsBusy = False
            If ListaLiquidacionesCliente.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de la lista de LiquidacionesClientes", _
                                             Me.ToString(), "TerminoTraerLiquidacionesCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End If

    End Sub
#End Region

#Region "Propiedades"

    Private _IsBusy As Boolean
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Private _ListaLiquidacionesCliente As List(Of CFPortafolio.LiquidacionesCliente)
    Public Property ListaLiquidacionesCliente As List(Of CFPortafolio.LiquidacionesCliente)
        Get
            Return _ListaLiquidacionesCliente
        End Get
        Set(value As List(Of CFPortafolio.LiquidacionesCliente))
            _ListaLiquidacionesCliente = value
            If Not IsNothing(value) Then
                ListaLiquidacionesClienteSelected = _ListaLiquidacionesCliente.FirstOrDefault
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaLiquidacionesCliente"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaLiquidacionesClientePaged"))
        End Set
    End Property

    Private _ListaLiquidacionesClienteSelected As CFPortafolio.LiquidacionesCliente
    Public Property ListaLiquidacionesClienteSelected As CFPortafolio.LiquidacionesCliente
        Get
            Return _ListaLiquidacionesClienteSelected
        End Get
        Set(value As CFPortafolio.LiquidacionesCliente)
            _ListaLiquidacionesClienteSelected = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaLiquidacionesClienteSelected"))
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
#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
