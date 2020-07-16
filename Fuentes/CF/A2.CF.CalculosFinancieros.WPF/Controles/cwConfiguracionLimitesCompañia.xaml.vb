Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports A2.OYD.OYDServer.RIA.Web.CFCalculosFinancieros

Partial Public Class cwConfiguracionLimitesCompañia
    Inherits Window
    Implements INotifyPropertyChanged

    Private mobjVM As CompaniasViewModel
    Private Indice As Integer
    Private StrTipoOperacionEditado As String




    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal pmobjVM As CompaniasViewModel, ByVal Index As Integer)
        'myBusyIndicator.IsBusy = True
        IsBusy = True
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


        HabilitarCampoValor = False
        HabilitarCampoPorcentaje = False
        MostrarBotonEditar = Visibility.Collapsed
        MostrarBotonNuevo = Visibility.Visible

        Indice = Index
        inicializar()

        InitializeComponent()
        Me.LayoutRoot.DataContext = Me

        mobjVM = pmobjVM

        ' myBusyIndicator.IsBusy = False

    End Sub

    'Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
    '    Me.DialogResult = True
    'End Sub

    'Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
    '    Me.DialogResult = False
    'End Sub

#Region "Propiedades"
    Private _strTipoConcepto As String
    Public Property strTipoConcepto() As String
        Get
            Return _strTipoConcepto
        End Get
        Set(ByVal value As String)
            _strTipoConcepto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoConcepto"))
        End Set
    End Property


    Private _HabilitarCampoValor As Boolean
    Public Property HabilitarCampoValor() As Boolean
        Get
            Return _HabilitarCampoValor
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCampoValor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarCampoValor"))
        End Set
    End Property


    Private _HabilitarCampoPorcentaje As Boolean
    Public Property HabilitarCampoPorcentaje() As Boolean
        Get
            Return _HabilitarCampoPorcentaje
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCampoPorcentaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarCampoPorcentaje"))
        End Set
    End Property

    Private _dblValor As Double
    Public Property dblValor() As Double
        Get
            Return _dblValor
        End Get
        Set(ByVal value As Double)
            _dblValor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValor"))
        End Set
    End Property

    Private _dblPorcentaje As Double
    Public Property dblPorcentaje() As Double
        Get
            Return _dblPorcentaje
        End Get
        Set(ByVal value As Double)
            _dblPorcentaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblPorcentaje"))
        End Set
    End Property

    Private _MostrarBotonEditar As Visibility
    Public Property MostrarBotonEditar() As Visibility
        Get
            Return _MostrarBotonEditar
        End Get
        Set(ByVal value As Visibility)
            _MostrarBotonEditar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarBotonEditar"))
        End Set
    End Property

    Private _MostrarBotonNuevo As Visibility
    Public Property MostrarBotonNuevo() As Visibility
        Get
            Return _MostrarBotonNuevo
        End Get
        Set(ByVal value As Visibility)
            _MostrarBotonNuevo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarBotonNuevo"))
        End Set
    End Property

    Private _logValor As Boolean
    Public Property logValor As Boolean
        Get
            Return _logValor
        End Get
        Set(value As Boolean)
            _logValor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logValor"))
        End Set
    End Property

    Private _logPorcentaje As Boolean
    Public Property logPorcentaje As Boolean
        Get
            Return _logPorcentaje
        End Get
        Set(value As Boolean)
            _logPorcentaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logPorcentaje"))
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

    Private _itemComboConcepto As OYDUtilidades.ItemCombo
    Public Property itemComboConcepto As OYDUtilidades.ItemCombo
        Get
            Return _itemComboConcepto
        End Get
        Set(value As OYDUtilidades.ItemCombo)
            _itemComboConcepto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("itemComboConcepto"))
        End Set
    End Property



    Private _strTipoParticipacion As String
    Public Property strTipoParticipacion() As String
        Get
            Return _strTipoParticipacion
        End Get
        Set(ByVal value As String)
            _strTipoParticipacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoParticipacion"))
        End Set
    End Property

