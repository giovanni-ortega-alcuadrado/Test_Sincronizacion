Imports Telerik.Windows.Controls
' Descripción:    Child Window creado para mostrar las Ordenes Pendientes de Tesorería.
' Creado por:     Sebastian Londoño Benitez
' Fecha:          Marzo 5/2013

Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Client

Partial Public Class cwOrdenesPendientesTesoreria
    Inherits Window
    Implements INotifyPropertyChanged

    Dim dcProxy As TesoreriaDomainContext

    Public Sub New(ByVal strModuloTesoreria As String)
        Try
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me
            IsBusy = True
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New TesoreriaDomainContext()
            Else
                dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If
            If strModuloTesoreria = "N" Then strModuloTesoreria = "N%"
            dcProxy.Load(dcProxy.OrdenesPendientesTesoreria_ConsultarQuery(strModuloTesoreria,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenesPendientes, "Consultar")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "cwOrdenesPendientesTesoreria.New", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

#Region "Eventos de Controles"

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaOrdenesTesoreria As List(Of OyDTesoreria.OrdenesTesoreri)
    Public Property ListaOrdenesTesoreria() As List(Of OyDTesoreria.OrdenesTesoreri)
        Get
            Return _ListaOrdenesTesoreria
        End Get
        Set(ByVal value As List(Of OyDTesoreria.OrdenesTesoreri))
            _ListaOrdenesTesoreria = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaOrdenesTesoreria"))
        End Set
    End Property

    Private _OrdenesTesoreriaSelected As OyDTesoreria.OrdenesTesoreri
    Public Property OrdenesTesoreriaSelected() As OyDTesoreria.OrdenesTesoreri
        Get
            Return _OrdenesTesoreriaSelected
        End Get
        Set(ByVal value As OyDTesoreria.OrdenesTesoreri)
            _OrdenesTesoreriaSelected = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("OrdenesTesoreriaSelected"))
        End Set
    End Property

    Private _IsBusy As Boolean = False
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoTraerOrdenesPendientes(ByVal lo As LoadOperation(Of OyDTesoreria.OrdenesTesoreri))
        IsBusy = False
        If Not lo.HasError Then
            ListaOrdenesTesoreria = dcProxy.OrdenesTesoreris.ToList
            If ListaOrdenesTesoreria.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No se encontraron órdenes de tesorería pendientes", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes Pendientes", _
                                             Me.ToString(), "TerminoTraerOrdenesPendientes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
