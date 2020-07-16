Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports A2Utilidades.Mensajes

Partial Public Class cwDetalleCustodiasCambiarFechaReciboView
    Inherits Window
    Implements INotifyPropertyChanged


#Region "Variables"
    Private vm As CustodiaViewModel
    Private cwFechaCierrePortafolio As DateTime? = Nothing
    Private cwTipoInversion As String = String.Empty
    Private cwReciboActual As DateTime? = Nothing
#End Region

#Region "Inicializacion"
    Public Sub New(ByVal pmobjVM As CustodiaViewModel, ByVal pcwTipoInversionAnterior As String, ByVal pcwMdtmFechaCierrePortafolio As DateTime)
        Try
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me
            vm = pmobjVM
            cwFechaRecibo = DateAndTime.Today
            cwTipoInversion = pcwTipoInversionAnterior
            cwFechaCierrePortafolio = pcwMdtmFechaCierrePortafolio
            cwReciboActual = vm.CustodiSelected.Recibo
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización de la ventana", Me.ToString(), "Inicializacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
#End Region

#Region "Métodos para control de eventos"

    Private Async Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.DialogResult = True
            If Not IsNothing(cwFechaRecibo) Then

                Dim logPortafolioHabilitado As Boolean = False
                vm.CustodiSelected.Recibo = cwFechaRecibo

                logPortafolioHabilitado = Await vm.ValidarFechaCierrePortafolio(False)

                If Not logPortafolioHabilitado Then
                    mostrarMensaje("A la custodia no se le puede reclasificar el tipo de inversión porque el portafolio del cliente " & vm.DetalleCustodiaSelected.Comitente.Trim() _
                                   & "-" & vm.DetalleCustodiaSelected.Nombre _
                                   & " se encuentra cerrado para la fecha de reclasificación de inversión (Fecha de cierre de portafolio: " _
                                   & cwFechaCierrePortafolio.Value.Year & "/" & cwFechaCierrePortafolio.Value.Month & "/" & cwFechaCierrePortafolio.Value.Day _
                                   & ", fecha de reclasificación de inversión: " & cwFechaRecibo.Value.Year & "/" _
                                   & cwFechaRecibo.Value.Month & "/" & cwFechaRecibo.Value.Day & ").", _
                                   Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                    vm.CancelarEditarRegistro()
                Else
                    If Not IsNothing(vm.DetalleCustodiaSelected) Then
                        vm.DetalleCustodiaSelected.FechaReclasificacionInversion = cwFechaRecibo
                        vm.DetalleCustodiaSelected.Usuario = Program.Usuario
                        vm.ActualizarRegistro()
                    End If
                End If
            Else
                vm.CancelarEditarRegistro()
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.DialogResult = False
            vm.CancelarEditarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnCancelar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        If Not Me.DialogResult Then
            vm.CancelarEditarRegistro()
        End If
        'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub

#End Region

#Region "Propiedades"

    Private _cwFechaRecibo As System.Nullable(Of System.DateTime)
    Public Property cwFechaRecibo As System.Nullable(Of System.DateTime)
        Get
            Return _cwFechaRecibo
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _cwFechaRecibo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("cwFechaRecibo"))
        End Set
    End Property

#End Region

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    'Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
