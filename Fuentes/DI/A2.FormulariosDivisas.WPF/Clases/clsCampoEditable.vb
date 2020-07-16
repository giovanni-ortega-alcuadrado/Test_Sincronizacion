Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Public Class clsCampoEditable
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler _
    Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub


    Private _strNombreCampo As String
    Public Property strNombreCampo() As String
        Get
            Return _strNombreCampo
        End Get
        Set(ByVal value As String)
            _strNombreCampo = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _logEditable As Nullable(Of Boolean)
    Public Property logEditable() As Nullable(Of Boolean)
        Get
            Return _logEditable
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            _logEditable = value
            NotifyPropertyChanged()
        End Set
    End Property
End Class
