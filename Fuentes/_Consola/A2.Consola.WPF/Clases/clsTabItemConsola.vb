
Imports System
Imports Telerik.Windows.Controls
Imports System.ComponentModel

Public Class clsTabItemConsola
    Implements INotifyPropertyChanged

    Private ReadOnly mainConsola As A2Consola

    Public Sub New(ByVal objConsola As A2Consola, ByVal pobjTabItem As TabItem)
        Me.mainConsola = objConsola
        Me.TabItem = pobjTabItem

        Me.RemoveItemCommand = New DelegateCommand(Function()
                                                       Me.mainConsola.TabConsola_RemoveItem(Me)
                                                   End Function, Function() Me.mainConsola.ddcDocumentosTabControl.Items.Count > 1)
    End Sub

    Public Property Header As String
    Public Property UrlOrigen As String

    Private _isSelected As Boolean
    Public Property IsSelected As Boolean
        Get
            Return Me._isSelected
        End Get
        Set(ByVal value As Boolean)

            If Me._isSelected <> value Then
                Me._isSelected = value
                Me.OnPropertyChanged("IsSelected")
            End If
        End Set
    End Property

    Private _objTabItem As TabItem
    Public Property TabItem As TabItem
        Get
            Return Me._objTabItem
        End Get
        Set(ByVal value As TabItem)
            Me._objTabItem = value
        End Set
    End Property

    Public Property RemoveItemCommand As DelegateCommand
    Public Event PropertyChanged As PropertyChangedEventHandler
    Private Event INotifyPropertyChanged_PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Sub OnPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub
End Class
