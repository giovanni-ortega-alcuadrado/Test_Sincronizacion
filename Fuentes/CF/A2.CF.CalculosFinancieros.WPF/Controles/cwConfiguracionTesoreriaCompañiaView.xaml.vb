Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports A2.OYD.OYDServer.RIA.Web.CFCalculosFinancieros

Partial Public Class cwConfiguracionTesoreriaCompañiaView
    Inherits Window
    Implements INotifyPropertyChanged

    Private mobjVM As CompaniasViewModel
    Private Indice As Integer
    Private StrTipoOperacionEditado As String


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged


    Public Sub New()
        InitializeComponent()
        'IsBusyTipoOperacion = False
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
            IsBusyTipoOperacion = True
            HabilitarBotonesExtra = Visibility.Collapsed
            HabilitarBotones = Visibility.Visible

            Indice = Index
            inicializar()

            'Me.DataContext = mobjVM
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me

            mobjVM = pmobjVM
            'IsBusyTipoOperacion = False
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

    End Sub


#Region "Propiedades"
    Private _strTipoOperacion As String
    Public Property strTipoOperacion() As String
        Get
            Return _strTipoOperacion
        End Get
        Set(ByVal value As String)
            _strTipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoOperacion"))
        End Set
    End Property

    Private _intDiasAplicaTransaccion As Integer
    Public Property intDiasAplicaTransaccion() As Integer
        Get
            Return _intDiasAplicaTransaccion
        End Get
        Set(ByVal value As Integer)
            _intDiasAplicaTransaccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intDiasAplicaTransaccion"))
        End Set
    End Property


    Private _intDiasPago As Integer
    Public Property intDiasPago() As Integer
        Get
            Return _intDiasPago
        End Get
        Set(ByVal value As Integer)
            _intDiasPago = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intDiasPago"))
        End Set
    End Property

    Private _strTipoTransaccion As String
    Public Property strTipoTransaccion() As String
        Get
            Return _strTipoTransaccion
        End Get
        Set(ByVal value As String)
            _strTipoTransaccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoTransaccion"))
        End Set
    End Property

    Private _strOperador As String
    Public Property strOperador() As String
        Get
            Return _strOperador
        End Get
        Set(ByVal value As String)
            _strOperador = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strOperador"))
        End Set
    End Property

    Private _dtmHoraInicial As System.Nullable(Of System.DateTime)
    Public Property dtmHoraInicial() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmHoraInicial
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmHoraInicial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmHoraInicial"))
        End Set
    End Property


    Private _dtmHoraFinal As System.Nullable(Of System.DateTime)
    Public Property dtmHoraFinal() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmHoraFinal
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmHoraFinal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmHoraFinal"))
        End Set
    End Property


    Private _dblMontoMínimo As Double
    Public Property dblMontoMínimo() As Double
        Get
            Return _dblMontoMínimo
        End Get
        Set(ByVal value As Double)
            _dblMontoMínimo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblMontoMínimo"))
        End Set
    End Property


    Private _dblMontoMáximo As Double
    Public Property dblMontoMáximo() As Double
        Get
            Return _dblMontoMáximo
        End Get
        Set(ByVal value As Double)
            _dblMontoMáximo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblMontoMáximo"))
        End Set
    End Property

    Private _HabilitarBotonesExtra As Visibility
    Public Property HabilitarBotonesExtra() As Visibility
        Get
            Return _HabilitarBotonesExtra
        End Get
        Set(ByVal value As Visibility)
            _HabilitarBotonesExtra = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarBotonesExtra"))
        End Set
    End Property


    Private _HabilitarBotones As Visibility
    Public Property HabilitarBotones() As Visibility
        Get
            Return _HabilitarBotones
        End Get
        Set(ByVal value As Visibility)
            _HabilitarBotones = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarBotones"))
        End Set
    End Property


    Private _itemComboOperacion As OYDUtilidades.ItemCombo
    Public Property itemComboOperacion As OYDUtilidades.ItemCombo
        Get
            Return _itemComboOperacion
        End Get
        Set(value As OYDUtilidades.ItemCombo)
            _itemComboOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("itemComboOperacion"))
        End Set
    End Property

    Private _IsBusyTipoOperacion As Boolean
    Public Property IsBusyTipoOperacion() As Boolean
        Get
            Return _IsBusyTipoOperacion
        End Get
        Set(ByVal value As Boolean)
            _IsBusyTipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusyTipoOperacion"))
        End Set
    End Property

    
    Private _lngIDConcepto As Integer
    Public Property lngIDConcepto() As Integer
        Get
            Return _lngIDConcepto
        End Get
        Set(ByVal value As Integer)
            _lngIDConcepto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDConcepto"))
        End Set
    End Property

    Private _strDetalleConcepto As String
    Public Property strDetalleConcepto() As String
        Get
            Return _strDetalleConcepto
        End Get
        Set(ByVal value As String)
            _strDetalleConcepto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDetalleConcepto"))
        End Set
    End Property



