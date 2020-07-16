Imports Telerik.Windows.Controls
Imports System.Windows.Data
Imports System.ComponentModel

Public Class clsTabHabilitar
    Implements INotifyPropertyChanged

    Private _Codigo As String
    Public Property Codigo() As String
        Get
            Return _Codigo
        End Get
        Set(ByVal value As String)
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property

    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Private _TabVisible As Visibility
    Public Property TabVisible() As Visibility
        Get
            Return _TabVisible
        End Get
        Set(ByVal value As Visibility)
            _TabVisible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TabVisible"))
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
