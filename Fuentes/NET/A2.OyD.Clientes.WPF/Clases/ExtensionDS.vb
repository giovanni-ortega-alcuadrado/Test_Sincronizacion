Imports Telerik.Windows.Controls
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks

Imports OpenRiaServices.DomainServices.Client
Imports A2.OYD.OYDServer.RIA.Web

Public Module OperationExtensions

    <Extension()>
    Public Function AsTask(Of T As OperationBase)(operation As T) As Task(Of T)
        Dim tcs As New TaskCompletionSource(Of T)(operation.UserState)

        AddHandler operation.Completed, Sub(sender, e)
                                            If operation.HasError AndAlso Not operation.IsErrorHandled Then
                                                tcs.TrySetException(operation.[Error])
                                                operation.MarkErrorAsHandled()
                                            ElseIf operation.IsCanceled Then
                                                tcs.TrySetCanceled()
                                            Else
                                                tcs.TrySetResult(operation)
                                            End If
                                        End Sub

        Return tcs.Task
    End Function
End Module
