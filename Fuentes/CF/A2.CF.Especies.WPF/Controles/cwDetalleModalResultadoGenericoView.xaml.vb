Imports Telerik.Windows.Controls

Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies


Partial Public Class cwDetalleModalResultadoGenericoView
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Variables"

#End Region

#Region "Propiedades"

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

    Private _ListaDetalle As List(Of ResultadoGenerico)
    Public Property ListaDetalle() As List(Of ResultadoGenerico)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of ResultadoGenerico))
            _ListaDetallePaginada = Nothing
            _ListaDetalle = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaDetalle"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaDetallePaginada"))
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de ProcesarPortafolio para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaDetallePaginada As PagedCollectionView = Nothing
    Public ReadOnly Property ListaDetallePaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalle) Then
                If IsNothing(_ListaDetallePaginada) Then
                    Dim view = New PagedCollectionView(_ListaDetalle)
                    _ListaDetallePaginada = view
                    Return view
                Else
                    Return (_ListaDetallePaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para cobro de utilidades (estilos y consulta de los registros)
    ''' </summary>
    Public Sub New(ByVal TituloVentanaModal As String, ByVal strDetalle As String, ByVal strDescripcion As String)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            'Me.DataContext = mobjVM
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me
            Dim ResultadoGenericosVM As List(Of ResultadoGenerico)
            ResultadoGenericosVM = New List(Of ResultadoGenerico)

            Dim Item As ResultadoGenerico
            Item = New ResultadoGenerico()

            Item.strDetalle = strDetalle
            Item.strDescripcion = strDescripcion


            ResultadoGenericosVM.Add(Item)

           

            Title = TituloVentanaModal

            ListaDetalle = ResultadoGenericosVM
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al asignar los datos a la lista de la ventana modal.", Me.ToString(), "ConsultarResultadoGenerico", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

#End Region

#Region "Métodos para control de eventos"

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            'DetalleSeleccionado.dblValorPagoAdicional = dblValorPagoAdicional
            'DetalleSeleccionado.logPagado = logPagado

            'mobjVM.DetalleSeleccionado = DetalleSeleccionado

            Me.DialogResult = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.Close()
            'Me.DialogResult = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#Region "Manejadores error"

    Private Sub dgGrid_BindingValidationError(sender As Object, e As ValidationErrorEventArgs)
        Try
            ' Control de error del bindding del grid
            If Not e.Error.Exception Is Nothing Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema para presentar los datos del detalle.", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, e.Error.Exception)
            End If
            e.Handled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga de los datos", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class

Public Class ResultadoGenerico
    Private _strDetalle As String
    Public Property strDetalle() As String
        Get
            Return _strDetalle
        End Get
        Set(value As String)
            _strDetalle = value
        End Set
    End Property

    Private _strDescripcion As String
    Public Property strDescripcion() As String
        Get
            Return _strDescripcion
        End Get
        Set(value As String)
            _strDescripcion = value
        End Set
    End Property
End Class

