Imports Telerik.Windows.Controls
Imports System.ComponentModel

Partial Public Class MensajeGMF
    Inherits Window
    Implements INotifyPropertyChanged

    Public Sub New()
        InitializeComponent()
        Me.LayoutRoot.DataContext = Me
        generargmf.Encima = False
        generargmf.Debajo = False
        generargmf.NoGenera = False
    End Sub

    Private _generargmf As GeneraGmf = New GeneraGmf
    Public Property generargmf As GeneraGmf
        Get
            Return _generargmf
        End Get
        Set(ByVal value As GeneraGmf)
            _generargmf = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("generargmf"))
        End Set
    End Property


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Private Sub RadioButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.DialogResult = True
        Me.Close()
    End Sub
End Class
Public Class GeneraGmf

    Public Property Encima As Boolean

    Public Property Debajo As Boolean

    Public Property NoGenera As Boolean
End Class
