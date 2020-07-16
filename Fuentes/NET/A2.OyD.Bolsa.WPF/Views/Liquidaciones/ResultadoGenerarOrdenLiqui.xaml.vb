Imports Telerik.Windows.Controls
Imports System.ComponentModel

Partial Public Class ResultadoGenerarOrdenLiqui
    Inherits Window
    Implements INotifyPropertyChanged

    Public Sub New(ByVal MensajeResultado As String) ', ByVal NroAccionesGen As Integer, ByVal NroRentaGen As Integer)
        InitializeComponent()
        Me.LayoutRoot.DataContext = Me
        Mensaje = MensajeResultado
        'If NroAccionesGen <> 0 Or NroRentaGen <> 0 Then
        '    A2Utilidades.Mensajes.mostrarMensaje("Se generaron un total de: " & CStr(Int(NroAccionesGen + NroRentaGen)) & " ordenes satisfactoriamente " & vbCrLf & vbCrLf & " -Total Ordenes Acciones: " & NroAccionesGen & vbCrLf & " -Total Ordenes Renta Fija: " & NroRentaGen, _
        '                 Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
        'End If
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub


    Private _Mensaje As String
    Public Property Mensaje() As String
        Get
            Return _Mensaje
        End Get
        Set(ByVal value As String)
            _Mensaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Mensaje"))
        End Set
    End Property

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
