Imports Telerik.Windows.Controls
Imports System.Collections.ObjectModel
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.ComponentModel

Partial Public Class cwConfiguracionParametrosCompaniaView
    Inherits Window
    Implements INotifyPropertyChanged

    Private mobjVM As CompaniasViewModel

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Inicializa la ventana para cobro de utilidades (estilos y consulta de los registros)
    ''' </summary>
    Public Sub New(ByVal pmobjVM As CompaniasViewModel, ByVal Index As Integer)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
          
            'Me.DataContext = mobjVM
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me

            dtmFechaInicialParametro = Now
            inicializar()

            mobjVM = pmobjVM
            'IsBusyTipoOperacion = False
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

    End Sub

    Private Async Sub inicializar()
        Try


            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty, True)

            If Not IsNothing(mobjVM.ListaDetalleParametrosPaginadaCompania) Then
                If Not IsNothing(mobjVM.DetalleParametrosCompaniaSeleccionado) Then
                    strCategoriaParametro = mobjVM.DetalleParametrosCompaniaSeleccionado.strCategoriaParametro
                    strParametro = mobjVM.DetalleParametrosCompaniaSeleccionado.strParametro
                    strValorParametro = CStr(IIf(mobjVM.DetalleParametrosCompaniaSeleccionado.strValorParametro = Nothing, String.Empty, mobjVM.DetalleParametrosCompaniaSeleccionado.strValorParametro))
                    strDescripcionParametro = mobjVM.DetalleParametrosCompaniaSeleccionado.strDescripcionParametro
                    logManejaFechaParametro = mobjVM.DetalleParametrosCompaniaSeleccionado.logManejaFechaParametro
                    'dtmFechaInicialParametro = CDate(IIf(CDate(mobjVM.DetalleParametrosCompaniaSeleccionado.dtmFechaInicialParametro) = Nothing, Now, CDate(mobjVM.DetalleParametrosCompaniaSeleccionado.dtmFechaInicialParametro)))
                    If logManejaFechaParametro = True Then
                        MostrarFechaInicial = Visibility.Visible
                        If IsNothing(mobjVM.DetalleParametrosCompaniaSeleccionado.dtmFechaInicialParametro) Then
                            dtmFechaInicialParametro = Now
                        Else
                            dtmFechaInicialParametro = CDate(mobjVM.DetalleParametrosCompaniaSeleccionado.dtmFechaInicialParametro)
                        End If

                    Else
                        MostrarFechaInicial = Visibility.Collapsed
                        dtmFechaInicialParametro = Now
                    End If




                    'HabilitarBotonesEditar = Visibility.Visible
                    'HabilitarBotones = Visibility.Collapsed
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Debe selecionar un registro para editar ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Me.DialogResult = True
                End If
            Else
                'HabilitarBotones = Visibility.Visible
            End If
            'IsBusyTipoOperacion = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para inicializar el detalle tesoreria.", Me.ToString(), "inicializar()", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub



    'Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles btnAceptar.Click
    '    Me.DialogResult = True
    'End Sub

    'Private Sub btnCerrar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles btnCerrar.Click
    '    Me.DialogResult = False
    'End Sub

  

#Region "Propiedades"
    Private _strParametro As String
    Public Property strParametro() As String
        Get
            Return _strParametro
        End Get
        Set(ByVal value As String)
            _strParametro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strParametro"))
        End Set
    End Property

    Private _strCategoriaParametro As String
    Public Property strCategoriaParametro() As String
        Get
            Return _strCategoriaParametro
        End Get
        Set(ByVal value As String)
            _strCategoriaParametro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCategoriaParametro"))
        End Set
    End Property

    Private _strValorParametro As String
    Public Property strValorParametro() As String
        Get
            Return _strValorParametro
        End Get
        Set(ByVal value As String)
            _strValorParametro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strValorParametro"))
        End Set
    End Property

    Private _strDescripcionParametro As String
    Public Property strDescripcionParametro() As String
        Get
            Return _strDescripcionParametro
        End Get
        Set(ByVal value As String)
            _strDescripcionParametro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionParametro"))
        End Set
    End Property

    Private _logManejaFechaParametro As Boolean
    Public Property logManejaFechaParametro() As Boolean
        Get
            Return _logManejaFechaParametro
        End Get
        Set(ByVal value As Boolean)
            _logManejaFechaParametro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logManejaFechaParametro"))
        End Set
    End Property

    Private _dtmFechaInicialParametro As Date
    Public Property dtmFechaInicialParametro() As Date
        Get
            Return _dtmFechaInicialParametro
        End Get
        Set(ByVal value As Date)
            _dtmFechaInicialParametro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaInicialParametro"))
        End Set
    End Property



    Private _MostrarFechaInicial As Visibility = Visibility.Collapsed
    Public Property MostrarFechaInicial() As Visibility
        Get
            Return _MostrarFechaInicial
        End Get
        Set(ByVal value As Visibility)
            _MostrarFechaInicial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarFechaInicial"))
        End Set
    End Property


#End Region

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If mobjVM.ValidarDetalleParametrosCompania(strCategoriaParametro, strParametro, strValorParametro, strDescripcionParametro, logManejaFechaParametro, dtmFechaInicialParametro) Then
                mobjVM.EditarDetalleParametrosCompania(strValorParametro, logManejaFechaParametro, dtmFechaInicialParametro.Date, dtmFechaInicialParametro.Date.ToString())
            End If
            Me.Close()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón editar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub


    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class
