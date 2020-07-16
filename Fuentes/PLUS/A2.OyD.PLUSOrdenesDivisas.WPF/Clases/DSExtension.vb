Imports Telerik.Windows.Controls
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Imports A2.OYD.OYDServer.RIA.Web

Module DomainContextExtension

    <Extension()>
    Public Function LoadAsync(Of T As Entity)(ByVal source As DomainContext, query As EntityQuery(Of T)) As Task(Of LoadResult(Of T))
        Return source.LoadAsync(query, LoadBehavior.KeepCurrent)
    End Function

    <Extension()>
    Public Function LoadAsync(Of T As Entity)(ByVal source As DomainContext, query As EntityQuery(Of T), loadBehavior As LoadBehavior) As Task(Of IEnumerable(Of T))
        Dim taskCompletionSource As TaskCompletionSource(Of IEnumerable(Of T)) = New TaskCompletionSource(Of IEnumerable(Of T))()
        source.Load(query, loadBehavior, Sub(loadOperation)
                                             Dim strMsg As String = "Se generaron las siguientes inconsistencias durante la validación de los datos: " & vbNewLine

                                             If loadOperation.HasError Then
                                                 If loadOperation.ValidationErrors.Count > 0 Then
                                                     For Each objVal In loadOperation.ValidationErrors
                                                         strMsg &= strMsg & objVal.ErrorMessage() & vbNewLine
                                                     Next objVal

                                                     taskCompletionSource.TrySetException(New Exception(strMsg))
                                                 Else
                                                     taskCompletionSource.TrySetException(loadOperation.Error)
                                                 End If
                                                 loadOperation.MarkErrorAsHandled()
                                             ElseIf loadOperation.IsCanceled Then
                                                 'taskCompletionSource.TrySetCanceled()
                                                 taskCompletionSource.TrySetException(New Exception("Se canceló la tarea para el procesamiento de la información de acuerdo con la información recibida."))
                                             Else
                                                 taskCompletionSource.TrySetResult(loadOperation.Entities)
                                             End If
                                         End Sub, Nothing)

        Return taskCompletionSource.Task
    End Function

    <Extension()>
    Public Function LoadEntityAsync(Of T As Entity)(ByVal source As DomainContext, query As EntityQuery(Of T)) As Task(Of IEnumerable(Of T))
        Return source.LoadEntityAsync(query, LoadBehavior.KeepCurrent)
    End Function

    <Extension()>
    Public Async Function LoadEntityAsync(Of T As Entity)(ByVal source As DomainContext, query As EntityQuery(Of T), loadBehavior As LoadBehavior) As Task(Of IEnumerable(Of T))
        Dim entity As IEnumerable(Of T) = Await source.LoadAsync(query, loadBehavior)
        Return CType(entity.FirstOrDefault, Global.System.Collections.Generic.IEnumerable(Of T))
    End Function

End Module
