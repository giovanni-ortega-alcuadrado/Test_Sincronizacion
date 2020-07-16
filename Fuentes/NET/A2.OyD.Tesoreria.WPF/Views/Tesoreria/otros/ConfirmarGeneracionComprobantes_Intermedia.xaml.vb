Imports Telerik.Windows.Controls
Imports System.ComponentModel

Partial Public Class ConfirmarGeneracionComprobantes_Intermedia
    Inherits Window
    Implements INotifyPropertyChanged
    Dim objViewModel As GenerarComprobantesEgresos_IntermediaViewModel

    Public Sub New(ByVal pobjVMGenerarCEIntermedia As GenerarComprobantesEgresos_IntermediaViewModel)
        Try
            Me.Resources.Add("VMCEIntermedia", pobjVMGenerarCEIntermedia)
            objViewModel = pobjVMGenerarCEIntermedia

            InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
        objViewModel.ConfirmacionGenerarCE()
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
