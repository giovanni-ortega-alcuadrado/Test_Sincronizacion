Imports Telerik.Windows.Controls

Imports System.ComponentModel
Imports System.Collections

Public Class BaseViewModel
    Inherits UserControl
    Implements INotifyPropertyChanged

    Public Overridable Sub CambioItem(pstrNombrePropiedad As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(pstrNombrePropiedad))
    End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
