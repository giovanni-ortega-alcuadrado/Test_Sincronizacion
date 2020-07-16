Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports Microsoft.VisualBasic.CompilerServices
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client


Partial Public Class EstadosColasLEO
    Inherits Window
    Implements INotifyPropertyChanged

    Dim plngIDOrden As Integer, strTipo As String, strClase As String, pstrEstadoLEO As String, objViewModel As ActualizarLEOViewModel

    Public Sub New(ByVal plngIDOrdenVM As Integer, ByVal pstrTipoVM As String, ByVal pstrClaseVM As String, ByVal pstrEstadoLEOVM As String, ByVal plogDevolver As Boolean, ByVal pobjViewModel As ActualizarLEOViewModel)
        Try
            Me.DataContext = Me

            InitializeComponent()
            plngIDOrden = plngIDOrdenVM
            strTipo = pstrTipoVM
            strClase = pstrClaseVM

            If plogDevolver = False Then
                If pstrEstadoLEOVM.ToUpper = "R" Then
                    pstrEstadoLEO = "L"
                ElseIf pstrEstadoLEOVM.ToUpper = "L" Then
                    pstrEstadoLEO = "C"
                Else
                    pstrEstadoLEO = pstrEstadoLEOVM
                End If
            Else
                If pstrEstadoLEOVM.ToUpper = "C" Then
                    pstrEstadoLEO = "L"
                ElseIf pstrEstadoLEOVM.ToUpper = "L" Then
                    pstrEstadoLEO = "R"
                Else
                    pstrEstadoLEO = pstrEstadoLEOVM
                End If
            End If

            objViewModel = pobjViewModel



        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "EstadosColasLEO", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ActualizarLEOView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If Me.Resources.Contains("A2VM") Then
                If Not IsNothing(CType(Me.Resources("A2VM"), A2UtilsViewModel).DiccionarioCombosA2) Then
                    If CType(Me.Resources("A2VM"), A2UtilsViewModel).DiccionarioCombosA2.ContainsKey("USUARIO_OPERADOR_COLAS_LEO") Then
                        If CType(Me.Resources("A2VM"), A2UtilsViewModel).DiccionarioCombosA2("USUARIO_OPERADOR_COLAS_LEO").Where(Function(i) i.ID = Program.Usuario).Count > 0 Then
                            Login = Program.Usuario
                        End If
                    End If
                End If
            End If

            dtmMovimiento = Now
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "ActualizarLEOView_Loaded", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#Region "Métodos para control de eventos"
    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If String.IsNullOrEmpty(Login) Then
                mostrarMensaje("Debe de seleccionar el usuario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(dtmMovimiento) Then
                mostrarMensaje("Debe de seleccionar la Fecha.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            objViewModel.ActualizarEstado(plngIDOrden, strTipo, strClase, pstrEstadoLEO, Login, dtmMovimiento)
            Me.DialogResult = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Try
            objViewModel.logRealizarConsultaOrdenes = True
            Me.DialogResult = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region
#Region "Propiedades"


    Private _Login As String
    Public Property Login() As String
        Get
            Return _Login
        End Get
        Set(ByVal value As String)
            _Login = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Login"))
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





#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged


End Class