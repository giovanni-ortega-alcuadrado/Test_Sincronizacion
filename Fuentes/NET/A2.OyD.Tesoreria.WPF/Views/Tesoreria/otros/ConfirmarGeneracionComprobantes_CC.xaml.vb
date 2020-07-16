Imports Telerik.Windows.Controls
Imports System.ComponentModel

Partial Public Class ConfirmarGeneracionComprobantes_CC
    Inherits Window
    Implements INotifyPropertyChanged
    Dim objViewModel As GenerarCE_CarteraColectivasViewModel

    Public Sub New(ByVal pobjVMGenerarCECC As GenerarCE_CarteraColectivasViewModel)
        Try
            Me.Resources.Add("VMCECarteraCo", pobjVMGenerarCECC)
            objViewModel = pobjVMGenerarCECC

            InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        If objViewModel.ListaConfirmacionUsuario.Where(Function(i) i.Confirmar = True).Count > 0 Then
            Me.DialogResult = True
            objViewModel.ConfirmacionGenerarCE()
        Else
            A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar al menos un registro para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If

    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
