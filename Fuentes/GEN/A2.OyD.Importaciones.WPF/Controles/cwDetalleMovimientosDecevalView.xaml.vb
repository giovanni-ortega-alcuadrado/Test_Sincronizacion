Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones

Partial Public Class cwDetalleMovimientosDecevalView
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Variables"

    Private mobjVM As cwMovimientosDecevalView

#End Region

#Region "Propiedades"

    Private _logLineaSeleccionada As System.Nullable(Of System.Boolean)
    Public Property logLineaSeleccionada() As System.Nullable(Of System.Boolean)
        Get
            Return _logLineaSeleccionada
        End Get
        Set(ByVal value As System.Nullable(Of System.Boolean))
            _logLineaSeleccionada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logLineaSeleccionada"))
        End Set
    End Property

    Private _intLineaArchivo As System.Nullable(Of Integer)
    Public Property intLineaArchivo() As System.Nullable(Of Integer)
        Get
            Return _intLineaArchivo
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intLineaArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intLineaArchivo"))
        End Set
    End Property

    Private _strIDEspecie As String
    Public Property strIDEspecie() As String
        Get
            Return _strIDEspecie
        End Get
        Set(ByVal value As String)
            _strIDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIDEspecie"))
        End Set
    End Property

    Private _strISIN As String
    Public Property strISIN() As String
        Get
            Return _strISIN
        End Get
        Set(ByVal value As String)
            _strISIN = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strISIN"))
        End Set
    End Property

    Private _lngIDFungible As System.Nullable(Of Integer)
    Public Property lngIDFungible() As System.Nullable(Of Integer)
        Get
            Return _lngIDFungible
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _lngIDFungible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDFungible"))
        End Set
    End Property

    Private _dblNroCuenta As System.Nullable(Of Double)
    Public Property dblNroCuenta() As System.Nullable(Of Double)
        Get
            Return _dblNroCuenta
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblNroCuenta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblNroCuenta"))
        End Set
    End Property

    Private _lngIDComitente As String
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDComitente"))
        End Set
    End Property

    Private _strNroDocumento As String
    Public Property strNroDocumento() As String
        Get
            Return _strNroDocumento
        End Get
        Set(ByVal value As String)
            _strNroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNroDocumento"))
        End Set
    End Property

    Private _strNombre As String
    Public Property strNombre() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombre"))
        End Set
    End Property

    Private _dtmMovimiento As System.Nullable(Of System.DateTime)
    Public Property dtmMovimiento() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmMovimiento
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmMovimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmMovimiento"))
        End Set
    End Property

    Private _dblCantidad As System.Nullable(Of Double)
    Public Property dblCantidad() As System.Nullable(Of Double)
        Get
            Return _dblCantidad
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblCantidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblCantidad"))
        End Set
    End Property

    Private _dblVlrValorizado As System.Nullable(Of Double)
    Public Property dblVlrValorizado() As System.Nullable(Of Double)
        Get
            Return _dblVlrValorizado
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblVlrValorizado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblVlrValorizado"))
        End Set
    End Property

    Private _strDescripcionMovimiento As String
    Public Property strDescripcionMovimiento() As String
        Get
            Return _strDescripcionMovimiento
        End Get
        Set(ByVal value As String)
            _strDescripcionMovimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionMovimiento"))
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As TitulosMovimientos
    Public Property DetalleSeleccionado() As TitulosMovimientos
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As TitulosMovimientos)
            _DetalleSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DetalleSeleccionado"))
        End Set
    End Property

    Private WithEvents _ListaClientesActivosNoBloqueados As List(Of OyDImportaciones.ClientesActivosNoBloqueados)
    Public Property ListaClientesActivosNoBloqueados() As List(Of OyDImportaciones.ClientesActivosNoBloqueados)
        Get
            Return _ListaClientesActivosNoBloqueados
        End Get
        Set(ByVal value As List(Of OyDImportaciones.ClientesActivosNoBloqueados))
            _ListaClientesActivosNoBloqueados = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaClientesActivosNoBloqueados"))
        End Set
    End Property

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para cobro de utilidades (estilos y consulta de los registros)
    ''' </summary>
    Public Sub New(ByVal pmobjVM As cwMovimientosDecevalView, ByVal DetalleSeleccionadoVM As TitulosMovimientos, ByVal HabilitarEdicionDetalleVM As Boolean, ByVal ListaClientesActivosNoBloqueadosVM As List(Of OyDImportaciones.ClientesActivosNoBloqueados))
        Try
            CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            'inicializar()

            'Me.DataContext = mobjVM
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me

            mobjVM = pmobjVM
            DetalleSeleccionado = DetalleSeleccionadoVM
            ListaClientesActivosNoBloqueados = ListaClientesActivosNoBloqueadosVM

            intLineaArchivo = DetalleSeleccionado.intLineaArchivo
            strIDEspecie = DetalleSeleccionado.strIDEspecie
            strISIN = DetalleSeleccionado.strISIN
            lngIDFungible = DetalleSeleccionado.lngIDFungible
            dblNroCuenta = DetalleSeleccionado.dblNroCuenta
            lngIDComitente = DetalleSeleccionado.lngIDComitente
            strNroDocumento = DetalleSeleccionado.strNroDocumento
            strNombre = DetalleSeleccionado.strNombre
            dtmMovimiento = DetalleSeleccionado.dtmMovimiento
            dblCantidad = DetalleSeleccionado.dblCantidad
            dblVlrValorizado = DetalleSeleccionado.dblVlrValorizado
            strDescripcionMovimiento = DetalleSeleccionado.strDescripcionMovimiento

        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

    End Sub

#End Region

#Region "Métodos para control de eventos"

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try

            If Not IsNothing(DetalleSeleccionado) Then

                DetalleSeleccionado.lngIDComitente = lngIDComitente
                DetalleSeleccionado.dblVlrValorizado = dblVlrValorizado

                mobjVM.DetalleSeleccionado = DetalleSeleccionado

            End If

            Me.DialogResult = True

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            'Me.Close()
            Me.DialogResult = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub seleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            ' Seleccionar el texto del control en el cual el usuario se ubicó
            MyBase.OnGotFocus(e)

            If TypeOf sender Is TextBox Then
                CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        'App.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

