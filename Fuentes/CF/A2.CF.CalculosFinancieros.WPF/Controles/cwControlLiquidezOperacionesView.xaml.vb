Imports Telerik.Windows.Controls

Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Partial Public Class cwControlLiquidezOperacionesView
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Variables"

    Private mobjVM As ControlLiquidezOperacionesViewModel
    Private mobjDetallePorDefecto As ControlLiquidezOperacionesDetalle

#End Region

#Region "Propiedades"

    Private Async Sub cwControlLiquidezOperacionesView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Try
            Await inicializar()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante el método cwControlLiquidezOperacionesView_Loaded", Me.Name, "cwControlLiquidezOperacionesView_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private _strDescripcion As String
    Public Property strDescripcion() As String
        Get
            Return _strDescripcion
        End Get
        Set(ByVal value As String)
            _strDescripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcion"))
        End Set
    End Property

    Private _strSigno As String
    Public Property strSigno() As String
        Get
            Return _strSigno
        End Get
        Set(ByVal value As String)
            _strSigno = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strSigno"))
        End Set
    End Property

    Private _dblValor As System.Nullable(Of Double)
    Public Property dblValor() As System.Nullable(Of Double)
        Get
            Return _dblValor
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblValor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValor"))
        End Set
    End Property

    Private _IsBusy As Boolean = False
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As ControlLiquidezOperacionesDetalle
    Public Property DetalleSeleccionado() As ControlLiquidezOperacionesDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As ControlLiquidezOperacionesDetalle)
            _DetalleSeleccionado = value
        End Set
    End Property

    Private _HabilitarEncabezado As Boolean = False
    Public Property HabilitarEncabezado() As Boolean
        Get
            Return _HabilitarEncabezado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncabezado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarEncabezado"))
        End Set
    End Property

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para cobro de utilidades (estilos y consulta de los registros)
    ''' </summary>
    Public Sub New(ByVal pmobjVM As ControlLiquidezOperacionesViewModel, ByVal DetalleSeleccionadoVM As ControlLiquidezOperacionesDetalle, ByVal mobjDetallePorDefectoVM As ControlLiquidezOperacionesDetalle, ByVal HabilitarEncabezadoVM As Boolean)
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
            Me.DataContext = Me

            DetalleSeleccionado = DetalleSeleccionadoVM
            mobjDetallePorDefecto = mobjDetallePorDefectoVM
            HabilitarEncabezado = HabilitarEncabezadoVM

            mobjVM = pmobjVM

            If Not IsNothing(DetalleSeleccionado) Then

                DetalleSeleccionado = DetalleSeleccionadoVM

                strDescripcion = DetalleSeleccionado.strDescripcion
                strSigno = DetalleSeleccionado.strSigno
                dblValor = DetalleSeleccionado.dblValor

            End If

        Catch ex As Exception
            IsBusy = False
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

    End Sub

    Private Async Function inicializar() As Task
        Try
            IsBusy = True

            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty, True)

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método inicializar.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Métodos para control de eventos"

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try

            If ValidarDetalle() Then

                If IsNothing(DetalleSeleccionado) Then

                    Dim objNvoDetalle As New ControlLiquidezOperacionesDetalle
                    Dim objNuevaLista As New List(Of ControlLiquidezOperacionesDetalle)

                    Program.CopiarObjeto(Of ControlLiquidezOperacionesDetalle)(mobjDetallePorDefecto, objNvoDetalle)

                    objNvoDetalle.intIDControlLiquidezOperacionesDetalle = -New Random().Next(0, 1000000)

                    If IsNothing(mobjVM.ListaDetalle) Then
                        mobjVM.ListaDetalle = New List(Of ControlLiquidezOperacionesDetalle)
                    End If

                    objNuevaLista = mobjVM.ListaDetalle

                    objNvoDetalle.strDescripcion = strDescripcion
                    objNvoDetalle.strSigno = strSigno
                    objNvoDetalle.dblValor = dblValor
                    objNvoDetalle.strDescripcionSigno = CStr(IIf(strSigno = "A", "+", "-"))

                    objNuevaLista.Add(objNvoDetalle)
                    mobjVM.ListaDetalle = objNuevaLista
                    DetalleSeleccionado = mobjVM.ListaDetalle.First

                    mobjVM.DetalleSeleccionado = DetalleSeleccionado

                Else
                    DetalleSeleccionado.strDescripcion = strDescripcion
                    DetalleSeleccionado.strSigno = strSigno
                    DetalleSeleccionado.dblValor = dblValor
                    DetalleSeleccionado.strDescripcionSigno = CStr(IIf(strSigno = "A", "+", "-"))
                End If

                mobjVM.CalcularTotal()

                Me.DialogResult = True

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            '------------------------------------------------------------------------------------------------------------------------------------------------
            '-- Valida que por lo menos exista un detalle para poder crear todo un registro
            '------------------------------------------------------------------------------------------------------------------------------------------------

            If String.IsNullOrEmpty(strDescripcion) Then
                strMsg = String.Format("{0}{1} + La descripción es un campo requerido.", strMsg, vbCrLf)
            End If

            If String.IsNullOrEmpty(strSigno) Then
                strMsg = String.Format("{0}{1} + El signo es un campo requerido.", strMsg, vbCrLf)
            End If

            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle.", Me.ToString(), "ValidarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            'Me.Close()
            Me.DialogResult = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnCerrar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub seleccionarFocoControl(sender As Object, e As RoutedEventArgs)
        Try
            ' Seleccionar el texto del control en el cual el usuario se ubicó
            MyBase.OnGotFocus(e)

            If TypeOf sender Is TextBox Then
                CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
