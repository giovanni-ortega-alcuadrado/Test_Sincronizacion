Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports A2.OYD.OYDServer.RIA.Web

Partial Public Class cwConfiguracionRendimientos
    Inherits Window
    Implements INotifyPropertyChanged

    Private mobjVM As BancosViewModel
    Dim opcion As Integer


    Public Sub New(ByVal pmobjVM As BancosViewModel, ByVal Index As Integer)
        'inicializar()

        InitializeComponent()
        Me.LayoutRoot.DataContext = Me

        mobjVM = pmobjVM
        opcion = Index
        Iniciar()
    End Sub



    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' JCM20160301
    ''' Método para habilitar o deshabilitar los botonos, de editar o de nuevo registro si la opcón es 0 es nuevo registro, si es 1 es un registro para editar
    ''' 
    ''' </summary>

    Private Sub Iniciar()
        Try

            If opcion = 0 Then
                MostrarBotonesEditar = Visibility.Collapsed
                MostrarBotonesNuevo = Visibility.Visible

                ObtenerProximosValores()

                
            Else
                MostrarBotonesEditar = Visibility.Visible
                MostrarBotonesNuevo = Visibility.Collapsed

                
                dblValorInicial = mobjVM.ListaBancoTasasRendimientosSeleccionado.dblValorInicial
                dblValorFinal = mobjVM.ListaBancoTasasRendimientosSeleccionado.dblValorFinal
                dblTasaRendimiento = mobjVM.ListaBancoTasasRendimientosSeleccionado.dblTasaRendimiento
                  
            End If
        Catch

        End Try
    End Sub

    ''' <summary>
    ''' JCM20160301
    ''' Ingresar un registro al grid, se ingresar un solo registro o se pueden ingresar varios registros
    ''' 
    ''' </summary>

    Private Sub IngresarDatosGrid(ByVal Accion As String)
        If Accion = "GuardaryContinuar" Then
            mobjVM.IngresarDetalleBancoTasasRendimientos(dblValorInicial, dblValorFinal, dblTasaRendimiento)
        ElseIf Accion = "GuardarySalir" Then
            mobjVM.IngresarDetalleBancoTasasRendimientos(dblValorInicial, dblValorFinal, dblTasaRendimiento)
            Me.DialogResult = False
        End If
    End Sub

    Private Sub Limpiarvalores()
        txtValorInicial.Value = Nothing
        txtValorFinal.Value = Nothing
        txtPorcentaje.Value = Nothing
    End Sub

#Region "Propiedades"
    Private _dblValorInicial As Double
    Public Property dblValorInicial As Double
        Get
            Return _dblValorInicial
        End Get
        Set(value As Double)
            _dblValorInicial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValorInicial"))
        End Set
    End Property

    Private _dblValorFinal As Double
    Public Property dblValorFinal As Double
        Get
            Return _dblValorFinal
        End Get
        Set(value As Double)
            _dblValorFinal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValorFinal"))
        End Set
    End Property

    Private _dblTasaRendimiento As Double
    Public Property dblTasaRendimiento As Double
        Get
            Return _dblTasaRendimiento
        End Get
        Set(value As Double)
            _dblTasaRendimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblTasaRendimiento"))
        End Set
    End Property


    Private _MostrarBotonesEditar As Visibility
    Public Property MostrarBotonesEditar() As Visibility
        Get
            Return _MostrarBotonesEditar
        End Get
        Set(ByVal value As Visibility)
            _MostrarBotonesEditar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarBotonesEditar"))
        End Set
    End Property


    Private _MostrarBotonesNuevo As Visibility
    Public Property MostrarBotonesNuevo() As Visibility
        Get
            Return _MostrarBotonesNuevo
        End Get
        Set(ByVal value As Visibility)
            _MostrarBotonesNuevo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarBotonesNuevo"))
        End Set
    End Property

    Private _logValorMaximo As Boolean
    Public Property logValorMaximo() As Boolean
        Get
            Return _logValorMaximo
        End Get
        Set(ByVal value As Boolean)
            _logValorMaximo = value
            If value Then
                dblValorFinal = Program.ValorMaximoRangoTasasBanco
            Else
                dblValorFinal = 0
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logValorMaximo"))
        End Set
    End Property


