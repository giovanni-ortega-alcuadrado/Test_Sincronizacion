Imports Telerik.Windows.Controls
Public Class RelayCommand
    Implements ICommand
    ReadOnly _Execute As Action(Of Object)
    ReadOnly _CanExecute As Predicate(Of Object)
    Public Sub New(ByVal execute As Action(Of Object), ByVal canExecute As Predicate(Of Object))
        If execute Is Nothing Then
            Throw New ArgumentNullException("execute", "execute is null.")
        End If
        _Execute = execute
        _CanExecute = canExecute
    End Sub
    Public Sub New(ByVal execute As Action(Of Object))
        Me.New(execute, Nothing)
    End Sub
    Public Function CanExecute(ByVal parameter As Object) As Boolean Implements System.Windows.Input.ICommand.CanExecute
        Return If(_CanExecute Is Nothing, True, _CanExecute(parameter))
    End Function
    Public Sub Execute(ByVal parameter As Object) Implements System.Windows.Input.ICommand.Execute
        If CanExecute(parameter) Then
            _Execute(parameter)
        End If
    End Sub
    Public Event CanExecuteChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements System.Windows.Input.ICommand.CanExecuteChanged
    Public Sub RaiseCanExecuteChanged()
        RaiseEvent CanExecuteChanged(Me, Nothing)
    End Sub
End Class
