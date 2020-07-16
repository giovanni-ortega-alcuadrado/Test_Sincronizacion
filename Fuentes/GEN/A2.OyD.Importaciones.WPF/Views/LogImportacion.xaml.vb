Imports System.ComponentModel

Partial Public Class LogImportacion
    Inherits Window
    Implements INotifyPropertyChanged



    Public Sub New()
        InitializeComponent()
        Me.LayoutRoot.DataContext = Me
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

    Private _texto As String
    Public Property texto As String
        Get
            Return _texto
        End Get
        Set(value As String)
            _texto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("texto"))
        End Set
    End Property

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        'App.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
