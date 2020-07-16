Imports Telerik.Windows.Controls
Imports System.ComponentModel

Partial Public Class IdentificacionAportes_CancelacionView
    Inherits Window
    Implements INotifyPropertyChanged
    Dim objViewModel As IdentificacionAportesViewModel

    Public Sub New(ByVal pobjVM As IdentificacionAportesViewModel)
        Try
            Me.Resources.Add("VMIdentificacionAportes", pobjVM)
            objViewModel = pobjVM

            InitializeComponent()

            objViewModel.ConsultarDatosPendientes()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        objViewModel.CancelarPendientes()
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Private Sub btnConsultarPendientes_Click(sender As Object, e As RoutedEventArgs)
        objViewModel.ConsultarDatosPendientes()
    End Sub
End Class