#End Region

    Private Async Sub inicializar()
        Try


            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty, True)

            If Not IsNothing(mobjVM.ListaDetalleTesoreria) AndAlso Indice >= 0 Then
                If Not IsNothing(mobjVM.DetalleTesoreriaSeleccionado) Then
                    strTipoOperacion = mobjVM.DetalleTesoreriaSeleccionado.strTipoOperacion
                    intDiasAplicaTransaccion = CInt(mobjVM.DetalleTesoreriaSeleccionado.intDiasAplicaTransaccion)
                    StrTipoOperacionEditado = mobjVM.DetalleTesoreriaSeleccionado.strTipoOperacion
                    lngIDConcepto = CInt(mobjVM.DetalleTesoreriaSeleccionado.lngIDConcepto)
                    strDetalleConcepto = mobjVM.DetalleTesoreriaSeleccionado.strDetalleConcepto
                    intDiasPago = CInt(mobjVM.DetalleTesoreriaSeleccionado.intDiasPago)


                    HabilitarBotonesExtra = Visibility.Visible
                    HabilitarBotones = Visibility.Collapsed
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Debe selecionar un registro para editar ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Me.DialogResult = True
                End If
            Else
                HabilitarBotones = Visibility.Visible
            End If
            IsBusyTipoOperacion = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para inicializar el detalle tesoreria.", Me.ToString(), "inicializar()", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub

    

    Private Sub btnGuardaryContinuar_Click(sender As Object, e As RoutedEventArgs)

        GuardarDatosGrid(0)

    End Sub

    Private Sub cbTipoDeOperacion_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbTipoDeTransaccion_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub cbOperador_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub btnSalir_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub btnGuardarySalir_Click(sender As Object, e As RoutedEventArgs)

        GuardarDatosGrid(1)


    End Sub

    Private Sub GuardarDatosGrid(ByVal opcion As Integer)
        'Dim StrCuentaContable As String

        'If StrIDCuentaContable Is Nothing Then
        '    StrCuentaContable = ""
        'Else
        '    StrCuentaContable = StrIDCuentaContable
        'End If
        Try
            If mobjVM.ValidarDetalleTesoreria(strTipoOperacion, intDiasAplicaTransaccion, lngIDConcepto, intDiasPago) Then
                mobjVM.IngresarDetalleTesoreria(strTipoOperacion, itemComboOperacion.Descripcion, intDiasAplicaTransaccion, lngIDConcepto, strDetalleConcepto, intDiasPago)
                If opcion = 1 Then
                    Me.Close()
                Else
                    lngIDConcepto = Nothing
                    strTipoOperacion = String.Empty
                    intDiasAplicaTransaccion = 0
                    intDiasPago = 0
                    strDetalleConcepto = String.Empty
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón guardar.", Me.ToString(), "GuardarDatosGrid", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub

  

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If mobjVM.ValidarDetalleTesoreria(strTipoOperacion, intDiasAplicaTransaccion, LngIDConcepto, intDiasPago) Then
                mobjVM.EditarDetalleTesoreria(strTipoOperacion, itemComboOperacion.Descripcion, intDiasAplicaTransaccion, intDiasPago, lngIDConcepto, strDetalleConcepto, CBool(IIf(strTipoOperacion = StrTipoOperacionEditado, 1, 0)))
                Me.DialogResult = True
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub


    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.DialogResult = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnCerrar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaD(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        'If Not IsNothing(pobjItem) Then
        '    'CType(Me.DataContext, CompaniasTesoreria).ListaDetalleTesoreria.strIDCuentaContable = pobjItem.CodItem
        '    StrIDCuentaContable = pobjItem.CodItem
        '    'CType(Me.DataContext, CompaniasTesoreria).strIDCuentaContable
        'End If
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            LngIDConcepto = CInt(pobjItem.IdItem)
            strDetalleConcepto = pobjItem.Nombre
        End If
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda_1(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)

    End Sub
End Class


