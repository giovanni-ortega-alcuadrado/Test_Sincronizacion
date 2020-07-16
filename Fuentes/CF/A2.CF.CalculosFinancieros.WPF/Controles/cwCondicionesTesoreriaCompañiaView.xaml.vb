Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports A2.OYD.OYDServer.RIA.Web.CFCalculosFinancieros

Partial Public Class cwCondicionesTesoreriaCompañiaView
    Inherits Window
    Implements INotifyPropertyChanged

    Private mobjVM As CompaniasViewModel
    Private Indice As Integer
    Private StrTipoOperacionEditado As String

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
            IsBusyTipoOperacion = True
            HabilitarBotonesEditar = Visibility.Collapsed
            HabilitarBotones = Visibility.Visible
            Indice = Index
            strTextoAceptar = "Guardar y Continuar"
            strTextoSalir = "Cerrar"

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




    'Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
    '    Me.DialogResult = True
    'End Sub

    'Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
    '    Me.DialogResult = False
    'End Sub

  

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            lngIDConceptoCT = CInt(pobjItem.IdItem)
            strDetalleConceptoCT = pobjItem.Nombre
        End If
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If mobjVM.ValidarDetalleCondicionesTesoreria(strTipoOperacionCT, lngIDConceptoCT) Then
                mobjVM.EditarDetalleCondicionesTesoreria(strTipoOperacionCT, itemComboOperacion.Descripcion, lngIDConceptoCT, strDetalleConceptoCT, CBool(IIf(strTipoOperacionCT = StrTipoOperacionEditado, 1, 0)))
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón editar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Propiedades"
    Private _strTipoOperacionCT As String
    Public Property strTipoOperacionCT() As String
        Get
            Return _strTipoOperacionCT
        End Get
        Set(ByVal value As String)
            _strTipoOperacionCT = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoOperacionCT"))
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

    Private _lngIDConceptoCT As Integer
    Public Property lngIDConceptoCT() As Integer
        Get
            Return _lngIDConceptoCT
        End Get
        Set(ByVal value As Integer)
            _lngIDConceptoCT = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDConceptoCT"))
        End Set
    End Property

    Private _strDetalleConceptoCT As String
    Public Property strDetalleConceptoCT() As String
        Get
            Return _strDetalleConceptoCT
        End Get
        Set(ByVal value As String)
            _strDetalleConceptoCT = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDetalleConceptoCT"))
        End Set
    End Property

    Private _strTextoAceptar As String
    Public Property strTextoAceptar() As String
        Get
            Return _strTextoAceptar
        End Get
        Set(ByVal value As String)
            _strTextoAceptar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTextoAceptar"))
        End Set
    End Property

    Private _strTextoSalir As String
    Public Property strTextoSalir() As String
        Get
            Return _strTextoSalir
        End Get
        Set(ByVal value As String)
            _strTextoSalir = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTextoSalir"))
        End Set
    End Property


    Private _HabilitarBotonesEditar As Visibility
    Public Property HabilitarBotonesEditar() As Visibility
        Get
            Return _HabilitarBotonesEditar
        End Get
        Set(ByVal value As Visibility)
            _HabilitarBotonesEditar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarBotonesEditar"))
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


#End Region

    Private Async Sub inicializar()
        Try


            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty, True)

            If Not IsNothing(mobjVM.ListaDetalleCondicionesPaginadaTesoreria) AndAlso Indice >= 0 Then
                If Not IsNothing(mobjVM.DetalleTesoreriaCondicioneSeleccionado) Then
                    strTipoOperacionCT = mobjVM.DetalleTesoreriaCondicioneSeleccionado.strTipoOperacionCT
                    'intDiasAplicaTransaccion = CInt(mobjVM.DetalleTesoreriaSeleccionado.intDiasAplicaTransaccion)
                    StrTipoOperacionEditado = mobjVM.DetalleTesoreriaCondicioneSeleccionado.strTipoOperacionCT
                    lngIDConceptoCT = CInt(mobjVM.DetalleTesoreriaCondicioneSeleccionado.lngIDConceptoCT)
                    strDetalleConceptoCT = mobjVM.DetalleTesoreriaCondicioneSeleccionado.strDetalleConceptoCT
                    'intDiasPago = CInt(mobjVM.DetalleTesoreriaSeleccionado.intDiasPago)
                    strTextoAceptar = "Aceptar"
                    strTextoSalir = "Salir"


                    HabilitarBotonesEditar = Visibility.Visible
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



    Private Sub btnGuardarySalir_Click(sender As Object, e As RoutedEventArgs)
        GuardarDatosGrid(1)
    End Sub

    Private Sub btnGuardaryContinuar_Click(sender As Object, e As RoutedEventArgs)
        GuardarDatosGrid(0)
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub GuardarDatosGrid(ByVal opcion As Integer)
        Try
            If mobjVM.ValidarDetalleCondicionesTesoreria(strTipoOperacionCT, lngIDConceptoCT) Then
                mobjVM.IngresarDetalleCondicionesTesoreria(strTipoOperacionCT, itemComboOperacion.Descripcion, lngIDConceptoCT, strDetalleConceptoCT)
                If opcion = 1 Then
                    Me.Close()
                Else
                    lngIDConceptoCT = Nothing
                    strTipoOperacionCT = String.Empty
                    strDetalleConceptoCT = String.Empty
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class
