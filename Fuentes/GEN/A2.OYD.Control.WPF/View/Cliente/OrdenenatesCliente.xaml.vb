Imports A2Utilidades.Mensajes

Partial Public Class OrdenantesCliente
    Inherits UserControl

#Region "Constantes"

    Private Const STR_NOMBRE_VIEW_MODEL As String = "VM"

#End Region

#Region "Variables"

    Private mobjVM As OrdenantesClienteViewModel ' Referencia al view model del control

#End Region

#Region "Propiedades"

    Private Shared IdOrdenanteDep As DependencyProperty = DependencyProperty.Register("IdOrdenante", GetType(String), GetType(OrdenantesCliente), New PropertyMetadata(AddressOf cambioPropiedadDep))
    Private Shared IdComitenteDep As DependencyProperty = DependencyProperty.Register("IdComitente", GetType(String), GetType(OrdenantesCliente), New PropertyMetadata(AddressOf cambioPropiedadDep))

    Public Property IdComitente As String
        Get
            Return (Me.GetValue(IdComitenteDep).ToString)
        End Get
        Set(ByVal value As String)
            Me.SetValue(IdComitenteDep, value)
            Me.mobjVM.IdComitente = value
        End Set
    End Property

    Public Property IdOrdenante As String
        Get
            Return (Me.GetValue(IdOrdenanteDep).ToString)
        End Get
        Set(ByVal value As String)
            Me.SetValue(IdOrdenanteDep, value)
            Me.mobjVM.IdOrdenante = value
        End Set
    End Property

#End Region

#Region "Callback"

    ''' <summary>
    ''' Call back de alguna de las dependecy property
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Shared Sub cambioPropiedadDep(ByVal sender As Object, ByVal args As DependencyPropertyChangedEventArgs)

    End Sub

#End Region

#Region "Inicialización"

    Public Sub New()
        Try
            InitializeComponent()

            mobjVM = CType(Me.LayoutRoot.Resources(STR_NOMBRE_VIEW_MODEL), OrdenantesClienteViewModel)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del buscador de ordenantes del cliente.", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub OrdenantesCliente_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If Not Me.GetValue(IdOrdenanteDep) Is Nothing Then
                Me.mobjVM.IdOrdenante = Me.GetValue(IdOrdenanteDep).ToString
            End If

            If Not Me.GetValue(IdComitenteDep) Is Nothing Then
                Me.mobjVM.IdComitente = Me.GetValue(IdComitenteDep).ToString
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del buscador de ordenantes del cliente.", Me.Name, "OrdenantesCliente_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos de controles"

    Private Sub cboOrdenantes_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Try

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al asignar el ordenante seleccionado.", Me.Name, "cboOrdenantes_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class