#End Region


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try

            If mobjVM.ValidarDetalleLimites(strTipoConcepto, dblPorcentaje, CBool(IIf(IsNothing(logPorcentaje), 0, logPorcentaje)), dblValor, CBool(IIf(IsNothing(logValor), 0, logValor))) Then
                'mobjVM.EditarDetalleLimites(CStr(cbTipoConcepto.SelectedValue), CType(cbTipoConcepto.SelectionBoxItem, A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo).Descripcion, chkValoraPesos.IsChecked.Value, txtValor.Value, chkValoraPorcentaje.IsChecked.Value, txtPorcentaje.Value)
                mobjVM.EditarDetalleLimites(strTipoConcepto, itemComboConcepto.Descripcion, logValor, dblValor, logPorcentaje, dblPorcentaje, CBool(IIf(strTipoConcepto = StrTipoOperacionEditado, 1, 0)))
                Me.Close()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón editar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Async Sub inicializar()
        Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty, True)

        If Not IsNothing(mobjVM.ListaDetalleLimites) And Indice >= 0 Then
            If Not IsNothing(mobjVM.DetalleLimitesSeleccionado) Then

                strTipoConcepto = mobjVM.DetalleLimitesSeleccionado.strTipoConcepto
                StrTipoOperacionEditado = mobjVM.DetalleLimitesSeleccionado.strTipoConcepto
                strTipoParticipacion = mobjVM.EncabezadoSeleccionado.strParticipacion


                If mobjVM.DetalleLimitesSeleccionado.logValor = True Then
                    HabilitarCampoValor = True
                    dblValor = CDbl(mobjVM.DetalleLimitesSeleccionado.dblValor)
                    'chkValoraPesos.IsChecked = True
                    logValor = True
                Else
                    HabilitarCampoValor = False
                End If
                If mobjVM.DetalleLimitesSeleccionado.logPorcentaje = True Then
                    HabilitarCampoPorcentaje = True
                    dblPorcentaje = CDbl(mobjVM.DetalleLimitesSeleccionado.dblPorcentaje)
                    'chkValoraPorcentaje.IsChecked = True
                    logPorcentaje = True
                Else
                    HabilitarCampoPorcentaje = False
                End If

                MostrarBotonEditar = Visibility.Visible
                MostrarBotonNuevo = Visibility.Collapsed
            Else
                MostrarBotonNuevo = Visibility.Visible
            End If
        Else
            MostrarBotonNuevo = Visibility.Visible
            strTipoParticipacion = mobjVM.EncabezadoSeleccionado.strParticipacion
        End If
        IsBusy = False
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub ValoraPesos_Checked(sender As Object, e As RoutedEventArgs)
        HabilitarCampoValor = True
    End Sub

    Private Sub ValoraPesos_Unchecked(sender As Object, e As RoutedEventArgs)
        HabilitarCampoValor = False
        dblValor = 0
    End Sub

    Private Sub ValoraPorcentaje_Checked(sender As Object, e As RoutedEventArgs)
        HabilitarCampoPorcentaje = True
    End Sub

    Private Sub ValoraPorcentaje_Unchecked(sender As Object, e As RoutedEventArgs)
        HabilitarCampoPorcentaje = False
        dblPorcentaje = 0
    End Sub



    Private Sub GuardarDatosGrid(ByVal opcion As Integer)
        Try
            If mobjVM.ValidarDetalleLimites(strTipoConcepto, dblPorcentaje, CBool(IIf(IsNothing(logPorcentaje), 0, logPorcentaje)), dblValor, CBool(IIf(IsNothing(logValor), 0, logValor))) Then
                'mobjVM.IngresarDetalleLimites(CStr(cbTipoConcepto.SelectedValue), CType(cbTipoConcepto.SelectionBoxItem, A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo).Descripcion, chkValoraPesos.IsChecked.Value, txtValor.Value, chkValoraPorcentaje.IsChecked.Value, txtPorcentaje.Value)

                mobjVM.IngresarDetalleLimites(strTipoConcepto, itemComboConcepto.Descripcion, logValor, dblValor, logPorcentaje, dblPorcentaje)


                If opcion = 1 Then
                    Me.Close()
                Else
                    strTipoConcepto = String.Empty
                    logPorcentaje = False
                    logValor = False
                    dblValor = 0
                    dblPorcentaje = 0
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón guardar.", Me.ToString(), "GuardarDatosGrid", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    '------------------------------------------------------------------------------------------------------------------------------------------------
    '-- Valida que dependiendo del Tipo de Participación se muestre el buscador seleccionado, las opciones son:No Particionado (Solo buscadores OyD), con clases
    '--(Solo buscador companias), Estrategia General (Los dos buscadores), Estrategia individual (Ninguno)
    'ID caso de prueba:  CP038
    '------------------------------------------------------------------------------------------------------------------------------------------------

    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            If IsNothing(strTipoConcepto) Then
                strMsg = String.Format("{0}{1} + Es necesario ingresar un tipo de concepto.", strMsg, vbCrLf)
            End If

            If dblPorcentaje = 0 And dblValor = 0 Then
                strMsg = String.Format("{0}{1} + Debe ingresar el porcentaje o el valor.", strMsg, vbCrLf)
            End If

            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle de los limites. " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle de los limites.", Me.ToString(), "ValidarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

    Private Sub btnGuardaryContinuar_Click(sender As Object, e As RoutedEventArgs)
        GuardarDatosGrid(0)
    End Sub

    Private Sub btnGuardarySalir_Click(sender As Object, e As RoutedEventArgs)
        GuardarDatosGrid(1)
    End Sub
End Class