#End Region


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Me.DialogResult = False
    End Sub

    Private Sub btnGuardaryContinuar_Click(sender As Object, e As RoutedEventArgs)
        If Validar() = True Then
            IngresarDatosGrid("GuardaryContinuar")
            Limpiarvalores()
            ObtenerProximosValores()
        End If
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        If Validar() = True Then
            mobjVM.EditarDetalleBancoTasasRendimientos(txtValorInicial.Value, txtValorFinal.Value, txtPorcentaje.Value)
            Me.DialogResult = False
        End If
    End Sub

    Private Sub btnGuardarySalir_Click(sender As Object, e As RoutedEventArgs)
        If Validar() = True Then
            IngresarDatosGrid("GuardarySalir")
        End If
    End Sub

    ''' <summary>
    ''' JCM20160301
    ''' Validar los datos antes de ser ingresado o editados en el grid 
    ''' </summary>


    Private Function Validar() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            If dblTasaRendimiento <= 0 Then
                If logValorMaximo <> True Then
                    strMsg = String.Format("{0}{1} + Es necesario ingresar una tasa de porcentaje que sea mayor que 0.", strMsg, vbCrLf)
                End If
            End If

            If dblTasaRendimiento > 100 Then
                strMsg = String.Format("{0}{1} + La tasa debe ser un porcentaje menor que 100.", strMsg, vbCrLf)
            End If


            If dblValorFinal <= 0 Then
                strMsg = String.Format("{0}{1} + Es necesario que el valor final sea mayor que 0.", strMsg, vbCrLf)
            End If

            If dblValorInicial < 0 Then
                strMsg = String.Format("{0}{1} + Es necesario que el valor final sea mayor o igual que 0.", strMsg, vbCrLf)
            End If


            If dblValorFinal <> 0 Or dblValorFinal <> 0 Then
                If dblValorInicial >= dblValorFinal Then
                    strMsg = String.Format("{0}{1} + El valor inicial debe ser menor al valor final", strMsg, vbCrLf)
                End If
            End If

            If dblValorFinal > Program.ValorMaximoRangoTasasBanco Then
                strMsg = String.Format("{0}{1} + El valor final excede el máximo permitido", strMsg, vbCrLf)
            End If

            'If mobjVM.ValidarRegistroDetalleTasasRendimientos = False Then
            '    strMsg = String.Format("{0}{1} + Ya existe un registro ingresado con valor inicial 0 y valor final 0, no es posible ingresar mas registros", strMsg, vbCrLf)
            'End If

            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle de las tasas de rendimientos. " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If


        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle de las tasas de rendimientos.", Me.ToString(), "ValidarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

    Private Sub ObtenerProximosValores()
        Dim strMsg As String = String.Empty
        If mobjVM.ValidarRegistroDetalleTasasRendimientos = True Then
            If mobjVM.ListaBancoTasasRendimientosProximosValores.dblValorInicial <> 0 Then
                dblValorInicial = mobjVM.ListaBancoTasasRendimientosProximosValores.dblValorInicial
                dblValorFinal = 0
                dblTasaRendimiento = 0
                logValorMaximo = False
            Else
                dblValorInicial = 0
                dblValorFinal = 0
                dblTasaRendimiento = 0
                logValorMaximo = False
                'strMsg = "El valor inicial no puede ser mayor que el límite permitido"
                'A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle de las tasas de rendimientos. " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Else
            dblValorInicial = 0
            dblValorFinal = 0
            dblTasaRendimiento = 0
            logValorMaximo = False
        End If
    End Sub


End Class
